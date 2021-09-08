using System;
using UnityEngine;

public class DebugButtons : MonoBehaviour
{
    private static DebugButtons instance;
    [SerializeField] private KeyCode[] KeyCodes = new KeyCode[0];

    public static Action<KeyCode> OnKeyDown;
    public static Action<KeyCode> OnKeyHeld;
    public static Action<KeyCode> OnKeyUp;

    #region Instance Stuff
    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
        transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }
    private void OnDestroy()
    {
        if (instance == this) instance = null;
    }
    #endregion

#if UNITY_EDITOR
    private void Update()
    {
        foreach (KeyCode key in KeyCodes)
        {
            if (Input.GetKeyDown(key)) OnKeyDown?.Invoke(key);
            if (Input.GetKey(key)) OnKeyHeld?.Invoke(key);
            if (Input.GetKeyUp(key)) OnKeyUp?.Invoke(key);
        }
    }
#endif

}