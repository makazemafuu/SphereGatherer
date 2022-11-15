using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IAMenu : MonoBehaviour
{
    public void Restart()
    {
        Debug.Log("Restart the game !");
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {

#if UNITY_EDITOR
UnityEditor.EditorApplication.isPlaying = false;
#endif

        Debug.Log("Quit !");
        Application.Quit();
    }
}
