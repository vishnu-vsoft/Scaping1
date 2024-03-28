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
            var rows = document.DocumentNode.SelectNodes("//table[@class='newstyle']/tbody/tr[@class='newstyle-tr']");

            if (rows != null)
            {
                foreach (var row in rows)
                {
                    // Extract values from each cell
                    var values = row.SelectNodes("td").Where(td => td.Name == "td").Select(td=>td.InnerText.Trim());

                    // Print extracted values
                    Console.WriteLine(string.Join(", ", values));
                }
            }

        }
    }
}
