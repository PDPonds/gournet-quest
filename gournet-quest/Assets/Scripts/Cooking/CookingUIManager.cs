using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CookingUIManager : MonoBehaviour
{
    [Header("===== Time =====")]
    [SerializeField] Image actionTimeFill;
    [Header("===== Delay =====")]
    [SerializeField] GameObject delayBorder;
    [SerializeField] TextMeshProUGUI delayTimeText;
    [Header("===== Progression =====")]
    [SerializeField] Image progressionFill;
    [Header("===== Summary Process =====")]
    [SerializeField] GameObject success_UI;
    [SerializeField] GameObject fail_UI;
    [Header("===== LoadingScene =====")]
    public GameObject loadingPanel;
    public Image loadingFill;

    public void UpdateActionTime(float c, float m)
    {
        float p = c / m;
        actionTimeFill.fillAmount = p;
    }

    public void UpdateProgressionBar(float c, float m)
    {
        float p = c / m;
        Debug.Log(p);
        progressionFill.fillAmount = p;
    }


    public void ShowDelayBorder()
    {
        delayBorder.SetActive(true);
    }

    public void HideDelayBorder()
    {
        delayBorder.SetActive(false);
    }

    public void UpdateDelayTime(float c)
    {
        delayTimeText.text = c.ToString("F0");
    }

    public void ShowSucess()
    {
        success_UI.SetActive(true);
    }

    public void HideSuccess()
    {
        success_UI.SetActive(false);
    }

    public void ShowFail()
    {
        fail_UI.SetActive(true);
    }

    public void HideFail()
    {
        fail_UI.SetActive(false);
    }

}
