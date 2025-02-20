using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    AfterDefenceGame
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get => instance; }

    private void Awake()
    {
        if (instance != null) Destroy(this);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// Start
    /// </summary>
    [Header("State")]
    private State state;
    public State State { get => state; set => state = value; }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
