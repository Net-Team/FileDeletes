using System.Collections.Generic;
using Topshelf;

namespace FileDeletes
{
    /// <summary>
    /// 表示文件删除服务
    /// </summary>
    class FileDeletesControl : ServiceControl
    {
        private List<FileDeleter> filesDeleters;

        /// <summary>
        /// 开始执行
        /// </summary>
        /// <param name="hostControl"></param>
        /// <returns></returns>
        public bool Start(HostControl hostControl)
        {
            var config = Config.Load();
            this.filesDeleters = new List<FileDeleter>();
            foreach (var item in config.Options)
            {
                var filesDeleter = new FileDeleter(item);
                filesDeleters.Add(filesDeleter);
            }
            return true;
        }

        /// <summary>
        /// 停止执行
        /// </summary>
        /// <param name="hostControl"></param>
        /// <returns></returns>
        public bool Stop(HostControl hostControl)
        {
            foreach (var item in this.filesDeleters)
            {
                item.Dispose();
            }
            this.filesDeleters.Clear();
            return true;
        }
    }
}
