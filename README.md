# TinySpider
## 轻量级的网页爬虫框架

这是一个轻量级的多线程网页爬虫框架。它仅仅封装了基本的任务调度功能。本身不具备HTML代码解析功能，这需要使用者自行实现或选用一些第三方的HTML解析库。

TinySpider框架具备下以特点：
* 轻量级，源代码量非常小，框架本身不依赖其它第三方组件。
* 最小限度封装，不涉及到HTTP通信和HTML解析，虽然IDownloader下载器默认实现采用了dotnet自带的WebClient。
* 半成品，它不能在你的代码里直接new出来用。你必须实现一个IHtmlParse解析器和IPipeline管道。
* 开放式,本框架的全部接口部件均可由使用者自行实现和替换。
  

框架部件定义:
  * IScheduler:         任务调度器,负责对线程和URL任务进行分配调度。
  * IUrlStore:          URL存放器,负责管理需要爬的URL。
  * IDownloader:        下载器,负责下载目标URL的HTML源页面。
  * IHtmlParser:        解析器,负责解析HTML页面代码,从中提取出需要爬的URL,以及页面上你关注的内容。(必须由使用者实现)
  * IPipeline:          处理管线,负责接收解析器提取出来的内容。(必须由使用者实现)
  * IWebProxyPool:      代理服务器池,负责管理和提供Web代理服务器。


***

# 使用例程：
```cs
    /// <summary>
    /// 实现HTML解析接口
    /// 本例程采用HtmlAgilityPack库对html进行解析  你也可以用正规表达式或其它一些HTML处理库
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


    /// <summary>
    /// 实现一个IPipeline
    /// </summary>
    class MyPipeline : IPipeline
    {
        public void FetchItem(PageData page)
        {
            Data data =(Data)page.Data1;
            Console.WriteLine("接收到爬虫数据:" + page.SourceUrl);
        }
    }
```

运行


```cs
  class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello TinySpider!");
            //入口点
            var entryUrl = new Uri("https://www.sohu.com/");

            //目标网站文本编码
            var web_encode = Encoding.UTF8;
            //爬虫的并发线程
            var threads = 40;

            var spider = new TinySpider.TinySpider(
                    new Scheduler(new UrlStore()) { WorkerThreads = threads },
                    new Downloader() { Encoding = web_encode },
                    new MyHtmlParser(),
                    new MyPipeline()
                );

            //启动爬虫  Run()方法会一直阻塞至所有任务完成
            spider.Run(entryUrl);

            Console.WriteLine("\r\n\r\n");
            Console.WriteLine("Press Any Key To Exit...");
            Console.ReadKey();
        }
    }

```