using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("===== Main Menu =====")]
    [SerializeField] GameObject mainMenu_Camera;
    [SerializeField] GameObject mainMenu_Directional_Light;
    [SerializeField] GameObject mainMenu_Post_Processing;
    [SerializeField] GameObject mainMenu_Canvas;
    [SerializeField] GameObject mainMenu_EventSystem;

    private void Awake()
    {
        InitMainMenuObject();
    }

    void InitMainMenuObject()
    {
        GameObject cameraObj = Instantiate(mainMenu_Camera);
        GameObject directionalLightObj = Instantiate(mainMenu_Directional_Light);
        GameObject postProcessingObj = Instantiate(mainMenu_Post_Processing);
        GameObject canvasObj = Instantiate(mainMenu_Canvas);
        GameObject eventSystemObj = Instantiate(mainMenu_EventSystem);


    }

}
