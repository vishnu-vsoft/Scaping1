﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Batch;
using HtmlAgilityPack;
using Microsoft.Azure.Batch.Protocol.Models;
using PuppeteerSharp;
using System.Reflection.Metadata;

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

            var nodes = document.DocumentNode.SelectNodes("//*[@id=\"gvBkAcInfo\"]");
            var table = nodes.Where(x=> x.FirstChild.Name == "#text" );
            //var nodes1 = document.DocumentNode.SelectNodes("//*[@id=\"GridViewAwbTracking\"]");




            if (nodes != null)
            {
                foreach (var item in nodes)
                {
                    Console.WriteLine(item.InnerText);
                }

            }
        }
    }
}
