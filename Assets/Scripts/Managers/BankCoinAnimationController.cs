using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankCoinAnimationController : MonoBehaviour
{
    [SerializeField] private CoinAssetData _data;
    [SerializeField] private CoinAnim _coinPrefab;
    [SerializeField] private Transform _coinTransform;
    [SerializeField] private Transform _testPlayer;

    [HideInInspector] public List<CoinAnim> coins50k = new List<CoinAnim>();
    [HideInInspector] public List<CoinAnim> coins10k = new List<CoinAnim>();
    [HideInInspector] public List<CoinAnim> coins5k = new List<CoinAnim>();
    [HideInInspector] public List<CoinAnim> coins1k = new List<CoinAnim>();
    [HideInInspector] public List<CoinAnim> coins500 = new List<CoinAnim>();
    [HideInInspector] public List<CoinAnim> coins100 = new List<CoinAnim>();

    // Create a dictionary referencing each list by denomination
    private Dictionary<int, List<CoinAnim>> _coinLists;

    private int[] coinValues = new int[] { 100, 500, 1000, 5000, 10000, 50000 };

    private void Awake()
    {
        _coinLists = new Dictionary<int, List<CoinAnim>>()
        {
            { 100, coins100 },
            { 500, coins500 },
            { 1000, coins1k },
            { 5000, coins5k },
            { 10000, coins10k },
            { 50000, coins50k }
        };
    }

    public void SendAllCoinsToPlayer(Transform target)
    {
        foreach (var coinList in _coinLists.Values)
        {
            foreach (var coin in coinList)
            {
                coin.SetPositions(coin.transform.position, target.position, true);
            }

            coinList.Clear();
        }
    }

    [ContextMenu("test500")]
    public void Test500()
    {
        TakeCoinsFromTable(500, _testPlayer);
    }

    [ContextMenu("test100of1000")]
    public void TestSplit()
    {
        SplitCoin(GetFirstCoinByValue(1000), 100);
    }

    [ContextMenu("testAdd")]
    public void TestAdd()
    {
        GenerateCoinsIntoTable(50000, _testPlayer);
    }

    public void TakeCoinsFromTable(int coinAmount, Transform targetPlayerPos)
    {
        StartCoroutine(TakeCoins(coinAmount, targetPlayerPos));
    }
    
    IEnumerator TakeCoins(int coinAmount, Transform targetPlayerPos)
    {
        if (AreAllCoinsEmpty())
        {
            Debug.Log("all empty coins");
            yield break;
        }

        

        Debug.Log("Taking coins");

        if(coinAmount > GetTotalCoinValue())
        {
            coinAmount = GetTotalCoinValue();
        }
        int coinRequested = coinAmount;


        // Calculate the number of each type of coin you would need based on the total coin amount.
        int num50kCoins = coinAmount / 50000;
        coinAmount %= 50000;

        int num10kCoins = coinAmount / 10000;
        coinAmount %= 10000;

        int num5kCoins = coinAmount / 5000;
        coinAmount %= 5000;

        int num1kCoins = coinAmount / 1000;
        coinAmount %= 1000;

        int num500Coins = coinAmount / 500;
        coinAmount %= 500;

        int num100Coins = coinAmount / 100;

        if (coinAmount > 0)
        {
            num100Coins = 1;
        }

        // Adjust num50kCoins based on the existing number of smaller coins
        if (num50kCoins > 0 && _coinLists[10000].Count > 0)
        {
            int available50kFrom10ks = _coinLists[10000].Count / 5; // Each 50k coin requires 5 x 10k coins
            int coinsToConvert = Math.Min(num50kCoins, available50kFrom10ks);

            num50kCoins -= coinsToConvert; // Reduce the number of 50k coins needed
            num10kCoins += coinsToConvert * 5; // Increase the number of 10k coins needed
        }

        // Adjust num10kCoins based on the existing number of smaller coins
        if (num10kCoins > 0 && _coinLists[5000].Count > 0)
        {
            int available10kFrom5ks = _coinLists[5000].Count / 2; // Each 10k coin requires 2 x 5k coins
            int coinsToConvert = Math.Min(num10kCoins, available10kFrom5ks);

            num10kCoins -= coinsToConvert; // Reduce the number of 10k coins needed
            num5kCoins += coinsToConvert * 2; // Increase the number of 5k coins needed
        }

        // Adjust num5kCoins based on the existing number of smaller coins
        if (num5kCoins > 0 && _coinLists[1000].Count > 0)
        {
            int available5kFrom1ks = _coinLists[1000].Count * 2; // Each 5k coin can be made from 2 x 1k coins
            int coinsToConvert = Math.Min(num5kCoins / 2, _coinLists[1000].Count); // Max 1k coins we can use

            num5kCoins -= coinsToConvert * 2; // Reduce the number of 5k coins needed
            num1kCoins += coinsToConvert; // Increase the number of 1k coins needed
        }

        // Adjust num1kCoins based on the existing number of smaller coins
        if (num1kCoins > 0 && _coinLists[500].Count > 0)
        {
            int available1kFrom500s = _coinLists[500].Count * 2; // Each 1k coin can be made from 2 x 500 coins
            int coinsToConvert = Math.Min(num1kCoins / 2, _coinLists[500].Count); // Max 500 coins we can use

            num1kCoins -= coinsToConvert * 2; // Reduce the number of 1k coins needed
            num500Coins += coinsToConvert; // Increase the number of 500 coins needed
        }

        // Adjust num500Coins based on the existing number of smaller coins
        if (num500Coins > 0 && _coinLists[100].Count > 0)
        {
            int available500From100s = _coinLists[100].Count / 5; // Each 500 coin requires 5 x 100 coins
            int coinsToConvert = Math.Min(num500Coins, available500From100s); // Max 500 coins we can use

            num500Coins -= coinsToConvert; // Reduce the number of 500 coins needed
            num100Coins += coinsToConvert * 5; // Increase the number of 100 coins needed
        }

        // Now you can proceed with your existing logic for splitting coins based on the adjusted counts.
        // (You can add your yield return logic or any other operations here)


        // Handling for 50k coins
        if (num50kCoins > 0 && _coinLists[50000].Count < num50kCoins)
        {
            while (_coinLists[50000].Count < num50kCoins)
            {
                CoinAnim old = GetAvailableBiggerCoin(50000);

                if (old == null)
                {
                    // Increase the next smaller coin type (10k) if there aren't enough 50k coins
                    num10kCoins += (num50kCoins - _coinLists[50000].Count) * 5;
                    num50kCoins = _coinLists[50000].Count;
                    break;
                }
                else
                {
                    yield return new WaitForSeconds(0.3f);
                }

                SplitCoin(old, 50000);
            }
            
        }

        // Handling for 10k coins
        if (num10kCoins > 0 && _coinLists[10000].Count < num10kCoins)
        {
            while (_coinLists[10000].Count < num10kCoins)
            {
                CoinAnim old = GetAvailableBiggerCoin(10000);

                if (old == null)
                {
                    num5kCoins += (num10kCoins - _coinLists[10000].Count) * 2;
                    num10kCoins = _coinLists[10000].Count;
                    break;
                }
                else
                {
                    yield return new WaitForSeconds(0.3f);
                }

                SplitCoin(old, 10000);
            }
        }

        // Handling for 5k coins
        if (num5kCoins > 0 && _coinLists[5000].Count < num5kCoins)
        {
            while (_coinLists[5000].Count < num5kCoins)
            {
                CoinAnim old = GetAvailableBiggerCoin(5000);

                if (old == null)
                {
                    num1kCoins += (num5kCoins - _coinLists[5000].Count) * 5;
                    num5kCoins = _coinLists[5000].Count;
                    break;
                }
                else
                {
                    yield return new WaitForSeconds(0.3f);
                }

                SplitCoin(old, 5000);
            }

        }

        // Handling for 1k coins
        if (num1kCoins > 0 && _coinLists[1000].Count < num1kCoins)
        {
            while (_coinLists[1000].Count < num1kCoins)
            {
                CoinAnim old = GetAvailableBiggerCoin(1000);

                if (old == null)
                {
                    num500Coins += (num1kCoins - _coinLists[1000].Count) * 2;
                    num1kCoins = _coinLists[1000].Count;
                    break;
                }
                else
                {
                    yield return new WaitForSeconds(0.3f);
                }

                SplitCoin(old, 1000);
            }

        }

        // Handling for 500 coins
        if (num500Coins > 0 && _coinLists[500].Count < num500Coins)
        {
            while (_coinLists[500].Count < num500Coins)
            {
                CoinAnim old = GetAvailableBiggerCoin(500);

                if (old == null)
                {
                    num100Coins += (num500Coins - _coinLists[500].Count) * 5;
                    num500Coins = _coinLists[500].Count;
                    break;
                }
                else
                {
                    yield return new WaitForSeconds(0.3f);
                }

                SplitCoin(old, 500);
            }

        }

        // Handling for 100 coins
        if (num100Coins > 0 && _coinLists[100].Count < num100Coins)
        {
            Debug.Log("Needing 100 coins");

            while (_coinLists[100].Count < num100Coins)
            {
                CoinAnim old = GetAvailableBiggerCoin(100);

                if (old == null)
                {
                    Debug.LogWarning("Not enough coins of 100 or larger to satisfy the requirement.");
                    break;
                }
                else
                {
                    yield return new WaitForSeconds(0.3f);
                }

                SplitCoin(old, 100);
            }
        }


        yield return new WaitForSeconds(0.3f);

        

        Debug.Log($"Coins: {coinAmount}, 50k: {num50kCoins}, 10k: {num10kCoins}, 5k: {num5kCoins}, 1k: {num1kCoins}, 500: {num500Coins}, 100: {num100Coins}");

        // Spawn the coins in your game world or UI
        SpawnCoinsFromTable(num50kCoins, 50000, targetPlayerPos);
        SpawnCoinsFromTable(num10kCoins, 10000, targetPlayerPos);
        SpawnCoinsFromTable(num5kCoins, 5000, targetPlayerPos);
        SpawnCoinsFromTable(num1kCoins, 1000, targetPlayerPos);
        SpawnCoinsFromTable(num500Coins, 500, targetPlayerPos);
        SpawnCoinsFromTable(num100Coins, 100, targetPlayerPos);

    }

    private bool AreAllCoinsEmpty()
    {
        return coins50k.Count == 0 &&
               coins10k.Count == 0 &&
               coins5k.Count == 0 &&
               coins1k.Count == 0 &&
               coins500.Count == 0 &&
               coins100.Count == 0;
    }

    private int GetSmallestCoinAvailable()
    {
        foreach (List<CoinAnim> coins in _coinLists.Values)
        {
            if (coins.Count > 0) return coins[0].value;
        }

        return 0;
    }

    private int GetTotalSmallestCoinValue()
    {
        int total = 0;

        foreach (List<CoinAnim> coins in _coinLists.Values)
        {
            if (coins.Count > 0) 
            {
                foreach(CoinAnim coin in coins)
                {
                    total += coin.value;
                }
            }
        }

        return total;
    }

    private int GetTotalCoinValue()
    {
        int totalValue = 0;

        foreach (var coinList in _coinLists.Values)
        {
            foreach (var coin in coinList)
            {
                totalValue += coin.value; // Assuming each CoinAnim has a property called Value
            }
        }

        return totalValue;
    }

    private CoinAnim GetFirstCoinByValue(int value)
    {
        // Check if the dictionary has a list for the specified coin value
        if (_coinLists.ContainsKey(value) && _coinLists[value].Count > 0)
        {
            // Return the first CoinAnim in the list for the specified value
            return _coinLists[value][0];
        }

        // If no coin with the specified value is found, return null
        return null;
    }

    private int GetNearestCoinTypeValue(int inputValue)
    {
        // Sort the array to ensure ascending order
        Array.Sort(coinValues);

        int nearestValue = coinValues[0]; // Default to the smallest value

        foreach (int coinValue in coinValues)
        {
            if (inputValue < coinValue)
            {
                break; // Stop if the next value is higher than the input
            }
            nearestValue = coinValue; // Update nearestValue to the last valid coinValue
        }

        return nearestValue;
    }

    private CoinAnim GetAvailableBiggerCoin(int inputValue)
    {
        // Sort the array to ensure ascending order
        Array.Sort(coinValues);

        foreach (int coinValue in coinValues)
        {
            if (inputValue >= coinValue)
            {
                continue; // Stop if the next value is higher than the input
            }

            if (_coinLists[coinValue].Count > 0)
            {
                return _coinLists[coinValue][0];
            }

        }

        return null;
    }

    private void SpawnCoinsFromTable(int count, int coinValue, Transform target)
    {
        if (count == 0 || _coinLists.Count == 0 || !_coinLists.ContainsKey(coinValue) || AreAllCoinsEmpty()) return;

        if ( _coinLists[coinValue].Count == 0)
        {

        }

        for (int i = 0; i < count; i++)
        {

            // Instantiate the coin at the spawn position
            //CoinAnim coin = Instantiate(_coinPrefab, spawnPosition, Quaternion.identity, this.transform);
            CoinAnim coin = _coinLists[coinValue][0];
            _coinLists[coinValue].RemoveAt(0);

            coin.SetPositions(coin.transform.position, target.position, true);

        }
    }

    public void GenerateCoinsIntoTable(int coinAmount, Transform start)
    {
        int num50kCoins = coinAmount / 50000;
        coinAmount %= 50000;

        int num10kCoins = coinAmount / 10000;
        coinAmount %= 10000;

        int num5kCoins = coinAmount / 5000;
        coinAmount %= 5000;

        int num1kCoins = coinAmount / 1000;
        coinAmount %= 1000;

        int num500Coins = coinAmount / 500;
        coinAmount %= 500;

        int num100Coins = coinAmount / 100;

        if(coinAmount > 0 && coinAmount < 100)
        {
            num100Coins = 1;
        }

        Debug.Log($"Coins: {coinAmount}, 50k: {num50kCoins}, 10k: {num10kCoins}, 5k: {num5kCoins}, 1k: {num1kCoins}, 500: {num500Coins}, 100: {num100Coins}");

        // Spawn the coins in your game world or UI
        SpawnCoins(num50kCoins, 50000, start);
        SpawnCoins(num10kCoins, 10000, start);
        SpawnCoins(num5kCoins, 5000, start);
        SpawnCoins(num1kCoins, 1000, start);
        SpawnCoins(num500Coins, 500, start);
        SpawnCoins(num100Coins, 100, start);
    }

    private void SpawnCoins(int count, int coinValue, Transform start)
    {
        if (count == 0) return;

        for (int i = 0; i < count; i++)
        {
            // Random offset for position
            float xOffset = UnityEngine.Random.Range(-100f, 100f);
            float yOffset = UnityEngine.Random.Range(-80f, 80f);
            Vector3 spawnPosition = new Vector3(i * 2.0f + xOffset, yOffset, 0);

            // Instantiate the coin at the spawn position
            CoinAnim coin = Instantiate(_coinPrefab, start.position, Quaternion.identity, _coinTransform);

            // Set up the coin's properties
            CoinData data = _data.GetCoinDataByAmount(coinValue);
            Sprite sprite = data.sprite;
            coin.SetSprite(sprite);
            coin.SetPositions(start.position, _coinTransform.position + new Vector3(xOffset, yOffset, 0));
            coin.SetValueString(ConvertToK(data.amount));
            coin.value = data.amount;

            // Add the spawned coin to the respective list in the dictionary
            if (_coinLists.ContainsKey(coinValue))
            {
                _coinLists[coinValue].Add(coin);
            }
        }
    }

    private void SplitCoin(CoinAnim oldCoin, int lowerValue)
    {
        int splitCount = oldCoin.value / lowerValue;

        for (int i = 0; i < splitCount; i++)
        {
            float xOffset = UnityEngine.Random.Range(-60f, 60f);
            float yOffset = UnityEngine.Random.Range(-40f, 40f);
            Vector3 spawnPosition = new Vector3(xOffset, yOffset, 0);

            CoinAnim newCoin = Instantiate(_coinPrefab, oldCoin.transform.position, Quaternion.identity, _coinTransform);
            CoinData data = _data.GetCoinDataByAmount(lowerValue);
            newCoin.SetSprite(data.sprite);
            newCoin.SetPositions(oldCoin.transform.position, spawnPosition + oldCoin.transform.position, false, 2f);
            newCoin.SetValueString(ConvertToK(data.amount));
            newCoin.value = data.amount;

            _coinLists[lowerValue].Add(newCoin);

            _coinLists[oldCoin.value].Remove(oldCoin);
            Destroy(oldCoin.gameObject);
        }
    }

    public void ResetTable()
    {
        if (_coinLists == null)
        {
            return;
        }

        foreach (var coinList in _coinLists.Values)
        {
            if(coinList == null)
            {
                continue;
            }

            foreach (CoinAnim coin in coinList)
            {
                Destroy(coin.gameObject);
            }
            coinList.Clear(); // Clear each list of coins after destroying the objects
        }
    }

    public string ConvertToK(int number)
    {
        if (number >= 1000)
        {
            return (number / 1000).ToString() + "k";
        }
        return number.ToString();
    }

    private int GetAvailableHigherDenomination(int currentDenomination)
    {
        // Ordered list of denominations from lowest to highest
        int[] denominations = { 100, 500, 1000, 5000, 10000, 50000 };

        // Find the current denomination index
        int currentIndex = Array.IndexOf(denominations, currentDenomination);
        if (currentIndex == -1) return -1; // Invalid denomination

        // Look for the next available higher denomination with coins in stock
        for (int i = currentIndex + 1; i < denominations.Length; i++)
        {
            int higherDenomination = denominations[i];

            // Check if coins of this denomination are available
            if (_coinLists.ContainsKey(higherDenomination) && _coinLists[higherDenomination].Count > 0)
            {
                return higherDenomination;
            }
        }

        return -1; // No available higher denomination with coins
    }

    
}
