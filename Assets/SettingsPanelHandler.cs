using DG.Tweening;
//using Mini_Roulette;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsPanelHandler : MonoBehaviour
{
    public RectTransform PanelTransform;
    public float XOffPos = 700;
    public Button ExitBtn;
    public Button fullScreenBtn;
    public Toggle SoundToggle;
    private bool SoundActive;
    public UnityAction SwapSpriteSequence;
    public TMP_Text playerName;

    // Settings
    public Button openSpaceBtn;
    public Image openSpaceImg;

    private void Awake()
    {
        CheckPlayerprefs();
        SoundToggle.onValueChanged.RemoveAllListeners();
        SoundToggle.onValueChanged.AddListener(ToggleSound);
        ExitBtn.onClick.RemoveAllListeners();
        ExitBtn.onClick.AddListener(OnExitBtnClick);

        if (openSpaceImg.enabled)
            openSpaceImg.enabled = false;

        openSpaceBtn.onClick.AddListener(() => { HideSettings(); });
        fullScreenBtn.onClick.AddListener(() => { ShowFullScreen(); });
    }

    void CheckPlayerprefs()
    {
        ToggleSound(false);//PlayerPrefs.GetInt("SoundActive", 1) == 0);
    }

    public void ToggleSound(bool value)
    {
        SoundActive = value;
        SoundToggle.isOn = value;
        PlayerPrefs.SetInt("SoundActive", SoundActive ? 1 : 0);
        // AudioListener.pause = !value;
        if (value)
        {
            AudioListener.volume = 1;
        }
        else
            AudioListener.volume = 0;
    }

    public void ShowSettings()
    {
        openSpaceImg.enabled = true;
        gameObject.SetActive(true);
        PanelTransform.DOKill();
        PanelTransform.DOAnchorPosX(0, 0.25f).From(new Vector2(XOffPos, PanelTransform.anchoredPosition.y));
    }

    public void HideSettings()
    {
        openSpaceImg.enabled=false;
        PanelTransform.DOKill();
        PanelTransform.DOAnchorPosX(XOffPos, 0.25f).OnComplete(() => gameObject.SetActive(false));
        SwapSpriteSequence?.Invoke();
    }

    private void ShowFullScreen()
    {
        APIController.FullScreen();
        HideSettings();
    }

    public void OnExitBtnClick()
    {
      SpeedTambolaGameController.Controller.ExitWebGL();
    
        HideSettings();
    }
}
