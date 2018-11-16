using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace FileDeletes
{
    /// <summary>
    /// 表示文件删除
    /// </summary>
    public class FileDeleter : IDisposable
    {
        /// <summary>
        /// 延时timer
        /// </summary>
        private readonly Timer timer;

        /// <summary>
        /// 结果匹配
        /// </summary>
        private readonly SuffixMatch suffixMatch;

        /// <summary>
        /// 当前选项
        /// </summary>
        private Option option;

        /// <summary>
        /// 日志
        /// </summary>
        private ILogger Logger;

        /// <summary>
        /// 是否释放
        /// </summary>
        private bool isDisposed = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option"></param>
        public FileDeleter(Option option)
        {
            this.Logger = new LoggerFactory().AddConsole().AddDebugger().CreateLogger(nameof(FileDeletes));
            this.option = option;
            this.suffixMatch = new SuffixMatch(option.Filter);
            this.timer = new Timer(this.Start, null, TimeSpan.Zero, Timeout.InfiniteTimeSpan);
        }

        /// <summary>
        /// 开始执行
        /// </summary>
        private void Start(object state)
        {
            try
            {
                var dir = new System.IO.DirectoryInfo(option.FolderPath);
                var files = dir.EnumerateFiles("*.*", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    if (this.isDisposed == true)
                    {
                        break;
                    }
                    if (this.suffixMatch.IsMatch(file.Name) && file.CreationTime.AddDays(option.DaysBefore) < DateTime.Now)
                    {
                        this.Logger.LogInformation($"{file.FullName} 删除成功.");
                        file.Delete();
                    }
                }

                if (this.isDisposed == false)
                {
                    this.timer.Change(option.Interval, Timeout.InfiniteTimeSpan);
                }
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex.ToString());
            }
        }


        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            this.isDisposed = true;
            this.timer.Dispose();
        }

        /// <summary>
        /// 表示结果匹配
        /// </summary>
        private class SuffixMatch
        {
            /// <summary>
            /// 正则
            /// </summary>
            private readonly Regex regex;

            /// <summary>
            /// 结果匹配
            /// </summary>
            /// <param name="pattern">匹配规则，*代表任意</param>
            public SuffixMatch(string pattern)
            {
                if (string.IsNullOrEmpty(pattern) == true)
                {
                    this.regex = new Regex(".*");
                }
                else
                {
                    this.regex = new Regex(Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\|", "|"), RegexOptions.IgnoreCase);
                }
            }

            /// <summary>
            /// 是否与规则匹配
            /// </summary>
            /// <param name="input">输入项</param>
            /// <returns></returns>
            public bool IsMatch(string input)
            {
                return this.regex.IsMatch(input);
            }
        }
    }
}
