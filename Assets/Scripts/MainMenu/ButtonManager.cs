using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngineInternal;

public class ButtonManager : MonoBehaviour
{
    public void startbutton()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("ingame");
    }

    public void gotoGameEnd()
    {
        Application.Quit();

        //�׽�Ʈ��
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
