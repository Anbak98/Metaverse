using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : NetworkBehaviour
{
    private static EnemyManager instance;
    public static EnemyManager Instance {  get => instance; }

    [SerializeField] private float spawnInterval = 0.2f;
    [SerializeField] private int endCount = 10;
    public NetworkVariable<int> score = new NetworkVariable<int>(
        0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public TextMeshProUGUI scoreText; // UI 텍스트

    [SerializeField] List<GameObject> destinationTile;
    [SerializeField] EnemyPool enemyPool;
    List<GameObject> enemys = new();

    float time;

    private void Awake()
    {
        instance = this;    
    }

    private void Start()
    {
        time = 0;
    }

    public override void OnNetworkSpawn()
    {
        // 현재 값으로 UI 초기화
        scoreText.text = $"Score: {score.Value}";

        // 값이 변경될 때 UI를 업데이트하는 이벤트 등록
        score.OnValueChanged += OnScoreChanged;
    }
    public override void OnNetworkDespawn()
    {
        score.OnValueChanged -= OnScoreChanged;
    }

    void OnScoreChanged(int previous, int next)
    {
        PlayerPrefs.SetInt("Score", score.Value);
        // 현재 값으로 UI 초기화
        scoreText.text = $"Score: {score.Value}";
    }

    void Update()
    {
        time -= Time.deltaTime;
        if(time < 0 )
        {
            GameObject obj = enemyPool.GetObject();
            if (obj == null) return;
            obj.transform.position = destinationTile[0].transform.position;
            GoblinController gc = obj.GetComponent<GoblinController>();
            foreach (GameObject _ in destinationTile)
            {
                gc.path.Enqueue(_);
            }
            gc.Init();
            enemys.Add(obj);
            time = spawnInterval;
        }

        if(score.Value > endCount)
        {
            if(PlayerPrefs.HasKey("MaxScore"))
            {
                if(PlayerPrefs.GetInt("MaxScore") < PlayerPrefs.GetInt("Score"))
                {
                    PlayerPrefs.SetInt("MaxScore", PlayerPrefs.GetInt("Score"));
                }
            }
            PlayerPrefs.SetInt("MaxScore", PlayerPrefs.GetInt("Score"));
            Debug.Log(PlayerPrefs.GetInt("MaxScore"));
            NetworkManager.Singleton.SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
        }
    }
}
