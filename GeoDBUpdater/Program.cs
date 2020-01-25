
using CsvHelper;
using GeoDBUpdater.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;

namespace GeoDBUpdater
{
    class Program
    {
        /// <summary>
        /// It could be improoved by implementing Quartz.Net to have updating by schedule
        /// Also we ccan convert console app to a windows service easly via http://topshelf-project.com/
        /// And finally we can use NLog or other logging framework to keep log and notifiy admin about any issues
        /// We migh want to add more exceptions to specify them as well as to see an innerexception's messages
        /// And etc 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Do();
            Console.Read();
        }

        private static void Do()
        {
            try
            {
                Console.WriteLine("Checking folder...  {0}", Properties.Settings.Default.Temp);

                var tempPath = Path.Combine(Environment.CurrentDirectory, Properties.Settings.Default.Temp);
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }
                else
                {
                    Directory.GetFiles(tempPath).ToList().ForEach(File.Delete);
                }

                DirectoryInfo di = new DirectoryInfo(tempPath);

                if (di != null)
                {
                    Console.WriteLine("Folder exists  {0} ", tempPath);

                    var tempFileName = string.Format("{0}.zip", Guid.NewGuid().ToString());
                    var fullTempFileName = Path.Combine(tempPath, tempFileName);

                    using (WebClient wc = new WebClient())
                    {
                        wc.DownloadProgressChanged += (sender, e) =>
                        {
                            Console.WriteLine("\r Downloading...  {0}%   ", e.ProgressPercentage);
                        };

                        wc.DownloadFileCompleted += (sender, e) =>
                        {
                            if (e.Error == null)
                            {
                                Console.WriteLine("Downloading has been completed!");
                                ReadFileAndUpdateDatabase(fullTempFileName);
                            }
                            else
                            {
                                Console.WriteLine("Uppps... Cannot download file... There are some errors...");
                            }
                        };

                        Console.WriteLine("Downloading file from {0}", Properties.Settings.Default.LastCSVLink);
                        wc.DownloadFileAsync(new System.Uri(Properties.Settings.Default.LastCSVLink), fullTempFileName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
        }

        private static void ReadFileAndUpdateDatabase(string fullTempFileName)
        {
            try
            {
                if (File.Exists(fullTempFileName))
                {
                    Console.WriteLine("File processing starting...");
                    using (var file = File.OpenRead(fullTempFileName))
                    using (var zip = new ZipArchive(file, ZipArchiveMode.Read))
                    {
                        foreach (var entry in zip.Entries)
                        {
                            using (var stream = entry.Open())
                            {
                                // convert stream to string
                                var reader = new StreamReader(stream);
                                var text = reader.ReadToEnd();

                                // Here we are dealing with predefined list of files
                                // No magic here and logic is hard-coded
                                var filname = entry.Name.ToLower();
                                if (filname.StartsWith("GeoLite2-Country-Blocks-IPv4".ToLower()))
                                {
                                    UpdateCountryBlockIPv4(text);
                                }
                                else if (filname.StartsWith("GeoLite2-Country-Locations".ToLower()) &&
                                    filname.EndsWith("-" + CountryLocationsPostfixes.DE.ToLower() + ".csv"))
                                {
                                    CountryLocations(text, CountryLocationsPostfixes.DE.ToLower());
                                }
                                else if (filname.StartsWith("GeoLite2-Country-Locations".ToLower()) &&
                                    filname.EndsWith("-" + CountryLocationsPostfixes.EN.ToLower() + ".csv"))
                                {
                                    CountryLocations(text, CountryLocationsPostfixes.EN.ToLower());
                                }
                                else if (filname.StartsWith("GeoLite2-Country-Locations".ToLower()) &&
                                    filname.EndsWith("-" + CountryLocationsPostfixes.ES.ToLower() + ".csv"))
                                {
                                    CountryLocations(text, CountryLocationsPostfixes.ES.ToLower());
                                }
                                else if (filname.StartsWith("GeoLite2-Country-Locations".ToLower()) &&
                                    filname.EndsWith("-" + CountryLocationsPostfixes.FR.ToLower() + ".csv"))
                                {
                                    CountryLocations(text, CountryLocationsPostfixes.FR.ToLower());
                                }
                                else if (filname.StartsWith("GeoLite2-Country-Locations".ToLower()) &&
                                    filname.EndsWith("-" + CountryLocationsPostfixes.JA.ToLower() + ".csv"))
                                {
                                    CountryLocations(text, CountryLocationsPostfixes.JA.ToLower());
                                }
                                else if (filname.StartsWith("GeoLite2-Country-Locations".ToLower()) &&
                                    filname.EndsWith("-" + CountryLocationsPostfixes.PTBR.ToLower() + ".csv"))
                                {
                                    CountryLocations(text, CountryLocationsPostfixes.PTBR.ToLower());
                                }
                                else if (filname.StartsWith("GeoLite2-Country-Locations".ToLower()) &&
                                    filname.EndsWith("-" + CountryLocationsPostfixes.RU.ToLower() + ".csv"))
                                {
                                    CountryLocations(text, CountryLocationsPostfixes.RU.ToLower());
                                }
                                else if (filname.StartsWith("GeoLite2-Country-Locations".ToLower()) &&
                                    filname.EndsWith("-" + CountryLocationsPostfixes.ZHCH.ToLower() + ".csv"))
                                {
                                    CountryLocations(text, CountryLocationsPostfixes.ZHCH.ToLower());
                                }
                            }
                        }
                    }
                    Console.WriteLine("File processing is finished.");
                }
            }
            catch (Exception ex)
            {
                //TODO Any logging framework like NLog and etc
                Console.WriteLine("Error: {0}", ex.Message);
            }
        }

        private static void UpdateCountryBlockIPv4(string csvString)
        {
            List<CountryBlockIPv4Record> items = new List<CountryBlockIPv4Record>();

            // Parse file to class CountryBlockIPv4 
            using (TextReader reader = new StringReader(csvString))
            {
                CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                csv.Configuration.Delimiter = ",";
                csv.Configuration.MissingFieldFound = null;
                while (csv.Read())
                {
                    var item = csv.GetRecord<CountryBlockIPv4Record>();
                    items.Add(item);
                }
            }

            if (items.Any())
            {
                var removingResult = 0;
                // Clean CountryBlockIPv4 table
                using (var db = new ApplicationContext())
                {
                    var entities = db.CountryBlockIPv4.ToList();
                    if (entities.Count > 0) // N-uploading
                    {
                        db.CountryBlockIPv4.RemoveRange(entities);
                        removingResult += db.SaveChanges();
                        if (removingResult > 0)
                        {
                            Console.WriteLine("CountryBlockIPv4 has been cleaned with {0} records", removingResult);
                        }
                    }
                    else // First uploading
                    {
                        removingResult += 1;
                    }
                }

                if (removingResult > 0)
                {
                    // Populate CountryBlockIPv4 table
                    using (var db = new ApplicationContext())
                    {
                        foreach (var item in items)
                        {
                            // I use simplest way to update data and well it works )))
                            // There are different approaches how to sync data...
                            // Only for demonstration perprse I use the approach )))
                            var n = new CountryBlockIPv4
                            {
                                id = Guid.NewGuid(),
                                geoname_id = item.geoname_id,
                                is_anonymous_proxy = item.is_anonymous_proxy,
                                is_satellite_provider = item.is_satellite_provider,
                                network = item.network,
                                registered_country_geoname_id = item.registered_country_geoname_id,
                                represented_country_geoname_id = item.represented_country_geoname_id
                            };

                            db.CountryBlockIPv4.Add(n);
                        }

                        var insertingResult = db.SaveChanges();
                        if (insertingResult > 0)
                        {
                            Console.WriteLine("CountryBlockIPv4 has been updated with {0} records", insertingResult);
                        }
                    }
                }
            }
        }

        private static void CountryLocations(string csvString, string postfixLocalCode)
        {
            List<CountryLocationRecord> items = new List<CountryLocationRecord>();

            // Parse file to class CountryLocation 
            using (TextReader reader = new StringReader(csvString))
            {
                CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                csv.Configuration.Delimiter = ",";
                csv.Configuration.MissingFieldFound = null;
                while (csv.Read())
                {
                    var item = csv.GetRecord<CountryLocationRecord>();
                    items.Add(item);
                }
            }

            if (items.Any())
            {
                var removingResult = 0;
                // Clean CountryLocation table where locale_code is CountryLocationsPostfixes.DE
                using (var db = new ApplicationContext())
                {
                    var entities = db.CountryLocation.Where(x => x.locale_code.Trim() == postfixLocalCode).ToList();
                    if (entities.Count > 0) // N-uploading
                    {
                        db.CountryLocation.RemoveRange(entities);
                        removingResult += db.SaveChanges();
                        if (removingResult > 0)
                        {
                            Console.WriteLine("CountryLocation for {0} has been cleaned with {1} records", postfixLocalCode, removingResult);
                        }
                    }
                    else // First uploading
                    {
                        removingResult += 1;
                    }
                }

                if (removingResult > 0)
                {
                    // Populate CountryLocation table
                    using (var db = new ApplicationContext())
                    {
                        foreach (var item in items)
                        {
                            // I use simplest way to update data and well it works )))
                            // There are different approaches how to sync data...
                            // Only for demonstration perprse I use the approach )))
                            var n = new CountryLocation
                            {
                                id = Guid.NewGuid(),
                                geoname_id = item.geoname_id,
                                locale_code = item.locale_code,
                                continent_code = item.continent_code,
                                continent_name = item.continent_name,
                                country_iso_code = item.country_iso_code,
                                country_name = item.country_name,
                                is_in_european_union = item.is_in_european_union
                            };

                            db.CountryLocation.Add(n);
                        }

                        var insertingResult = db.SaveChanges();
                        if (insertingResult > 0)
                        {
                            Console.WriteLine("CountryLocation has been updated with {0} records", insertingResult);
                        }
                    }
                }
            }
        }



    }
}
