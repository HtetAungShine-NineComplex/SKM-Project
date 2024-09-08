using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPos : MonoBehaviour
{
    public RoomUserItem currentUser;
    public RectTransform rectTransform;

    private void Awake()
    {
        currentUser = null;
    }
}
