using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace BigDataHackDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            var reader = new StreamReader(File.OpenRead(@"C:\users\latra\desktop\usersProjects.csv"));
            var data = readData(reader);
            var links = countLinks(data);
            var sortedLinks = sortLinks(links);
        }
    
        public static Dictionary<string, Dictionary<string, int>> sortLinks(Dictionary<string, Dictionary<string, int>> links)
        {
            Dictionary<string, Dictionary<string, int>> sortedLinks = new Dictionary<string, Dictionary<string, int>>();
            foreach (var country in links.Keys)
            {
                Dictionary<string, int> linkCounts = new Dictionary<string, int>();
                linkCounts = links[country];

                var sorted = linkCounts.Select(d => d).OrderByDescending(d => d.Value).ToDictionary(k => k.Key, e => e.Value);
                sortedLinks.Add(country,sorted);
            }

            return sortedLinks;
        }
        public static Dictionary<string, Dictionary<string, int>> countLinks(Dictionary<string, List<string>> data)
        {
            Dictionary<string, Dictionary<string, int>> links = new Dictionary<string, Dictionary<string, int>>();
            foreach (var key in data.Keys)
            {
                var countryList = data[key]; //list of strings (countries)

                var numCountries = countryList.Count;
                for(int i=0; i < numCountries; i++)
                {
                    var country1 = countryList[i];
                    for(int j=0; j < numCountries; j++)
                    {
                        var country2 = countryList[j];

                        if (!country1.Equals(country2))
                        {
                            Dictionary<string, int> linkCounts = new Dictionary<string, int>();
                            if (links.ContainsKey(country1))
                            {
                                linkCounts = links[country1];

                                if (linkCounts.ContainsKey(country2))
                                {
                                    linkCounts[country2]++;
                                }
                                else
                                {
                                    linkCounts.Add(country2, 1);
                                }
                            }
                            else
                            {
                                linkCounts.Add(country2, 1);
                                links.Add(country1, linkCounts);
                            }
                        }
                    }
                }
            }

            return links;
        }


        public static Dictionary<string, List<string>> readData(StreamReader reader) 
        {
            Dictionary<string, List<string>> data = new Dictionary<string, List<string>>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                var repoID = values[1];
                var country = values[2];
                country = country.Replace("\"", "");

                List<string> countries;
                if (!data.TryGetValue(repoID, out countries))
                {
                    countries = new List<string>();
                    data[repoID] = countries;
                }
                countries.Add(country);
            }
            return data;
        }
    }
}
