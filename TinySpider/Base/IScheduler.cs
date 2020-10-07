using System;
using System.Collections.Generic;
using System.Text;

namespace TinySpider.Base
{
    /// <summary>
    /// 调度器接口
    /// </summary>
    public interface IScheduler
    {
        void PushUrl(Uri url);

        void Run(Action<Uri> callback);

      
    }
}
