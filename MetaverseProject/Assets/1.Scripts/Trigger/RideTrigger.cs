using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class RideTrigger : NetworkBehaviour
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
                RequestChangeAvatarServerRpc(targetNetObj.NetworkObjectId);
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        obj.SetActive(false);
    }

    public Sprite sprite;

    public void ChangeAvatar(ulong targetId, Sprite _sprite)
    {
        sprite = _sprite;
        RequestChangeAvatarServerRpc(targetId);
    }

    [ServerRpc]
    public void RequestChangeAvatarServerRpc(ulong targetId)
    {
        NetworkObject targetNetObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[targetId];
        if (targetNetObj != null)
        {
            AcceptChangeAvatarClientRpc(targetId);
            targetNetObj.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
        }
    }

    [ClientRpc]
    public void AcceptChangeAvatarClientRpc(ulong targetId)
    {
        NetworkObject targetNetObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[targetId];
        if (targetNetObj != null)
        {
            targetNetObj.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
        }
    }
}
