using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void onRestartButtonPressed()
    {
        //Debug.Log("StartAR: " + PlayerPrefs.GetString("Teamname"));
        SceneManager.LoadScene("AR_GameScene");


    }
}
