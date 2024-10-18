using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CoinAssetData", menuName = "SKMGamePlay/CoinAssetData", order = 1)]
public class CoinAssetData : ScriptableObject
{
    public List<CoinData> data;

    public Sprite GetSpriteByAmount(int amount)
    {
        foreach (CoinData coin in data)
        {
            if (coin.amount == amount)
            {
                return coin.sprite;
            }
        }

        return null;
    }
}

[Serializable]
public class CoinData
{
    public int amount;
    public Sprite sprite;
}
