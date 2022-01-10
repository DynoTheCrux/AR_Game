using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    [SerializeField]
    private Animator transition;


    public void onRestartButtonPressed()
    {
        //Debug.Log("StartAR: " + PlayerPrefs.GetString("Teamname"));
        //SceneManager.LoadScene("AR_GameScene");
        StartCoroutine(LoadNewScene());

    }


    IEnumerator LoadNewScene()
    {
        transition.SetTrigger("startTransition");

        yield return new WaitForSeconds(1);


        SceneManager.LoadScene("AR_GameScene");
    }
}
