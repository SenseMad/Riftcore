using UnityEngine;

namespace Riftcore.Infrastructure.Logging
{
    public static class GameLog
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        public static void Log(string message)
        {
            Debug.Log(message);
        }

        public static void Warning(string message)
        {
            Debug.LogWarning(message);
        }

        public static void Error(string message)
        {
            Debug.LogError(message);
        }
#else
        public static void Log(string message) { }
        public static void Warning(string message) { }
        public static void Error(string message) { }
#endif
    }
}
