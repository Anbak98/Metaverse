using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private NetworkManager m_NetworkMnaager;
    NetworkVariable<int> yesVote = new NetworkVariable<int>(0);

    private void Awake()
    {
        m_NetworkMnaager = GetComponent<NetworkManager>();
    }

    private void Start()
    {
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!m_NetworkMnaager.IsClient && !m_NetworkMnaager.IsServer)
        {
            StartButton();
        }
        else
        {
            StatusLabels();

            SubmitEnterGame();
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
        if(m_NetworkMnaager.IsServer) GUILayout.Label("EnterCount: " + yesVote.Value + "/" + m_NetworkMnaager.ConnectedClients.Count);
    }

    void SubmitEnterGame()
    {
        if (GUILayout.Button(m_NetworkMnaager.IsServer ? "Vote" : "Request Vote"))
        {
            if(m_NetworkMnaager.IsClient) IncreaseVote();
            if (yesVote.Value == m_NetworkMnaager.ConnectedClients.Count)
            {
                if (m_NetworkMnaager.IsServer) m_NetworkMnaager.SceneManager.LoadScene("GameScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }       
        }
    }

    [Rpc(SendTo.Server)]
    void IncreaseVote()
    {
        yesVote.Value++;
        Debug.Log(yesVote.Value);
    }
}
