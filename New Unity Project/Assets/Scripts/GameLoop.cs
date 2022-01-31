using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;
using UnityEngine.SceneManagement;


// TODO: Sounds,  try Climber recognition


[RequireComponent(typeof(ARRaycastManager))]



public class ScoreObject
{
    public int score = 0;
    public string teamName = "NoName";
}


public class GameLoop : MonoBehaviour
{

    [SerializeField]
    private Animator transition;

    [SerializeField]
    private GameObject dragon;

    [SerializeField]
    AudioManager myAudio;

    [SerializeField]
    private GameObject coinsItem;

    [SerializeField]
    private TextMeshProUGUI showLifes;

    [SerializeField]
    private GameObject BackgroundPanel;

    private GameObject spawnedDragon;

    private ARRaycastManager _arRaycastManager;
    private ARPlaneManager _arPlaneManager;

    [SerializeField]
    private Camera _arCamera;

    private List<Vector3> outlinePoints = new List<Vector3>();
    private int oldQ = 0;
    List<Vector3> Q = new List<Vector3>();

    private float speedDragon = 0.15f;
    private Vector3 oldPos = Vector3.zero;

    private int lifes = 5;
    private int score = 0;
    

    enum gameState
    {
        OBJECT_APPEARS,
        DRAGON_MOVES,
        DRAGON_COLLECTED,
        PLAYER_COLLECTED,
        COINS_COLLECTED,
        GAME_OVER
    }

    gameState state = gameState.OBJECT_APPEARS;



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Now Starting");
        
        SetPlaneOutline planeOutline = GameObject.FindObjectOfType<SetPlaneOutline>();
        outlinePoints = planeOutline.GetOutlinePoints();

        Debug.Log("Outline Points: ");
        Debug.Log(outlinePoints[0].x + "," + outlinePoints[0].y + "," + outlinePoints[0].z);
        Debug.Log(outlinePoints[1].x + "," + outlinePoints[1].y + "," + outlinePoints[1].z);
        Debug.Log(outlinePoints[2].x + "," + outlinePoints[2].y + "," + outlinePoints[2].z);
        Debug.Log(outlinePoints[3].x + "," + outlinePoints[3].y + "," + outlinePoints[3].z);


        // spawn in other quadrant of the game. Q1 (upper line midpoint), Q2 (right line midpoint)....

        
        Q.Add(new Vector3((outlinePoints[0].x + outlinePoints[1].x) / 2.0f, (outlinePoints[0].y + outlinePoints[1].y) / 2.0f, (outlinePoints[0].z + outlinePoints[1].z) / 2.0f));
        Q.Add(new Vector3((outlinePoints[1].x + outlinePoints[2].x) / 2.0f, (outlinePoints[1].y + outlinePoints[2].y) / 2.0f, (outlinePoints[1].z + outlinePoints[2].z) / 2.0f));
        Q.Add(new Vector3((outlinePoints[2].x + outlinePoints[3].x) / 2.0f, (outlinePoints[2].y + outlinePoints[3].y) / 2.0f, (outlinePoints[2].z + outlinePoints[3].z) / 2.0f));
        Q.Add(new Vector3((outlinePoints[3].x + outlinePoints[0].x) / 2.0f, (outlinePoints[3].y + outlinePoints[0].y) / 2.0f, (outlinePoints[3].z + outlinePoints[0].z) / 2.0f));

        // calculate random starting position. not perfect if plane is non rectangular....

        float posx = Random.Range(outlinePoints[0].x, outlinePoints[1].x);
        float posy = Random.Range(outlinePoints[0].y, outlinePoints[2].y);
        float posz = outlinePoints[0].z;

        Vector3 pos = new Vector3(posx, posy, posz);

        // better raycasting against the plane??

        spawnedDragon = Instantiate(dragon, pos , dragon.transform.rotation);
        oldPos = spawnedDragon.transform.position;
        spawnedDragon.GetComponent<Animation>().Play();
        BackgroundPanel.SetActive(true);
        showLifes.SetText("Lifes: " + lifes.ToString());




    }

    private void Awake()
    {
        

        _arRaycastManager = GetComponent<ARRaycastManager>();
        _arPlaneManager = GetComponent<ARPlaneManager>();
        BackgroundPanel.SetActive(false);

        Debug.Log("First inactive");
            
        this.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log(state);
        switch (state)
        {
            case gameState.OBJECT_APPEARS:
                // Spawn Items randomly
                coinsItem = randomlySpawnItem(coinsItem);
                coinsItem.SetActive(true);
                coinsItem.transform.localScale = new Vector3(1, 1, 1);
                state = gameState.DRAGON_MOVES;

                break;

            case gameState.DRAGON_MOVES:

                // Move Dragon towards the randomly spawned object --> how to make sure they are not too close?
                
                moveDragon(coinsItem.transform.position - new Vector3(0, 0.2f, 0)); // Workaround because of weird offset...

                // Wait for input from player
                if (TryTouchItem()) 
                {
                    myAudio.playCoinsClink();
                    
                    StartCoroutine(AnimateCollection(true));
                    //coinsItem.SetActive(false);
                    score += 1;

                    if (score%10 == 0)
                    {
                        speedDragon += 0.05f;
                    }

                    state = gameState.COINS_COLLECTED;
                }

                // Check if dragon is at position earlier
                // TODO: Add sounds? 
                if ((spawnedDragon.transform.position + new Vector3(0, 0.2f ,0)) == coinsItem.transform.position) // Workaround because of weird offset... 0.2
                {

                    // animate shrinking of coins? Add sound?
                    //myAudio.playDragonRoar();
                    myAudio.playDragonRoar();
                    StartCoroutine(AnimateCollection(false));
                    //
                    //coinsItem.SetActive(false);
                    lifes -= 1;
                    showLifes.SetText("Lifes: " + lifes.ToString());

                    state = gameState.COINS_COLLECTED;
                }

                

                break;

            case gameState.DRAGON_COLLECTED:

                // If dragon is at pos of the spawned object take a life and spawn new item if still life left


                if (lifes < 1)
                {
                    // Game over
                    state = gameState.GAME_OVER;
                } else
                {
                    state = gameState.OBJECT_APPEARS;
                }

                break;

            case gameState.PLAYER_COLLECTED:

                // If player collected score a point and spawn new item

                state = gameState.OBJECT_APPEARS;

                break;

            case gameState.COINS_COLLECTED:

                    //wait for coroutine to finish?    

                break;

            case gameState.GAME_OVER:

                Debug.Log("Game Over, starting scoreboard scene");

                ScoreObject myScore = new ScoreObject();
                myScore.score = score;
                myScore.teamName = PlayerPrefs.GetString("Teamname", "NoName");
                string ScoreAndTeam = JsonUtility.ToJson(myScore);

                PlayerPrefs.SetString("ScoreAndTeam", ScoreAndTeam);

                StartCoroutine(LoadNewScene());
                break;

            default:

                Debug.Log("Something went wrong");

                break;


        }


    }


    void moveDragon(Vector3 pos)
    {
        Vector3 direction = pos - spawnedDragon.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);        

        spawnedDragon.transform.position = Vector3.MoveTowards(spawnedDragon.transform.position, pos, speedDragon * Time.deltaTime);
        spawnedDragon.transform.rotation = Quaternion.Slerp(spawnedDragon.transform.rotation, targetRotation, 2f * Time.deltaTime);


    }

    GameObject randomlySpawnItem(GameObject item)
    {
        int newQ = 0;
        float posx = 0f;
        float posy = 0f;
        float posz = outlinePoints[0].z;
        while (newQ == oldQ)
        {
            newQ = Random.Range(0, 4);
        }
        Debug.Log("Old Q: " + oldQ);
        Debug.Log("New Q: " + newQ);

        switch (newQ)
        {
            case 0:
                // Q0 left up
                posx = Random.Range(outlinePoints[0].x, Q[0].x);
                posy = Random.Range(outlinePoints[0].y, Q[3].y);        
                break;

            case 1:
                // Q1 right up
                posx = Random.Range(Q[0].x, outlinePoints[1].x);
                posy = Random.Range(outlinePoints[1].y, Q[1].y);
                break;

            case 2:
                // Q2 right down
                posx = Random.Range(Q[2].x, outlinePoints[2].x);
                posy = Random.Range(outlinePoints[0].y, Q[1].y);
                break;

            case 3:
                // Q3 left down
                posx = Random.Range(outlinePoints[3].x, Q[2].x);
                posy = Random.Range(outlinePoints[3].y, Q[3].y);
                break;

            default:
                Debug.Log("Something went wrong");
                break;
        }

        oldQ = newQ;


        Vector3 pos = new Vector3(posx, posy, posz);

        return item = Instantiate(item, pos, Quaternion.LookRotation(Vector3.up)); // seems good
    }

    bool TryTouchItem() // Works now
    {
        if (Input.touchCount > 0)
        {
            Touch theTouch = Input.GetTouch(0);

            if (theTouch.phase == TouchPhase.Began)
            {
                Debug.Log("Got the touch");

                Ray ray = Camera.main.ScreenPointToRay(theTouch.position);
                
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo))
                {
                    Debug.Log("Got the raycast");

                    var rig = hitInfo.collider.GetComponent<Rigidbody>();

                    if (rig != null)
                    {
                        Debug.Log("Collided with object");
                        return true;
                    }
                    
                }            
            }
        }
        
        return false;
    }

    IEnumerator LoadNewScene()
    {
        transition.SetTrigger("startTransition");

        yield return new WaitForSeconds(1);


        SceneManager.LoadScene("ScoreScene");
    }

    IEnumerator AnimateCollection(bool PlayerCollected)
    {

        if (PlayerCollected)
        {
            //FindObjectOfType<AudioManager>().playCoinsClink();
            coinsItem.GetComponent<Animation>().Play();
            //dragon.GetComponent<Animation>().Play("SJ001_hurt"); // would be nice but hard to time it right? 
            yield return new WaitForSeconds(0.25f);

            coinsItem.SetActive(false);
            state = gameState.PLAYER_COLLECTED;
        }
        else
        {
            //FindObjectOfType<AudioManager>().playDragonRoar();
            coinsItem.GetComponent<Animation>().Play();
            //dragon.GetComponent<Animation>().Play("SJ001_skill1");
            yield return new WaitForSeconds(0.25f);
            coinsItem.SetActive(false);
            state = gameState.DRAGON_COLLECTED;
        }

    }



}
