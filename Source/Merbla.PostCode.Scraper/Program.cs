#region Using Directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Merbla.PostCode.Scraper.Maps;
using Merbla.PostCode.Scraper.Models.Google;
using RestSharp;
using log4net;

#endregion

namespace Merbla.PostCode.Scraper
{
    internal class Program
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof (Program));
        private static WebProxy Proxy;
        private static RestClient Client;

        private static void Main(string[] args)
        {
            try
            {
                Client = new RestClient {BaseUrl = "http://maps.googleapis.com/", Proxy = Proxy};

                Run();
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                Console.ReadLine();
            }
        }

        private static void Run()
        {
            _log.Info("Started");
            var postCodes = GetDistinctPostCodes();
            
            _log.Info(postCodes.Count() + " Records read from CSV");

            var asParallellStartTime = DateTime.Now;
            var updated = new List<Models.PostCode>();
             
            _log.Info("Getting Lat/Long by AsParallel");
            postCodes.AsParallel().ForAll((p) =>
                                              {
                                                  var result = Populate(p);
                                                  updated.Add(result);
                                              });

            var asParallellEndTime = DateTime.Now.Subtract(asParallellStartTime);
            _log.Info("AsParallel took " + asParallellEndTime);


            var tplPostCodes = new List<Models.PostCode>();
            var tplStartTime = DateTime.Now;

            _log.Info("Getting Lat/Long by TPL");
            var parent = Task.Factory.StartNew(() =>
            {
                foreach (var p in postCodes)
                {
                    var task = Task<Models.PostCode>.Factory.StartNew(
                        () => Populate(p),
                        TaskCreationOptions.AttachedToParent);
                    tplPostCodes.Add(task.Result);
                }
            });
            parent.Wait();

            var tplEndTime = DateTime.Now.Subtract(tplStartTime);
            _log.Info("TPL took " + tplEndTime);


            WriteCsv(updated);
            Console.WriteLine("Finished... Press Enter");
            Console.ReadLine();
        }

        private static void WriteCsv(IEnumerable<Models.PostCode> postCodes)
        {
            var csvFile = Guid.NewGuid() + ".csv";
            using (var f = File.Create(csvFile))
            {
                var csv = new CsvHelper.CsvHelper(f);
                csv.Configuration.ClassMapping<WritePostCodeMap, Models.PostCode>();
                csv.Writer.WriteRecords(postCodes);
            }
        }

        private static IEnumerable<Models.PostCode> GetPostCodes()
        {
            IEnumerable<Models.PostCode> postCodes;
            using (var fileStream = File.Open("Test.csv", FileMode.Open, FileAccess.ReadWrite,FileShare.ReadWrite))
            {
                var csv = new CsvHelper.CsvHelper(fileStream);
                csv.Configuration.ClassMapping<ReadPostCodeMap, Models.PostCode>();
                postCodes = csv.Reader.GetRecords<Models.PostCode>().ToList();
            }
            return postCodes;
        }

        private static IEnumerable<Models.PostCode> GetDistinctPostCodes()
        {
            var distinctCodes =
                GetPostCodes().Select(p => new { PostCode = p.Code, p.State })
                    .Distinct()
                    .Select(s => new Models.PostCode {Code = s.PostCode, State = s.State});

            return distinctCodes;
        }


        private static Models.PostCode Populate(Models.PostCode postCode)
        {
            var result = GetLatLongForPostCode(postCode.Code.ToString(), postCode.State);

            if (result != null)
            {
                postCode.Latitude = result.Latitude;
                postCode.Longitude = result.Longitude;
            }

            return postCode;
        }

        private static Result GetLatLongForPostCode(string postCode, string state)
        {
            var resource = string.Format("maps/api/geocode/json?address={0}+{1}+Australia&sensor=false", postCode, state);
            var request = new RestRequest {Resource = resource};
            var response = Client.Execute<Result>(request);
            return response.Data;
        }
    }
}