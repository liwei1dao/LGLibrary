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
    }
}