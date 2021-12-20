using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveTeamName : MonoBehaviour
{

    public TMP_InputField inputTeamname;


    private void Awake()
    {
        inputTeamname = GetComponent<TMP_InputField>();
    }


    public void onEndEdit()
    {        
        PlayerPrefs.SetString("Teamname", inputTeamname.text);
    }

}
