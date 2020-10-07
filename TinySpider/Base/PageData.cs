using System;
using System.Collections.Generic;
using System.Text;

namespace TinySpider.Base
{
    public class PageData
    {
        /// <summary>
        /// 源地址
        /// </summary>
        public Uri SourceUrl { get; set; }
        /// <summary>
        /// 页面源代码
        /// </summary>
        public string Html { get; set; } 

        /// <summary>
        /// 解析出来的目标数据项
        /// </summary>
        public object Data1 { get; set; }
        public object Data2 { get; set; }
    }
}
