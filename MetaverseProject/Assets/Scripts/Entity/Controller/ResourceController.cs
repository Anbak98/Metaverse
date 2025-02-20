using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    private BaseController baseController;
    private StatHandler statHandler;

    public float CurrentHealth { get; private set; }
    public float MaxHealth => statHandler.Health;

    private void Awake()
    {
        baseController = GetComponent<BaseController>();
        statHandler = GetComponent<StatHandler>();
    }

    private void Start()
    {
        CurrentHealth = statHandler.Health;
    }

    public bool ChangeHealth(float change)
    {
        CurrentHealth += change;
        CurrentHealth = CurrentHealth > MaxHealth ? MaxHealth : CurrentHealth;
        CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth;

        if (CurrentHealth <= 0f)
        {
            Death();
        }

        return true;
    }

    private void Death()
    {
        DefenceGameManager.Instance.score.Value++;
        gameObject.SetActive(false);
        CurrentHealth = MaxHealth;
    }
}
