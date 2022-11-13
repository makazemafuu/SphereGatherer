using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public void StartDemo()
    {
        Debug.Log("Start the game !");
        SceneManager.LoadScene(0);
    }
}
