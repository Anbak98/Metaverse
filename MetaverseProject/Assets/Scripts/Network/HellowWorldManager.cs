using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HellowWorldManager : MonoBehaviour
{
    private NetworkManager m_NetworkMnaager;

    private void Awake()
    {
        m_NetworkMnaager = GetComponent<NetworkManager>();
    }

    private void Start()
    {
        RequestSpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId);
    }

[ServerRpc]
private void RequestSpawnPlayerServerRpc(ulong clientId)
{
    // 서버에서 클라이언트의 Battle Scene에 캐릭터를 스폰
    SpawnPlayerForClient(clientId);
}
    
[ServerRpc]
private void SpawnPlayerForClient(ulong clientId)
{
    if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var client))
    {
        GameObject player = Instantiate(NetworkManager.Singleton.NetworkConfig.PlayerPrefab);
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
    }
}

private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if(!m_NetworkMnaager.IsClient && !m_NetworkMnaager.IsServer)
        {
            StartButton();
        }
        else
        {
            StatusLabels();

            SubmitNewPosition();
        }

        GUILayout.EndArea();
    }

    void StartButton()
    {
        if (GUILayout.Button("Host")) m_NetworkMnaager.StartHost();
        if (GUILayout.Button("Client")) m_NetworkMnaager.StartClient();
        if (GUILayout.Button("Server")) m_NetworkMnaager.StartServer();
    }

    void StatusLabels()
    {
        var mode = m_NetworkMnaager.IsHost ?
            "Host" : m_NetworkMnaager.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " +
            m_NetworkMnaager.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Node: " + mode);
    }
    
    void SubmitNewPosition()
    {
        if(GUILayout.Button(m_NetworkMnaager.IsServer ? "Move" : "Request Position Change"))
        {
            if (m_NetworkMnaager.IsServer && !m_NetworkMnaager.IsClient)
            {
                foreach (ulong uid in m_NetworkMnaager.ConnectedClientsIds)
                {
                    m_NetworkMnaager.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<HellowWorldPlayer>();
                }
            }
            else
            {
                var playerObject = m_NetworkMnaager.SpawnManager.GetLocalPlayerObject();
                var player = playerObject.GetComponent<HellowWorldPlayer> ();
                player.Move();
            }
        }
    }
}
