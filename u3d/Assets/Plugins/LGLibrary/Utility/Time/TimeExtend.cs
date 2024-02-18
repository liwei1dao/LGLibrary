using System;

namespace LG
{
    /// <summary>
    /// 文件类工具
    /// </summary>
    public static class TimeExtend
    {
        ///获取当前时间戳 bflag 秒/毫秒
        public static long GetTimeStamp(bool bflag)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long ret = 0;
            if (bflag)
                ret = Convert.ToInt64(ts.TotalSeconds);
            else
                ret = Convert.ToInt64(ts.TotalMilliseconds);

            return ret;
        }
    }
}