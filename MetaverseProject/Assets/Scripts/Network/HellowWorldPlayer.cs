using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HellowWorldPlayer : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

    public override void OnNetworkSpawn()
    {
        if(IsOwner)
        {
            Move();
        }
    }

    public void Move()
    {
        SubmitPositionRequestRpc();
    }

    [Rpc(SendTo.Server)]
    void SubmitPositionRequestRpc()
    {
        var reandomPosition = GetRandomPositionOnPlane();
        transform.position = reandomPosition;
        Position.Value = transform.position;
    }

    Vector3 GetRandomPositionOnPlane() => new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));

    private void Update()
    {
        transform.position = Position.Value;
    }
}
