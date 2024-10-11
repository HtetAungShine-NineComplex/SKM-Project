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
        public string balance;
    }

    [Serializable]
    public class UserInfoResponse
    {
        public string status;
        public UserInfoData data;
    }
}
