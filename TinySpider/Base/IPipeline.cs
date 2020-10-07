using System;
using System.Collections.Generic;
using System.Text;

namespace TinySpider.Base
{
    /// <summary>
    /// 数据处理管线
    /// </summary>
    public interface IPipeline
    {
        void FetchItem(PageData page);
    }
}
