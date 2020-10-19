using Hangfire;
using Hangfire.MemoryStorage.Dto;
using HangFire.Domain.Repositories;
using HangFire.Models;
using HangFire.Service.Interfaces;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ScrapySharp.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HangFire
{
    public class ScrapingJobService : IScrapingJobService
    {
        private readonly string imageFolder = @"C:\Lab\Smart-Apartment-Data\03 - HangFire\HangFire\Images\";
        private List<String> _invokedURLs = new List<string>();
        private List<String> _savedImageList = new List<string>();
        private HtmlWeb _web = new HtmlWeb();
        private readonly IBackgroundJobClient _backgroundJobClient;
        readonly IScanningUrlRepository _scanningUrlRepository;
        public ScrapingJobService(IScanningUrlRepository scanningUrlRepository
           ,IBackgroundJobClient backgroundJobClient
            )
        {
            _backgroundJobClient = backgroundJobClient;
            _scanningUrlRepository = scanningUrlRepository;
        }
        public void StartScraping(string urlToScraping)
        {
            ScrapUrls(urlToScraping);
        }
        public void ScanUrlsWithQueue()
        {
            _backgroundJobClient.Enqueue(() => ProcessUrls());

        }
        public void ScanUrls()
        {
            ProcessUrls();
            Console.ReadLine();

        }
        public void ProcessUrls()
        {
            var data = _scanningUrlRepository.TakeAll(250000);
            var now = DateTime.Now;
           foreach(var f in data)
            {
                // _backgroundJobClient.Enqueue(() => GetUrlDataWithAgilityPack(f.Url));
                 _backgroundJobClient.Enqueue(() => GetUrlDataWithHttpWeb(f.Url));
                 Console.WriteLine($"Duration: {DateTime.Now.Subtract(now)}- {f.Id} - {f.Url}");
            }

        }
        #region Helpers
        public void GetUrlDataWithAgilityPack(string url)
        {
            try
            {
                //COMMENT: Do something else with this HTML code
                var htmlData= _web.Load(url);
            }
            catch(Exception ex) { Console.WriteLine(ex.Message); }
        }
        public void GetUrlDataWithHttpWeb(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;

                    if (String.IsNullOrWhiteSpace(response.CharacterSet))
                        readStream = new StreamReader(receiveStream);
                    else
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                    //COMMENT: Do something else with this HTML code
                    string data = readStream.ReadToEnd();

                    response.Close();
                    readStream.Close();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
        public void ScrapUrls(string url)
        {
            //Avoid to save the same Url twice and stop this process when urls count had reached 250K
            if (_invokedURLs.Find(f => f == url) != null || _invokedURLs.Count() == 250000)
                return;

            HtmlDocument _document = _web.Load(url);

            foreach (var anchors in _document.DocumentNode.CssSelect("a"))
            {
                var anchorURL = anchors.GetAttributeValue("href");
                foreach (var node in _document.DocumentNode.CssSelect("img"))
                {
                    var imageURL = (node.GetAttributeValue("src"));
                    //Avoid to save the same image twice
                    if (_savedImageList.Find(f => f == imageURL) != null)
                        return;
                    _backgroundJobClient.Enqueue(() => SaveImagesLocaly(imageURL));
                }           
                _backgroundJobClient.Enqueue(() => ScrapUrls(url));
            }
        }
        public void SaveURlToDabase(string url)
        {
            _scanningUrlRepository.AddAsync(new ScanningUrl
            {
                Url = url
            });
        }
        public void SaveImagesLocaly(string imageURL)
        {
            try
            {

            using (WebClient oClient = new WebClient())
                {
                    var fullPathImage = $"{imageFolder}{Guid.NewGuid()}.jpg";
                    oClient.DownloadFile(new Uri(imageURL), fullPathImage);

                    _savedImageList.Add(imageURL);

                    Console.WriteLine($"Image saved loacaly: {imageURL}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"this image can't be saved: {imageURL}");
            }
        }
        #endregion


    }
}
