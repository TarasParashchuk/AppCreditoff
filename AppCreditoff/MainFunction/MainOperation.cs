using System;
using System.Linq;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AppCreditoff.Model;
using Newtonsoft.Json;

namespace AppCreditoff.MainFunction
{
    public class MainOperation
    {
        private const string main_url = @"https://dengi-bistro.com.ua/mobile.html";
        private const string logo_url = @"https://dengi-bistro.com.ua/";
        private HelpFunction helpFunction;

        public MainOperation()
        {
            helpFunction = new HelpFunction();
        }

        public List<Model_Creditoff> Sort_Sum()
        {
            return helpFunction.Open_Favorites().OrderByDescending(u => u.suma).ToList();
        }

        public List<Model_Creditoff> Sort_Rating()
        {
            return helpFunction.Open_Favorites().OrderByDescending(u => u.logo_count_star).ToList();
        }

        public List<Model_Creditoff> Sort_Time()
        {
            var list_time = new List<ModelTime>();
            var List_company = new List<Model_Creditoff>();

            var list = helpFunction.Open_Favorites();
            var count = 0;

            foreach (var items in list)
            {
                var date = items.time_credit.Split(' ');
                var num_date = Convert.ToDouble(date[0]);
                var str_date = date[1];
                var strok = 0.0;

                switch (str_date[0])
                {
                    case 'д': strok = num_date; break;
                    case 'м': strok = num_date * 30; break;
                    case 'л': strok = num_date * 365; break;
                };
                var data = new ModelTime(count, strok, str_date);
                list_time.Add(data);
                count++;
            }
            var list_time_sorted = list_time.OrderByDescending(u => u.time);

            foreach (var items in list_time_sorted)
                List_company.Add(list[items.id]);

            return List_company;
        }

        public void Updated()
        {
            var information_credit = new string[5];
            var company_name = string.Empty;
            var url = string.Empty;
            var company_logo = logo_url;
            var count_main = 0;
            var List_company = new List<Model_Creditoff>();

            var web = new HtmlWeb();
            var doc = web.Load(main_url);

            var node = doc.DocumentNode.SelectNodes("//div[@class='company']");

            foreach (var main_inf in node)
            {
                company_logo = logo_url + main_inf.SelectSingleNode("//img[@class='company_logo']").Attributes["src"].Value;
                company_name = main_inf.SelectSingleNode("//p[@class='comp_name']").InnerText;
                url = main_inf.SelectSingleNode("//a[@class='getcredit']").Attributes["data-href"].Value;

                var index = company_name.LastIndexOf(" ");
                var count = 0;

                if (index != -1)
                    company_name = company_name.Insert(index + 1, "\n");

                var inf_credit = doc.DocumentNode.SelectSingleNode("//div[2]/div[1]/div/div/div/div/div/div[2]/div[1]");

                for (var i = 0; i < inf_credit.ChildNodes.Count / 2; i++)
                {
                    try
                    {
                        information_credit[i] = inf_credit.SelectSingleNode("//p[@class='info_text']").InnerText.Trim();
                        inf_credit.SelectSingleNode("//p[@class='info_text']").Remove();
                    }
                    catch
                    {
                        information_credit[i] = string.Empty;
                    }
                }

                var str = doc.GetElementbyId("pjax-landing").SelectSingleNode("//div[@class='company_stars']").InnerHtml;
                foreach (Match m in Regex.Matches(str, "fa fa-star star_full")) count++;
                information_credit[4] = doc.GetElementbyId("pjax-landing").SelectSingleNode("//div[@class='comp_message']").InnerText;
                var sum = Convert.ToInt32(information_credit[0].Remove(information_credit[0].LastIndexOf("г")).TrimEnd());

                var items = new Model_Creditoff(count_main, company_logo, company_name, sum, information_credit[1], information_credit[2], information_credit[3], information_credit[4], count, url);
                List_company.Add(items);

                Array.Clear(information_credit, 0, information_credit.Length);
                main_inf.SelectSingleNode("//div[@class='company']").Remove();

                count_main++;
            }

            var json_file = JsonConvert.SerializeObject(List_company);
            System.IO.File.WriteAllText(OpenActivity.file_name, json_file);
        }
    }
}
