using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ScrapMsiaData
{
    class Program
    {
        static void Main(string[] args)
        {
            // Start scraping.
            StartScrap();
            Console.ReadLine();
        }

        /// <summary>
        /// Initiate scraping
        /// </summary>
        static async void StartScrap()
        {
            // Set the main page.
            string page = "http://www.data.gov.my/data/en_US/dataset";

            // Get the main page.
            string result = await DownloadPageAsync(page);

            // Get number of pages.
            int totalPage = GetTotalPage(result);

            // Get page information.
            List<List<Page>> pageInfo = await GetPageInfoAsync(totalPage);

            //// Construct the xml structure
            //string xmlStructure = Construct(pageInfo);

        }

        /// <summary>
        /// Download page
        /// </summary>
        /// <param name="url"></param>
        /// <returns>Return the html doc of the given url</returns>
        static async Task<string> DownloadPageAsync(string url)
        {
            try
            {
                // Use HttpClient.
                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(url))
                using (HttpContent content = response.Content)
                {
                    // Return the result.
                    return await content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DownloadPageAsync(): " + ex.Message.ToString());
                return String.Empty;
            }
        }

        /// <summary>
        /// Get the total number of page
        /// </summary>
        /// <param name="page"></param>
        /// <returns>Total number of page</returns>
        static int GetTotalPage(string page)
        {
            try
            {
                // Declare html document.
                HtmlDocument htmlDoc = new HtmlDocument();

                // Load html document.
                htmlDoc.LoadHtml(page);

                // Take the desired nodes collection.
                HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='pagination pagination-centered']//ul//li");

                // Always take the fourth li element.
                return Convert.ToInt16(nodes.ElementAt(4).InnerText);

            }
            catch (Exception ex)
            {
                Console.WriteLine("GetTotalPage(): " + ex.Message.ToString());
                return 0;
            }
        }

        /// <summary>
        /// Get page information
        /// </summary>
        /// <param name="totalPage"></param>
        /// <returns></returns>
        static async Task<List<List<Page>>> GetPageInfoAsync(int totalPage)
        {
            try
            {
                // Create a list list of page.
                List<List<Page>> pagesList = new List<List<Page>>();
                
                // Loop through the number of pages.
                for (int i = 1; i <= totalPage; i++)
                {
                    // Download page.
                    string result = await DownloadPageAsync(String.Format("http://www.data.gov.my/data/en_US/dataset?page={0}", i));

                    // Append list of page into list list of page.
                    pagesList.Add(GetPageInfo(result));

                    // Debug
                    Console.WriteLine("Current page: " + i.ToString());
                }

                // Return list list of pages.
                return pagesList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetPageInfoAsync(): " + ex.Message.ToString());
                return null;
            }
        }

        /// <summary>
        /// Get individual page information
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        static List<Page> GetPageInfo(string html)
        {
            try
            {
                List<Page> pageList = new List<Page>();
                HtmlDocument htmlDoc = new HtmlDocument();

                // Set prefix url
                string pagePrefix = "http://www.data.gov.my";

                // Load html 
                htmlDoc.LoadHtml(html);

                // Get data contents.
                HtmlNodeCollection listContent = htmlDoc.DocumentNode.SelectNodes("//ul[@class='dataset-list unstyled']//li[@class='dataset-item']");

                // Loop through contents.
                foreach (HtmlNode content in listContent)
                {
                    Page page = new Page();
                    string url = String.Empty;               
                    List<Resources> resourceList = new List<Resources>();
                    
                    //Console.WriteLine(content.InnerText.Trim());

                    // Get title.
                    page.Title = content.SelectSingleNode(".//h3[@class='dataset-heading']//a").InnerText;

                    // Get url.
                    url = content.SelectSingleNode(".//h3[@class='dataset-heading']//a[@href]").GetAttributeValue("href", String.Empty);

                    // Get title url.
                    page.UrlTitle = pagePrefix + url;

                    // Get date created.
                    page.DateCreated = content.SelectSingleNode(".//div[@class='col-sm-12 mampu-dt-org']//div//span").InnerText;

                    // Get organization.
                    page.Org = content.SelectSingleNode(".//div[@class='col-sm-12 mampu-dt-org']//div//a[@href]").InnerText;

                    // Get organization url.
                    page.UrlOrg = pagePrefix + content.SelectSingleNode(".//div[@class='col-sm-12 mampu-dt-org']//div//a[@href]").GetAttributeValue("href", String.Empty);

                    // Get description.
                    page.Desc = content.SelectSingleNode(".//div[@class='dataset-content content-result']//div[@style='margin-top:5%;']").InnerText;

                    // Get list of resources.
                    HtmlNodeCollection listResources = content.SelectNodes(String.Format(".//ul[@class='dataset-resources unstyled ']//li//a[@href='{0}']",url));

                    // Loop through resources.
                    foreach (HtmlNode resources in listResources)
                    {
                        Resources resource = new Resources();

                        // Get resource type.
                        resource.Type = resources.InnerText;

                        // Append resource into list of resource.
                        resourceList.Add(resource);
                    }

                    // Append page into list of page.
                    pageList.Add(page);                    

                    // Debug.
                    Console.WriteLine("Title: " + page.Title);

                    // Clear.
                    page = null;
                }

                // Return list of page.
                return pageList;

            }
            catch (Exception ex)
            {
                Console.WriteLine("GetPageInfo(): " + ex.Message.ToString());
                return null;
            }
        }

        //static string Construct(List<List<Page>> pageInfo)
        //{
        //    try
        //    {
        //        string info = String.Empty;

        //        foreach(List<Page> pages in pageInfo)
        //        {
        //            foreach (Page page in pages)
        //            {
        //                page.

        //            }

        //        }

        //        return info;

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Construct: " + ex.Message.ToString());
        //        return String.Empty;
        //    }

        //}





    }
}