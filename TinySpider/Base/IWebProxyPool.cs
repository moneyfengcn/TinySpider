using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace TinySpider.Base
{
    /// <summary>
    /// WebProxy服务器池
    /// </summary>
    public interface IWebProxyPool
    {
        /// <summary>
        /// 从池中取一个可用的代理
        /// </summary>
        /// <returns></returns>
        IWebProxy GetProxy();

    }
}
