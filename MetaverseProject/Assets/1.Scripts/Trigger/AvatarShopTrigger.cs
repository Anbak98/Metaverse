using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class AvatarShopNPC : MonoBehaviour 
{
    UIManager uiManager;
    private void Start()
    {
        uiManager = FindFirstObjectByType<UIManager>();
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.Z))
        {
            uiManager.ChangeState(UIState.AvatarShop);
            Debug.Log("sS");
        }
    }
}
