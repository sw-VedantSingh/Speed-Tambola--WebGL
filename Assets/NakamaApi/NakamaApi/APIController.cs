using System;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
//using Go_Rush;

public class APIController : MonoBehaviour
{

    #region REST_API_VARIABLES
    public static APIController instance;
    [Header("==============================================")]

    #endregion
    public PlayerData myPlayerData = new PlayerData();
    public GameData gamedata;
    public Action OnUserDetailsUpdate;
    public bool isWin = false;
    public UserGameData userDetails;
    public List<BetDetails> betDetails = new List<BetDetails>();
    public bool isPlayByDummyData;

    #region GetLoginDetails&Awake


#if UNITY_WEBGL
    [DllImport("__Internal")]
    public static extern void GetLoginData();
    [DllImport("__Internal")]
    public static extern void FullScreen();
    [DllImport("__Internal")]
    public static extern void ShowDeposit();

    [DllImport("__Internal")]
    public static extern void CloseWindow();
    [DllImport("__Internal")]
    public static extern void InitPlayerBet(string type, int index, string game_user_Id, string game_Id, string metaData, bool isAbleToCancel, double bet_amount);

    [DllImport("__Internal")]
    public static extern void AppPlayerBet(string type, int index, string id, string metaData, string game_user_Id, string game_Id, double bet_amount);
    [DllImport("__Internal")]
    public static extern void CancelPlayerBet(string type, string id, string metaData, string game_user_Id, string game_Id, double amount);
    [DllImport("__Internal")]
    public static extern void FinilizePlayerBet(string type, string id, string metadata, string game_user_Id, string game_Id);
    [DllImport("__Internal")]
    public static extern void WinningsPlayerBet(string type, string id, string metadata, string game_user_Id, string game_Id, double win_amount, double spend_amount);





    public void InitPlayerBetResponse(string data)
    {
        BetResponse response = JsonUtility.FromJson<BetResponse>(data);
        if (response.status)
        {
            BetDetails bet = betDetails.Find(x => x.index == response.index);
            bet.betID = response.message;
            bet.Status = BetProcess.Success;

        }
        else
        {
            betDetails.RemoveAll(x => x.index == response.index);
        }
    }
    public void CancelPlayerBetResponse(string data)
    {
        BetResponse response = JsonUtility.FromJson<BetResponse>(data);
        if (betDetails.Exists(x => x.index == response.index))
        {
            BetDetails bet = betDetails.Find(x => x.index == response.index);
            bet.Status = response.status ? BetProcess.Success : BetProcess.Failed;

        }
    }
    public void AddPlayerBetResponse(string data)
    {
        BetResponse response = JsonUtility.FromJson<BetResponse>(data);
        if (betDetails.Exists(x => x.index == response.index))
        {
            BetDetails bet = betDetails.Find(x => x.index == response.index);
            bet.Status = response.status ? BetProcess.Success : BetProcess.Failed;

        }
    }
    public void FinilizePlayerBetResponse(string data)
    {
        BetResponse response = JsonUtility.FromJson<BetResponse>(data);
        if (betDetails.Exists(x => x.index == response.index))
        {
            BetDetails bet = betDetails.Find(x => x.index == response.index);
            bet.Status = response.status ? BetProcess.Success : BetProcess.Failed;
        }

    }
    public void WinningsPlayerBetResponse(string data)
    {
        BetResponse response = JsonUtility.FromJson<BetResponse>(data);
        if (betDetails.Exists(x => x.index == response.index))
        {
            BetDetails bet = betDetails.Find(x => x.index == response.index);
            bet.Status = response.status ? BetProcess.Success : BetProcess.Failed;
        }
    }

#endif

    public void SetUserData(string data)
    {
        Debug.Log("Response from webgl ::::: " + data);
        if (data.Length < 30)
        {
            userDetails = new UserGameData();
            userDetails.balance = 5000;
            userDetails.currency_type = "USD";
            //userDetails.Id = UnityEngine.Random.Range(5000, 500000) + SystemInfo.deviceUniqueIdentifier.ToGuid().ToString();
            //userDetails.token = UnityEngine.Random.Range(5000, 500000) + SystemInfo.deviceUniqueIdentifier.ToGuid().ToString();
            //userDetails.name = SystemInfo.deviceName + SystemInfo.deviceModel;
            userDetails.name = "User_" + UnityEngine.Random.Range(100, 999);
            isPlayByDummyData = true;
        }
        else
        {
            userDetails = JsonUtility.FromJson<UserGameData>(data);
            isPlayByDummyData = userDetails.isBlockApiConnection;
        }
        if (userDetails.bootAmount == 0)
            userDetails.bootAmount = 25;
        Debug.Log(JsonUtility.ToJson(userDetails));
        OnUserDetailsUpdate?.Invoke();
    }

    public void GetLoginDataResponseFromWebGL(string data)
    {
    }
    void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        GetLoginData();
#else
        SetUserData("");
#endif
    }

    private void Awake()
    {
        Debug.Log("$$$ %%%%%%%%%%%%%%%%%%%%%%%%%%");
        instance = this;
    }
    #endregion

    public PlayerData GeneratePlayerDataForBot(string botAccountData)
    {
        BotData account = JsonUtility.FromJson<BotData>(botAccountData);
        PlayerWalletData walletData = JsonUtility.FromJson<PlayerWalletData>(account.wallet);
        PlayerData player = new();
        player.playerID = account.user.id;
        player.playerName = account.user.display_name;
        player.isBot = true;
        ProfilePicture profile = JsonUtility.FromJson<ProfilePicture>(account.user.avatar_url);
        player.profilePicURL = profile.ProfileUrl;
        player.avatarIndex = (int)profile.ProfileType;
        player.gold = (walletData.CashDepositVal / 100) + (walletData.CashDepositVal / 1000);
        player.silver = (walletData.SilverVal / 100);
        player.money = player.gold;
        player.totalWinnings = walletData.NetWinning;
        return player;
    }

    #region API
    int id = 0;

    public int InitlizeBet(double amount, string metadata, bool isAbleToCancel = false)
    {
        if (isPlayByDummyData)
            return 0;
        id += 1;
        if (id > 1000)
            id = 1;
        //var data = new
        //{
        //    type = "InitlizeBet",
        //    Index = id,
        //    Game_user_Id = userDetails.Id,
        //    Game_Id = userDetails.game_Id,
        //    MetaData = metadata,
        //    IsAbleToCancel = isAbleToCancel,
        //    Bet_amount = amount
        //};
        BetDetails bet = new BetDetails();
        bet.index = id;
        bet.betID = id.ToString();
        bet.Status = BetProcess.Processing;
        bet.IsAbleToCancel = isAbleToCancel;
        betDetails.Add(bet);
#if UNITY_WEBGL
        Debug.Log("Init Bet Data");
        InitPlayerBet("InitlizeBet",id,userDetails.Id,userDetails.game_Id,metadata,isAbleToCancel,amount);
#endif
        return id;
    }


    public void AddBet(double amount, string metadata, int index)
    {
        if (isPlayByDummyData)
            return;


        foreach (var item in betDetails)
        {
            Debug.Log(item.betID);
        }

        if (betDetails.Exists(x => x.index == index))
        {
            BetDetails bet = betDetails.Find(x => x.index == index);

            //var data = new
            //{

            //    type = "AddBet",
            //    Index = index,
            //    Id = bet.betID,
            //    MetaData = metadata,
            //    Game_user_Id = userDetails.Id,
            //    Game_Id = userDetails.game_Id,
            //    Bet_amount = amount
            //};
#if UNITY_WEBGL
            Debug.Log("Add Bet Data");
            AppPlayerBet("AddBet",index,bet.betID,metadata,userDetails.Id,userDetails.game_Id,amount);
#endif
        }
    }

    public void CancelBet(int index, string metadata, double amount)
    {
        if (isPlayByDummyData)
            return;
        if (betDetails.Exists(x => x.index == index))
        {
            BetDetails bet = betDetails.Find(x => x.index == index);
            if (!bet.IsAbleToCancel)
            {
                bet.Status = BetProcess.Failed;
                return;
            }
            bet.Status = BetProcess.Processing;
            //var data = new
            //{
            //    type = "CancelBet",
            //    Id = bet.betID,
            //    MetaData = metadata,
            //    Game_user_Id = userDetails.Id,
            //    Game_Id = userDetails.game_Id,
            //};
#if UNITY_WEBGL
            Debug.Log("Cancel Bet Data");
            CancelPlayerBet("CancelBet",bet.betID,metadata,userDetails.Id,userDetails.game_Id,amount);
#endif
        }
    }
    public void FinilizeBet(int index, string metadata)
    {
        if (isPlayByDummyData)
            return;
        if (betDetails.Exists(x => x.index == index))
        {
            BetDetails bet = betDetails.Find(x => x.index == index);
            if (!bet.IsAbleToCancel)
            {
                bet.Status = BetProcess.Failed;
                return;
            }
            bet.Status = BetProcess.Processing;
            //var data = new
            //{
            //    type = "FinilizeBet",
            //    Id = bet.betID,
            //    MetaData = metadata,
            //    Game_user_Id = userDetails.Id,
            //    Game_Id = userDetails.game_Id,
            //};
#if UNITY_WEBGL
            Debug.Log("Finalize Bet Data");
            FinilizePlayerBet("FinilizeBet",bet.betID,metadata,userDetails.Id,userDetails.game_Id);
#endif
        }

    }

    public void WinningsBet(int index, double amount, double spend_amount, string metadata)
    {
        if (isPlayByDummyData)
            return;
        if (betDetails.Exists(x => x.index == index ))
        {
            BetDetails bet = betDetails.Find(x => x.index == index);
            bet.Status = BetProcess.Processing;
            //var data = new
            //{
            //    type = "WinningsBet",
            //    Id = bet.betID,
            //    MetaData = metadata,
            //    Game_user_Id = userDetails.Id,
            //    Game_Id = userDetails.game_Id,
            //    Win_amount = amount,
            //    Spend_amount = amount,
            //};
#if UNITY_WEBGL
            Debug.Log("Winning Bet Data");
            WinningsPlayerBet("WinningsBet",bet.betID,metadata,userDetails.Id,userDetails.game_Id,amount,spend_amount);
#endif
        }
    }

    public BetProcess CheckBetStatus(int index)
    {
        if (isPlayByDummyData)
            return BetProcess.Success;
        if (betDetails.Exists(x => x.index == index))
        {
            BetDetails bet = betDetails.Find(x => x.index == index);
            return bet.Status;
        }
        return BetProcess.Failed;
    }
    #endregion
}

[System.Serializable]
public class UserGameData
{
    public string Id;
    public string name;
    public string token;
    public double balance;
    public string currency_type;
    public string game_Id;
    public bool isBlockApiConnection;
    public double bootAmount;
}


[System.Serializable]
public class BetDetails
{
    public string betID;
    public int index;
    public BetProcess Status;
    public bool IsAbleToCancel;
}

public enum BetProcess
{
    Processing = 0,
    Success = 1,
    Failed = 2,
    None = 3
}

public class BetResponse
{
    public bool status;
    public string message;
    public int index;
}
