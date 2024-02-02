using System.Security.Cryptography.X509Certificates;

namespace LG
{

    public static class Log
    {
        public static void Exception(string msg)
        {
            throw new Exception(msg);
        }
        public static void Error(string msg)
        {
            UnityEngine.Debug.LogError(msg);
        }

        public static void Debug(string msg)
        {
            UnityEngine.Debug.LogError(msg);
        }

    }
}