using System;
using System.Collections.Generic;
using System.Text;
using TinySpider.Base;

namespace Demo
{
    /// <summary>
    /// 实现HTML解析接口
    /// 本例程采用HtmlAgilityPack库对html进行解析
    /// HtmlAgilityPack具体使用方法请自行google
    /// </summary>
    class MyHtmlParser : IHtmlParser
    {
        //实现html解析
        //TODO:页面上的相关超链放在Page的Links中，否则调度器将没有可用的url进行调度 
        public PageData Parse(Uri sourceUrl, string html, out List<Uri> links)
        {
            links = new List<Uri>();
            var data = new PageData();

            var doc = new HtmlAgilityPack.HtmlDocument();
            //加载html代码到文档对象
            doc.LoadHtml(html);

            //提取文档中的所有超链接
            var all_links = doc.DocumentNode.Descendants("a");
            foreach (var item in all_links)
            {
                if (item.Attributes.Contains("href"))
                {
                    //取到html标签中的href中的值，它不一定是个完整的url
                    string path = item.Attributes["href"].Value;
                    //借助Uri类对URL进行格式化整理
                    var uri = new Uri(baseUri: sourceUrl, relativeUri: path);


                    //限定一下uri范围
                    if (uri.AbsoluteUri.Contains("sohu.com/"))
                    {
                        //输出给调度器
                        links.Add(uri);
                    }
                }
            }

            //提取你关注的信息内容
            //寄放在Page对象的Data1、Data2中
            //后续在Pipeline管线中对Data1、Data2进行存储或其它处理
            data.Data1 = new Data()
            {
                Url = sourceUrl,
                HTML = html
            };


            return data;
        }
    }

    public class Data
    {
        public string HTML { get; set; }
        public Uri Url { get; set; }
    }
}
