using System;
using System.Collections.Generic;
using System.Text;

namespace TinySpider.Base
{
    /// <summary>
    /// 页面下载器接口
    /// </summary>
    public interface IDownloader
    {
        string RequestUrl(Uri url);
    }
}
