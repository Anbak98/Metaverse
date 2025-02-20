using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GoblinController : BaseController
{
    public Queue<GameObject> path = new();
    Queue<GameObject> backup = new();
    GameObject nextPath;

    public void OnEnable()
    {
        path = new(backup);
    }

    public void Init()
    {
        backup = new(path);
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
        }
    }
}
