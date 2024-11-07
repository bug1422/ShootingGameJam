using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasEvent : MonoBehaviour
{
    private static CanvasEvent instance;
    // Start is called before the first frame update
    Transform winScene;
    Transform hud;

    void Start()
    {
        var len = transform.childCount;
        for (int i = 0; i < len; i++)
        {
            var obj = transform.GetChild(i);
            if (obj.name == "WinScene")
            {
                obj.gameObject.SetActive(false);
                winScene = obj;
            }
            else if (obj.name == "HUD")
            {
                obj.gameObject.SetActive(true);
                hud = obj;
            }
        }
        PlayerControl.OnWin += OpenWinScene;
    }
    private void OnDestroy()
    {
        PlayerControl.OnWin -= OpenWinScene;
    }
    void OpenWinScene()
    {
        hud.gameObject.SetActive(false);
        var list = winScene.GetComponentsInChildren<Button>();
        var name = SceneManager.GetActiveScene().name;
        var nextSceneName = "";
        switch (name)
        {
            case "Level_1":
                nextSceneName = "Level_2";
                break;
            case "Level_2":
                nextSceneName = "Level_3";
                break;
            case "Level_3":
                list[0].gameObject.SetActive(false);
                nextSceneName = "Level_1";
                break;
        }
        list[0].onClick.AddListener(() =>
        {
            SceneManager.LoadScene(nextSceneName);
        });
        list[1].onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
        list[2].onClick.AddListener(() =>
        {
            SceneManager.LoadScene("MainMenu");
        });
        winScene.gameObject.SetActive(true);
    }
    public void OpenNextStage()
    {

    }
}
