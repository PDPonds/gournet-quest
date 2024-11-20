using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public PlayerManager playerManager;

    [Header("===== General =====")]
    [SerializeField] GameObject Player;

    [Header("===== Main Menu =====")]
    [SerializeField] GameObject mainMenu_Camera;
    [SerializeField] GameObject mainMenu_Directional_Light;
    [SerializeField] GameObject mainMenu_Post_Processing;
    [SerializeField] GameObject mainMenu_Canvas;
    [SerializeField] GameObject mainMenu_EventSystem;

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
        GameObject eventSystem = Instantiate(mainMenu_EventSystem);
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
        this.playerManager = playerManager;
    }

}
