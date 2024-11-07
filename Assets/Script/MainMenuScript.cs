using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void EnterGame()
    {
        SceneManager.LoadScene("Level_1");
    }
#if UNITY_WEBPLAYER
        public static string webplayerQuitURL = "http://google.com";
#endif
    public void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_WEBPLAYER
                Application.OpenURL(webplayerQuitURL);
        #else
                Application.Quit();
        #endif
    }
}
