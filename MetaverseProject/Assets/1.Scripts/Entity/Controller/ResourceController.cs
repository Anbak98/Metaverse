using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    private BaseController baseController;
    private StatHandler statHandler;
    private AvataHandler avataHandler;

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

    public void ChangeAvatar(Sprite sprite)
    {
        NetworkObject targetNetObj = gameObject.GetComponent<NetworkObject>();
        if (targetNetObj != null)
        {
            avataHandler.ChangeAvatar(targetNetObj.NetworkObjectId, sprite);
        }
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
