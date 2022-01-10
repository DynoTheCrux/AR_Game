using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartAR : MonoBehaviour
{

    [SerializeField]
    private Animator transition;

    public void onPlayButtonPressed()
    {
        Debug.Log("StartAR: " + PlayerPrefs.GetString("Teamname"));

        StartCoroutine(LoadNewScene());



    }

    IEnumerator LoadNewScene()
    {
        transition.SetTrigger("startTransition");

        yield return new WaitForSeconds(1);


        SceneManager.LoadScene("AR_GameScene");
    }

}
