using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    string NoScoreAndTeam = "{}";

    [SerializeField]
    private TextMeshProUGUI showScore;

    [SerializeField]
    private TextMeshProUGUI showHighScore;

    private void Awake()
    {
        


        ScoreObject myScore = JsonUtility.FromJson<ScoreObject>(PlayerPrefs.GetString("ScoreAndTeam", NoScoreAndTeam));
        Debug.Log("Got current Score: " + myScore.teamName + "," + myScore.score.ToString());

        ScoreObject highScore = JsonUtility.FromJson<ScoreObject>(PlayerPrefs.GetString("HighScore", NoScoreAndTeam));
        Debug.Log("Got current high Score: " + highScore.teamName + "," + highScore.score.ToString());

        showScore.SetText(myScore.teamName + " - " + myScore.score);

        if (highScore.score < myScore.score)
        {
            showHighScore.SetText(myScore.teamName + " - " + myScore.score);
            //string highScoreJson = JsonUtility.ToJson(highScore);
            PlayerPrefs.SetString("HighScore", JsonUtility.ToJson(myScore));

        } else
        {

            showHighScore.SetText(highScore.teamName + " - " + highScore.score);
        }


    }

    // Start is called before the first frame update
    void Start()
    {


        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
