using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using Unity.VisualScripting;

public class SpeedTambolaGameController : MonoBehaviour
{


    [SerializeField] GameObject win;
    public GameObject GamePlayCanvas;
    public SettingsPanelHandler SettingsPanel;
    public AlertPanelHandler AlertHandler;
    public ConfirmationPanelHandler ConfirmationPanel;
    //public GameObject HowToPlayPanel;
    public bool Gameplay = false;
    public bool CentralTimer = false;
    public static SpeedTambolaGameController Controller;
    public TMP_Text FirstRank;
    public TMP_Text LastRank;
    public TMP_Text FirstName;
    public TMP_Text LastName;
    public List<Image> MatchResult;
    [SerializeField] int BotScore = 0;

    [Header("NEW UPDATE")]

    public Button HowtoplayBtn;
    public GameObject HowtoplayScreen;

    public Button SettingBtn;
    public Button BackBtn;
    public Button homeBtn;
    public Button BacktoLobby;
    public Button PlayAgain;

    public GameObject LobbyPanel;
    public GameObject Gameplaypanel;
    public InsufficientPanelHandler insufficientPanelHandler;

    //public MatchControllerNakama matchController;
    void Awake()
    {
        Application.runInBackground = true;
        Controller = this;
        SettingBtn.onClick.AddListener(() => { CallSettingAction(); });
        HowtoplayBtn.onClick.AddListener(() => { HowtoplayBTN(); });
        BackBtn.onClick.AddListener(() => { HidePanel(HowtoplayScreen); });
        homeBtn.onClick.AddListener(() => { ExitWebGL(); });
        PlayAgain.onClick.AddListener(() => { PlayAgainbtn(); });
        BacktoLobby.onClick.AddListener(() => { ExitWebGL(); });



        //matchController.playAgainGame.AddListener(SpeedTambolaGameManager.Instance.RestartGame);
    }
    public void PlayAgainbtn()
    {

        SpeedTambolaGameManager.Instance.ResultPanel.SetActive(false);

        MatchMakingTambola.Instance.Showmatching();
        SpeedTambolaGameManager.Instance.RestartGame();


    }

    public void HowtoplayBTN()
    {
        HowtoplayScreen.SetActive(true);
        SettingsPanel.HideSettings();

    }
    private void Start()
    {
        // CentralTimer = false;
    }
    //public void ShowExitPopup()
    //{
    //    SettingsPanel.HideMe();
    //    ConfirmationPanel.ShowMessageWithAction("Are you sure you want to exit?", ExitGame, null, "Exit Game", "Yes, Exit", "Cancel", AlertType.Exit);
    //}
    public void CallSettingAction()
    {
        if (SettingsPanel.PanelTransform.anchoredPosition.x > 600) SettingsPanel.ShowSettings();
            //MiniRouletteUIController.instance.settingsPanel.SwapSpriteSequence = null;
            //MiniRouletteUIController.instance.settingsPanel.SwapSpriteSequence += () => DOTween.Sequence().Append(setting.transform.DOScale(Vector2.zero, 0.125f).OnComplete(() => setting.image.sprite = SettingSprite)).Append(setting.transform.DOScale(Vector2.one, 0.125f).SetEase(Ease.OutBounce));
            //DOTween.Sequence().Append(setting.transform.DOScale(Vector2.zero, 0.125f).OnComplete(() => setting.image.sprite = CloseSprite)).Append(setting.transform.DOScale(Vector2.one, 0.125f).SetEase(Ease.OutBounce));
        else SettingsPanel.HideSettings();
            //DOTween.Sequence().Append(setting.transform.DOScale(Vector2.zero, 0.125f).OnComplete(() => setting.image.sprite = SettingSprite)).Append(setting.transform.DOScale(Vector2.one, 0.125f).SetEase(Ease.OutBounce));
    }

    int GenerateBotScore()
    {
        BotScore = 0;
        int random = UnityEngine.Random.Range(0, 2);
        int value = 100 * UnityEngine.Random.Range(5, 26);
        if (random == 0) BotScore -= value;
        else BotScore += value;
        if (BotScore < 0) BotScore *= -1;
        else if(BotScore > 7000) BotScore = 7000;
        Debug.Log($"Random Value: {BotScore}");
        return BotScore;
    }
    public void Results()
    {
        SpeedTambolaGameManager.Instance.val = DateTime.Now;
        SpeedTambolaGameManager.Instance.endTime = SpeedTambolaGameManager.Instance.val.AddSeconds(500000000);


        //int _winProbality = UnityEngine.Random.Range(1, 11);

        //int[] randomValue = { 100, 200, 300, 500, 1000, 2000, 3000, 5000, 6000, 8000 };

        //int rand = UnityEngine.Random.Range(0, randomValue.Length);/*Peepee Poopoo, aka useless*/

        //BotScore = rand;/*Peepee Poopoo, aka useless*/
        BotScore = GenerateBotScore();
       
        //BotScore = randomValue[UnityEngine.Random.Range(0, randomValue.Length)];
        //if (_winProbality <= 4)
        //{
        //    BotScore = UnityEngine.Random.Range(SpeedTambolaScoreManager.ScoreInstance.TotalScore, SpeedTambolaScoreManager.ScoreInstance.TotalScore + 100);
        //}
        //else
        //{
        //    BotScore = Math.Abs(UnityEngine.Random.Range(SpeedTambolaScoreManager.ScoreInstance.TotalScore - 100, SpeedTambolaScoreManager.ScoreInstance.TotalScore));
        //}

        //int BotScore = (Random.Range(16, 46) - 1) * 100;
        if (BotScore > SpeedTambolaScoreManager.ScoreInstance.TotalScore)
        {
            MatchResult[1].gameObject.SetActive(true);
            FirstRank.text = BotScore.ToString();
            FirstName.text = "Bot";
            LastName.text = "Player";
            LastRank.text = SpeedTambolaScoreManager.ScoreInstance.TotalScore.ToString();
            SpeedTambolaGameManager.Instance.WinningAmtCalculation();

        }
        else
        {
            MatchResult[0].gameObject.SetActive(true);
            FirstRank.text = SpeedTambolaScoreManager.ScoreInstance.TotalScore.ToString();
            LastRank.text = BotScore.ToString();
            FirstName.text = "Player";
            LastName.text = "Bot";
            SpeedTambolaGameManager.Instance.WinningAmtCalculation();

            string message = "Game Won";
            TransactionMetaData val = new();
            val.Amount = (APIController.instance.userDetails.bootAmount * 2 / 100) * 80;
            val.Info = message;

            APIController.instance.WinningsBet(SpeedTambolaGameManager.Instance.betIndex, (APIController.instance.userDetails.bootAmount * 2 / 100) * 80, APIController.instance.userDetails.bootAmount, val, (Success) =>
            {
                if (Success)
                {
                    Debug.Log("Winning Bet Success -----------------------");
                }
                else
                {
                    // MiniRouletteUIController.instance.ExitWebGL();
                    Debug.Log("Winning Bet Failed -----------------");

                }
            }, "");

        }
    }

    public void ShowSettings()
    {
        SettingsPanel.ShowSettings();
    }

    //private void CallSettingAction()
    //{
    //    if (MiniRouletteUIController.instance.settingsPanel.PanelTransform.anchoredPosition.x > 600)
    //    {
    //        MiniRouletteUIController.instance.settingsPanel.ShowSettings();
    //        MiniRouletteUIController.instance.settingsPanel.SwapSpriteSequence = null;
    //        MiniRouletteUIController.instance.settingsPanel.SwapSpriteSequence += () => DOTween.Sequence().Append(setting.transform.DOScale(Vector2.zero, 0.125f).OnComplete(() => setting.image.sprite = SettingSprite)).Append(setting.transform.DOScale(Vector2.one, 0.125f).SetEase(Ease.OutBounce));
    //        DOTween.Sequence().Append(setting.transform.DOScale(Vector2.zero, 0.125f).OnComplete(() => setting.image.sprite = CloseSprite)).Append(setting.transform.DOScale(Vector2.one, 0.125f).SetEase(Ease.OutBounce));
    //    }
    //    else
    //    {
    //        MiniRouletteUIController.instance.settingsPanel.HideSettings();
    //    }
    //}

    public void HidePanel(GameObject Panel)
    {
        Panel.SetActive(false);
       
    }
    public void FullScreen()
    {
        APIController.FullScreen();
    }


    public void ExitWebGL()
    {
#if UNITY_WEBGL
        APIController.CloseWindow();
#endif
    }
    public void ShowInsufficientPopUp()
    {
        insufficientPanelHandler.ShowMessageWithAction("You don't have enough money! kindly add money to your wallet..", APIController.instance.OnClickDepositBtn, ExitWebGL, "In sufficient balance", "ADD CASH", " EXIT TO LOBBY ", AlertType.Low);
        Invoke(nameof(insufficientPanelHandler.HideMe), 2f);
    }
}