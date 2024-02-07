using System.IO;

namespace LG
{
    /// <summary>
    /// 文件类工具
    /// </summary>
    public static class FilesExtend
    {
        /// <summary>
        /// 判断文件或者路径存在
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool IsKeepFileOrDirectory(string Path)
        {
            if (Directory.Exists(Path))
            {
                return true;
            }
            else
            {
                return File.Exists(Path);
            }
        }
        /// <summary>
        /// 读取文件到字符串
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadFileToStr(string Path)
        {
            if (File.Exists(Path))
            {
                return File.ReadAllText(Path);
            }
            else
            {
                Log.Error("文件不存在:" + Path);
                return string.Empty;
            }
        }
        /// <summary>
        /// 写入文件到目标目录
        /// </summary>
        /// <param name="_FilePath"></param>
        /// <param name="_WriteStr"></param>
        public static void WriteStrToFile(string Path, string Str)
        {
            string _Directory = Path.Substring(0, Path.LastIndexOf('/'));
            if (!Directory.Exists(_Directory))
            {
                Directory.CreateDirectory(_Directory);
            }
            FileStream fs = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(Str);
            sw.Flush();
            sw.Close();
            fs.Close();
        }
        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="targetPath"></param>
        public static void CopyFile(string filePath, string targetPath)
        {
            string targetDir = Path.GetDirectoryName(targetPath);
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }
            System.IO.File.Copy(filePath, targetPath, true);
        }

        /// <summary>
        /// 清除非指定后缀的文件
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="_Suffix"></param>
        public static void ClearDirFile(string srcPath, string[] _Suffix)
        {
            DirectoryInfo dir = new DirectoryInfo(srcPath);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
            foreach (FileSystemInfo i in fileinfo)
            {
                if (i is DirectoryInfo)            //判断是否文件夹
                {
                    ClearDirFile(i.FullName, _Suffix);
                    DirectoryInfo tmpdir = new DirectoryInfo(i.FullName);
                    FileSystemInfo[] tmpfileinfo = tmpdir.GetFileSystemInfos();
                    if (tmpfileinfo.Length == 0)
                    {//此目录下已经不存在文件 删除此目录
                        Directory.Delete(i.FullName);
                    }
                }
                else
                {
                    bool IsClear = true;
                    for (int n = 0; n < _Suffix.Length; n++)
                    {
                        if (i.FullName.EndsWith(_Suffix[n]))
                        {
                            IsClear = false;
                            break;
                        }
                    }

                    if (IsClear)
                        File.Delete(i.FullName);      //删除指定文件
                }
            }
        }

        /// <summary>
        /// 清除文件夹下子文件
        /// </summary>
        /// <param name="srcPath"></param>
        public static void ClearDirectory(string srcPath)
        {
            if (Directory.Exists(srcPath))
            {
                DirectoryInfo dir = new DirectoryInfo(srcPath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)            //判断是否文件夹
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        subdir.Delete(true);          //删除子目录和文件
                    }
                    else
                    {
                        File.Delete(i.FullName);      //删除指定文件
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(srcPath);
            }
        }
    }

}