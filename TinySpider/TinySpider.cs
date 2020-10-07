using System;
using System.Collections.Generic;
using System.Text;
using TinySpider.Base;

namespace TinySpider
{
    public class TinySpider
    {
        private IScheduler Scheduler;
        private IDownloader Downloader;
        private IHtmlParser HtmlParser;
        private IPipeline Pipeline;

        public TinySpider(IScheduler scheduler, IDownloader downloader, IHtmlParser htmlParser, IPipeline pipeline)
        {
            Scheduler = scheduler;
            Downloader = downloader;
            HtmlParser = htmlParser;
            Pipeline = pipeline;
        }



        public void Run(Uri entryUrl)
        {
            Action<Uri> callback = new Action<Uri>(TaskCallback);

            Scheduler.PushUrl(entryUrl);

            Scheduler.Run(callback);
        }

        /// <summary>
        /// 调度任务
        /// </summary>
        /// <param name="url"></param>
        protected void TaskCallback(Uri url)
        {
            //下载页面
            var html = Downloader.RequestUrl(url);

            //解析页面
            var data = HtmlParser.Parse(url, html, out var links);

            if (links != null)
            {
                //将解析器输出的url压入调度器
                foreach (var item in links)
                {
                    Scheduler.PushUrl(item);
                }
            }

            data.SourceUrl = url;
            data.Html = html;

            //将解析页面得到的数据库传给管线
            if (data != null) Pipeline.FetchItem(data);
        }
    }
}
