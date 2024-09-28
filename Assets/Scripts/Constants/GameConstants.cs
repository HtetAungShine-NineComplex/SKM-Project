using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace THZ
{
    public static class GameConstants
    {
        //Scene names
        public const string LOGIN_SCENE = "Login";
        public const string LOBBY_SCENE = "Lobby";
        public const string ROOM_SCENE = "Room";
        public const string SHAN_ROOM_SCENE = "ShanRoom";


        //Room Settings
        public const string GAME_ROOMS_GROUP_NAME = "games";
        public const int GAME_ROOMS_MAX_USER = 8;

        //Extensions

        //shan
        public const string SHAN_EXTENSION_ID = "SKMProject";
        public const string SHAN_EXTENSION_CLASS = "game.shan.ShanExtension";

        //Game Event Params
        //public const string USER_ID = "userID";
        public const string USER_NAME = "userName";
        public const string USER_NAME_ARRAY = "userNameArray";
        public const string USER_ARRAY = "userArray";

        //Game Event Names
        public const string BOT_JOINED = "botJoined"; //send from client to server
        public const string START_GAME = "startGame"; //send from client to server
        public const string BET_STARTED = "betStarted"; //send from server to client
        public const string GAME_STARTED = "gameStarted"; //send from server to client
        public const string COUNTDOWN = "countdown"; //send from server to client
        public const string OWNER = "owner"; //send from server to client
        public const string BANKER = "banker"; //send from server to client
        public const string START_CURRENT_TURN = "startCurrentTurn"; //send from server
        public const string PLAYER_HIT = "playerHit"; //send from server
        public const string PLAYER_WIN = "playerWin"; //send from server
        public const string PLAYER_LOSE = "playerLose"; //send from server
        public const string PLAYER_TOTAL_VALUE = "playerTotalValue"; //send from server
        public const string PLAYER_HAND_CARDS = "playerHandCards"; //send from server
        public const string HIT = "hit"; //send from client
        public const string STAND = "stand"; //send from client
        public const string ROOM_PLAYER_LIST = "roomPlayerList"; //send from server

        //shan
        public const string PLAYER_DO = "playerDo"; //send from server
        public const string PLAYER_CARD_ARRAY = "playerCardArray"; //send from server
        public const string PLAYER_DRAW = "playerDraw"; //send from server
        public const string DRAW_CARD = "drawCard"; //send from client
        public const string BET = "bet"; //send from client
        public const string PLAYER_BET = "playerBet"; //send from server

        //Data Keys
        //Amount
        public const string BET_AMOUNT = "betAmount";
        public const string TOTAL_AMOUNT = "totalAmount";
        public const string BANK_AMOUNT = "bankAmount";
        public const string AMOUNT_CHANGED = "amountChanged";

        // Card
        public const string CARD_NAME = "cardName";
        public const string CARD_VALUE = "cardValue";
        public const string SUIT = "suit";
        public const string IS_ACE = "isAce";
        public const string IS_DO = "isDo";
        public const string TOTAL_VALUE = "totalValue";
        public const string MODIFIER = "modifier";

        // Card Names
        // Clubs
        public const string CLUB_ACE = "Club Ace";
        public const string CLUB_TWO = "Club 2";
        public const string CLUB_THREE = "Club 3";
        public const string CLUB_FOUR = "Club 4";
        public const string CLUB_FIVE = "Club 5";
        public const string CLUB_SIX = "Club 6";
        public const string CLUB_SEVEN = "Club 7";
        public const string CLUB_EIGHT = "Club 8";
        public const string CLUB_NINE = "Club 9";
        public const string CLUB_TEN = "Club 10";
        public const string CLUB_JACK = "Club Jack";
        public const string CLUB_QUEEN = "Club Queen";
        public const string CLUB_KING = "Club King";

        // Diamonds
        public const string DIAMOND_ACE = "Diamond Ace";
        public const string DIAMOND_TWO = "Diamond 2";
        public const string DIAMOND_THREE = "Diamond 3";
        public const string DIAMOND_FOUR = "Diamond 4";
        public const string DIAMOND_FIVE = "Diamond 5";
        public const string DIAMOND_SIX = "Diamond 6";
        public const string DIAMOND_SEVEN = "Diamond 7";
        public const string DIAMOND_EIGHT = "Diamond 8";
        public const string DIAMOND_NINE = "Diamond 9";
        public const string DIAMOND_TEN = "Diamond 10";
        public const string DIAMOND_JACK = "Diamond Jack";
        public const string DIAMOND_QUEEN = "Diamond Queen";
        public const string DIAMOND_KING = "Diamond King";

        // Hearts
        public const string HEART_ACE = "Heart Ace";
        public const string HEART_TWO = "Heart 2";
        public const string HEART_THREE = "Heart 3";
        public const string HEART_FOUR = "Heart 4";
        public const string HEART_FIVE = "Heart 5";
        public const string HEART_SIX = "Heart 6";
        public const string HEART_SEVEN = "Heart 7";
        public const string HEART_EIGHT = "Heart 8";
        public const string HEART_NINE = "Heart 9";
        public const string HEART_TEN = "Heart 10";
        public const string HEART_JACK = "Heart Jack";
        public const string HEART_QUEEN = "Heart Queen";
        public const string HEART_KING = "Heart King";

        // Spades
        public const string SPADE_ACE = "Spade Ace";
        public const string SPADE_TWO = "Spade 2";
        public const string SPADE_THREE = "Spade 3";
        public const string SPADE_FOUR = "Spade 4";
        public const string SPADE_FIVE = "Spade 5";
        public const string SPADE_SIX = "Spade 6";
        public const string SPADE_SEVEN = "Spade 7";
        public const string SPADE_EIGHT = "Spade 8";
        public const string SPADE_NINE = "Spade 9";
        public const string SPADE_TEN = "Spade 10";
        public const string SPADE_JACK = "Spade Jack";
        public const string SPADE_QUEEN = "Spade Queen";
        public const string SPADE_KING = "Spade King";

    }
}
