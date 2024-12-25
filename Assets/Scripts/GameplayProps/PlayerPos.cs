using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerPos : MonoBehaviour
{
    public RoomUserItem currentUser;
    public RectTransform rectTransform;
    public RectTransform coinTransform;
    public GameObject bankObj;
    public GameObject betPanel;
    public TMP_Text betTxt;

    private void Awake()
    {
        currentUser = null;
    }

    private void Start()
    {
        bankObj.SetActive(false);
    }

    private void Update()
    {
        if (currentUser == null)
        {
            bankObj.SetActive(false);
            betPanel.SetActive(false);
        }
        else 
        {
            betPanel.SetActive(!currentUser.IsBank);
            bankObj.SetActive(currentUser.IsBank);
        }
    }

    public void SetCurrentUser(RoomUserItem item)
    {
        currentUser = item;
        currentUser.SetBankObject(bankObj);
        currentUser.SetPlayerCoinsRoot(coinTransform);
        currentUser.SetBetText(betTxt);
    }
}
