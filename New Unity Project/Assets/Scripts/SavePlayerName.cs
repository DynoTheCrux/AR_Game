using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SavePlayerName : MonoBehaviour
{
    public TMP_InputField inputTeamname;
    void Start()
    {
        var input = gameObject.GetComponent<TMP_InputField>();
        //var se = new InputField.SubmitEvent();
        //se.AddListener(SubmitName);
        //input.onEndEdit = se;

        //or simply use the line below, 
        input.onEndEdit.AddListener(SubmitName);  // This also works
    }

    private void SubmitName(string arg0)
    {
        PlayerPrefs.SetString("Teamname", arg0);

        Debug.Log("SavePlayerName: " + arg0);
    }
}
