using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Batch;
using HtmlAgilityPack;
using Microsoft.Azure.Batch.Protocol.Models;
using PuppeteerSharp;
using System.Reflection.Metadata;
using Aspose.Slides.Export.Web;
using Aspose.Slides;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Scaping1.Models;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

namespace Scaping1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScrapingController : ControllerBase
    {
        
        [HttpPost]
        public async Task Awb(string id)
        {
            var launchOptions = new LaunchOptions
            {

                Headless = false, // = false for testing
                ExecutablePath = "C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe"
            };
            StatusHistory statusHistory = new StatusHistory(); 
            AWBStatus awbstatus = new AWBStatus(); 
            FlightDetail flightDetail = new FlightDetail();
            var jsonObject = new
            {
                data = new
                {
                    StatusHistory = statusHistory,
                    AWBStatus = awbstatus,
                    FlightDetail = flightDetail
                }
            };


            var jsonArray = new List<object>();

            var browser = await Puppeteer.LaunchAsync(launchOptions);
            var page = await browser.NewPageAsync();
            await page.GoToAsync("https://6ecargo.goindigo.in/FrmAWBTracking.aspx");

            string[] split = id.Split("-");
            string prefix = split.First();
            string axbno = split.Last();


            var pfix =  page.WaitForSelectorAsync("#txtPrefix").Result;
            await pfix.TypeAsync(prefix);

            var ano = await page.WaitForSelectorAsync("#TextBoxAWBno");
            await ano.TypeAsync(axbno);

            await page.ClickAsync("#ButtonGO");
            Thread.Sleep(5000);
            var htmlSource = await page.GetContentAsync();
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(htmlSource);

            
            HtmlNode AWS = document.DocumentNode.SelectSingleNode("//*[@id=\"rptTrackAWBs_ctl00_btnAWBNumber\"]");
            if (AWS != null)
            {
                awbstatus.AWB = AWS.Attributes["value"].Value;
            }
            //Console.WriteLine(status.LastActivity);
            HtmlNode lastActvitydate = document.DocumentNode.SelectSingleNode("//*[@id=\"pnlMultipleAWBs\"]/div/div[2]/span");
            awbstatus.LastActivity = lastActvitydate.InnerText.Trim();

            var table1 = document.DocumentNode.SelectNodes("//*[@id=\"pnlShowData\"]/div[1]/table/tbody/tr/td[3]/table");
            if (table1 != null)
            {
                foreach (var row in table1)
                {
                    HtmlNode destination = row.SelectSingleNode(".//td[1]");
                    statusHistory.Destination = destination.InnerText.Trim();
                }
            }

            var og = document.DocumentNode.SelectNodes("//*[@id=\"pnlShowData\"]/div[1]/table/tbody/tr/td[1]/table");
            if (og != null)
            {
                foreach (var row in og)
                {
                    HtmlNode origin = row.SelectSingleNode(".//td[1]");
                    statusHistory.Origin = origin.InnerText.Trim();
                }
            }

            

            


            
            var statusHistroy = document.DocumentNode.SelectNodes("//*[@id=\"GridViewAwbTracking\"]");
            //var nextRow = document.DocumentNode.SelectNodes("//*[@id=\"pnlShowData\"]/div[2]/div/div[2]/table");
            //var NewRowValues = nextRow.Descendants().Select(x=>x.NextSibling);
            //Console.WriteLine(NewRowValues);
            if (statusHistroy != null)
            {
                foreach (var row in statusHistroy)
                {
                    
                    HtmlNode flightNumber = row.SelectSingleNode(".//td[5]");
                    flightDetail.Number = flightNumber.InnerText.Trim();
                    HtmlNode flightDate = row.SelectSingleNode(".//td[6]");
                    flightDetail.Date = flightDate.InnerText.Trim();
                    HtmlNode flightOrgin = row.SelectSingleNode(".//td[7]");
                    flightDetail.Origin = flightOrgin.InnerText.Trim();
                    HtmlNode flightDestination = row.SelectSingleNode(".//td[8]");
                    flightDetail.Destination = flightDestination.InnerText.Trim();
                    HtmlNode flightMilestone = row.SelectSingleNode(".//td[2]");
                    statusHistory.MileStone = flightMilestone.InnerText.Trim();
                    HtmlNode flightLastActivityDate = row.SelectSingleNode(".//td[10]");
                    awbstatus.LastActivityDate = flightLastActivityDate.InnerText.Trim();
                    HtmlNode pieces = row.SelectSingleNode(".//td[3]");
                    awbstatus.Pieces = pieces.InnerText.Trim();
                    statusHistory.Pcs = pieces.InnerText.Trim();
                    HtmlNode actualPieceAndWeight = row.SelectSingleNode("/html/body/form/div[4]/div[3]/div/div[2]/div/div[2]/table/tbody/tr[3]/td/span");
                    var aboveValue = actualPieceAndWeight.InnerText.Trim();
                    string[] apw = aboveValue.Split("/");
                    statusHistory.ActualPieces = apw.First();
                    statusHistory.Weight = apw.Last();
                    var flightNo = flightNumber.InnerText.Trim();
                    

                    string aboveData = flightNo.Substring(0,2);
                    statusHistory.FlightNo = flightNumber.InnerText.Trim();
                    statusHistory.AirlineCode = aboveData;

                    HtmlNode flightTime = row.SelectSingleNode(".//td[6]");
                    statusHistory.FlightDate = flightTime.InnerText.Trim();

                    HtmlNode udl = row.SelectSingleNode(".//td[9]");
                    statusHistory.ULD = udl.InnerText.Trim();

                    HtmlNode eventDateTime = row.SelectSingleNode(".//td[10]");
                    statusHistory.EventDateTime = eventDateTime.InnerText.Trim();






                }
                jsonArray.Add(jsonObject);

                
            }
             
            var bookingAndAcceptance = document.DocumentNode.SelectNodes("//*[@id=\"gvBkAcInfo\"]");
            if (bookingAndAcceptance != null)
            {
                foreach (var row in bookingAndAcceptance)
                {
                    HtmlNode status = row.SelectSingleNode(".//td[1]");
                    awbstatus.Status = status.InnerText.Trim();
                    statusHistory.Status = status.InnerText.Trim();
                }
            }
            string json = JsonConvert.SerializeObject(jsonArray, Formatting.Indented);
            Console.WriteLine(json);

        }
    }
}
