using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shan.API
{
    [Serializable]
    public struct APILoginRequest
    {
        public string phone_number;
        public string password;

        public APILoginRequest(string phone, string password)
        {
            this.phone_number  = phone;
            this.password = password;
        }
    }

    [Serializable]
    public struct LoginResponse
    {
        public string status;
        public LoginData data;
        public string message;
    }

    [Serializable]
    public struct LoginData
    {
        public LoginUser user;
        public string token;
    }

    [Serializable]
    public struct LoginUser
    {
        public int user_id;
        public string phone_number;
        public string name;
        public string balance;
    }

    [Serializable]
    public class UserInfoData
    {
        public int user_id;
        public string phone_number;
        public string name;
        public float balance;
    }

    [Serializable]
    public class UserInfoResponse
    {
        public string status;
        public UserInfoData data;
    }

    [Serializable]
    public class RoomConfigData
    {
        public int id;
        public string name;
        public int min_amount;
        public int max_amount;
        public List<int> bet_amounts;
        public DateTime created_at;
        public DateTime updated_at;
    }

    [Serializable]
    public class RoomConfigResponse
    {
        public string status;
        public List<RoomConfigData> data;
        public string message;
    }

    [Serializable]
    public class BetInfo
    {
        public int bet_amount ;
    }

    [Serializable]
    public class GameHistoryData
    {
        public int id ;
        public string match_id ;
        public int user_id ;
        public GameInfo game_info ;
        public DateTime created_at ;
        public DateTime updated_at ;
    }

    [Serializable]
    public class GameInfo
    {
        public string match_id ;
        public DateTime timestamp ;
        public MatchResult match_result ;
        public List<Participant> participants ;
        public int total_bet_pool ;
    }

    [Serializable]
    public class MatchResult
    {
        public int total_amount_won ;
        public List<int> winning_participant_ids ;
    }

    [Serializable]
    public class Participant
    {
        public Result result ;
        public int user_id ;
        public BetInfo bet_info ;
        public string username ;
        public string user_type ;
        public List<string> card_numbers ;
    }

    [Serializable]
    public class Result
    {
        public string win_loss ;
        public int amount_won_lost ;
    }

    [Serializable]
    public class GameHistoryResponse
    {
        public string status ;
        public List<GameHistoryData> data ;
        public string message ;
    }



}
