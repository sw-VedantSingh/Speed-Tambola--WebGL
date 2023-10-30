using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SpeedTambolaButtonRenderer : MonoBehaviour
{
    public Button cell;
    public TMP_Text value;
    public int AssignedNumber;
    public void isPressed()
    {
        if (isValid())
        {
            cell.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
            cell.interactable = false;
            SpeedTambolaGameManager.Instance.TimerValue = 0;
            MarkButton();
            //if (SpeedTambolaGameManager.Instance.IDKMeter.fillAmount >= 1.0f)
            //{
            //    SpeedTambolaGameManager.Instance.IDKMeter.fillAmount = 0.0f;
            //    SpeedTambolaScoreManager.ScoreInstance.FullIndicator.SetActive(false);
            //}
            SpeedTambolaGameManager.Instance.IDKMeter.fillAmount += SpeedTambolaGameManager.Instance.FillRate;

            if (SpeedTambolaGameManager.Instance.isBingo(SpeedTambolaGameManager.Instance.store.firstRow_list, SpeedTambolaGameManager.Instance.store.firstRow_list.Count) && !SpeedTambolaGameManager.Instance.FinishedCombinationsInOrder.Exists(x => x[0] == SpeedTambolaGameManager.Instance.store.firstRow_list[0] && x[^1] == SpeedTambolaGameManager.Instance.store.firstRow_list[^1]) && !SpeedTambolaGameManager.Instance.firstalreadyMarked)
                SpeedTambolaGameManager.Instance.FinishedCombinationsInOrder.Add(SpeedTambolaGameManager.Instance.store.firstRow_list);

            if (SpeedTambolaGameManager.Instance.isBingo(SpeedTambolaGameManager.Instance.store.secondRow_list, SpeedTambolaGameManager.Instance.store.secondRow_list.Count) && !SpeedTambolaGameManager.Instance.FinishedCombinationsInOrder.Exists(x => x[0] == SpeedTambolaGameManager.Instance.store.secondRow_list[0] && x[^1] == SpeedTambolaGameManager.Instance.store.secondRow_list[^1]) && !SpeedTambolaGameManager.Instance.secondalreadyMarked)
                SpeedTambolaGameManager.Instance.FinishedCombinationsInOrder.Add(SpeedTambolaGameManager.Instance.store.secondRow_list);

            if (SpeedTambolaGameManager.Instance.isBingo(SpeedTambolaGameManager.Instance.store.thirdRow_list, SpeedTambolaGameManager.Instance.store.thirdRow_list.Count) && !SpeedTambolaGameManager.Instance.FinishedCombinationsInOrder.Exists(x => x[0] == SpeedTambolaGameManager.Instance.store.thirdRow_list[0] && x[^1] == SpeedTambolaGameManager.Instance.store.thirdRow_list[^1]) && !SpeedTambolaGameManager.Instance.thirdalreadyMarked)
                SpeedTambolaGameManager.Instance.FinishedCombinationsInOrder.Add(SpeedTambolaGameManager.Instance.store.thirdRow_list);

            if (SpeedTambolaGameManager.Instance.isBingo(SpeedTambolaGameManager.Instance.store.fourthRow_list, SpeedTambolaGameManager.Instance.store.fourthRow_list.Count) && !SpeedTambolaGameManager.Instance.FinishedCombinationsInOrder.Exists(x => x[0] == SpeedTambolaGameManager.Instance.store.fourthRow_list[0] && x[^1] == SpeedTambolaGameManager.Instance.store.fourthRow_list[^1]) && !SpeedTambolaGameManager.Instance.fourthalreadyMarked)
                SpeedTambolaGameManager.Instance.FinishedCombinationsInOrder.Add(SpeedTambolaGameManager.Instance.store.fourthRow_list);

            if (SpeedTambolaGameManager.Instance.isBingo(SpeedTambolaGameManager.Instance.store.fifthRow_list, SpeedTambolaGameManager.Instance.store.fifthRow_list.Count) && !SpeedTambolaGameManager.Instance.FinishedCombinationsInOrder.Exists(x => x[0] == SpeedTambolaGameManager.Instance.store.fifthRow_list[0] && x[^1] == SpeedTambolaGameManager.Instance.store.fifthRow_list[^1]) && !SpeedTambolaGameManager.Instance.fifthalreadyMarked)
                SpeedTambolaGameManager.Instance.FinishedCombinationsInOrder.Add(SpeedTambolaGameManager.Instance.store.fifthRow_list);

            if (SpeedTambolaGameManager.Instance.isBingo(SpeedTambolaGameManager.Instance.store.b_list, SpeedTambolaGameManager.Instance.store.b_list.Count) && !SpeedTambolaGameManager.Instance.FinishedCombinationsInOrder.Exists(x => x[0] == SpeedTambolaGameManager.Instance.store.b_list[0] && x[^1] == SpeedTambolaGameManager.Instance.store.b_list[^1]) && !SpeedTambolaGameManager.Instance.BalreadyMarked)
                SpeedTambolaGameManager.Instance.FinishedCombinationsInOrder.Add(SpeedTambolaGameManager.Instance.store.b_list);

            if (SpeedTambolaGameManager.Instance.isBingo(SpeedTambolaGameManager.Instance.store.i_list, SpeedTambolaGameManager.Instance.store.i_list.Count) && !SpeedTambolaGameManager.Instance.FinishedCombinationsInOrder.Exists(x => x[0] == SpeedTambolaGameManager.Instance.store.i_list[0] && x[^1] == SpeedTambolaGameManager.Instance.store.i_list[^1]) && !SpeedTambolaGameManager.Instance.IalreadyMarked)
                SpeedTambolaGameManager.Instance.FinishedCombinationsInOrder.Add(SpeedTambolaGameManager.Instance.store.i_list);

            if (SpeedTambolaGameManager.Instance.isBingo(SpeedTambolaGameManager.Instance.store.n_list, SpeedTambolaGameManager.Instance.store.n_list.Count) && !SpeedTambolaGameManager.Instance.FinishedCombinationsInOrder.Exists(x => x[0] == SpeedTambolaGameManager.Instance.store.n_list[0] && x[^1] == SpeedTambolaGameManager.Instance.store.n_list[^1]) && !SpeedTambolaGameManager.Instance.NalreadyMarked)
                SpeedTambolaGameManager.Instance.FinishedCombinationsInOrder.Add(SpeedTambolaGameManager.Instance.store.n_list);

            if (SpeedTambolaGameManager.Instance.isBingo(SpeedTambolaGameManager.Instance.store.g_list, SpeedTambolaGameManager.Instance.store.g_list.Count) && !SpeedTambolaGameManager.Instance.FinishedCombinationsInOrder.Exists(x => x[0] == SpeedTambolaGameManager.Instance.store.g_list[0] && x[^1] == SpeedTambolaGameManager.Instance.store.g_list[^1]) && !SpeedTambolaGameManager.Instance.GalreadyMarked)
                SpeedTambolaGameManager.Instance.FinishedCombinationsInOrder.Add(SpeedTambolaGameManager.Instance.store.g_list);

            if (SpeedTambolaGameManager.Instance.isBingo(SpeedTambolaGameManager.Instance.store.o_list, SpeedTambolaGameManager.Instance.store.o_list.Count) && !SpeedTambolaGameManager.Instance.FinishedCombinationsInOrder.Exists(x => x[0] == SpeedTambolaGameManager.Instance.store.o_list[0] && x[^1] == SpeedTambolaGameManager.Instance.store.o_list[^1]) && !SpeedTambolaGameManager.Instance.OalreadyMarked)
                SpeedTambolaGameManager.Instance.FinishedCombinationsInOrder.Add(SpeedTambolaGameManager.Instance.store.o_list);

            if (SpeedTambolaGameManager.Instance.isBingo(SpeedTambolaGameManager.Instance.store.Diagonal1_list, SpeedTambolaGameManager.Instance.store.Diagonal1_list.Count) && !SpeedTambolaGameManager.Instance.FinishedCombinationsInOrder.Exists(x => x[0] == SpeedTambolaGameManager.Instance.store.Diagonal1_list[0] && x[^1] == SpeedTambolaGameManager.Instance.store.Diagonal1_list[^1]) && !SpeedTambolaGameManager.Instance.Diag1alreadyMarked)
                SpeedTambolaGameManager.Instance.FinishedCombinationsInOrder.Add(SpeedTambolaGameManager.Instance.store.Diagonal1_list);

            if (SpeedTambolaGameManager.Instance.isBingo(SpeedTambolaGameManager.Instance.store.Diagonal2_list, SpeedTambolaGameManager.Instance.store.Diagonal2_list.Count) && !SpeedTambolaGameManager.Instance.FinishedCombinationsInOrder.Exists(x => x[0] == SpeedTambolaGameManager.Instance.store.Diagonal2_list[0] && x[^1] == SpeedTambolaGameManager.Instance.store.Diagonal2_list[^1]) && !SpeedTambolaGameManager.Instance.Diag2alreadyMarked)
                SpeedTambolaGameManager.Instance.FinishedCombinationsInOrder.Add(SpeedTambolaGameManager.Instance.store.Diagonal2_list);

            if (SpeedTambolaGameManager.Instance.isBingo(SpeedTambolaGameManager.Instance.store.FourCorners_list, SpeedTambolaGameManager.Instance.store.FourCorners_list.Count) && !SpeedTambolaGameManager.Instance.FinishedCombinationsInOrder.Exists(x => x[0] == SpeedTambolaGameManager.Instance.store.FourCorners_list[0] && x[1] == SpeedTambolaGameManager.Instance.store.FourCorners_list[1] && x[2] == SpeedTambolaGameManager.Instance.store.FourCorners_list[2] && x[^1] == SpeedTambolaGameManager.Instance.store.FourCorners_list[^1]) && !SpeedTambolaGameManager.Instance.FourCornersMarked)
                SpeedTambolaGameManager.Instance.FinishedCombinationsInOrder.Add(SpeedTambolaGameManager.Instance.store.FourCorners_list);
            // if (SpeedTambolaScoreManager.ScoreInstance.isAnyButton)
            // {
            // 	if (SpeedTambolaGameManager.Instance.shuffleCall.Contains(Convert.ToInt32(cell.GetComponentInChildren<TextMeshProUGUI>().text)))
            // 	SpeedTambolaGameManager.Instance.shuffleCall.Remove(Convert.ToInt32(cell.GetComponentInChildren<TextMeshProUGUI>().text));
            // }
            if (SpeedTambolaScoreManager.ScoreInstance.is2x)
            {
                SpeedTambolaGameManager.Instance.points += 200;
                if (test1 != null) StopCoroutine(test1);
                test1 = StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("Perfect Score!", 200, '+'));
                SpeedTambolaScoreManager.ScoreInstance.SetScore(SpeedTambolaGameManager.Instance.points);
            }
            else
            {
                SpeedTambolaGameManager.Instance.points += 100;
                if (test1 != null) StopCoroutine(test1);
                test1 = StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("Perfect Score!", 100, '+'));
                SpeedTambolaScoreManager.ScoreInstance.SetScore(SpeedTambolaGameManager.Instance.points);
            }
            //StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("Perfect Score!", 100, '+'));
        }
        else
        {
            if (SpeedTambolaScoreManager.ScoreInstance.TotalScore >= 100)
            {
                StartCoroutine("Blink");
                SpeedTambolaGameManager.Instance.points -= 100;
                if (test1 != null) StopCoroutine(test1);
                test1 = StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("Wrong Select!", 100, '-'));
                SpeedTambolaScoreManager.ScoreInstance.SetScore(SpeedTambolaGameManager.Instance.points);
            }

            else
            {
                int buffer = SpeedTambolaGameManager.Instance.points;
                SpeedTambolaGameManager.Instance.points -= SpeedTambolaGameManager.Instance.points;
                if (test1 != null) StopCoroutine(test1);
                test1 = StartCoroutine(SpeedTambolaScoreManager.ScoreInstance.ScoreUpdates("Penalty!", 100, '-'));
                SpeedTambolaScoreManager.ScoreInstance.SetScore(SpeedTambolaGameManager.Instance.points);
                StartCoroutine("Blink");
            }
        }
    }
    Coroutine test1;
    IEnumerator Blink()
    {
        cell.GetComponent<Image>().color = new Color32(255, 0, 0, 175);
        cell.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        yield return new WaitForSeconds(0.5f);
        cell.GetComponent<Image>().color = Color.white;
        cell.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
    }

    void OnEnable()
    {
        Invoke(nameof(AssignData), 1 / 60);
    }

    private void AssignData()
    {
        SpeedTambolaGameManager.Instance.IDKMeter.fillAmount = 0.0f;
        cell.interactable = true;
        cell.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        //SpeedTambolaScoreManager.ScoreInstance.RanFourPanel.SetActive(false);
    }

    public bool isValid()
    {
        if (value.text == SpeedTambolaGameManager.Instance.Called[SpeedTambolaGameManager.Instance.Called.Count - 1]) return true;
        if (SpeedTambolaScoreManager.ScoreInstance.isAnyButton || !SpeedTambolaGameController.Controller.CentralTimer)
        {
            SpeedTambolaScoreManager.ScoreInstance.isAnyButton = false;
            SpeedTambolaScoreManager.ScoreInstance.isAbilityActive = false;
            SpeedTambolaGameController.Controller.Gameplay = true;
            SpeedTambolaScoreManager.ScoreInstance.AbilityAndScoreUpdates.SetActive(false);
            foreach (Transform i in SpeedTambolaScoreManager.ScoreInstance.AbilityAndScoreUpdates.transform)
            {
                i.gameObject.SetActive(true);
            }
            SpeedTambolaScoreManager.ScoreInstance.AbilityAndScoreUpdates.transform.GetChild(0).gameObject.SetActive(false);
            SpeedTambolaScoreManager.ScoreInstance.DisableAbilities();
            SpeedTambolaScoreManager.ScoreInstance.InactiveAbilities[SpeedTambolaScoreManager.ScoreInstance.objectIndex].SetActive(false);
            SpeedTambolaScoreManager.ScoreInstance.ButtonGridBottom.SetActive(true);

            //if (SpeedTambolaScoreManager.ScoreInstance.UnusedAbilities.Count > 0)
            //{
            //    SpeedTambolaScoreManager.ScoreInstance.InactiveAbilities[SpeedTambolaScoreManager.ScoreInstance.UnusedAbilities.Count - 1].SetActive(true);
            //    SpeedTambolaScoreManager.ScoreInstance.UnusedAbilities.Remove(SpeedTambolaScoreManager.ScoreInstance.UnusedAbilities[SpeedTambolaScoreManager.ScoreInstance.UnusedAbilities.Count - 1]);
            //    SpeedTambolaScoreManager.ScoreInstance.UnusedAbilityActive = true;
            //}
            return true;
        }

        if (SpeedTambolaScoreManager.ScoreInstance.isRanFour || !SpeedTambolaGameController.Controller.CentralTimer)
        {
            SpeedTambolaScoreManager.ScoreInstance.isRanFour = false;
            SpeedTambolaScoreManager.ScoreInstance.isAbilityActive = false;
            SpeedTambolaGameController.Controller.Gameplay = true;
            SpeedTambolaGameManager.Instance.TimerValue = 0;
            //SpeedTambolaScoreManager.ScoreInstance.RanFourPanel.SetActive(false);
            // if (SpeedTambolaGameManager.Instance.shuffleCall.Contains(Convert.ToInt32(cell.GetComponentInChildren<TextMeshProUGUI>().text)))
            // 	SpeedTambolaGameManager.Instance.shuffleCall.Remove(Convert.ToInt32(cell.GetComponentInChildren<TextMeshProUGUI>().text));
            return true;
        }
        return false;
    }

    private void MarkButton()
    {
        SpeedTambolaGameManager.Instance.OnButtonPress(AssignedNumber);
    }
}