using Sfs2X.Entities.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAnimationController : MonoBehaviour
{
    [SerializeField] private CardAnim _cardAnim;
    [SerializeField] private PlayerPos[] playerPositions;
    [SerializeField] private Transform _cardRoot;
    [SerializeField] private RectTransform _startPos;

    public void ListenEvents()
    {
        Managers.NetworkManager.GameStarted += DistributeCardsToAll;
    }

    public void DistributeCardsToAll(ISFSObject obj)
    {
        StartCoroutine(DistributeCardsAnimation());
    }

    public void DistributeCardToSinglePlayer(string cardName, RoomUserItem item, bool isDrawing)
    {
        foreach (PlayerPos pos in playerPositions)
        {
            if (pos.currentUser == null) continue;

            if (pos.currentUser == item)
            {
                CardAnim card = Instantiate(_cardAnim, _cardRoot);
                card.addCardName = cardName;
                card.hasCard = true;
                card.isDraw = true;
                card.SetPositions(_startPos.anchoredPosition, pos);

                break;
            }
        }
    }

    public void DistributeBlankCardToSinglePlayer(RoomUserItem item)
    {
        foreach (PlayerPos pos in playerPositions)
        {
            if (pos.currentUser == null) continue;

            if (pos.currentUser == item)
            {
                CardAnim card = Instantiate(_cardAnim, _cardRoot);
                card.hasCard = false;
                card.SetPositions(_startPos.anchoredPosition, pos);

                break;
            }
        }
    }

    IEnumerator DistributeCardsAnimation()
    {
        for (int i = 0; i < 2; i++)
        {
            foreach (PlayerPos pos in playerPositions)
            {
                if (pos.currentUser == null) continue;

                CardAnim card = Instantiate(_cardAnim, _cardRoot);
                card.SetPositions(_startPos.anchoredPosition, pos);

                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}