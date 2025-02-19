using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GoblinController : BaseController
{
    public Queue<GameObject> path = new();
    GameObject nextPath;

    public void Init()
    {
        nextPath = path.Dequeue();
    }

    protected override void HandleAction()
    {
        base.HandleAction();
        Move();

    }

    public void Move()
    {
        if (nextPath is not null)
        {
            Bounds bounds = nextPath.GetComponent<Collider2D>().bounds;
            if (transform.position.x > bounds.min.x && transform.position.x < bounds.max.x && transform.position.y > bounds.min.y && transform.position.y < bounds.max.y)
            {
                nextPath = path.Dequeue();
            }
            movementDirection = new Vector2(nextPath.transform.position.x - transform.position.x, nextPath.transform.position.y - transform.position.y).normalized;
            Debug.Log(nextPath.transform.position.x);
            Debug.Log(nextPath.transform.position.y);
            Debug.Log(movementDirection);
        }
    }
}
