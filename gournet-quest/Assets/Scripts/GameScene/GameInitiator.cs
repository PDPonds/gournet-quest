using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitiator : MonoBehaviour
{
    [SerializeField] GameObject gameScene_PostProcessing;
    [SerializeField] GameObject gameScene_DirectionalLight;

    private void Start()
    {
        BindObj();
        GameManager.Instance.InitPlayer(Vector3.zero);
    }

    void BindObj()
    {
        GameObject postProcessing = Instantiate(gameScene_PostProcessing);
        GameObject directionalLight = Instantiate(gameScene_DirectionalLight);
    }

}
