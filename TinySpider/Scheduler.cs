using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TinySpider.Base;

namespace TinySpider
{
    public class Scheduler : IScheduler, IDisposable
    {
        private const int Number_Of_Threads = 20;//默认20线程

        private Semaphore m_Semaphore;

        private volatile int TaskCount = 0;
        private IUrlStore UrlStore;


        public Scheduler(IUrlStore urlStore)
        {
            UrlStore = urlStore;
        }

        /// <summary>
        /// 工作线程配置
        /// </summary>
        public int WorkerThreads { get; set; } = Number_Of_Threads;


        public void PushUrl(Uri url)
        {
            UrlStore.AddUrl(url);
        }


        public void Run(Action<Uri> callback)
        {
            m_Semaphore = new Semaphore(WorkerThreads, WorkerThreads);

            while (GetNextUrl(out var url) || TaskCount > 0)
            {
                if (url != null)
                {
                    m_Semaphore.WaitOne();
                    Interlocked.Increment(ref TaskCount);
                    Task.Run(() => DoTask(url, callback));
                }
                else
                {
                    Thread.Sleep(1 * 1000);
                }
            }
        }



        private void DoTask(Uri url, Action<Uri> callback)
        {
            try
            {
                callback(url);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                m_Semaphore.Release();

                Interlocked.Decrement(ref TaskCount);
            }
        }

        private bool GetNextUrl(out Uri url)
        {
            url = UrlStore.GetUrl();
            return url != null;
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
                    if (m_Semaphore != null)
                    {
                        m_Semaphore.Dispose();
                        m_Semaphore = null;
                    }
                }

                // TODO: 释放未托管的资源(未托管的对象)并替代终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~Scheduler()
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
