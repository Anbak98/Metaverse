using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIState
{
    Home,
    Game,
    Score
}

public class UIManager : MonoBehaviour
{
    static UIManager instance;
    public static UIManager Instance => instance;

    private void Awake()
    {
        instance = this;
    }

    public void OnClickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
