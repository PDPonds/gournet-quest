using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grill_Cooking_MiniGame : MonoBehaviour
{
    [SerializeField] Image topFill;
    [SerializeField] Image bottomFill;
    [SerializeField] RectTransform curPositionVisual;
    [SerializeField] RectTransform fireBorder;

    float startPos;
    float endPos;

    float curIncreaseCount;
    float decreaseSpeed;

    float curIncreaseProgressionPerTime;

    float curPosition;

    float curProgression;

    private void Update()
    {
        DecreasePositionPerTime();
        IncreaseProgression();
        UpdateVisualPosition();
    }

    void UpdateVisualPosition()
    {
        float ySize = fireBorder.sizeDelta.y;
        float curPos = ySize * curPosition;
        curPositionVisual.anchoredPosition = new Vector2(1, curPos);
    }

    public void IncreaseFirePoint()
    {
        curPosition += curIncreaseCount;
    }

    public void Setup(float increaseCount, float decreaseSpeed, float increaseProgressionPerTime)
    {
        curIncreaseCount = increaseCount;
        this.decreaseSpeed = decreaseSpeed;
        curIncreaseProgressionPerTime = increaseProgressionPerTime;
    }

    void IncreaseProgression()
    {
        if (curPosition > startPos && curPosition < endPos)
        {
            curProgression += curIncreaseProgressionPerTime * Time.deltaTime;
            CookingManager.Instance.cookingUIManager.UpdateProgressionBar(curProgression, 1f);
            if (curProgression >= 1f)
            {
                CookingManager.Instance.SwitchPhase(CookingPhase.Process_Success);
            }
        }
    }

    void DecreasePositionPerTime()
    {
        if (curPosition > 0) curPosition -= decreaseSpeed * Time.deltaTime;
    }

    public void RandomLength(float length)
    {
        startPos = Random.Range(0, 1f - length);
        endPos = startPos + length;

        bottomFill.fillAmount = startPos;
        float top = Mathf.Abs(1 - endPos);
        topFill.fillAmount = top;
    }

}
