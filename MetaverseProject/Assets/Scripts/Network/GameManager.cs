using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    private NetworkManager m_NetworkManager;

    public TextMeshProUGUI yesVoteText; // UI �ؽ�Ʈ

    public NetworkVariable<int> yesVote = new NetworkVariable<int>(
        0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void Awake()
    {
        // NetworkManager�� ���� ������Ʈ�� ���� ���ɼ��� �����Ƿ� FindObjectOfType ���
        m_NetworkManager = FindObjectOfType<NetworkManager>();
    }

    public override void OnNetworkSpawn()
    {
        // ���� ������ UI �ʱ�ȭ
        yesVoteText.text = $"Yes Votes: {yesVote.Value}\n Max Score: {PlayerPrefs.GetInt("MaxScore")}";

        // ���� ����� �� UI�� ������Ʈ�ϴ� �̺�Ʈ ���
        yesVote.OnValueChanged += OnYesVoteChanged;
    }

    public override void OnNetworkDespawn()
    {
        yesVote.OnValueChanged -= OnYesVoteChanged;
    }

    private void OnYesVoteChanged(int previousValue, int newValue)
    {
        yesVoteText.text = $"Yes Votes: {yesVote.Value}\n Max Score: {PlayerPrefs.GetInt("MaxScore")}";

        if (yesVote.Value == m_NetworkManager.ConnectedClients.Count)
        {
            m_NetworkManager.SceneManager.LoadScene("GameScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
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
            SubmitEnterGame();
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

    void SubmitEnterGame()
    {
        //if (GUILayout.Button(m_NetworkManager.IsServer ? "Vote" : "Request Vote"))
        //{
        //    if (IsClient)
        //    {
        //    }
        //}
    }

    //[ServerRpc(RequireOwnership = false)]
    //void IncreaseVoteServerRpc()
    //{
    //    yesVote.Value++;
    //    Debug.Log($"Vote Count: {yesVote.Value}");

    //    // ��� �÷��̾ ��ǥ�ϸ� �� ��ȯ
    //    if (yesVote.Value == m_NetworkManager.ConnectedClients.Count)
    //    {
    //        m_NetworkManager.SceneManager.LoadScene("GameScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    //    }
    //}
}
