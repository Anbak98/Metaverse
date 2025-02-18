using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : NetworkBaseController
{
    private Camera followCam ;

    protected override void Start()
    {
        base.Start();
        followCam = Camera.main;
    }

    protected override void Update()
    {
        base.Update();
        followCam.transform.position = new Vector3(transform.position.x, transform.position.y, followCam.transform.position.z);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsOwner && collision.gameObject.CompareTag("Dungeon"))
        {
            Destroy(this);
            SceneManager.LoadScene("NetworkingDebugScene");
        }
    }
}
