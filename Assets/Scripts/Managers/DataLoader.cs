using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    [SerializeField] private NetworkData _networkData;
    public NetworkData NetworkData => _networkData;
}
