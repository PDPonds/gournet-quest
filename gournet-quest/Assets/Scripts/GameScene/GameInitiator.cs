using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameInitiator : MonoBehaviour
{
    
    [SerializeField] GameObject gameScene_Camera;
    [SerializeField] GameObject gameScene_PostProcessing;
    [SerializeField] GameObject gameScene_DirectionalLight;

    private void Start()
    {
        GameManager.Instance.InitPlayer(Vector3.zero);
        BindObj();
    }

    void BindObj()
    {
        GameObject camera = Instantiate(gameScene_Camera);
        CameraController camController = camera.GetComponent<CameraController>();
        camController.SetupCamera(GameManager.Instance.playerManager.transform);

        GameObject postProcessing = Instantiate(gameScene_PostProcessing);
        GameObject directionalLight = Instantiate(gameScene_DirectionalLight);
    }

}
