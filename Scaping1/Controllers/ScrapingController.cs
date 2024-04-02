using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Batch;
using HtmlAgilityPack;
using Microsoft.Azure.Batch.Protocol.Models;
using PuppeteerSharp;
using System.Reflection.Metadata;
<<<<<<< Updated upstream
=======
using Aspose.Slides.Export.Web;
using Aspose.Slides;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Scaping1.Models;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
>>>>>>> Stashed changes

namespace Scaping1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScrapingController : ControllerBase
    {
<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
        [HttpPost]
        public async Task Awb(string id)
        {
            var launchOptions = new LaunchOptions
            {

                Headless = false, // = false for testing
                ExecutablePath = "C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe"
            };
<<<<<<< Updated upstream
            
=======
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
>>>>>>> Stashed changes

            var browser = await Puppeteer.LaunchAsync(launchOptions);
            var page = await browser.NewPageAsync();
            await page.GoToAsync("https://6ecargo.goindigo.in/FrmAWBTracking.aspx");

            string[] split = id.Split("-");
            string prefix = split.First();
            string axbno = split.Last();


            var pfix = page.WaitForSelectorAsync("#txtPrefix").Result;
            await pfix.TypeAsync(prefix);

            var ano = await page.WaitForSelectorAsync("#TextBoxAWBno");
            await ano.TypeAsync(axbno);

            await page.ClickAsync("#ButtonGO");
            Thread.Sleep(5000);
            var htmlSource = await page.GetContentAsync();
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(htmlSource);

<<<<<<< Updated upstream
            var nodes = document.DocumentNode.SelectNodes("//*[@id=\"gvBkAcInfo\"]");
            var table = nodes.Where(x=> x.FirstChild.Name == "#text" );
            //var nodes1 = document.DocumentNode.SelectNodes("//*[@id=\"GridViewAwbTracking\"]");




            if (nodes != null)
=======

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
>>>>>>> Stashed changes
            {
                foreach (var item in nodes)
                {
<<<<<<< Updated upstream
                    Console.WriteLine(item.InnerText);
=======
                    HtmlNode destination = row.SelectSingleNode(".//td[1]");
                    statusHistory.Destination = destination.InnerText.Trim();
                    awbstatus.Destination = destination.InnerText.Trim();
>>>>>>> Stashed changes
                }

            }
<<<<<<< Updated upstream
=======

            var og = document.DocumentNode.SelectNodes("//*[@id=\"pnlShowData\"]/div[1]/table/tbody/tr/td[1]/table");
            if (og != null)
            {
                foreach (var row in og)
                {
                    HtmlNode origin = row.SelectSingleNode(".//td[1]");
                    statusHistory.Origin = origin.InnerText.Trim();
                    awbstatus.Origin = origin.InnerText.Trim();
                }
            }


            List<FlightDetail> flightDetailsList = new List<FlightDetail>();



            

            List<StatusHistory> statusHistories = new List<StatusHistory>();
            var statusHist = document.DocumentNode.SelectNodes("//*[@id=\"GridViewAwbTracking\"]/tbody").FirstOrDefault()?.ChildNodes?.Where(x=>x.HasClass("newstyle-tr"));
            if (statusHistory != null) {
                foreach (var row in statusHist) {
                    StatusHistory status = new StatusHistory();
                    status.Station = row.ChildNodes[1].InnerText.Trim();
                    status.MileStone = row.ChildNodes[2].InnerText.Trim();
                    status.Pcs = row.ChildNodes[3].InnerText.Trim();
                    status.Weight = row.ChildNodes[4].InnerText.Trim();
                    status.FlightNo = row.ChildNodes[5].InnerText.Trim();
                    status.FlightDate = row.ChildNodes[6].InnerText.Trim();
                    status.Origin = row.ChildNodes[7].InnerText.Trim();
                    status.Destination = row.ChildNodes[8].InnerText.Trim();
                    status.ULD = row.ChildNodes[9].InnerText.Trim();  

                    statusHistories.Add(status);    

                    Console.WriteLine(statusHistories);

                }
               
            }

            var bookingAndAcceptance = document.DocumentNode.SelectNodes("//*[@id=\"gvBkAcInfo\"]/tbody").FirstOrDefault()?.ChildNodes?.Where(x => x.HasClass("newstyle-tr"));
            //*[@id="gvBkAcInfo"]/tbody
            List<FlightDetail> flightDetails = new List<FlightDetail>();
            if (bookingAndAcceptance != null)
            {
                foreach (var row in bookingAndAcceptance)
                {
                    FlightDetail flightDetail1 = new FlightDetail();
                    var sampleNumber = row.ChildNodes[6].InnerText;
                    string[] aboveSplit = sampleNumber.Split(' ');
                    flightDetail1.Number = aboveSplit.First();
                    flightDetail.Date = aboveSplit.Last();
                    flightDetail1.Origin = row.ChildNodes[2].InnerText.Trim();
                    flightDetail1.Destination = row.ChildNodes[3].InnerText.Trim();
                    flightDetails.Add(flightDetail1 );
                }
            }

            List<FlightSchedules> flights = new List<FlightSchedules>();
            FlightSchedules flightSchedules = new FlightSchedules();
            string flightNumber = flightDetails.Select(x=>x.Number).First();
            flightSchedules.FlightNo = flightNumber;
            flightSchedules.AirlineCode = flightNumber.Substring(0, 2);
            flightSchedules.Station = flightDetails.Select(x=>x.Origin).First();
            flightSchedules.Destination = flightDetails.Select(x=>x.Destination).First();
            flightSchedules.ETA = statusHistories.Where(x => x.MileStone == "Arrived").Select(x => x.FlightDate).First();
            flightSchedules.ETD = statusHistories.Where(x => x.MileStone == "Departed").Select(x => x.FlightDate).First();
            


            string json = JsonConvert.SerializeObject(jsonArray, Formatting.Indented);
            Console.WriteLine(json);
>>>>>>> Stashed changes
        }
    }
}
        
    

