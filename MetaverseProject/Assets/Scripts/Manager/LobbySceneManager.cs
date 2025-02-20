using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class LobbySceneManager : NetworkBehaviour
{
    private NetworkManager m_NetworkManager;

    public TextMeshProUGUI yesVoteText;
    public NetworkVariable<int> yesVote = new NetworkVariable<int>(
        0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void Awake()
    {
        m_NetworkManager = FindObjectOfType<NetworkManager>();
    }

    public override void OnNetworkSpawn()
    {
        yesVoteText.text = $"Yes Votes: {yesVote.Value}\n Max Score: {PlayerPrefs.GetInt("MaxScore")}";

        yesVote.OnValueChanged += OnYesVoteChanged;
    }

    public override void OnNetworkDespawn()
    {
        yesVote.OnValueChanged -= OnYesVoteChanged;
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!m_NetworkManager.IsClient && !m_NetworkManager.IsServer)
        {
            StartButton();
        }
        else
        {
            StatusLabels();
        }
        GUILayout.EndArea();
    }

    void StartButton()
    {
        if (GUILayout.Button("Host")) m_NetworkManager.StartHost();
        if (GUILayout.Button("Client")) m_NetworkManager.StartClient();
        if (GUILayout.Button("Server")) m_NetworkManager.StartServer();
    }

    void StatusLabels()
    {
        var mode = m_NetworkManager.IsHost ? "Host" :
                   m_NetworkManager.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " +
            m_NetworkManager.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Node: " + mode);
        if (m_NetworkManager.IsServer)
            GUILayout.Label("EnterCount: " + yesVote.Value + "/" + m_NetworkManager.ConnectedClients.Count);
    }
    private void OnYesVoteChanged(int previousValue, int newValue)
    {
        yesVoteText.text = $"Yes Votes: {yesVote.Value}\n Max Score: {PlayerPrefs.GetInt("MaxScore")}";

        if (yesVote.Value == m_NetworkManager.ConnectedClients.Count)
        {
            StartCoroutine(LoadGameScene());
            yesVoteText.text = $"wait a moment...";
        }
    }

    IEnumerator LoadGameScene()
    {
        yield return new WaitForSeconds(3);
        m_NetworkManager.SceneManager.LoadScene("GameScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
