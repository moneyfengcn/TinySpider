using System;
using System.Collections.Generic;
using System.Text;
using TinySpider.Base;

namespace Demo
{
    class MyPipeline : IPipeline
    {
        public void FetchItem(PageData page)
        {
            var data = page.Data1;
            Console.WriteLine("接收到爬虫数据:" + page.SourceUrl);
        }
    }
}
