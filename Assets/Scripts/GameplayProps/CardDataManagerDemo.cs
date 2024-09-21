using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using THZ;

public class CardDataManagerDemo : MonoBehaviour
{
    [SerializeField] private Sprite[] _cardSprites;

    private Dictionary<string, Sprite> _cardSpriteData;

    private void Awake()
    {
        
    }

    private void Start()
    {
        _cardSpriteData = new Dictionary<string, Sprite>
        {
            { GameConstants.SPADE_ACE, _cardSprites[0] },
            { GameConstants.SPADE_TWO, _cardSprites[1] },
            { GameConstants.SPADE_THREE, _cardSprites[2] },
            { GameConstants.SPADE_FOUR, _cardSprites[3] },
            { GameConstants.SPADE_FIVE, _cardSprites[4] },
            { GameConstants.SPADE_SIX, _cardSprites[5] },
            { GameConstants.SPADE_SEVEN, _cardSprites[6] },
            { GameConstants.SPADE_EIGHT, _cardSprites[7] },
            { GameConstants.SPADE_NINE, _cardSprites[8] },
            { GameConstants.SPADE_TEN, _cardSprites[9] },
            { GameConstants.SPADE_JACK, _cardSprites[10] },
            { GameConstants.SPADE_QUEEN, _cardSprites[11] },
            { GameConstants.SPADE_KING, _cardSprites[12] },
            { GameConstants.DIAMOND_ACE, _cardSprites[13] },
            { GameConstants.DIAMOND_TWO, _cardSprites[14] },
            { GameConstants.DIAMOND_THREE, _cardSprites[15] },
            { GameConstants.DIAMOND_FOUR, _cardSprites[16] },
            { GameConstants.DIAMOND_FIVE, _cardSprites[17] },
            { GameConstants.DIAMOND_SIX, _cardSprites[18] },
            { GameConstants.DIAMOND_SEVEN, _cardSprites[19] },
            { GameConstants.DIAMOND_EIGHT, _cardSprites[20] },
            { GameConstants.DIAMOND_NINE, _cardSprites[21] },
            { GameConstants.DIAMOND_TEN, _cardSprites[22] },
            { GameConstants.DIAMOND_JACK, _cardSprites[23] },
            { GameConstants.DIAMOND_QUEEN, _cardSprites[24] },
            { GameConstants.DIAMOND_KING, _cardSprites[25] },
            { GameConstants.HEART_ACE, _cardSprites[26] },
            { GameConstants.HEART_TWO, _cardSprites[27] },
            { GameConstants.HEART_THREE, _cardSprites[28] },
            { GameConstants.HEART_FOUR, _cardSprites[29] },
            { GameConstants.HEART_FIVE, _cardSprites[30] },
            { GameConstants.HEART_SIX, _cardSprites[31] },
            { GameConstants.HEART_SEVEN, _cardSprites[32] },
            { GameConstants.HEART_EIGHT, _cardSprites[33] },
            { GameConstants.HEART_NINE, _cardSprites[34] },
            { GameConstants.HEART_TEN, _cardSprites[35] },
            { GameConstants.HEART_JACK, _cardSprites[36] },
            { GameConstants.HEART_QUEEN, _cardSprites[37] },
            { GameConstants.HEART_KING, _cardSprites[38] },
            { GameConstants.CLUB_ACE, _cardSprites[39] },
            { GameConstants.CLUB_TWO, _cardSprites[40] },
            { GameConstants.CLUB_THREE, _cardSprites[41] },
            { GameConstants.CLUB_FOUR, _cardSprites[42] },
            { GameConstants.CLUB_FIVE, _cardSprites[43] },
            { GameConstants.CLUB_SIX, _cardSprites[44] },
            { GameConstants.CLUB_SEVEN, _cardSprites[45] },
            { GameConstants.CLUB_EIGHT, _cardSprites[46] },
            { GameConstants.CLUB_NINE, _cardSprites[47] },
            { GameConstants.CLUB_TEN, _cardSprites[48] },
            { GameConstants.CLUB_JACK, _cardSprites[49] },
            { GameConstants.CLUB_QUEEN, _cardSprites[50] },
            { GameConstants.CLUB_KING, _cardSprites[51] }
        };


    }

    public Sprite GetCardSprite(string cardName)
    {
        return _cardSpriteData[cardName];
    }
}
