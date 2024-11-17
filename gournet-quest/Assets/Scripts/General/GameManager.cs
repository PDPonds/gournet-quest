using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [Header("===== General =====")]
    [SerializeField] GameObject Player;

    [Header("===== Main Menu =====")]
    [SerializeField] GameObject mainMenu_Camera;
    [SerializeField] GameObject mainMenu_Directional_Light;
    [SerializeField] GameObject mainMenu_Post_Processing;
    [SerializeField] GameObject mainMenu_Canvas;
    [SerializeField] GameObject mainMenu_EventSystem;
    [SerializeField] GameObject mainMenu_Manager;

    private void Awake()
    {
        BindMainMenuObject();

        DontDestroyOnLoad(gameObject);
    }

    void BindMainMenuObject()
    {
        GameObject cameraObj = Instantiate(mainMenu_Camera);
        GameObject directionalLightObj = Instantiate(mainMenu_Directional_Light);
        GameObject postProcessingObj = Instantiate(mainMenu_Post_Processing);
        GameObject canvasObj = Instantiate(mainMenu_Canvas);
        GameObject eventSystemObj = Instantiate(mainMenu_EventSystem);
        GameObject mainMenuManager = Instantiate(mainMenu_Manager);
    }

    public IEnumerator LoadLevelAsync(int sceneIndex, Image progressionBar)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            progressionBar.fillAmount = progressValue;
            yield return null;
        }
    }

    public void InitPlayer(Vector3 spawnPosition)
    {
        GameObject playerObj = Instantiate(Player, spawnPosition, Quaternion.identity);
        PlayerManager playerManager = playerObj.GetComponent<PlayerManager>();
        playerManager.SetupPlayer();
    }

}
