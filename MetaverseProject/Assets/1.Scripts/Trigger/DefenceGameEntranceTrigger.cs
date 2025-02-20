using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceGameEntranceTrigger : MonoBehaviour
{
    [SerializeField] GameObject description;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponentInParent<LobbySceneManager>().yesVote.Value += 1;
            description.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponentInParent<LobbySceneManager>().yesVote.Value -= 1;
            description.SetActive(false);
        }
    }
}
