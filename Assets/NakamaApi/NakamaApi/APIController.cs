using System;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
using Unity.VisualScripting;

using System.Security.Cryptography;

public class APIController : MonoBehaviour
{

    #region REST_API_VARIABLES
    public static APIController instance;
    [Header("==============================================")]

    #endregion
    public Action OnUserDetailsUpdate;
    public Action OnUserBalanceUpdate;
    public Action OnUserDeposit;
    public bool isWin = false;
    public bool IsBotInGame = true;
    public GameWinningStatus winningStatus;
    public UserGameData userDetails;
    public List<BetDetails> betDetails = new List<BetDetails>();
    public bool isPlayByDummyData;
    public double maxWinAmount;
    public bool isClickDeopsit= false;
    public string testJson;

#if UNITY_WEBGL
    #region WebGl Events

    [DllImport("__Internal")]
    public static extern void GetLoginData();
    [DllImport("__Internal")]
    public static extern void DisconnectGame(string message);
    [DllImport("__Internal")]
    public static extern void GetUpdatedBalance();
    [DllImport("__Internal")]
    public static extern void FullScreen();
    [DllImport("__Internal")]
    private static extern void ShowDeposit();

    [DllImport("__Internal")]
    public static extern void CloseWindow();


    private Action<BotDetails> GetABotAction;
    [DllImport("__Internal")]
    private static extern void GetABot();

    [DllImport("__Internal")]
    private static extern void InitPlayerBet(string type, int index, string game_user_Id, string game_Id, string metaData, string isAbleToCancel, double bet_amount,int isBot);

    [DllImport("__Internal")]
    private static extern void AddPlayerBet(string type, int index, string id, string metaData, string game_user_Id, string game_Id, double bet_amount, int isBot);
    [DllImport("__Internal")]
    private static extern void CancelPlayerBet(string type, string id, string metaData, string game_user_Id, string game_Id, double amount, int isBot);
    [DllImport("__Internal")]
    private static extern void FinilizePlayerBet(string type, string id, string metadata, string game_user_Id, string game_Id, int isBot);
    [DllImport("__Internal")]
    private static extern void WinningsPlayerBet(string type, string id, string metadata, string game_user_Id, string game_Id, double win_amount, double spend_amount, int isBot);

    #endregion

    #region WebGl Response
    [ContextMenu("check json")]
    public void CheckJson()
    {
        InitPlayerBetResponse(testJson);
    }
    public void GetABotResponse(string data)
    {
        Debug.Log("get bot response :::::::----::: " + data);

        BotDetails bot = new BotDetails();
        bot = JsonUtility.FromJson<BotDetails>(data);
        userDetails.isWin = !bot.isWin;
        isWin = !bot.isWin;
        GetABotAction?.Invoke(bot);
        GetABotAction = null;
    }
    public void UpdateBalanceResponse(string data)
    {
        Debug.Log("Balance Updated response  :::::::----::: " + data);
        userDetails.balance = double.Parse(data);
        OnUserBalanceUpdate?.Invoke();
        if (isClickDeopsit)
        {
            OnUserDeposit?.Invoke();
        }
    }

    public void InitPlayerBetResponse(string data)
    {
        Debug.Log("init bet response :::::::----::: "+data);
        InitBetDetails response = JsonUtility.FromJson<InitBetDetails>(data);
        BetDetails bet = betDetails.Find(x => x.index == response.index);
        if (response.status)
        {
            winningStatus = response.message;
            Debug.Log("init bet response :::::::----::: " + response.message);
            Debug.Log("init bet response :::::::----::: " + winningStatus.Id);
            bet.betID = winningStatus.Id;
            bet.Status = BetProcess.Success;
            bet.action?.Invoke(true);
        }
        else
        {
            bet.action?.Invoke(false);
            betDetails.RemoveAll(x => x.index == response.index);
        }
        bet.action = null;
    }

    public void CancelPlayerBetResponse(string data)
    {
        Debug.Log("cancel bet response :::::::----::: " + data);
        BetResponse response = JsonUtility.FromJson<BetResponse>(data);
        if (betDetails.Exists(x => x.index == response.index))
        {
            BetDetails bet = betDetails.Find(x => x.index == response.index);
            if (response.status)
            {
                bet.Status = response.status ? BetProcess.Success : BetProcess.Failed;
                bet.action?.Invoke(true);
            }
            else
            {
                bet.action?.Invoke(false);

            }
            bet.action = null;
        }

    }

    public void AddPlayerBetResponse(string data)
    {
        Debug.Log("add bet response :::::::----::: " + data);
        BetResponse response = JsonUtility.FromJson<BetResponse>(data);
        if (betDetails.Exists(x => x.index == response.index))
        {
            BetDetails bet = betDetails.Find(x => x.index == response.index);
            if (response.status)
            {
                bet.Status = response.status ? BetProcess.Success : BetProcess.Failed;

                bet.action?.Invoke(true);
            }
            else
            {
                bet.action?.Invoke(false);
            }
            bet.action = null;
        }
    }

    public void FinilizePlayerBetResponse(string data)
    {
        BetResponse response = JsonUtility.FromJson<BetResponse>(data);
        if (betDetails.Exists(x => x.index == response.index))
        {
            BetDetails bet = betDetails.Find(x => x.index == response.index);
            if (response.status)
            {
                bet.Status = response.status ? BetProcess.Success : BetProcess.Failed;
                bet.action?.Invoke(true);
            }
            else
            {
                bet.action?.Invoke(false);
            }
            bet.action = null;
        }
    }

    public void WinningsPlayerBetResponse(string data)
    {
        Debug.Log("winning bet response :::::::----::: " + data);
        BetResponse response = JsonUtility.FromJson<BetResponse>(data);
        if (betDetails.Exists(x => x.index == response.index))
        {
            BetDetails bet = betDetails.Find(x => x.index == response.index);
            if (response.status)
            {
                bet.Status = response.status ? BetProcess.Success : BetProcess.Failed;
                bet.action?.Invoke(true);
            }
            else
            {
                bet.action?.Invoke(false);
            }
            bet.action = null;
        }
    }
    #endregion


    public void OnClickDepositBtn()
    {
        isClickDeopsit = true;
        ShowDeposit();
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
            userDetails.Id = UnityEngine.Random.Range(5000, 500000) + SystemInfo.deviceUniqueIdentifier.ToGuid().ToString();
            userDetails.token = UnityEngine.Random.Range(5000, 500000) + SystemInfo.deviceUniqueIdentifier.ToGuid().ToString();
            //userDetails.name = SystemInfo.deviceName + SystemInfo.deviceModel;
            userDetails.name = "User_" + UnityEngine.Random.Range(100, 999);
            isPlayByDummyData = true;
            userDetails.hasBot = true;
            userDetails.isBlockApiConnection = true;

        }
        else
        {
            userDetails = JsonUtility.FromJson<UserGameData>(data);
            isPlayByDummyData = userDetails.isBlockApiConnection;
            isWin = userDetails.isWin;
            maxWinAmount = userDetails.maxWin;
        }
        IsBotInGame = userDetails.hasBot;
        if (userDetails.bootAmount == 0)
            userDetails.bootAmount = 25;
        Debug.Log(JsonUtility.ToJson(userDetails));
        OnUserDetailsUpdate?.Invoke();
        OnUserBalanceUpdate?.Invoke();
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
        instance = this;
    }
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

    public void GetBot(Action<BotDetails> action)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        GetABotAction = action;
        GetABot();
#endif
    }

    public int InitlizeBet(double amount, TransactionMetaData metadata, bool isAbleToCancel = false, Action<bool> action = null, string playerId = "", bool isBot = false)
    {
        if (isPlayByDummyData)
        {
            Debug.Log(amount);
            if(playerId == "" || playerId == userDetails.Id)
            {
                userDetails.balance -= amount;
                OnUserBalanceUpdate.Invoke();
            }
            action?.Invoke(true);
            return 0;
        }
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
        bet.IsAbleToCancel = isAbleToCancel ? "true" : "false";
        betDetails.Add(bet);
#if UNITY_WEBGL
        Debug.Log("Init Bet Data");
        bet.action = action;
        InitPlayerBet("InitlizeBet",id,userDetails.Id, playerId == "" ? userDetails.Id : playerId, JsonUtility.ToJson(metadata),bet.IsAbleToCancel,amount,isBot?1:0);
#endif
        return id;
    }

    public void AddBet(int index, TransactionMetaData metadata, double amount,Action<bool> action = null, string playerId = "", bool isBot = false)
    {
        if (isPlayByDummyData)
        {
            if (playerId == "" || playerId == userDetails.Id)
            {
                userDetails.balance -= amount;
                OnUserBalanceUpdate.Invoke();
            }
            action?.Invoke(true);
            return;
        }

        foreach (var item in betDetails)
        {
            Debug.Log(item.betID);
        }

        if (betDetails.Exists(x => x.index == index))
        {
            BetDetails bet = betDetails.Find(x => x.index == index);

#if UNITY_WEBGL
            Debug.Log("Add Bet Data");
            bet.action = action;
            AddPlayerBet("AddBet",index,bet.betID, JsonUtility.ToJson(metadata), playerId == "" ? userDetails.Id : playerId, userDetails.game_Id,amount, isBot ? 1 : 0);
#endif
        }
    }

    public void CancelBet(int index, string metadata, double amount, Action<bool> action = null, string playerId = "", bool isBot = false)
    {
        if (isPlayByDummyData)
        {
            if (playerId == "" || playerId == userDetails.Id)
            {
                userDetails.balance += amount;
                OnUserBalanceUpdate.Invoke();
            }
            action?.Invoke(true);
            return;
        }

        if (betDetails.Exists(x => x.index == index))
        {
            BetDetails bet = betDetails.Find(x => x.index == index);
            bet.action = action;

            if (bet.IsAbleToCancel == "false")
            {
                bet.Status = BetProcess.Failed;
                return;
            }
            bet.Status = BetProcess.Processing;
#if UNITY_WEBGL
            Debug.Log("Cancel Bet Data");
            CancelPlayerBet("CancelBet",bet.betID,metadata, playerId == "" ? userDetails.Id : playerId, userDetails.game_Id,amount, isBot ? 1 : 0);
#endif
        }
    }
    public void FinilizeBet(int index, TransactionMetaData metadata,Action<bool> action = null, string playerId = "", bool isBot = false)
    {
        if (isPlayByDummyData)
        {
            action?.Invoke(true);
            return;
        }

        if (betDetails.Exists(x => x.index == index))
        {
            BetDetails bet = betDetails.Find(x => x.index == index);
            bet.action = action;
            if (bet.IsAbleToCancel == "false")
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
            FinilizePlayerBet("FinilizeBet",bet.betID, JsonUtility.ToJson(metadata), playerId == "" ? userDetails.Id : playerId, userDetails.game_Id, isBot ? 1 : 0);
#endif
        }

    }

    public void WinningsBet(int index, double amount, double spend_amount, TransactionMetaData metadata,Action<bool> action = null,string playerId = "", bool isBot = false)
    {

    
        if (isPlayByDummyData)
        {
            if (playerId == "" || playerId == userDetails.Id)
            {
                Debug.Log("Winning Bet Data **********");
                userDetails.balance += amount;
                OnUserBalanceUpdate.Invoke();
            }
            action?.Invoke(true);
            return;
        }

        if (betDetails.Exists(x => x.index == index ))
        {
            BetDetails bet = betDetails.Find(x => x.index == index);
            bet.Status = BetProcess.Processing;
            bet.action = action;
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
            WinningsPlayerBet("WinningsBet",bet.betID, JsonUtility.ToJson(metadata), playerId == "" ? userDetails.Id : playerId,userDetails.game_Id,amount,spend_amount, isBot ? 1 : 0);
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


public class InitBetDetails
{
    public bool status;
    public GameWinningStatus message;
    public int index;
}

[System.Serializable]
public class MinMaxOffest
{
    public float min;
    public float max;
}

[System.Serializable]
public class GameWinningStatus
{
    public string Id;
    public double Amount;
    public MinMaxOffest WinCutOff;
    public float WinProbablity;
    public string Game_Id;
    public string Operator;
    public DateTime create_at;
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
    public bool isWin;
    public bool hasBot;
    public float commission;
    public float maxWin;
}

[System.Serializable]
public class TransactionMetaData
{
    public double Amount;
    public string Info;
}
[System.Serializable]
public class BetDetails
{
    public string betID;
    public int index;
    public BetProcess Status;
    public string IsAbleToCancel;
    public Action<bool> action;

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


[System.Serializable]
public class BotDetails
{
    public string Id;
    public string name;
    public double balance;
    public bool isWin;
}
public static class MatchExtensions
{
    public static Guid ToGuid(this string id)
    {
        MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
        byte[] inputBytes = Encoding.Default.GetBytes(id);
        byte[] hashBytes = provider.ComputeHash(inputBytes);

        return new Guid(hashBytes);
    }
}
