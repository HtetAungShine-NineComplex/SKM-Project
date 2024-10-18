using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAnimController : MonoBehaviour
{
    [SerializeField] private CoinAssetData _data;
    [SerializeField] private CoinAnim _coinPrefab;
    [SerializeField] private Transform _coinTransform;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateCoins(int betAmount, Transform coinTrns)
    {
        _coinTransform = coinTrns;

        int num50kCoins = betAmount / 50000;
        betAmount %= 50000;

        int num10kCoins = betAmount / 10000;
        betAmount %= 10000;

        int num5kCoins = betAmount / 5000;
        betAmount %= 5000;

        int num1kCoins = betAmount / 1000;
        betAmount %= 1000;

        int num500Coins = betAmount / 500;
        betAmount %= 500;

        int num100Coins = betAmount / 100;

        Debug.Log($"Bet: {betAmount}, 50k: {num50kCoins}, 10k: {num10kCoins}, 5k: {num5kCoins}, 1k: {num1kCoins}, 500: {num500Coins}, 100: {num100Coins}");

        // Spawn the coins in your game world or UI
        SpawnCoins(num50kCoins, 50000);
        SpawnCoins(num10kCoins, 10000);
        SpawnCoins(num5kCoins, 5000);
        SpawnCoins(num1kCoins, 1000);
        SpawnCoins(num500Coins, 500);
        SpawnCoins(num100Coins, 100);
    }

    private void SpawnCoins(int count, int coinValue)
    {
        if(count  == 0) return;

        for (int i = 0; i < count; i++)
        {
            CoinAnim coin = Instantiate(_coinPrefab, new Vector3(i * 2.0f, 0, 0), Quaternion.identity, this.transform);

            Sprite sprite = _data.GetSpriteByAmount(coinValue);
            coin.SetSprite(sprite);
            coin.SetPositions(transform.position, _coinTransform);
        }
    }
}
