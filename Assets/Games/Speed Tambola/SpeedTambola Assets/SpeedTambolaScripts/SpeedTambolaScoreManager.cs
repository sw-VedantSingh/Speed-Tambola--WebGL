using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SpeedTambolaScoreManager : MonoBehaviour
{
	[SerializeField] TMP_Text ScoreDisplay ;
	public GameObject ButtonGridBottom;
	public int TotalScore = 0;
	public List<Button> FourRandomButtons;
	public bool isMeterFull = false;
	public bool isAbilityActive = false;
	public bool is2x = false;
	public bool isRanFour = false;
	public List<Sprite> PrimarySprites;
	public List<Sprite> SecondarySprites;
	//public GameObject RanFourPanel;
	public bool isAnyButton = false;
	public int objectIndex;
	public static SpeedTambolaScoreManager ScoreInstance;
	public float abilityDuration = 10f;
	public float buffer = 10f;
	int whole;
	public GameObject FullIndicator;
	public GameObject AbilityAndScoreUpdates;
	public List<GameObject> InactiveAbilities;
	public List<GameObject> UnusedAbilities = new();
	public GameObject Notification;
	[SerializeField] GameObject Score;
	public bool UnusedAbilityActive = false;
	void Awake()
	{
		ScoreInstance = this;
	}

	public IEnumerator ScoreUpdates(string message, int points, char update)
	{
		//Debug.LogError("Animation Coroutine");
		float messageDuration = 1.5f;
		Notification.SetActive(true);
		Notification.GetComponentInChildren<TextMeshProUGUI>().text = $"{message}";
		Notification.transform.DOKill();
		Notification.GetComponent<RectTransform>().DOAnchorPosY(21, 1).From(new Vector2(Notification.GetComponent<RectTransform>().position.x, -185), true);
		if (update == '-')
		{
			Score.GetComponent<Image>().color = Color.red;
			Score.GetComponentInChildren<TMP_Text>().text = $"{update}{points}";
		}
		else
		{
			Score.GetComponent<Image>().color = Color.green;
			Score.GetComponentInChildren<TMP_Text>().text = $"{update}{points}";
		}
		foreach (Image img in Notification.GetComponentsInChildren<Image>()) img.DOFade(1, 1).From(0);
		foreach (TMP_Text text in Notification.GetComponentsInChildren<TMP_Text>()) text.DOFade(1, 1).From(0);
		yield return new WaitForSeconds(messageDuration);
		Notification.GetComponent<RectTransform>().DOAnchorPosY(350, 1);
		foreach (Image img in Notification.GetComponentsInChildren<Image>()) img.DOFade(0, 1).From(1).OnComplete(() => Notification.SetActive(false));
		foreach (TMP_Text text in Notification.GetComponentsInChildren<TMP_Text>()) text.DOFade(0, 1).From(1);
		//SpeedTambolaGameManager.Instance.BingoButton.interactable = true;
	}

	public void SetScore(int value)
	{
		TotalScore = value;
		ScoreDisplay.text = TotalScore.ToString();
		//SpeedTambolaGameController.Controller.matchController.UpdatePoints(TotalScore.ToString());
		if (SpeedTambolaGameManager.Instance.IDKMeter.fillAmount >= 1.0f)
		{
			FullIndicator.SetActive(true);
			//DisableAbilities();
			//SpeedTambolaGameManager.Instance.IDKMeter.fillAmount = 0.0f;
			isMeterFull = true;
			Debug.Log("Unlocked an ability");
            StartCoroutine("ResetBar");
			//objectIndex = 0;
			//foreach (GameObject Unused in InactiveAbilities) if (Unused.activeSelf && !UnusedAbilities.Contains(Unused)) UnusedAbilities.Add(Unused);
			if (!isAbilityActive)
			{
                foreach (GameObject i in InactiveAbilities) i.SetActive(false);
                //if (UnusedAbilities.Count > 0) UnusedAbilities[UnusedAbilities.Count - 1].SetActive(true);
                InactiveAbilities[objectIndex].SetActive(true);
                objectIndex++;
                if (objectIndex >= InactiveAbilities.Count) objectIndex = 0;
            }
			//if(InactiveAbilities[objectIndex] == UnusedAbilities[UnusedAbilities.Count - 1]) UnusedAbilities.Remove(UnusedAbilities[UnusedAbilities.Count - 1]);
			
		}
	}

	IEnumerator ResetBar()
	{
		yield return new WaitForSeconds(0.5f);
		SpeedTambolaGameManager.Instance.IDKMeter.fillAmount = 0.0f;
        //isMeterFull = false;
        FullIndicator.SetActive(false);
    }

	public void DisableAbilities()
	{
		foreach (GameObject obj in InactiveAbilities) obj.SetActive(false);
		is2x = false;
		isAnyButton = false;
		isRanFour = false;
		UnusedAbilityActive = false;
	}

	void OnEnable()
	{
		DisableAbilities();
		AbilityAndScoreUpdates.SetActive(false);
		foreach (Transform i in AbilityAndScoreUpdates.transform)
		{
			i.gameObject.SetActive(true);
		}
		//RanFourPanel.SetActive(false);
		FullIndicator.SetActive(false);
	}

	public List<Sprite> Labels;

	public void AnyOnTheGrid()
	{
		Debug.Log("Anyongrid...in");
		if (isMeterFull || UnusedAbilityActive)
		{
            Debug.Log("Anyongrid...out");

            DisableAbilities();
			FullIndicator.SetActive(false);
            AbilityAndScoreUpdates.transform.GetChild(1).GetComponent<Image>().fillAmount = 1.0f;
            //InactiveAbilities[0].SetActive(true);
            UnusedAbilityActive = false;
			isAbilityActive = true;
			isMeterFull = false;
			isAnyButton = true;
			//SpeedTambolaGameManager.Instance.IDKMeter.fillAmount = 0;
			ButtonGridBottom.SetActive(false);
			AbilityAndScoreUpdates.SetActive(true);
			foreach (Transform i in AbilityAndScoreUpdates.transform)
			{
				i.gameObject.SetActive(false);
			}
			AbilityAndScoreUpdates.transform.GetChild(0).GetComponent<Image>().sprite = Labels[0];
			AbilityAndScoreUpdates.transform.GetChild(0).GetComponent<Image>().SetNativeSize();
			AbilityAndScoreUpdates.transform.GetChild(0).gameObject.SetActive(true);
			SpeedTambolaGameController.Controller.Gameplay = false;
		}
	}

	public void RandomFourAbility()
	{
		if (isMeterFull || UnusedAbilityActive)
		{
			DisableAbilities();
			UnusedAbilityActive = false;
			FullIndicator.SetActive(false);
			isAbilityActive = true;
			isMeterFull = false;
			isRanFour = true;
			//RanFourPanel.SetActive(true);
			SpeedTambolaGameController.Controller.Gameplay = false;
			//SpeedTambolaGameManager.Instance.IDKMeter.fillAmount = 0;
			//int unmarked = 0;
			// unmarked += SpeedTambolaGameManager.Instance.store.b_list.FindAll(x => !x.isMarked).Count;
			// unmarked += SpeedTambolaGameManager.Instance.store.i_list.FindAll(x => !x.isMarked).Count;
			// unmarked += SpeedTambolaGameManager.Instance.store.n_list.FindAll(x => !x.isMarked).Count;
			// unmarked += SpeedTambolaGameManager.Instance.store.g_list.FindAll(x => !x.isMarked).Count;
			// unmarked += SpeedTambolaGameManager.Instance.store.o_list.FindAll(x => !x.isMarked).Count;
			// Debug.Log(unmarked);
			SpeedTambolaGameManager.Instance.GetUnmarked(SpeedTambolaGameManager.Instance.store.b_list);
			SpeedTambolaGameManager.Instance.GetUnmarked(SpeedTambolaGameManager.Instance.store.i_list);
			SpeedTambolaGameManager.Instance.GetUnmarked(SpeedTambolaGameManager.Instance.store.n_list);
			SpeedTambolaGameManager.Instance.GetUnmarked(SpeedTambolaGameManager.Instance.store.g_list);
			SpeedTambolaGameManager.Instance.GetUnmarked(SpeedTambolaGameManager.Instance.store.o_list);
			Debug.Log(SpeedTambolaGameManager.Instance.Unmarked.Count);
			for (int j = 0; j < 4; j++)
			{
				int i = Random.Range(0, SpeedTambolaGameManager.Instance.Unmarked.Count);
				if (!SpeedTambolaGameManager.Instance.RandomFourNumbers.Contains(SpeedTambolaGameManager.Instance.Unmarked[i])) SpeedTambolaGameManager.Instance.RandomFourNumbers.Add(SpeedTambolaGameManager.Instance.Unmarked[i]);
			}
			for (int i = 0; i < SpeedTambolaGameManager.Instance.RandomFourNumbers.Count; i++)
			{
				if (SpeedTambolaGameManager.Instance.RandomFourNumbers[i].number >= 1 && SpeedTambolaGameManager.Instance.RandomFourNumbers[i].number <= 15)
				{
					FourRandomButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = $"B {SpeedTambolaGameManager.Instance.RandomFourNumbers[i].number}";
					FourRandomButtons[i].GetComponentInChildren<Image>().sprite = PrimarySprites[0];
				}
				else if (SpeedTambolaGameManager.Instance.RandomFourNumbers[i].number > 15 && SpeedTambolaGameManager.Instance.RandomFourNumbers[i].number <= 30)
				{
					FourRandomButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = $"I {SpeedTambolaGameManager.Instance.RandomFourNumbers[i].number}";
					FourRandomButtons[i].GetComponentInChildren<Image>().sprite = PrimarySprites[1];
				}
				else if (SpeedTambolaGameManager.Instance.RandomFourNumbers[i].number > 30 && SpeedTambolaGameManager.Instance.RandomFourNumbers[i].number <= 45)
				{
					FourRandomButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = $"N {SpeedTambolaGameManager.Instance.RandomFourNumbers[i].number}";
					FourRandomButtons[i].GetComponentInChildren<Image>().sprite = PrimarySprites[2];
				}
				else if (SpeedTambolaGameManager.Instance.RandomFourNumbers[i].number > 45 && SpeedTambolaGameManager.Instance.RandomFourNumbers[i].number <= 60)
				{
					FourRandomButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = $"G {SpeedTambolaGameManager.Instance.RandomFourNumbers[i].number}";
					FourRandomButtons[i].GetComponentInChildren<Image>().sprite = PrimarySprites[3];
				}
				else if (SpeedTambolaGameManager.Instance.RandomFourNumbers[i].number > 60 && SpeedTambolaGameManager.Instance.RandomFourNumbers[i].number <= 75)
				{
					FourRandomButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = $"O {SpeedTambolaGameManager.Instance.RandomFourNumbers[i].number}";
					FourRandomButtons[i].GetComponentInChildren<Image>().sprite = PrimarySprites[4];
				}
			}
		}
	}

	public void x2Combo()
	{
		if (isMeterFull || UnusedAbilityActive)
		{
			DisableAbilities();
			AbilityAndScoreUpdates.transform.GetChild(0).gameObject.SetActive(false);
			InactiveAbilities[1].SetActive(true);
			FullIndicator.SetActive(false);
			UnusedAbilityActive = false;
			isAbilityActive = true;
			is2x = true;
			isMeterFull = false;
			AbilityAndScoreUpdates.SetActive(true);
			AbilityAndScoreUpdates.transform.GetChild(1).GetComponent<Image>().fillAmount = 1.0f;
            //SpeedTambolaGameManager.Instance.IDKMeter.fillAmount = 0;
            Debug.Log("x2 Active");
		}
		//Debug.Log($"Object Index: {objectIndex}");
	}

	void Update()
	{
		if (isAbilityActive) UpdateAbilityDuration();
	}

	public void UpdateAbilityDuration()
	{
		if (isAbilityActive && is2x && SpeedTambolaGameController.Controller.CentralTimer)
		{
			abilityDuration -= Time.deltaTime;
			//Debug.Log((int)abilityDuration);
			float peepo = abilityDuration / buffer;
			AbilityAndScoreUpdates.transform.GetChild(1).GetComponent<Image>().fillAmount = peepo;
			// Label.gameObject.SetActive(true);
			// Label.text = "2X Timer";
			if (abilityDuration <= 0f)
			{
				AbilityAndScoreUpdates.SetActive(false);
				InactiveAbilities[objectIndex].SetActive(false);
				DisableAbilities();
				AbilityAndScoreUpdates.transform.GetChild(1).GetComponent<Image>().fillAmount = 1.0f;
				isAbilityActive = false;
				abilityDuration = buffer;
				//Label.gameObject.SetActive(false);
				//InactiveAbilities[objectIndex].SetActive(false);
				//if (UnusedAbilities.Count > 0)
				//{
				//	InactiveAbilities[UnusedAbilities.Count - 1].SetActive(true);
				//	UnusedAbilities.Remove(UnusedAbilities[UnusedAbilities.Count - 1]);
				//	UnusedAbilityActive = true;
				//}
				is2x = false;
			}
		}
		if (isAbilityActive && isRanFour && SpeedTambolaGameController.Controller.CentralTimer)
		{
			abilityDuration -= Time.deltaTime;
			whole = (int)abilityDuration;
			//RanFourPanel.SetActive(true);
			//AbilityDuration.GetComponent<TMP_Text>().text = $"0{whole.ToString()}";
			if (abilityDuration <= 0f)
			{
				abilityDuration = buffer;
				isAbilityActive = false;
				DisableAbilities();
				//RanFourPanel.SetActive(false);
				AbilityAndScoreUpdates.SetActive(false);
				InactiveAbilities[objectIndex].SetActive(false);
				//if (UnusedAbilities.Count > 0)
				//{
				//	InactiveAbilities[UnusedAbilities.Count - 1].SetActive(true);
				//	UnusedAbilities.Remove(UnusedAbilities[UnusedAbilities.Count - 1]);
				//	UnusedAbilityActive = true;
				//}
				isRanFour = false;
			}
		}
	}
}