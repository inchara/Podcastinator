using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
//using System.Net.HttpWebRequest;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace AudioPlaybackAgent1
{
    public class UrlProcessor
    {
        public static string audioUrl = string.Empty;
        public static string videoUrl = string.Empty;

        public Regex httpMp3Regex = new Regex(@"(http).*?(mp3)", RegexOptions.Compiled);
        public Regex mp3Regex = new Regex(@"href(\s*)=(\s*)\"".*?\.(mp3)", RegexOptions.Compiled);
        public Regex stripHref = new Regex(@"[^(href(\s*)=(\s*)\"")].*?\.(mp3)", RegexOptions.Compiled);

        public Regex ted1500kRegex = new Regex(@"mp4:.*?\.mp4", RegexOptions.Compiled);
        public delegate List<string> delGetLinks(string url);

        /*
        static void Main(string[] args)
        {
            UrlProcessor obj = new UrlProcessor();
            //string content = obj.Convert("http://msdn.microsoft.com/en-us/library/windows/hardware/hh162931.aspx");
            //string content = obj.Convert("http://www.npr.org/player/v2/mediaPlayer.html?action=1&t=1&islist=false&id=331332721&m=332366798&live=1");
            string content = //@"http://www.vedantany.org/prayer-chanting/khandana-bhava-bandhana.html";
                             //@"http://www.stephaniequinn.com/samples.htm";
                               @"http://www.ted.com/talks/ze_frank_are_you_human";

            obj.GetMp4FileLink(content);
            //System.IO.StreamWriter s = new StreamWriter(@"C:\Users\arsinha\Documents\Visual Studio 2013\Projects\Podcastinator_Arnab\Podcastinator_Arnab\test.txt", false);
            //s.WriteLine(content);
            //getYoutubeDirectLink(@"oo8NPFIf1Mg"); // https://www.youtube.com/watch?v=oo8NPFIf1Mg 
        }
        */
        public string htmlContent;
        public string url;
        public List<string> listOfUrls;
        public UrlProcessor(string url)
        {
            listOfUrls = new List<string>();
            this.url = url;
        }

        public string GetHtmlFromUrl()
        {
            System.Uri targetUri = new System.Uri(url);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(targetUri);
            var response = Task.Factory.FromAsync((cb, o) => ((HttpWebRequest)o).BeginGetResponse(cb, o), res => ((HttpWebRequest)res.AsyncState).EndGetResponse(res), request);
            var result = response.Result;
            var responseStream = result.GetResponseStream();
            string resultString;
            using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
            {
                resultString = reader.ReadToEnd();
            }
            return resultString;
        }

        public void GetMp4FileLink()
        {
            string htmlContent = this.GetHtmlFromUrl();

            /* Get the domain matched */
            string domain = GetDomain(url);
            if (domain.Contains(@"ted.com"))
            {
                string ted1500k = ted1500kRegex.Match(htmlContent).Value.ToString();
                string relPath = ted1500k.Substring("mp4:".Length);
                string newUrl = "http://video.ted.com/" + relPath;
                listOfUrls.Add(newUrl);
            }
        }

        public void GetText()
        {
            string htmlContent = this.GetHtmlFromUrl();
        }
        public void GetMp3FileLink()
        {
            string htmlContent = this.GetHtmlFromUrl();

            /* Match for links like: "http://vedantany.squarespace.com/storage/emusic/khandana%20bhava%20bandhana.mp3" */
            foreach (Match match in httpMp3Regex.Matches(htmlContent))
            {
                string newUrl = match.Value.ToString();
                if (!listOfUrls.Contains(newUrl))
                {
                    listOfUrls.Add(newUrl);
                }
            }

            /* Match for relative paths like: "Music/Canon.mp3" */
            foreach (Match match in mp3Regex.Matches(htmlContent))
            {
                string newUrl = string.Empty;
                string link = match.Value.ToString();
                string relPath = stripHref.Match(link).Value.ToString();

                /* Careful about the last '/' if it is there. */
                if (url.EndsWith("/"))
                {
                    newUrl = url + "../" + relPath;
                }
                else
                {
                    newUrl = url + "/../" + relPath;
                }

                if (!listOfUrls.Contains(newUrl))
                {
                    listOfUrls.Add(newUrl);
                }
            }

            //listOfUrls.Add("http://vedantany.squarespace.com/storage/emusic/khandana%20bhava%20bandhana.mp3");
            //return listOfUrls;
        }

        private string GetDomain(String http)
        {
            if (string.IsNullOrEmpty(http))
                return "";
            if (!(http.StartsWith("http:") || http.StartsWith("https:")))
                return "";
            try
            {
                Uri u = new Uri(http);
                return u.Host;
            }
            catch (Exception e) { return ""; }
        }
        


        public string Convert(string path)
        {
            WebClient wc = new WebClient();
            HtmlDocument doc = new HtmlDocument();
            doc.Load(wc.OpenRead(path));

            StringWriter sw = new StringWriter();
            ConvertTo(doc.DocumentNode, sw);
            sw.Flush();
            return sw.ToString();
        }

        public void ConvertTo(HtmlNode node, TextWriter outText)
        {
            string html;
            switch (node.NodeType)
            {
                case HtmlNodeType.Comment:
                    // don't output comments
                    break;

                case HtmlNodeType.Document:
                    ConvertContentTo(node, outText);
                    break;

                case HtmlNodeType.Text:
                    // script and style must not be output
                    string parentName = node.ParentNode.Name;
                    if ((parentName == "script") || (parentName == "style"))
                        break;

                    // get text
                    html = ((HtmlTextNode)node).Text;

                    // is it in fact a special closing node output as text?
                    if (HtmlNode.IsOverlappedClosingElement(html))
                        break;

                    // check the text is meaningful and not a bunch of whitespaces
                    if (html.Trim().Length > 0)
                    {
                        outText.Write(HtmlEntity.DeEntitize(html));
                    }
                    break;

                case HtmlNodeType.Element:
                    switch (node.Name)
                    {
                        case "p":
                            // treat paragraphs as crlf
                            outText.Write("\r\n");
                            break;
                    }

                    if (node.HasChildNodes)
                    {
                        ConvertContentTo(node, outText);
                    }
                    break;
            }
        }

        private void ConvertContentTo(HtmlNode node, TextWriter outText)
        {
            foreach (HtmlNode subnode in node.ChildNodes)
            {
                ConvertTo(subnode, outText);
            }
        }

    }
}
