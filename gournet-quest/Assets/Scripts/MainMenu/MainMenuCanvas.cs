using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuCanvas : MonoBehaviour
{
    [Header("===== Start Button =====")]
    [SerializeField] Button startGame_But;
    [Header("===== Loading Scene =====")]
    [SerializeField] GameObject loading_Panel;
    [SerializeField] Image loading_Progressing_Img;

    private void Awake()
    {
        startGame_But.onClick.AddListener(StartGame_Click);
    }

    void StartGame_Click()
    {
        loading_Panel.SetActive(true);
        StartCoroutine(GameManager.Instance.LoadLevelAsync(1, loading_Progressing_Img));
    }



}
