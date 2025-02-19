using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public const int MAX_COUNT = 10;

    [SerializeField] GameObject targetObject;

    private Stack<GameObject> pool = new();

    void Start()
    {
        for (int i = 0; i < MAX_COUNT; i++)
        {
            GameObject obj = Instantiate(targetObject);
            obj.SetActive(false);
            pool.Push(obj);
        }
    }

    public GameObject GetObject()
    {
        foreach(var obj in pool)
        {
            if (obj.activeSelf is false)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        if(pool.Count < MAX_COUNT)
        {
            GameObject gameObject = Instantiate(targetObject);
            gameObject.transform.SetParent(transform, false);
            gameObject.SetActive(true);
            pool.Push(gameObject);
            return gameObject;
        }

        return null;
    }
}
