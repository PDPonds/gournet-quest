using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sprinkle_Cooking_MiniGame : MonoBehaviour
{

    public List<SprinkleType> correctList = new List<SprinkleType>();
    public List<SprinkleType> obtacleList = new List<SprinkleType>();

    float max_delay;
    float sprinkle_delay;
    public float sprinkle_progression;

    [Header("===== Hit =====")]
    [SerializeField] Transform hitPoint;

    [Header("===== Prefab =====")]
    [SerializeField] GameObject sprikle_target_prefab;

    [Header("===== Sprite of Sprinkle Type =====")]
    [SerializeField] Sprite salt_Icon;
    [SerializeField] Sprite pepper_Icon;
    [SerializeField] Sprite ground_Chili_Icon;

    [Header("===== Init Point =====")]
    [SerializeField] Transform initPosition;

    public void Setup(float delayToInit)
    {
        max_delay = delayToInit;
        sprinkle_delay = delayToInit;
    }

    private void Update()
    {
        lookatCamera(hitPoint);

        if (CookingManager.Instance.isPhase(CookingPhase.Action))
        {
            sprinkle_delay -= Time.deltaTime;
            if (sprinkle_delay < 0)
            {
                sprinkle_delay = max_delay;
                RandomInitTarget();
            }
        }

    }

    void RandomInitTarget()
    {
        Process curProcess = GameManager.Instance.curCookingMenu.processes[CookingManager.Instance.curProcessIndex];
        Sprinkle_Process sprinkle = (Sprinkle_Process)curProcess;
        int rand = Random.Range(0, 10);
        if (rand <= sprinkle.success_Rate)
        {
            int i = Random.Range(0, correctList.Count);
            InitAndSetupSprinkleTarget(correctList[i]);
        }
        else
        {
            int i = Random.Range(0, obtacleList.Count);
            InitAndSetupSprinkleTarget(obtacleList[i]);
        }
    }

    void InitAndSetupSprinkleTarget(SprinkleType type)
    {
        Process curProcess = GameManager.Instance.curCookingMenu.processes[CookingManager.Instance.curProcessIndex];
        Sprinkle_Process sprinkle = (Sprinkle_Process)curProcess;
        GameObject go = Instantiate(sprikle_target_prefab, initPosition.position, Quaternion.identity);
        SpriteRenderer sprintRen = go.GetComponent<SpriteRenderer>();
        switch (type)
        {
            case SprinkleType.Salt:
                sprintRen.sprite = salt_Icon;
                break;
            case SprinkleType.Pepper:
                sprintRen.sprite = pepper_Icon;
                break;
            case SprinkleType.GroundChili:
                sprintRen.sprite = ground_Chili_Icon;
                break;
        }
        Sprinkle_MiniGame_Target target = go.GetComponent<Sprinkle_MiniGame_Target>();
        target.Setup(type, sprinkle.sprinkle_target_speed);
    }

    void lookatCamera(Transform transform)
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    public void ActivateHitPoint()
    {
        Process curProcess = GameManager.Instance.curCookingMenu.processes[CookingManager.Instance.curProcessIndex];
        Sprinkle_Process sprinkle = (Sprinkle_Process)curProcess;
        Collider2D collider = Physics2D.OverlapCircle(hitPoint.position, 1f);
        if (collider != null)
        {
            if (collider.CompareTag("Sprinkle_Target"))
            {
                if (collider.TryGetComponent<Sprinkle_MiniGame_Target>(out Sprinkle_MiniGame_Target target))
                {
                    if (correctList.Contains(target.type))
                    {
                        sprinkle_progression += sprinkle.GetIncrease_Progression_Count();
                        CookingManager.Instance.cookingUIManager.UpdateProgressionBar(sprinkle_progression, sprinkle.sprinkle_target_progression);
                        Destroy(target.gameObject);
                        if (sprinkle_progression >= sprinkle.sprinkle_target_progression)
                        {
                            CookingManager.Instance.SwitchPhase(CookingPhase.Process_Success);
                        }
                    }
                    else
                    {
                        if (sprinkle_progression > 0)
                        {
                            sprinkle_progression -= sprinkle.sprinkle_decrease_progression;
                            CookingManager.Instance.cookingUIManager.UpdateProgressionBar(sprinkle_progression, sprinkle.sprinkle_target_progression);
                            if (sprinkle_progression <= 0)
                            {
                                sprinkle_progression = 0;
                            }
                        }
                    }
                }
                else
                {
                    if (sprinkle_progression > 0)
                    {
                        sprinkle_progression -= sprinkle.sprinkle_decrease_progression;
                        CookingManager.Instance.cookingUIManager.UpdateProgressionBar(sprinkle_progression, sprinkle.sprinkle_target_progression);
                        if (sprinkle_progression <= 0)
                        {
                            sprinkle_progression = 0;
                        }
                    }
                }
            }
            else
            {
                if (sprinkle_progression > 0)
                {
                    sprinkle_progression -= sprinkle.sprinkle_decrease_progression;
                    CookingManager.Instance.cookingUIManager.UpdateProgressionBar(sprinkle_progression, sprinkle.sprinkle_target_progression);
                    if (sprinkle_progression <= 0)
                    {
                        sprinkle_progression = 0;
                    }
                }
            }
        }
        else
        {
            if (sprinkle_progression > 0)
            {
                sprinkle_progression -= sprinkle.sprinkle_decrease_progression;
                CookingManager.Instance.cookingUIManager.UpdateProgressionBar(sprinkle_progression, sprinkle.sprinkle_target_progression);
                if (sprinkle_progression <= 0)
                {
                    sprinkle_progression = 0;
                }
            }
        }
    }

}
