using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartAR : MonoBehaviour
{

    public void onPlayButtonPressed()
    {
        Debug.Log("StartAR: " + PlayerPrefs.GetString("Teamname"));
        SceneManager.LoadScene("AR_GameScene");


    }

}
