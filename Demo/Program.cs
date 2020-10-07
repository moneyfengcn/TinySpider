using System;
using System.Text;
using TinySpider;

namespace Demo
{
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
}
