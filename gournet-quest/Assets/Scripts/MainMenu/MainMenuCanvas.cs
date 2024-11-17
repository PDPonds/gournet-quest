using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuCanvas : MonoBehaviour
{
    [SerializeField] Button startGame_But;

    private void Awake()
    {
        startGame_But.onClick.AddListener(StartGame_Click);
    }

    void StartGame_Click()
    {
        SceneManager.LoadScene(1);
    }

}
