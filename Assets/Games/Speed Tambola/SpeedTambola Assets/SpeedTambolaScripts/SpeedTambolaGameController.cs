using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SpeedTambolaGameController : MonoBehaviour
{
    [SerializeField] GameObject win;
    public GameObject GamePlayCanvas;
    public SettingsPanelHandler SettingsPanel;
    public AlertPanelHandler AlertHandler;
    public ConfirmationPanelHandler ConfirmationPanel;
    //public GameObject HowToPlayPanel;
    public bool Gameplay = true;
    public bool CentralTimer = true;
    public static SpeedTambolaGameController Controller;
    public TMP_Text FirstRank;
    public TMP_Text LastRank;
    public TMP_Text FirstName;
    public TMP_Text LastName;
    public List<Image> MatchResult;
    //public MatchControllerNakama matchController;
    void Awake()
    {
        Application.runInBackground = true;
        Controller = this;
        //matchController.playAgainGame.AddListener(SpeedTambolaGameManager.Instance.RestartGame);
    }

    //public void ShowExitPopup()
    //{
    //    SettingsPanel.HideMe();
    //    ConfirmationPanel.ShowMessageWithAction("Are you sure you want to exit?", ExitGame, null, "Exit Game", "Yes, Exit", "Cancel", AlertType.Exit);
    //}

    public void Results()
    {
        int BotScore = (Random.Range(16, 46) - 1) * 100;
        if (BotScore > SpeedTambolaScoreManager.ScoreInstance.TotalScore)
        {
            MatchResult[1].gameObject.SetActive(true);
            FirstRank.text = BotScore.ToString();
            FirstName.text = "Bot";
            LastName.text = "Player";
            LastRank.text = SpeedTambolaScoreManager.ScoreInstance.TotalScore.ToString();
        }
        else
        {
            MatchResult[0].gameObject.SetActive(true);
            FirstRank.text = SpeedTambolaScoreManager.ScoreInstance.TotalScore.ToString();
            LastRank.text = BotScore.ToString();
            FirstName.text = "Player";
            LastName.text = "Bot";
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

    public void ExitGame()
    {
        APIController.CloseWindow();
    }

    //public void ShowHelp()
    //{
    //    HowToPlayPanel.SetActive(true);
    //}

    //public void HideHelp()
    //{
    //    HowToPlayPanel.SetActive(false);
    //}

    // public void UnloadSpeedTambola()
    // {
    // 	GameController.Instance.UnloadTambola();

    // }
}