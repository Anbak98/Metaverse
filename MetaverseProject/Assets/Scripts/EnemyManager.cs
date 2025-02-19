using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] List<GameObject> destinationTile;
    [SerializeField] ObjectPool objectPool;
    List<GameObject> enemys = new();

    readonly float spawnInterval = 5f;
    float time;

    private void Start()
    {
        time = 0;
    }

    void Update()
    {
        time -= Time.deltaTime;
        if(time < 0 )
        {
            GameObject obj = objectPool.GetObject();
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
    }
}
