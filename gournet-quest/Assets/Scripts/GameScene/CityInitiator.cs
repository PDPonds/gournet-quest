using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CityInitiator : MonoBehaviour
{

    [SerializeField] GameObject city_Camera;
    [SerializeField] GameObject city_PostProcessing;
    [SerializeField] GameObject city_DirectionalLight;

    private void Start()
    {
        GameManager.Instance.InitPlayer(Vector3.zero);
        BindObj();
    }

    void BindObj()
    {
        GameObject camera = Instantiate(city_Camera);
        CameraController camController = camera.GetComponent<CameraController>();
        camController.SetupCamera(GameManager.Instance.playerManager.transform);

        PlayerManager.Instance.cameraController = camController;

        GameObject postProcessing = Instantiate(city_PostProcessing);
        GameObject directionalLight = Instantiate(city_DirectionalLight);
    }

}
