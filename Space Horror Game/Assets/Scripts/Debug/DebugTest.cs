using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTest : MonoBehaviour
{
    private void Awake()
    {
        DebugButtons.OnKeyHeld -= OnPressA;
        DebugButtons.OnKeyHeld += OnPressA;
        DebugButtons.OnKeyHeld -= OnPressD;
        DebugButtons.OnKeyHeld += OnPressD;
        DebugButtons.OnKeyHeld -= OnPressAny;
        DebugButtons.OnKeyHeld += OnPressAny;
    }
    private void OnDestroy()
    {
        DebugButtons.OnKeyHeld -= OnPressA;
        DebugButtons.OnKeyHeld -= OnPressD;
        DebugButtons.OnKeyHeld -= OnPressAny;
    }

    void OnPressA(KeyCode key)
    {
        //if (key == KeyCode.A) Console.Log($"Pressed {key}.");
    }
    void OnPressD(KeyCode key)
    {
        //if (key == KeyCode.D) Console.Log($"Pressed {key}.");
    }
    void OnPressAny(KeyCode key)
    {
        //Console.Log($"Any Pressed {key}.");
    }
}