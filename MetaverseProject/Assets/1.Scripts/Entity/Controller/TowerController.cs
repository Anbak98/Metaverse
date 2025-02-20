using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TowerController : NetworkBaseController
{
    public List<GameObject> detectedMonsters;
    Detection detection;

    protected override void Awake()
    {
        base.Awake();
        detectedMonsters = new();
        detection = gameObject.GetComponentInChildren<Detection>();
    }

    protected override void Start()
    {
        base.Start();
        detection.Init(detectedMonsters);
    }

    protected override void HandleAction()
    {
        base.HandleAction();
        if (detectedMonsters.Count > 0)
        {
            AttackRequestServerRpc();
        }
    }

    [ServerRpc]
    private void AttackRequestServerRpc()
    {
        detectedMonsters[0].GetComponent<ResourceController>().ChangeHealth(-999);
    }
}
