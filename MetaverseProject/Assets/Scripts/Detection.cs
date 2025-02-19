using UnityEngine;
using System.Collections.Generic;

public class Detection : MonoBehaviour
{
    private List<GameObject> targetList; 

    public void Init(List<GameObject> _targetList)
    {
        targetList = _targetList;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && targetList != null && !targetList.Contains(collision.gameObject)) 
        {
            targetList.Add(collision.gameObject);
            Debug.Log($"{collision.gameObject.name} ������! ���� {targetList.Count}�� Ÿ�� ����.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && targetList != null && targetList.Contains(collision.gameObject))
        {
            targetList.Remove(collision.gameObject);
            Debug.Log($"{collision.gameObject.name} �������� ����! ���� {targetList.Count}�� Ÿ�� ����.");
        }
    }
}
