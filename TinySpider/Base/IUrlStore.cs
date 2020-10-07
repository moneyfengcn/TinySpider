using System;
using System.Collections.Generic;
using System.Text;

namespace TinySpider.Base
{
    /// <summary>
    /// URL存储器访问接口
    /// </summary>
    public interface IUrlStore
    {
        void AddUrl(Uri url);
        Uri GetUrl();
    }
}
