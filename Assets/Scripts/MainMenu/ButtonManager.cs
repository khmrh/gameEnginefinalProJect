using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngineInternal;

public class ButtonManager : MonoBehaviour
{
    public void startbutton()
    {
        SceneManager.LoadScene("ingame");
    }

    public void gotoGameEnd()
    {
        Application.Quit();

        //테스트용
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
