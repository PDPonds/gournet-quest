using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CookingPhase
{
    PrepareProcess, DelayToStart, Action, Process_Success, Process_Fail, SummaryMenu
}

public class CookingManager : Singleton<CookingManager>
{

    [HideInInspector] public CookingUIManager cookingUIManager;

    CookingPhase curPhase;
    [HideInInspector] public int curProcessIndex;

    [HideInInspector] public float curTime = 0;
    [HideInInspector] public float curDelayTime = 0;
    [HideInInspector] public float success_and_fail_time = 0;

    [Header("==== Delay To Start =====")]
    [SerializeField] float delayTime;

    [Header("==== Sprinkle Process =====")]
    [SerializeField] GameObject sprinkle_MiniGame;
    Sprinkle_Cooking_MiniGame sprinkle_cooking_miniGame;

    [SerializeField] GameObject salt;
    [SerializeField] GameObject pepper;

    [Header("==== Grill Process =====")]
    [SerializeField] GameObject grill_MiniGame;
    Grill_Cooking_MiniGame grill_cooking_miniGame;

    [SerializeField] GameObject grill_pan;

    [Header("==== Fried Process =====")]
    [SerializeField] GameObject fried_pan;

    [Header("==== Deep Fried Process =====")]
    [SerializeField] GameObject deepFried_pan;

    private void Awake()
    {
        cookingUIManager = GetComponent<CookingUIManager>();
        sprinkle_cooking_miniGame = sprinkle_MiniGame.GetComponent<Sprinkle_Cooking_MiniGame>();
        grill_cooking_miniGame = grill_MiniGame.GetComponent<Grill_Cooking_MiniGame>();
    }

    private void Start()
    {
        curProcessIndex = 0;
        SwitchPhase(CookingPhase.PrepareProcess);
    }

    private void Update()
    {
        UpdatePhase();
    }

    public void SwitchPhase(CookingPhase phase)
    {
        curPhase = phase;
        Process curProcess = GameManager.Instance.curCookingMenu.processes[curProcessIndex];

        switch (curPhase)
        {
            case CookingPhase.PrepareProcess:

                sprinkle_MiniGame.SetActive(false);
                grill_MiniGame.SetActive(false);
                salt.SetActive(false);
                pepper.SetActive(false);
                grill_pan.SetActive(false);
                fried_pan.SetActive(false);
                deepFried_pan.SetActive(false);

                cookingUIManager.HideFail();
                cookingUIManager.HideSuccess();

                cookingUIManager.UpdateProgressionBar(0f, 1f);

                switch (curProcess)
                {
                    case Sprinkle_Process sprinkle:

                        sprinkle_MiniGame.SetActive(true);
                        sprinkle_cooking_miniGame.Setup(sprinkle.sprinkle_delay_per_target);

                        sprinkle_cooking_miniGame.sprinkle_progression = 0;

                        if (sprinkle.sprinkleTypes.Length > 0)
                        {
                            for (int i = 0; i < sprinkle.sprinkleTypes.Length; i++)
                            {
                                sprinkle_cooking_miniGame.correctList.Add(sprinkle.sprinkleTypes[i]);

                                if (sprinkle.sprinkleTypes[i] == SprinkleType.Salt)
                                {
                                    salt.SetActive(true);
                                }
                                else if (sprinkle.sprinkleTypes[i] == SprinkleType.Pepper)
                                {
                                    pepper.SetActive(true);
                                }
                            }
                        }

                        if (sprinkle.sprinkle_obstacle.Length > 0)
                        {
                            for (int i = 0; i < sprinkle.sprinkle_obstacle.Length; i++)
                            {
                                sprinkle_cooking_miniGame.obtacleList.Add(sprinkle.sprinkle_obstacle[i]);
                            }
                        }

                        if (sprinkle.ingredients.Length > 0)
                        {
                            for (int i = 0; i < sprinkle.ingredients.Length; i++)
                            {
                                GameObject go = Instantiate(sprinkle.ingredients[i], Vector3.zero, Quaternion.identity);
                            }
                        }

                        break;
                    case Grill_Process grill:

                        grill_MiniGame.SetActive(true);
                        grill_pan.SetActive(true);

                        grill_cooking_miniGame.RandomLength(grill.grill_length);
                        grill_cooking_miniGame.Setup(grill.grill_increase_position_per_click, grill.grill_decrease_per_time , 
                            grill.grill_increase_progression_per_time);

                        if (grill.ingredients.Length > 0)
                        {
                            for (int i = 0; i < grill.ingredients.Length; i++)
                            {
                                GameObject go = Instantiate(grill.ingredients[i], Vector3.zero, Quaternion.identity);
                            }
                        }

                        break;
                }

                SwitchPhase(CookingPhase.DelayToStart);

                break;
            case CookingPhase.DelayToStart:

                curDelayTime = delayTime;
                cookingUIManager.ShowDelayBorder();

                break;
            case CookingPhase.Action:

                switch (curProcess)
                {
                    case Sprinkle_Process sprinkle:
                        break;
                    case Grill_Process grill:
                        break;
                }

                curTime = curProcess.process_Time_Sec;

                break;
            case CookingPhase.Process_Success:

                cookingUIManager.ShowSucess();
                success_and_fail_time = 3f;

                switch (curProcess)
                {
                    case Sprinkle_Process sprinkle:
                        break;
                    case Grill_Process grill:
                        break;
                }

                break;
            case CookingPhase.Process_Fail:

                cookingUIManager.ShowFail();
                success_and_fail_time = 3f;

                switch (curProcess)
                {
                    case Sprinkle_Process sprinkle:
                        break;
                    case Grill_Process grill:
                        break;
                }

                break;
            case CookingPhase.SummaryMenu:
                
                break;
        }


    }

    void UpdatePhase()
    {
        Process curProcess = GameManager.Instance.curCookingMenu.processes[curProcessIndex];

        switch (curPhase)
        {
            case CookingPhase.PrepareProcess:

                switch (curProcess)
                {
                    case Sprinkle_Process sprinkle:
                        break;
                    case Grill_Process grill:
                        break;
                }

                break;
            case CookingPhase.DelayToStart:

                curDelayTime -= Time.deltaTime;
                cookingUIManager.UpdateDelayTime(curDelayTime);
                if (curDelayTime <= 0)
                {
                    cookingUIManager.HideDelayBorder();
                    SwitchPhase(CookingPhase.Action);
                }

                break;
            case CookingPhase.Action:

                curTime -= Time.deltaTime;
                cookingUIManager.UpdateActionTime(curTime, curProcess.process_Time_Sec);
                if (curTime <= 0)
                {
                    curTime = 0;
                    SwitchPhase(CookingPhase.Process_Fail);
                }

                switch (curProcess)
                {
                    case Sprinkle_Process sprinkle:
                        break;
                    case Grill_Process grill:
                        break;
                }

                break;
            case CookingPhase.Process_Success:

                success_and_fail_time -= Time.deltaTime;
                if (success_and_fail_time <= 0)
                {
                    curProcessIndex++;
                    if (curProcessIndex >= GameManager.Instance.curCookingMenu.processes.Count)
                    {
                        SwitchPhase(CookingPhase.SummaryMenu);
                    }
                    else
                    {
                        SwitchPhase(CookingPhase.PrepareProcess);
                    }
                }

                switch (curProcess)
                {
                    case Sprinkle_Process sprinkle:
                        break;
                    case Grill_Process grill:
                        break;
                }

                break;
            case CookingPhase.Process_Fail:

                success_and_fail_time -= Time.deltaTime;
                if (success_and_fail_time <= 0)
                {
                    curProcessIndex++;
                    if (curProcessIndex >= GameManager.Instance.curCookingMenu.processes.Count)
                    {
                        SwitchPhase(CookingPhase.SummaryMenu);
                    }
                    else
                    {
                        SwitchPhase(CookingPhase.PrepareProcess);
                    }
                }

                switch (curProcess)
                {
                    case Sprinkle_Process sprinkle:
                        break;
                    case Grill_Process grill:
                        break;
                }
                break;
            case CookingPhase.SummaryMenu:
                Debug.Log("Summary Menu");
                break;
        }

    }

    public bool isPhase(CookingPhase phase)
    {
        return curPhase == phase;
    }

    public void SpaceAction()
    {
        if (isPhase(CookingPhase.Action))
        {
            Process curProcess = GameManager.Instance.curCookingMenu.processes[curProcessIndex];
            switch (curProcess)
            {
                case Sprinkle_Process sprinkle:
                    sprinkle_cooking_miniGame.ActivateHitPoint();
                    break;
                case Grill_Process grill:
                    grill_cooking_miniGame.IncreaseFirePoint();
                    break;
            }
        }
    }

}
