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
            
            FlightDetail flightDetail = new FlightDetail();
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
            var rows = document.DocumentNode.SelectNodes("//*[@id=\"GridViewAwbTracking\"]");
            //var nextRow = document.DocumentNode.SelectNodes("//*[@id=\"pnlShowData\"]/div[2]/div/div[2]/table");
            //var NewRowValues = nextRow.Descendants().Select(x=>x.NextSibling);
            //Console.WriteLine(NewRowValues);
            if (rows != null)
            {
                foreach (var row in rows)
                {
                    // Extract values from each cell
                    HtmlNode values = row.SelectSingleNode(".//td[5]");
                    flightDetail.Number = values.InnerText.Trim();

                    // Print extracted values
                    Console.WriteLine(flightDetail.Number);
                }
            }

        }
    }
}
