using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NetworkData", menuName = "Shan/NetworkData", order = 1)]
public class NetworkData : ScriptableObject
{
    [Header("Smartfox Server")]
    [Tooltip("IP address or domain name of the SmartFoxServer instance")]
    public string host = "127.0.0.1";

    [Tooltip("TCP listening port of the SmartFoxServer instance, used for TCP socket connection")]
    public int tcpPort = 9933;

    [Tooltip("UDP listening port of the SmartFoxServer instance, used for UDP communication")]
    public int UdpPort = 9933;

    public int webSocketPort = 8080;

    [Tooltip("Name of the SmartFoxServer Zone to join")]
    public string zone = "DelightShan";

    [Tooltip("Display SmartFoxServer client debug messages")]
    public bool debug = false;

    [Header("API")]
    public string URI;

    [Header("Endpoints")]
    public string login;
    public string register;
    public string userInfo;
    public string roomconfig;
    public string gameHistories;

    [Space]
    public string testToken;
}
