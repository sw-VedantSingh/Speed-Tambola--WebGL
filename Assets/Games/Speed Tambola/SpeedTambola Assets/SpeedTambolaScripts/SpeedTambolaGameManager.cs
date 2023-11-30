using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class SpeedTambolaGameManager : MonoBehaviour
{

    public static SpeedTambolaGameManager Instance;
    public SpeedTambolaPool pool;
    public GameObject Parent;
    public Image CallTimer;
    public TMP_Text RemainingTime;
    //public MatchControllerNakama matchController;
    [SerializeField] List<SpeedTambolaButtonRenderer> B_Grid = new();
    [SerializeField] List<SpeedTambolaButtonRenderer> I_Grid = new();
    [SerializeField] List<SpeedTambolaButtonRenderer> N_Grid = new();
    [SerializeField] List<SpeedTambolaButtonRenderer> G_Grid = new();
    [SerializeField] List<SpeedTambolaButtonRenderer> O_Grid = new();
    [SerializeField] List<SpeedTambolaButtonRenderer> First_Row = new();
    [SerializeField] List<SpeedTambolaButtonRenderer> Second_Row = new();
    [SerializeField] List<SpeedTambolaButtonRenderer> Third_Row = new();
    [SerializeField] List<SpeedTambolaButtonRenderer> Fourth_Row = new();
    [SerializeField] List<SpeedTambolaButtonRenderer> Fifth_Row = new();
    [SerializeField] List<SpeedTambolaButtonRenderer> Diagonal1 = new();
    [SerializeField] List<SpeedTambolaButtonRenderer> Diagonal2 = new();
    [SerializeField] List<SpeedTambolaButtonRenderer> Four_Corners = new();
    [SerializeField] float TimerDuration = 15f;
    public float TimerValue;
    [SerializeField] float TotalTimerValue;
    public float TotalTimeRemaining = 120f;
    float FillValue;
    public List<int> shuffleCall = new();
    [HideInInspector] public List<string> Called;
    int count = 0;
    public int points = 0;
    [SerializeField] Button SkipButton;
    public Combinations store = new Combinations();
    public Image IDKMeter;
    public float FillRate = 0.9f;
    public bool BalreadyMarked = false;
    public bool IalreadyMarked = false;
    public bool NalreadyMarked = false;
    public bool GalreadyMarked = false;
    public bool OalreadyMarked = false;
    public bool firstalreadyMarked = false;
    public bool secondalreadyMarked = false;
    public bool thirdalreadyMarked = false;
    public bool fourthalreadyMarked = false;
    public bool fifthalreadyMarked = false;
    public bool Diag1alreadyMarked = false;
    public bool Diag2alreadyMarked = false;
    public bool FourCornersMarked = false;
    [SerializeField] int BingoCount = 0;
    public List<NumberData> Unmarked;
    public List<NumberData> RandomFourNumbers;
    public Button BingoButton;
    public List<List<NumberData>> FinishedCombinationsInOrder = new();
    public GameObject ResultPanel;
    private float MessageTime = 2f;
    public DateTime startTime;
    public DateTime endTime;
    [SerializeField] TMP_Text Balance;
    public string currentAmountType = "EUR";

    [Header("NEW UPDATED")]
    public double playerAmount;
    public TMP_Text showBetAmount;
    public double WinningBetAmt;
    public GameObject Demo_txt;
    public string PlayerID;
    public string playerName;

    public TMP_Text Currency_Txt;
    public TMP_Text playerAmountTxt;
    public double betAmountIs;
    public TMP_Text WinnerBetAmt_Txt;
    public TMP_Text LoserBetAmt_Txt;
    public int betIndex;
    public bool IsGameFinished;
    public AudioSource BingoSuccess;
    public AudioSource BingoFail;
    public AudioSource ButtonClick;
    public bool IsGamestart;
    void Awake()
    {
        Instance = this;


        pool.TokenPool();

    }
    public DateTime val;
    void Start()
    {
        val = DateTime.Now;
        endTime = val.AddSeconds(500000000);
        Debug.Log("$$$ ##########################");
        //APIController.instance.OnUserDetailsUpdate += SubscribeToEvent;
        APIController.instance.OnUserDetailsUpdate += InitPlayerDetails;
        APIController.instance.OnUserBalanceUpdate += InitAmountDetails;
        APIController.instance.OnUserDeposit += OnDepositeActionCall;
        InitialSpawningNum();
        Invoke(nameof(InitialBetInGame), 2f);
    }
    public void InitialSpawningNum()
    {
        if (SpeedTambolaGameController.Controller.CentralTimer )
        {

            SpawnDigits(1, 15, 5, B_Grid, store.b_list, 1);
            SpawnDigits(16, 30, 5, I_Grid, store.i_list, 2);
            SpawnDigits(31, 45, 4, N_Grid, store.n_list, 3);
            SpawnDigits(46, 60, 5, G_Grid, store.g_list, 4);
            SpawnDigits(61, 75, 5, O_Grid, store.o_list, 5);
            SpawnShuffled();
            timedelay(count);
            /*new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, startTime.Minute + Mathf.FloorToInt(TotalTimeRemaining / 60), startTime.Second + (int)(TotalTimeRemaining % 60));*/
            //Debug.Log($"End Time: {endTime}");
            //Debug.Log($"Start Time: {startTime}");
        }


    }

    private void InitialBetInGame()
    {

        TransactionMetaData _metaData = new TransactionMetaData();
        _metaData.Info = "Initial Bet";
        _metaData.Amount = APIController.instance.userDetails.bootAmount;

        betIndex = APIController.instance.InitlizeBet(APIController.instance.userDetails.bootAmount, _metaData, false, (success) =>
        {
            if (success)
            {
                Debug.Log("initialbet Success....");
            }
            else
            {
                Debug.Log("initialbet failed....");

            }
        });
    }
    public void OnDepositeActionCall()
    {
        SpeedTambolaGameManager.Instance.ResultPanel.SetActive(false);

        MatchMakingTambola.Instance.Showmatching();
        SpeedTambolaGameManager.Instance.RestartGame();
    }
    public void WinningAmtCalculation()
    {
        WinningBetAmt = (APIController.instance.userDetails.bootAmount * 2);
        // if (playerwin)
        // {
        WinnerBetAmt_Txt.text = WinningBetAmt.ToString("");
        LoserBetAmt_Txt.text = "0";
        // }










    }

    public void InitPlayerDetails()
    {


        PlayerID = APIController.instance.userDetails.Id;

        playerName = APIController.instance.userDetails.name;

        Debug.Log(" User_Name : " + APIController.instance.userDetails.name + "......" + "UserId : " + APIController.instance.userDetails.name);

        SpeedTambolaGameController.Controller.SettingsPanel.playerName.text = playerName;
        if (APIController.instance.userDetails.isBlockApiConnection)
        {
            Demo_txt.SetActive(true);
        }
        else
        {
            Demo_txt.SetActive(false);
        }
        MatchMakingTambola.Instance.Showmatching();
    }
    public void InitAmountDetails()
    {
        // betAmountIs = APIController.instance.userDetails.bootAmount;
        Currency_Txt.text = APIController.instance.userDetails.currency_type;

        Debug.Log($"CURRENT_AMOUNT ----- {currentAmountType}");

        playerAmount = APIController.instance.userDetails.balance;
        playerAmountTxt.text = $"{playerAmount:F2} ";
        showBetAmount.text = $"{betAmountIs * 0.01:F2} {currentAmountType}";
        //string message = "Initialize Bet";
        //TransactionMetaData val = new();
        //val.Amount = betAmountIs;
        //val.Info = message;
        //betIndex = APIController.instance.InitlizeBet((betAmountIs * 0.01f), val, false, (Success) =>
        //{

        //    if (Success)
        //    {

        //    }
        //    else
        //    {
        //        // MiniRouletteUIController.instance.ExitWebGL();
        //        Debug.Log(".......");

        //    }

        //});
    }

    public void SubscribeToEvent()
    {
        Balance.text = $"{APIController.instance.userDetails.balance:F2} ";
        // showBetAmount= APIController.instance.userDetails.bootAmount;

        //  Currency_Txt.text = APIController.instance.userDetails.currency_type;

    }

    //void OnEnable()
    //{
    //    //ShowMe();
    //    // Invoke(nameof(RestartGame), 1 / 60);

    //}
    private void OnDisable()
    {
        APIController.instance.OnUserDetailsUpdate -= SubscribeToEvent;
    }

    // private void OnDisable()
    // {
    // 	HideMe();
    // }

    // public override void ShowMe()
    // {
    // 	gameObject.SetActive(true);
    // 	UIController.instance.AddToOpenPages(this);
    // }

    // public override void HideMe()
    // {
    // 	UIController.instance.RemoveFromOpenPages(this);
    // }

    // public override void OnBack()
    // {
    // 	matchController.ExitGame();
    // }
   // public int count;
    public void RestartGame()
    {
        MatchMakingTambola.Instance.count = 4;
       MatchMakingTambola.Instance.UpdateWaitingLobby(MatchMakingTambola.Instance.count);

        if (APIController.instance.userDetails.balance <= 0)
        {
            SpeedTambolaGameController.Controller.ShowInsufficientPopUp();
        }
        TransactionMetaData metadata = new TransactionMetaData();
        metadata.Amount = WinningBetAmt;
        metadata.Info = "Undecided";
        APIController.instance.WinningsBet(betIndex, WinningBetAmt, APIController.instance.userDetails.bootAmount, metadata);
        SpeedTambolaScoreManager.ScoreInstance.isAbilityActive = false;
        SpeedTambolaScoreManager.ScoreInstance.flag = false;
        // ResultPanel.SetActive(false);
        IsGameFinished = false;
        RemainingTime.color = Color.white;
        MessageTime = 2f;
        BingoCount = 0;
        TotalTimerValue = TotalTimeRemaining;
        SpeedTambolaScoreManager.ScoreInstance.AbilityAndScoreUpdates.transform.GetChild(1).GetComponent<Image>().fillAmount = 1.0f;
        SpeedTambolaScoreManager.ScoreInstance.abilityDuration = SpeedTambolaScoreManager.ScoreInstance.buffer;
        SpeedTambolaScoreManager.ScoreInstance.objectIndex = 0;
        SpeedTambolaGameController.Controller.CentralTimer = true;
        SpeedTambolaGameController.Controller.Gameplay = true;
        IsGameStart = false;

        SpeedTambolaScoreManager.ScoreInstance.DisableAbilities();
        SpeedTambolaScoreManager.ScoreInstance.ButtonGridBottom.SetActive(true);
        SpeedTambolaScoreManager.ScoreInstance.UnusedAbilities.Clear();
        IDKMeter.fillAmount = 0.0f;
        points -= points;
        SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
        shuffleCall.Clear();
        ResetBingoMarksAndLists();
        SpawnShuffled();
        SkipButton.interactable = true;
        for (int i = Parent.transform.childCount - 1; i >= 0; i--) Parent.transform.GetChild(i).SetParent(pool.PoolContainer.transform);
        timedelay(count = 0);
        DisableStars(B_Grid);
        DisableStars(I_Grid);
        DisableStars(N_Grid);
        DisableStars(G_Grid);
        DisableStars(O_Grid);
        SpawnDigits(1, 15, 5, B_Grid, store.b_list, 1);
        SpawnDigits(16, 30, 5, I_Grid, store.i_list, 2);
        SpawnDigits(31, 45, 4, N_Grid, store.n_list, 3);
        SpawnDigits(46, 60, 5, G_Grid, store.g_list, 4);
        SpawnDigits(61, 75, 5, O_Grid, store.o_list, 5);
        foreach (Image i in SpeedTambolaGameController.Controller.MatchResult) i.gameObject.SetActive(false);
        SpeedTambolaGameController.Controller.CentralTimer = true;
        SpeedTambolaGameController.Controller.Gameplay = true;
        TimerValue = TimerDuration;
        SpeedTambolaScoreManager.ScoreInstance.AbilityAndScoreUpdates.SetActive(false);
        SpeedTambolaScoreManager.ScoreInstance.Notification.SetActive(false);

    }
    void ResetBingoMarksAndLists()
    {
        BalreadyMarked = false;
        IalreadyMarked = false;
        NalreadyMarked = false;
        GalreadyMarked = false;
        OalreadyMarked = false;
        firstalreadyMarked = false;
        secondalreadyMarked = false;
        thirdalreadyMarked = false;
        fourthalreadyMarked = false;
        fifthalreadyMarked = false;
        Diag1alreadyMarked = false;
        Diag2alreadyMarked = false;
        FourCornersMarked = false;
        store.firstRow_list.Clear();
        store.secondRow_list.Clear();
        store.thirdRow_list.Clear();
        store.fourthRow_list.Clear();
        store.fifthRow_list.Clear();
        store.Diagonal1_list.Clear();
        store.Diagonal2_list.Clear();
        store.FourCorners_list.Clear();
        FinishedCombinationsInOrder.Clear();
    }
    public bool IsGameStart;
    void Update()
    {
        if (SpeedTambolaGameController.Controller.Gameplay && SpeedTambolaGameController.Controller.CentralTimer && IsGameStart) 
            UpdateTimer();
            UpdateCentralTimer();
        //if (SpeedTambolaGameController.Controller.CentralTimer)
    }

    public void SpawnDigits(int min, int max, int amt, List<SpeedTambolaButtonRenderer> grid, List<NumberData> combination, int columnIndex)
    {
        foreach (SpeedTambolaButtonRenderer button in grid)
        {
            button.cell.interactable = true;
            button.cell.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(0, 0, 0, 255);
        }
        foreach (SpeedTambolaButtonRenderer btn in grid)
        {
            btn.cell.interactable = true;
            //btn.cell.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
        }
        combination.Clear();
        int buffer;
        for (int i = 0; i < amt; i++)
        {
        Repeat: buffer = UnityEngine.Random.Range(min, max);
            if (!grid.Exists(x => x.value.text.Contains(buffer.ToString())))
            {
                //Debug.Log($"{grid[i].name}, {grid[i] == null} && {grid[i].GetComponentInParent<SpeedTambolaButtonRenderer>() == null}", grid[i].gameObject);
                grid[i].value.text = buffer.ToString();
                grid[i].AssignedNumber = buffer;
                combination.Add(new(buffer, false));
            }
            else goto Repeat;
        }
        StoreRowsAndColumns(combination, columnIndex);
    }

    public void StoreRowsAndColumns(List<NumberData> columns, int columnIndex)
    {
        if (columns.Count == 4)
        {
            store.firstRow_list.Add(columns[0]);
            store.secondRow_list.Add(columns[1]);
            store.fourthRow_list.Add(columns[2]);
            store.fifthRow_list.Add(columns[3]);
        }
        else
        {
            store.firstRow_list.Add(columns[0]);
            store.secondRow_list.Add(columns[1]);
            store.thirdRow_list.Add(columns[2]);
            store.fourthRow_list.Add(columns[3]);
            store.fifthRow_list.Add(columns[4]);
        }

        switch (columnIndex)
        {
            case 1:
                store.Diagonal1_list.Add(columns[0]);
                store.Diagonal2_list.Add(columns[4]);
                store.FourCorners_list.Add(columns[0]);
                store.FourCorners_list.Add(columns[4]);
                break;
            case 2:
                store.Diagonal1_list.Add(columns[1]);
                store.Diagonal2_list.Add(columns[3]);
                break;
            case 4:
                store.Diagonal1_list.Add(columns[3]);
                store.Diagonal2_list.Add(columns[1]);
                break;
            case 5:
                store.Diagonal1_list.Add(columns[4]);
                store.Diagonal2_list.Add(columns[0]);
                store.FourCorners_list.Add(columns[4]);
                store.FourCorners_list.Add(columns[0]);
                break;
        }
    }

    public void SpawnShuffled()
    {
        //initial value assignment to the shuffled array
        for (int i = 0; i <= 75; i++)
        {
            if (i == 0) continue;
            else shuffleCall.Add(i);
        }
        int buffer, temp;
        //the values inside the array getting shuffled down here
        for (int i = 0; i < 75; i++)
        {
            buffer = UnityEngine.Random.Range(1, 75);
            temp = shuffleCall[buffer];
            shuffleCall[buffer] = shuffleCall[i];
            shuffleCall[i] = temp;
        }
    }

    public void timedelay(int count)
    {
        IsGamestart= true;
        GameObject clone = pool.GetToken();

        clone.transform.SetParent(Parent.transform, false);
        clone.transform.localScale = Vector3.one;
        clone.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Bold;//-----------------------------> *Primery sprites font style

        if (shuffleCall[count] >= 0 && shuffleCall[count] <= 15)
        {
            clone.GetComponentInChildren<TextMeshProUGUI>().text = $"B \n{shuffleCall[count].ToString()}";
            clone.GetComponentInChildren<Image>().sprite = SpeedTambolaScoreManager.ScoreInstance.PrimarySprites[0];
            clone.GetComponentInChildren<Image>().SetNativeSize();
            clone.GetComponentInChildren<Image>().color = new Color32(255, 255, 255, 255);
        }
        else if (shuffleCall[count] >= 16 && shuffleCall[count] <= 30)
        {
            clone.GetComponentInChildren<TextMeshProUGUI>().text = $"I \n{shuffleCall[count].ToString()}";
            clone.GetComponentInChildren<Image>().sprite = SpeedTambolaScoreManager.ScoreInstance.PrimarySprites[1];
            clone.GetComponentInChildren<Image>().SetNativeSize();
            clone.GetComponentInChildren<Image>().color = new Color32(255, 255, 255, 255);
        }
        else if (shuffleCall[count] >= 31 && shuffleCall[count] <= 45)
        {
            clone.GetComponentInChildren<TextMeshProUGUI>().text = $"N \n{shuffleCall[count].ToString()}";
            clone.GetComponentInChildren<Image>().sprite = SpeedTambolaScoreManager.ScoreInstance.PrimarySprites[2];
            clone.GetComponentInChildren<Image>().SetNativeSize();
            clone.GetComponentInChildren<Image>().color = new Color32(255, 255, 255, 255);
        }
        else if (shuffleCall[count] >= 46 && shuffleCall[count] <= 60)
        {
            clone.GetComponentInChildren<TextMeshProUGUI>().text = $"G \n{shuffleCall[count].ToString()}";
            clone.GetComponentInChildren<Image>().sprite = SpeedTambolaScoreManager.ScoreInstance.PrimarySprites[3];
            clone.GetComponentInChildren<Image>().SetNativeSize();
            clone.GetComponentInChildren<Image>().color = new Color32(255, 255, 255, 255);
        }
        else if (shuffleCall[count] >= 61 && shuffleCall[count] <= 75)
        {
            clone.GetComponentInChildren<TextMeshProUGUI>().text = $"O \n{shuffleCall[count].ToString()}";
            clone.GetComponentInChildren<Image>().sprite = SpeedTambolaScoreManager.ScoreInstance.PrimarySprites[4];
            clone.GetComponentInChildren<Image>().SetNativeSize();
            clone.GetComponentInChildren<Image>().color = new Color32(255, 255, 255, 255);
        }
        Called.Add(shuffleCall[count].ToString());
        for (int i = 0; i < Parent.transform.childCount; i++)
        {
            if (i != Parent.transform.childCount - 1)
            {
                if (Parent.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text[0] == 'B')
                    Parent.transform.GetChild(i).GetComponentInChildren<Image>().sprite = SpeedTambolaScoreManager.ScoreInstance.SecondarySprites[0];
                if (Parent.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text[0] == 'I')
                    Parent.transform.GetChild(i).GetComponentInChildren<Image>().sprite = SpeedTambolaScoreManager.ScoreInstance.SecondarySprites[1];
                if (Parent.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text[0] == 'N')
                    Parent.transform.GetChild(i).GetComponentInChildren<Image>().sprite = SpeedTambolaScoreManager.ScoreInstance.SecondarySprites[2];
                if (Parent.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text[0] == 'G')
                    Parent.transform.GetChild(i).GetComponentInChildren<Image>().sprite = SpeedTambolaScoreManager.ScoreInstance.SecondarySprites[3];
                if (Parent.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text[0] == 'O')
                    Parent.transform.GetChild(i).GetComponentInChildren<Image>().sprite = SpeedTambolaScoreManager.ScoreInstance.SecondarySprites[4];

                // Parent.transform.GetChild(i).GetComponentInChildren<Image>().color = new Color32(255, 255, 255, 50);
                //Parent.transform.GetChild(i).GetChild(0).GetComponentInChildren<TMP_Text>().color = new Color32(255, 255, 255, 100);
                Parent.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Normal;     ////--------------------------->*secondry sprites font style

                Parent.transform.GetChild(i).GetComponentInChildren<Image>().SetNativeSize();
            }
        }
        //Parent.transform.GetChild();
        if (count >= 3)
        {
            //transform the first created obj to objpool
            GameObject cl = Parent.transform.GetChild(0).gameObject;
            cl.transform.SetParent(pool.PoolContainer.transform);
        }
        Debug.Log(count);
    }

    public void SkipCall()
    {
        if (SpeedTambolaGameController.Controller.Gameplay && SpeedTambolaGameController.Controller.CentralTimer)
        {
            TimerValue = 0;
            SkipButton.interactable = false;
            // RectTransform animImage = SkipButton.transform.GetChild(0).GetComponent<RectTransform>();
            // animImage.DOKill();
            // DOTween.Sequence().Append(animImage.DOLocalMoveX(250, 0.25f).OnComplete(() => animImage.localPosition = new(-250, 0))).Append(animImage.DOLocalMoveX(0, 0.25f));
            StartCoroutine(CoolDown());
        }
    }

    public IEnumerator CoolDown()
    {
        BingoButton.interactable = false;
        SkipButton.interactable = false;
        yield return new WaitForSeconds(0.5f);
        BingoButton.interactable = true;
        SkipButton.interactable = true;
    }

    public void UpdateTimer()
    {
        IsGamestart= false;
        TimerValue -= Time.deltaTime;
        FillValue = TimerValue / TimerDuration;
        CallTimer.fillAmount = FillValue;

        if (TimerValue <= 0 && count < 74)
        {
            TimerValue = TimerDuration;
            count++;
            timedelay(count);
        }

        if (count >= 74 && TimerValue <= 0)
        {
            //insert condition for traversing through all seventy five numbers
            /* Debug.Log("Ran through all 75 numbers");
			SpeedTambolaGameController.Controller.matchController.GameOverLocalPlayer();
			SpeedTambolaGameController.Controller.CentralTimer = false;
			SpeedTambolaGameController.Controller.Gameplay = false; */
            TotalTimerValue = 0;
        }
    }

    DateTime dateTimeOnPause;
    private void OnApplicationPause(bool pause)
    {
        if (SpeedTambolaGameController.Controller.CentralTimer)
        {
            if (pause) dateTimeOnPause = DateTime.Now;
            else TotalTimerValue -= (float)(DateTime.Now - dateTimeOnPause).TotalSeconds;
        }
    }

    public void UpdateCentralTimer()
    {
        TimeSpan remainingTime = endTime - DateTime.Now;

        ;

        //remainingTime.Minutes;
        //remainingTime.Seconds;

        float parse = Mathf.FloorToInt(TotalTimerValue / 60);
        float seconds = Mathf.FloorToInt(TotalTimerValue % 60);
        RemainingTime.text = string.Format("{0:00}:{1:00}", remainingTime.Minutes, remainingTime.Seconds);
        
        float blink = 0;
        float time = (remainingTime.Minutes * 60) + remainingTime.Seconds;
        Debug.Log($"Remaining Time: {time}");
        if (time <= 10)
        {
            blink = Mathf.PingPong((float)Time.timeSinceLevelLoad * 5, 1);
            RemainingTime.color = new Color(1, blink, blink, 1);
        }
        if (time <= 0)
        {
            Debug.Log("Time is up");

            //UIController.instance.ShowLoadingScreen();

            SpeedTambolaGameController.Controller.CentralTimer = false;

                ResultPanel.SetActive(true);
                Debug.Log("Time is up 2");
                SpeedTambolaGameController.Controller.Gameplay = false;

            SpeedTambolaGameController.Controller.Results();






        }
    }
    // if (CommonWaitingHandler.instance)
    // 	CommonWaitingHandler.instance.UpdateGameTimer(RemainingTime.text);
    // if (TotalTimerValue <= 20 && MessageTime > 0)
    // {
    // 	// SpeedTambolaScoreManager.ScoreInstance.Label.gameObject.SetActive(true);
    // 	// SpeedTambolaScoreManager.ScoreInstance.Label.text = "Last 20 Seconds!";
    // 	MessageTime -= Time.deltaTime;

    // 	if (MessageTime <= 0)
    // 	{
    // 		// SpeedTambolaScoreManager.ScoreInstance.Label.gameObject.SetActive(false);
    // 		// SpeedTambolaScoreManager.ScoreInstance.Label.text = string.Empty;
    // 		MessageTime = -1;
    // 	}
    // }

    public void OnButtonPress(int buttonValue)
    {
        if (store.b_list.Exists(x => x.number == buttonValue)) store.b_list.Find(x => x.number == buttonValue).isMarked = true;
        if (store.i_list.Exists(x => x.number == buttonValue)) store.i_list.Find(x => x.number == buttonValue).isMarked = true;
        if (store.n_list.Exists(x => x.number == buttonValue)) store.n_list.Find(x => x.number == buttonValue).isMarked = true;
        if (store.g_list.Exists(x => x.number == buttonValue)) store.g_list.Find(x => x.number == buttonValue).isMarked = true;
        if (store.o_list.Exists(x => x.number == buttonValue)) store.o_list.Find(x => x.number == buttonValue).isMarked = true;
        if (store.firstRow_list.Exists(x => x.number == buttonValue)) store.firstRow_list.Find(x => x.number == buttonValue).isMarked = true;
        if (store.secondRow_list.Exists(x => x.number == buttonValue)) store.secondRow_list.Find(x => x.number == buttonValue).isMarked = true;
        if (store.thirdRow_list.Exists(x => x.number == buttonValue)) store.thirdRow_list.Find(x => x.number == buttonValue).isMarked = true;
        if (store.fourthRow_list.Exists(x => x.number == buttonValue)) store.fourthRow_list.Find(x => x.number == buttonValue).isMarked = true;
        if (store.fifthRow_list.Exists(x => x.number == buttonValue)) store.fifthRow_list.Find(x => x.number == buttonValue).isMarked = true;
        if (store.Diagonal1_list.Exists(x => x.number == buttonValue)) store.Diagonal1_list.Find(x => x.number == buttonValue).isMarked = true;
        if (store.Diagonal2_list.Exists(x => x.number == buttonValue)) store.Diagonal2_list.Find(x => x.number == buttonValue).isMarked = true;
        if (store.FourCorners_list.Exists(x => x.number == buttonValue)) store.FourCorners_list.Find(x => x.number == buttonValue).isMarked = true;
    }

    /* public void AnimateStars(bool state, List<SpeedTambolaButtonRenderer> Grid)
	{
		foreach (SpeedTambolaButtonRenderer i in Grid)
		{
			i.cell.transform.GetChild(1).gameObject.SetActive(state);
			i.cell.transform.GetChild(1).transform.rotation = Quaternion.identity;
			i.cell.transform.GetChild(1).transform.DORotate(new Vector3(0, 180, 0), 0.5f, RotateMode.Fast);
			//Debug.Log($"Call {i}");
		}
	} */

    private Coroutine BingoRoutine;
    public void ClaimBingo()
    {
        Debug.Log("Claiming Bingo");
        // StartCoroutine(nameof(CoolDown));
        //BingoButton.interactable = false;
        if (BingoRoutine == null)
        {
            Debug.Log("Bingo Routine Validated");
            BingoRoutine = StartCoroutine(PerformBingo());
        }
    }

    public void DisableStars(List<SpeedTambolaButtonRenderer> Grid)
    {
        foreach (SpeedTambolaButtonRenderer i in Grid)
        {
            i.cell.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    Coroutine test1;

    private IEnumerator PerformBingo()
    {
        Debug.Log("Bingo Coroutine Begins");
        if (FinishedCombinationsInOrder.Count > 0)
        {
            
            Debug.Log("List count greater than 0");
            Debug.Log("Traversing through list");
            List<SpeedTambolaButtonRenderer> comboList = FinishedCombinationsInOrder[0] == store.firstRow_list ? First_Row : FinishedCombinationsInOrder[0] == store.secondRow_list ? Second_Row : FinishedCombinationsInOrder[0] == store.thirdRow_list ? Third_Row : FinishedCombinationsInOrder[0] == store.fourthRow_list ? Fourth_Row : FinishedCombinationsInOrder[0] == store.fifthRow_list ? Fifth_Row : FinishedCombinationsInOrder[0] == store.b_list ? B_Grid : FinishedCombinationsInOrder[0] == store.i_list ? I_Grid : FinishedCombinationsInOrder[0] == store.n_list ? N_Grid : FinishedCombinationsInOrder[0] == store.g_list ? G_Grid : FinishedCombinationsInOrder[0] == store.o_list ? O_Grid : FinishedCombinationsInOrder[0] == store.Diagonal1_list ? Diagonal1 : FinishedCombinationsInOrder[0] == store.Diagonal2_list ? Diagonal2 : Four_Corners;

            for (int i = 0; i < comboList.Count; i++)
            {
                Debug.Log("Animating Buttons");
                comboList[i].cell.transform.GetChild(1).gameObject.SetActive(true);
                comboList[i].cell.transform.GetChild(1).transform.rotation = Quaternion.identity;
                comboList[i].cell.transform.GetChild(1).transform.DORotate(new Vector3(0, 180, 0), 0.25f, RotateMode.Fast);
                yield return new WaitForSeconds(0.10f);
            }

            if (SpeedTambolaScoreManager.ScoreInstance.is2x)
            {
                points += 1000;
                if (test1 != null) StopCoroutine(test1);
                test1 = StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("Bingo!", 1000, '+', SpeedTambolaGameManager.Instance.BingoSuccess));
                SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
                //BingoSuccess.Play();
            }

            else
            {
                points += 500;
                if (test1 != null) StopCoroutine(test1);
                test1 = StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("Bingo!", 500, '+', SpeedTambolaGameManager.Instance.BingoSuccess));
                SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
                //BingoSuccess.Play();
            }
            if (!firstalreadyMarked) firstalreadyMarked = comboList == First_Row;
            if (!secondalreadyMarked) secondalreadyMarked = comboList == Second_Row;
            if (!thirdalreadyMarked) thirdalreadyMarked = comboList == Third_Row;
            if (!fourthalreadyMarked) fourthalreadyMarked = comboList == Fourth_Row;
            if (!fifthalreadyMarked) fifthalreadyMarked = comboList == Fifth_Row;
            if (!BalreadyMarked) BalreadyMarked = comboList == B_Grid;
            if (!IalreadyMarked) IalreadyMarked = comboList == I_Grid;
            if (!NalreadyMarked) NalreadyMarked = comboList == N_Grid;
            if (!GalreadyMarked) GalreadyMarked = comboList == G_Grid;
            if (!OalreadyMarked) OalreadyMarked = comboList == O_Grid;
            if (!Diag1alreadyMarked) Diag1alreadyMarked = comboList == Diagonal1;
            if (!Diag2alreadyMarked) Diag2alreadyMarked = comboList == Diagonal2;
            if (!FourCornersMarked) FourCornersMarked = comboList == Four_Corners;
            FinishedCombinationsInOrder.RemoveAt(0);
            BingoCount++;
            if (BingoCount == 13)
            {
                Debug.Log("Claimed all possible combinations");
                //SpeedTambolaGameController.Controller.matchController.GameOverLocalPlayer();
                SpeedTambolaGameController.Controller.Gameplay = false;
                SpeedTambolaGameController.Controller.CentralTimer = true;
                //SpeedTambolaGameController.Controller.Results();
            }
        }
        else
        {
            //Debug.Log("Deducting Points");
            if (SpeedTambolaScoreManager.ScoreInstance.TotalScore >= 500)
            {
                points -= 500;
                StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("False Claim!", 500, '-', SpeedTambolaGameManager.Instance.BingoFail));
                SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
                //BingoFail.Play();
            }
            else if (SpeedTambolaScoreManager.ScoreInstance.TotalScore <= 500)
            {
                points -= points;
                StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("False Claim!", 500, '-', SpeedTambolaGameManager.Instance.BingoFail));
                SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
                //BingoFail.Play();
            }
            
        }
        //insert condition for claiming all possible combinations
        BingoRoutine = null;
    }

    /* public void Bingo()
	{
		// Yay.text = $"B Column: {isBingo(store.b_list, store.b_list.Count)}\n I Column: {isBingo(store.i_list, store.i_list.Count)}\n N Column: {isBingo(store.n_list, store.n_list.Count)}\n G Column: {isBingo(store.g_list, store.g_list.Count)}\n O Column: {isBingo(store.o_list, store.o_list.Count)}\n First Row: {isBingo(store.firstRow_list, store.firstRow_list.Count)}\n Second Row: {isBingo(store.secondRow_list, store.secondRow_list.Count)}\n Third Row: {isBingo(store.thirdRow_list, store.thirdRow_list.Count)}\n Fourth Row: {isBingo(store.fourthRow_list, store.fourthRow_list.Count)}\n Fifth Row: {isBingo(store.fifthRow_list, store.fifthRow_list.Count)}\n Diagonal 1: {isBingo(store.Diagonal1_list, store.Diagonal1_list.Count)}\n Diagonal 2 Row: {isBingo(store.Diagonal2_list, store.Diagonal2_list.Count)}";
		if (FinishedCombinationsInOrder[0] == store.firstRow_list && !firstalreadyMarked)
		{
			if (SpeedTambolaScoreManager.ScoreInstance.is2x) points += 1000;
			else points += 500;
			SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
			FinishedCombinationsInOrder.Remove(store.firstRow_list);
			StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("BINGO!", 500, '+'));
			Debug.Log("First Row");
			firstalreadyMarked = true;
			AnimateStars(true, First_Row);
			return;
		}
		if (FinishedCombinationsInOrder[0] == store.secondRow_list && !secondalreadyMarked)
		{
			if (SpeedTambolaScoreManager.ScoreInstance.is2x) points += 1000;
			else points += 500;
			SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
			StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("BINGO!", 500, '+'));
			Debug.Log("Second Row");
			secondalreadyMarked = true;
			AnimateStars(true, Second_Row);
			return;
		}
		if (FinishedCombinationsInOrder[0] == store.thirdRow_list && !thirdalreadyMarked)
		{
			if (SpeedTambolaScoreManager.ScoreInstance.is2x) points += 1000;
			else points += 500;
			SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
			StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("BINGO!", 500, '+'));
			Debug.Log("Third Row");
			thirdalreadyMarked = true;
			AnimateStars(true, Third_Row);
			return;
		}
		if (FinishedCombinationsInOrder[0] == store.fourthRow_list && !fourthalreadyMarked)
		{
			if (SpeedTambolaScoreManager.ScoreInstance.is2x) points += 1000;
			else points += 500;
			SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
			StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("BINGO!", 500, '+'));
			Debug.Log("Fourth Row");
			fourthalreadyMarked = true;
			AnimateStars(true, Fourth_Row);
			return;
		}
		if (FinishedCombinationsInOrder[0] == store.fifthRow_list && !fifthalreadyMarked)
		{
			if (SpeedTambolaScoreManager.ScoreInstance.is2x) points += 1000;
			else points += 500;
			SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
			StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("BINGO!", 500, '+'));
			Debug.Log("Fifth Row");
			fifthalreadyMarked = true;
			AnimateStars(true, Fifth_Row);
			return;
		}
		if (FinishedCombinationsInOrder[0] == store.b_list && !BalreadyMarked)
		{
			if (SpeedTambolaScoreManager.ScoreInstance.is2x) points += 1000;
			else points += 500;
			SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
			StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("BINGO!", 500, '+'));
			Debug.Log("B");
			BalreadyMarked = true;
			AnimateStars(true, B_Grid);
		}
		if (FinishedCombinationsInOrder[0] == store.i_list && !IalreadyMarked)
		{
			if (SpeedTambolaScoreManager.ScoreInstance.is2x) points += 1000;
			else points += 500;
			SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
			StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("BINGO!", 500, '+'));
			Debug.Log("I");
			IalreadyMarked = true;
			AnimateStars(true, I_Grid);
			return;
		}
		if (FinishedCombinationsInOrder[0] == store.n_list && !NalreadyMarked)
		{
			if (SpeedTambolaScoreManager.ScoreInstance.is2x) points += 1000;
			else points += 500;
			SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
			StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("BINGO!", 500, '+'));
			Debug.Log("N");
			NalreadyMarked = true;
			AnimateStars(true, N_Grid);
			return;
		}
		if (FinishedCombinationsInOrder[0] == store.g_list && !GalreadyMarked)
		{
			if (SpeedTambolaScoreManager.ScoreInstance.is2x) points += 1000;
			else points += 500;
			SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
			StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("BINGO!", 500, '+'));
			Debug.Log("G");
			GalreadyMarked = true;
			AnimateStars(true, G_Grid);
			return;
		}
		if (FinishedCombinationsInOrder[0] == store.o_list && !OalreadyMarked)
		{
			if (SpeedTambolaScoreManager.ScoreInstance.is2x) points += 1000;
			else points += 500;
			SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
			StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("BINGO!", 500, '+'));
			Debug.Log("O");
			OalreadyMarked = true;
			AnimateStars(true, O_Grid);
			return;
		}
		if (FinishedCombinationsInOrder[0] == store.Diagonal1_list && !Diag1alreadyMarked)
		{
			if (SpeedTambolaScoreManager.ScoreInstance.is2x) points += 1000;
			else points += 500;
			SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
			StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("BINGO!", 500, '+'));
			Debug.Log("Diagonal1");
			Diag1alreadyMarked = true;
			AnimateStars(true, Diagonal1);
			return;
		}
		if (FinishedCombinationsInOrder[0] == store.Diagonal2_list && !Diag2alreadyMarked)
		{
			if (SpeedTambolaScoreManager.ScoreInstance.is2x) points += 1000;
			else points += 500;
			SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
			StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("BINGO!", 500, '+'));
			Debug.Log("Diagonal2");
			Diag2alreadyMarked = true;
			AnimateStars(true, Diagonal2);
			return;
		}
		if (FinishedCombinationsInOrder[0] == store.FourCorners_list && !FourCornersMarked)
		{
			if (SpeedTambolaScoreManager.ScoreInstance.is2x) points += 1000;
			else points += 500;
			SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
			StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("BINGO!", 500, '+'));
			Debug.Log("FourCorners");
			FourCornersMarked = true;
			AnimateStars(true, Four_Corners);
			return;
		}
		if (FinishedCombinationsInOrder.Count == 0)
		{
			if (SpeedTambolaScoreManager.ScoreInstance.TotalScore >= 500)
			{
				points -= 500;
				SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
				StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("False Claim!", 500, '-'));
			}
			else if (SpeedTambolaScoreManager.ScoreInstance.TotalScore <= 500)
			{
				points -= points;
				SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
				StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("False Claim!", points, '-'));
			}
		}
		StartCoroutine("CoolDown");
		return;
		/* if (isBingo(store.firstRow_list, store.firstRow_list.Count) && !firstalreadyMarked)
		{
			if (SpeedTambolaScoreManager.ScoreInstance.is2x) points += 1000;
			else points += 500;
			SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
			FinishedCombinationsInOrder.Add(store.firstRow_list);
			StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("BINGO!", 500, '+'));
			Debug.Log("");
			firstalreadyMarked = true;
			AnimateStars(true, First_Row);
			return;
		}
		if (isBingo(store.secondRow_list, store.secondRow_list.Count) && !secondalreadyMarked)
		{
			if (SpeedTambolaScoreManager.ScoreInstance.is2x) points += 1000;
			else points += 500;
			SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
			StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("BINGO!", 500, '+'));
			Debug.Log("");
			secondalreadyMarked = true;
			AnimateStars(true, Second_Row);
			return;
		}
		if (isBingo(store.thirdRow_list, store.thirdRow_list.Count) && !thirdalreadyMarked)
		{
			if (SpeedTambolaScoreManager.ScoreInstance.is2x) points += 1000;
			else points += 500;
			SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
			StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("BINGO!", 500, '+'));
			Debug.Log("");
			thirdalreadyMarked = true;
			AnimateStars(true, Third_Row);
			return;
		}
		if (isBingo(store.fourthRow_list, store.fourthRow_list.Count) && !fourthalreadyMarked)
		{
			if (SpeedTambolaScoreManager.ScoreInstance.is2x) points += 1000;
			else points += 500;
			SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
			StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("BINGO!", 500, '+'));
			Debug.Log("");
			fourthalreadyMarked = true;
			AnimateStars(true, Fourth_Row);
			return;
		}
		if (isBingo(store.fifthRow_list, store.fifthRow_list.Count) && !fifthalreadyMarked)
		{
			if (SpeedTambolaScoreManager.ScoreInstance.is2x) points += 1000;
			else points += 500;
			SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
			StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("BINGO!", 500, '+'));
			Debug.Log("");
			fifthalreadyMarked = true;
			AnimateStars(true, Fifth_Row);
			return;
		}
		if (isBingo(store.b_list, store.b_list.Count) && !BalreadyMarked)
		{
			if (SpeedTambolaScoreManager.ScoreInstance.is2x) points += 1000;
			else points += 500;
			SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
			StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("BINGO!", 500, '+'));
			Debug.Log("");
			BalreadyMarked = true;
			AnimateStars(true, B_Grid);
			return;
		}
		if (isBingo(store.i_list, store.i_list.Count) && !IalreadyMarked)
		{
			if (SpeedTambolaScoreManager.ScoreInstance.is2x) points += 1000;
			else points += 500;
			SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
			StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("BINGO!", 500, '+'));
			Debug.Log("");
			IalreadyMarked = true;
			AnimateStars(true, I_Grid);
			return;
		}
		if (isBingo(store.n_list, store.n_list.Count) && !NalreadyMarked)
		{
			if (SpeedTambolaScoreManager.ScoreInstance.is2x) points += 1000;
			else points += 500;
			SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
			StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("BINGO!", 500, '+'));
			Debug.Log("");
			NalreadyMarked = true;
			AnimateStars(true, N_Grid);
			return;
		}
		if (isBingo(store.g_list, store.g_list.Count) && !GalreadyMarked)
		{
			if (SpeedTambolaScoreManager.ScoreInstance.is2x) points += 1000;
			else points += 500;
			SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
			StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("BINGO!", 500, '+'));
			Debug.Log("");
			GalreadyMarked = true;
			AnimateStars(true, G_Grid);
			return;
		}
		if (isBingo(store.o_list, store.o_list.Count) && !OalreadyMarked)
		{
			if (SpeedTambolaScoreManager.ScoreInstance.is2x) points += 1000;
			else points += 500;
			SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
			StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("BINGO!", 500, '+'));
			Debug.Log("");
			OalreadyMarked = true;
			AnimateStars(true, O_Grid);
			return;
		}
		if (isBingo(store.Diagonal1_list, store.Diagonal1_list.Count) && !Diag1alreadyMarked)
		{
			if (SpeedTambolaScoreManager.ScoreInstance.is2x) points += 1000;
			else points += 500;
			SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
			StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("BINGO!", 500, '+'));
			Debug.Log("");
			Diag1alreadyMarked = true;
			AnimateStars(true, Diagonal1);
			return;
		}
		if (isBingo(store.Diagonal2_list, store.Diagonal2_list.Count) && !Diag2alreadyMarked)
		{
			if (SpeedTambolaScoreManager.ScoreInstance.is2x) points += 1000;
			else points += 500;
			SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
			StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("BINGO!", 500, '+'));
			Debug.Log("");
			Diag2alreadyMarked = true;
			AnimateStars(true, Diagonal2);
			return;
		}
		if (isBingo(store.FourCorners_list, store.FourCorners_list.Count) && !FourCornersMarked)
		{
			if (SpeedTambolaScoreManager.ScoreInstance.is2x) points += 1000;
			else points += 500;
			SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
			StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("BINGO!", 500, '+'));
			Debug.Log("");
			FourCornersMarked = true;
			AnimateStars(true, Four_Corners);
			return;
		}
		if (SpeedTambolaScoreManager.ScoreInstance.TotalScore >= 500)
		{
			points -= 500;
			SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
			StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("False Claim!", 500, '-'));
		}
		else if (SpeedTambolaScoreManager.ScoreInstance.TotalScore <= 500)
		{
			points -= points;
			SpeedTambolaScoreManager.ScoreInstance.SetScore(points);
			StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("False Claim!", points, '-'));
		}
		if(BingoCount == 12) SpeedTambolaGameController.Controller.SwitchToResults(ScoreManager.ScoreInstance.isWinning, ScoreManager.ScoreInstance.TotalScore);
	} */

    public bool isBingo(List<NumberData> combination, int length)
    {
        for (int i = 0; i < length; i++)
        {
            if (!combination[i].isMarked) return false;
        }
        //BingoCount++;
        return true;
    }

    public void GetUnmarked(List<NumberData> buttonList)
    {
        Unmarked.Clear();
        RandomFourNumbers.Clear();
        for (int i = 0; i < 4; i++)
        {
            if (!Unmarked.Contains(buttonList[i]) && !buttonList[i].isMarked) Unmarked.Add(buttonList[i]);
            else continue;
        }
    }

    [System.Serializable]
    public class Combinations
    {
        public List<NumberData> b_list = new();
        public List<NumberData> i_list = new();
        public List<NumberData> n_list = new();
        public List<NumberData> g_list = new();
        public List<NumberData> o_list = new();
        public List<NumberData> firstRow_list = new();
        public List<NumberData> secondRow_list = new();
        public List<NumberData> thirdRow_list = new();
        public List<NumberData> fourthRow_list = new();
        public List<NumberData> fifthRow_list = new();
        public List<NumberData> Diagonal1_list = new();
        public List<NumberData> Diagonal2_list = new();
        public List<NumberData> FourCorners_list = new();
    }

    // public void ShowSettings()
    // {

    // 	UIController.instance.settingsManager.EnableSettingsPannel(() =>
    // 	{
    // 		Debug.Log("settings Game eXited");
    // 		matchController.ExitGame();
    // 	});

    // }




    [System.Serializable]
    public class NumberData
    {
        public int number;
        public bool isMarked = false;
        public NumberData(int number, bool isMarked)
        {
            this.number = number;
            this.isMarked = isMarked;
        }
    }
    //[System.Serializable]
    //public class BotDetailsTambola
    //{
    //    public string playerID;
    //    public string name;

    //}
}