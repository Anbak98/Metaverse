using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatHandler : MonoBehaviour
{
    [Range(0, 100)] [SerializeField] private int health = 0;
    public int Health
    {
        get => health;
        set => health = Mathf.Clamp(value, 0, 100);
    }


    [Range(1f, 20f)][SerializeField] private float speed = 3f;
    public float Speed
    {
        get => speed;
        set => speed = Mathf.Clamp(value, 1f, 20f);
    }
}
