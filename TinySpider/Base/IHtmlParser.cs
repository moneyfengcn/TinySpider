using System;
using System.Collections.Generic;
using System.Text;

namespace TinySpider.Base
{
    /// <summary>
    /// html源页面解析接口
    /// </summary>
    public interface IHtmlParser
    {
        /// <summary>
        /// 解析HTML源码
        /// </summary>
        /// <param name="sourceUrl">HTML的来源地址</param>
        /// <param name="html">HTML内容</param>
        /// <param name="links">输出给调度器的URL</param>
        /// <returns>返回HTML中提取出来的数据</returns>
        PageData Parse(Uri sourceUrl, string html, out List<Uri> links);
    }
}
