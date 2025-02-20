using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class BoardTrigger : NetworkBehaviour
{
    [SerializeField] GameObject obj;
    [SerializeField] TextMeshProUGUI textMeshProUGUI;

    void OnTriggerStay2D(Collider2D collision)
    {
        obj.SetActive(true);
        textMeshProUGUI.text = $"BEST SCORE: {PlayerPrefs.GetInt("MaxScore")}";
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        obj.SetActive(false);
    }
}
