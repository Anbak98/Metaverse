using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneEntrance : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponentInParent<GameManager>().yesVote.Value += 1;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponentInParent<GameManager>().yesVote.Value -= 1;
        }
    }
}
