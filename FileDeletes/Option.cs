using System;

namespace FileDeletes
{
    /// <summary>
    /// 表示删除文件选项
    /// </summary>
    public class Option
    {
        /// <summary>
        /// 指定文件夹
        /// </summary>
        public string FolderPath { get; set; }

        /// <summary>
        /// 轮训间隔时间
        /// </summary>
        public TimeSpan Interval { get; set; } = TimeSpan.FromDays(1d);

        /// <summary>
        /// 过滤条件通配符
        /// </summary>
        public string Filter { get; set; } = "*";

        /// <summary>
        /// 指定天数之前
        /// </summary>
        public int DaysBefore { get; set; } = 30;
    }
}
