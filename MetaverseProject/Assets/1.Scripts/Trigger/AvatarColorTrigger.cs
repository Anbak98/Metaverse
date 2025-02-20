using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class AvatarColorTrigger : NetworkBehaviour
{
    [SerializeField] GameObject obj;

    void OnTriggerStay2D(Collider2D collision)
    {
        obj.SetActive(true);
        if (collision.CompareTag("Player") && IsOwner && Input.GetKey(KeyCode.Z))
        {
            NetworkObject targetNetObj = collision.GetComponent<NetworkObject>();
            if (targetNetObj != null)
            {
                RequestChangeAvatarColorServerRpc(targetNetObj.NetworkObjectId);
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        obj.SetActive(false);
    }

    Sprite sprite;

    [ServerRpc]
    public void RequestChangeAvatarColorServerRpc(ulong targetId)
    {
        NetworkObject targetNetObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[targetId];
        if (targetNetObj != null)
        {
            AcceptChangeAvatarColorClientRpc(targetId);
            targetNetObj.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        }
    }

    [ClientRpc]
    public void AcceptChangeAvatarColorClientRpc(ulong targetId)
    {
        NetworkObject targetNetObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[targetId];
        if (targetNetObj != null)
        {
            targetNetObj.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        }
    }
}
