using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPos : MonoBehaviour
{
    public RoomUserItem currentUser;
    public RectTransform rectTransform;
    public RectTransform coinTransform;
    public GameObject bankObj;

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
        }
    }

    public void SetCurrentUser(RoomUserItem item)
    {
        currentUser = item;
        currentUser.SetBankObject(bankObj);
        currentUser.SetPlayerCoinsRoot(coinTransform);
    }
}
