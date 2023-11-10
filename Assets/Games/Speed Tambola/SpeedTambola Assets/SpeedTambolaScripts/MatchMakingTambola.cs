using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchMakingTambola : MonoBehaviour
{
    public static MatchMakingTambola Instance;
    public List<Sprite> profiles = new();
    //public TMP_Text MyPlayerName;
    public Image OpponentScrollImg;
    public Image OpponentAvatarImg;
    public Image playerImage;
    public TMP_Text OpponentName;
    public TMP_Text CountdownTxt;
    // public Button BackBtn;
    [Header("PoolAmount")]
    public TMP_Text entryAmount;
    public TMP_Text pricePoolAmount;
    public TMP_Text GrandPricePoolAmount;
    public TMP_Text winnerCount;

    public GameObject Holder1;
    public GameObject Holder2;
    public GameObject line1, line2;
    public TMP_Text counttext;


    public TMP_Text ConnectingToServer;
    private Coroutine ScrollRoutine;
    private float OpponentProfileYPos;

    public Button _emptyBtn;

    private void Awake()
    {
        OpponentProfileYPos = OpponentAvatarImg.transform.parent.GetComponent<RectTransform>().anchoredPosition.y;
        Instance = this;
        _emptyBtn.onClick.AddListener(() => { SpeedTambolaGameController.Controller.SettingsPanel.HideSettings(); });
    }
    private void OnDisable()
    {

        StopCoroutine("StartGame");

    }
    private void Start()
    {
        //GameInitialize();



    }
    public void Showmatching()
    {
        Invoke(nameof(MatchMakingTambola.Instance.StopScrolling), 1f);
        if (!this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
        }
        StopCoroutine(nameof(StartGame));
        StartCoroutine(nameof(StartGame));
        entryAmount.text = "" + APIController.instance.userDetails.bootAmount;
        winnerCount.text = "1";//GameController.Instance.WinnerCount.ToString();
        pricePoolAmount.text = "" + APIController.instance.userDetails.bootAmount * 2;
        GrandPricePoolAmount.text = "" + APIController.instance.userDetails.bootAmount * 2;
        Holder1.SetActive(true);
        Holder2.SetActive(false);
        if (APIController.instance.userDetails.isBlockApiConnection)
        {
            entryAmount.text = "Free";
            Holder1.SetActive(false);
            Holder2.SetActive(true);
            GrandPricePoolAmount.text = "Free";
        }

        //UIController.instance.settingsPanel.ActivateExitBtn();

        CountdownTxt.gameObject.SetActive(false);



    }
    public void GameInitialize()
    {
        // this.gameObject.SetActive(true);
        //if (ScrollRoutine != null) StopCoroutine(ScrollRoutine);
        //ScrollRoutine = StartCoroutine(ScrollOpponentImg());


        //OpponentAvatarImg.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, OpponentProfileYPos);
        //OpponentName.gameObject.SetActive(false);
    }



    private void OnEnable()
    {
        if (ScrollRoutine != null) StopCoroutine(ScrollRoutine);
        ScrollRoutine = StartCoroutine(ScrollOpponentImg());


        OpponentAvatarImg.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, OpponentProfileYPos);
        OpponentName.gameObject.SetActive(false);
        //CountdownTxt.gameObject.SetActive(false);
        // BackBtn.gameObject.SetActive(true);

    }



    public void UpdateWaitingLobby(int count)
    {
        Debug.Log("Here one");
        //OpponentName.text = CarromGameManager.localInstance.gameState.players.Find(x => x.playerData.playerID != GameController.Instance.my_PlayerData.playerID).playerData.playerName;
        CountdownTxt.text = "Game will start in ";
        counttext.text = "" + count;

    }

    //private IEnumerator ShowServerConnect()
    //{
    //    Debug.Log("Displaying Show server connect");

    //    int count = 1;

    //    while (count <= 3)
    //    {
    //        CountdownTxt.text = $"Connencting to server" + (count == 1 ? "<color=#FFDC2C>." : count == 2 ? "<color=#FFDC2C>.." : "<color=#FFDC2C>...");
    //        count++;
    //        yield return new WaitForSeconds(1f);
    //    }

    //    CountdownTxt.text = "";
    //}
    private IEnumerator AnimateInfoTxt()
    {
        int count = 1;
        while (ConnectingToServer.gameObject.activeSelf)
        {
            ConnectingToServer.text = "Connecting to server" + (count == 1 ? "<color=#ffdc2c>." : count == 2 ? "<color=#ffdc2c>.." : "<color=#ffdc2c>...");
            count = count >= 3 ? 1 : count + 1;
            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerator StartGame()
    {
        ConnectingToServer.gameObject.SetActive(true);
        StopCoroutine(nameof(AnimateInfoTxt));
        StartCoroutine(nameof(AnimateInfoTxt));
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator ScrollOpponentImg()
    {
        this.gameObject.SetActive(true);
        CountdownTxt.gameObject.SetActive(true);
        CountdownTxt.enabled = true;

        // StartCoroutine(ShowServerConnect());
        StopCoroutine(nameof(AnimateInfoTxt));
        StartCoroutine(nameof(AnimateInfoTxt));


        OpponentScrollImg.transform.localPosition = Vector2.zero;
        OpponentScrollImg.material.SetVector("_Offset", Vector2.zero);

        while (true)
        {
            OpponentScrollImg.material.SetVector("_Offset", (Vector2)OpponentScrollImg.material.GetVector("_Offset") + 1 * Time.deltaTime * Vector2.down);
            yield return null;
        }



        //StopScrolling();
    }

    public void StopScrolling()
    {
        Debug.Log("Here stop scrolling....");

        OpponentAvatarImg.sprite = profiles[UnityEngine.Random.Range(0, profiles.Count)];
        StopCoroutine(ScrollRoutine);
        //BackBtn.gameObject.SetActive(false);
        int maxValues = UnityEngine.Random.Range(0, profiles.Count);

        float count = 1;
        DOTween.To(() => count, x => count = x, 1, 1).OnUpdate(() =>
        {
            OpponentScrollImg.material.SetVector("_Offset", (Vector2)OpponentScrollImg.material.GetVector("_Offset") + count * Time.deltaTime * Vector2.down);
        }).SetEase(Ease.Linear).OnComplete(() =>
        {
            Debug.Log("Finded opponenet player");
            StartCoroutine(StartGameCount());
            ConnectingToServer.gameObject.SetActive(false);
            CountdownTxt.gameObject.SetActive(true);


            OpponentAvatarImg.transform.parent.GetComponent<RectTransform>().DOAnchorPosY(0, 1).SetEase(Ease.OutExpo).OnComplete(() => { OpponentName.gameObject.SetActive(true); CountdownTxt.gameObject.SetActive(true); });
            DOTween.To(() => count, x => count = x, 0f, 1f).SetEase(Ease.Linear).OnUpdate(() =>
            {
                OpponentScrollImg.material.SetVector("_Offset", (Vector2)OpponentScrollImg.material.GetVector("_Offset") + count * Time.deltaTime * Vector2.down);
            }).OnComplete(() =>
            {

            });
        });

        SpeedTambolaGameManager.Instance.startTime = DateTime.Now;
        SpeedTambolaGameManager.Instance.endTime = SpeedTambolaGameManager.Instance.startTime.AddSeconds(SpeedTambolaGameManager.Instance.TotalTimeRemaining);

    }

    private IEnumerator StartGameCount()
    {
        yield return new WaitForEndOfFrame();
        int count = 4;
        for (int i = count; i > 0; i--)
        {
            yield return new WaitForSeconds(1);
            count -= 1;
            UpdateWaitingLobby(count);

            if (count <= 0)
            {
                this.gameObject.SetActive(false);
                SpeedTambolaGameController.Controller.Gameplaypanel.SetActive(true);
                SpeedTambolaGameManager.Instance.IsGameStart = true;
                //if (SpeedTambolaGameController.Controller.Gameplay)
                yield return new WaitForSeconds(1);

                //Invoke(nameof(SpeedTambolaGameManager.Instance.RestartGame), 1 / 60);
                yield break;
            }
        }
        //gameObject.GetComponent<GameStarter>().StartGame();


        //startPanel.GetComponent<GameStarter>().StartGame();
    }

}
