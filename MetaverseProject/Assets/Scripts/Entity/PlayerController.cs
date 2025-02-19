using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class PlayerController : NetworkBaseController
{
    private Camera followCam;
    [SerializeField] public GameObject handPrefab; // �ڽ� ������Ʈ ������
    private GameObject handInstance;

    public override void OnNetworkSpawn()
    {
        if (IsOwner) // ���� ������ ������Ʈ�� ����
        {
            SpawnHandServerRpc();
        }
    }

    [ServerRpc]
    private void SpawnHandServerRpc(ServerRpcParams rpcParams = default)
    {
        GameObject hand = Instantiate(handPrefab, transform.position, Quaternion.identity);
        NetworkObject networkObject = hand.GetComponent<NetworkObject>();
        networkObject.SpawnAsPlayerObject(OwnerClientId, false); // ��Ʈ��ũ�� ���
        HandSyncClientRpc();
    }

    [ClientRpc]
    private void HandSyncClientRpc()
    {
        List<NetworkObject> ownedObjs = NetworkManager.Singleton.SpawnManager.GetClientOwnedObjects(OwnerClientId);
        foreach (NetworkObject obj in ownedObjs)
        {
            Debug.Log(obj);
        }
        handInstance = ownedObjs[1].gameObject;
        Debug.Log(handInstance);
        handPivot = handInstance.transform;
    }

    private void OnDisconnectedFromServer()
    {
        Destroy(handPrefab);
    }

    protected override void Start()
    {
        if (!IsOwner) return;
        base.Start();
        Camera newCam = Instantiate(Camera.main);
        followCam = newCam;
        foreach (var cam in Resources.FindObjectsOfTypeAll<Camera>())
        {
            cam.gameObject.SetActive(false);
        }

        // ���� ������ ī�޶� Ȱ��ȭ
        followCam.gameObject.SetActive(true);
    }

    protected override void Update()
    {
        if (!IsOwner) return;
        base.Update();
        followCam.transform.position = new Vector3(transform.position.x, transform.position.y, followCam.transform.position.z);
        RequestSetHandPositionServerRpc();
        Debug.Log(handInstance);
    }

    [ServerRpc]
    void RequestSetHandPositionServerRpc()
    {
        handInstance.transform.position = transform.position;
    }

    protected override void HandleAction()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        movementDirection = new Vector2(horizontal, vertical).normalized;

        Vector2 mousePosition = Input.mousePosition;
        Vector2 worldPos = followCam.ScreenToWorldPoint(mousePosition);
        lookDirection = (worldPos - (Vector2)transform.position);

        if (lookDirection.magnitude < .9f)
        {
            lookDirection = Vector2.zero;
        }
        else
        {
            lookDirection = lookDirection.normalized;
        }
    }
}
