using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TinySpider.Base;

namespace TinySpider
{
    public class UrlStore : IUrlStore, IDisposable
    {
        private HashSet<string> m_urls = new HashSet<string>();
        private Queue<Uri> m_taskUrl = new Queue<Uri>();


        public void AddUrl(Uri url)
        {
            var szUrl = url.AbsoluteUri;
            lock (m_urls)
            {
                if (!m_urls.Contains(szUrl))
                {
                    m_urls.Add(szUrl);
                    m_taskUrl.Enqueue(url);
                }
            }
        }

        public Uri GetUrl()
        {
            Uri url = null;

            lock (m_urls)
            {
                if (m_taskUrl.Count > 0)
                {
                    url = m_taskUrl.Dequeue();
                }
            }
            return url;
        }

        #region IDisposable接口实现
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                }
                
                // TODO: 释放未托管的资源(未托管的对象)并替代终结器
                // TODO: 将大型字段设置为 null
                m_urls.Clear();
                m_urls = null;

                m_taskUrl.Clear();
                m_taskUrl = null;


                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~UrlStore()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
