using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NetworkData", menuName = "Shan/NetworkData", order = 1)]
public class NetworkData : ScriptableObject
{
    [Header("API")]
    public string URI;

    [Header("Endpoints")]
    public string login;
    public string register;
    public string userInfo;
    public string roomconfig;
}
