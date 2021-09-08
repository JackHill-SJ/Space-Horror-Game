using UnityEngine;

public static class Console
{
    public static void Log(string log)
    {
#if UNITY_EDITOR
        Debug.Log(log);
#endif
    }
    public static void LogWarning(string log)
    {
#if UNITY_EDITOR
        Debug.LogWarning(log);
#endif
    }
    public static void LogError(string log)
    {
#if UNITY_EDITOR
        Debug.LogError(log);
#endif
    }
    public static void LogForcePause(string log)
    {
#if UNITY_EDITOR
        Debug.LogError(log);
        Debug.LogError("Force Pausing...");
        UnityEditor.EditorApplication.isPaused = true;
#endif
    }
    public static void LogForceQuit(string log)
    {
#if UNITY_EDITOR
        Debug.LogError(log);
        Debug.LogError("Force Quitting...");
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}