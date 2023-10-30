using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsPanelHandler : MonoBehaviour
{
    public RectTransform PanelTransform;
    public float XOffPos = 700;
    public Button ExitBtn;
    public Toggle SoundToggle;
    private bool SoundActive;
    public UnityAction SwapSpriteSequence;

    private void Awake()
    {
        CheckPlayerprefs();
        SoundToggle.onValueChanged.RemoveAllListeners();
        SoundToggle.onValueChanged.AddListener(ToggleSound);
        ExitBtn.onClick.RemoveAllListeners();
        ExitBtn.onClick.AddListener(OnExitBtnClick);
    }

    void CheckPlayerprefs()
    {
        ToggleSound(false);
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
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            PanelTransform.DOKill();
            PanelTransform.DOAnchorPosX(0, 0.25f).From(new Vector2(XOffPos, PanelTransform.anchoredPosition.y));
        }
        else HideSettings();
       
    }

    public void HideSettings()
    {
        PanelTransform.DOKill();
        PanelTransform.DOAnchorPosX(XOffPos, 0.25f).OnComplete(() => gameObject.SetActive(false));
        SwapSpriteSequence?.Invoke();
    }

    public void OnExitBtnClick()
    {
       //MiniRouletteUIController.instance.ExitWebGL();
        HideSettings();
    }
}
