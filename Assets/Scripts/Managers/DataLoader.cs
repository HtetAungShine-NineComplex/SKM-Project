using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    public string CurrentName = string.Empty;

    [SerializeField] private NetworkData _networkData;
    public NetworkData NetworkData => _networkData;
}
