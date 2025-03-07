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
    [SerializeField] public GameObject handPrefab; // 자식 오브젝트 프리팹
    private GameObject handInstance;

    public override void OnNetworkSpawn()
    {
        if (IsOwner) // 내가 소유한 오브젝트만 실행
        {
            SpawnHandServerRpc();
        }
    }

    protected override void Start()
    {
        if (!IsOwner) return;
        base.Start();
        NetworkManager.Singleton.SceneManager.OnSceneEvent += HandleSceneEvent;
        Camera newCam = Instantiate(Camera.main);
        followCam = newCam;
        foreach (var cam in Resources.FindObjectsOfTypeAll<Camera>())
        {
            cam.gameObject.SetActive(false);
        }

        // 새로 생성한 카메라만 활성화
        followCam.gameObject.SetActive(true);
    }

    protected override void Update()
    {
        if (!IsOwner) return;
        base.Update();
        followCam.transform.position = new Vector3(transform.position.x, transform.position.y, followCam.transform.position.z);
        RequestSetHandPositionServerRpc();
    }

    protected override void FixedUpdate()
    {
        if (!IsOwner) return;
        
            
        
        base.FixedUpdate();
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

    [ServerRpc]
    private void SpawnHandServerRpc(ServerRpcParams rpcParams = default)
    {
        GameObject hand = Instantiate(handPrefab, transform.position, Quaternion.identity);
        NetworkObject networkObject = hand.GetComponent<NetworkObject>();
        networkObject.SpawnAsPlayerObject(OwnerClientId, false); // 네트워크에 등록
        HandSyncClientRpc();
    }

    [ClientRpc]
    private void HandSyncClientRpc()
    {
        List<NetworkObject> ownedObjs = NetworkManager.Singleton.SpawnManager.GetClientOwnedObjects(OwnerClientId);
        handInstance = ownedObjs[1].gameObject;
        handPivot = handInstance.transform;
    }

    [ServerRpc]
    void RequestSetHandPositionServerRpc()
    {
        handInstance.transform.position = transform.position;
    }

    private void HandleSceneEvent(SceneEvent evt)
    {
        if(evt.SceneEventType == SceneEventType.LoadComplete)
        {
            HandSyncClientRpc();
            Camera newCam = Instantiate(Camera.main);
            followCam = newCam;
            foreach (var cam in Resources.FindObjectsOfTypeAll<Camera>())
            {
                cam.gameObject.SetActive(false);
            }

            // 새로 생성한 카메라만 활성화
            followCam.gameObject.SetActive(true);
        }
    }
}
