using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;



[RequireComponent(typeof(ARRaycastManager))]
//[RequireComponent(typeof(Camera))]
//[RequireComponent(typeof(ARPlaneManager))]

public class GameLoop : MonoBehaviour
{

    [SerializeField]
    private GameObject dragon;

    [SerializeField]
    private GameObject coinsItem;

    private GameObject spawnedDragon;

    private ARRaycastManager _arRaycastManager;

    [SerializeField]
    private Camera _arCamera;

    //private ARPlaneManager _arPlaneManager;
    private List<Vector3> outlinePoints = new List<Vector3>();

    private float speedDragon = 0.2f;
    private Vector3 oldPos = Vector3.zero;

    private int lifes = 5;

    enum gameState
    {
        OBJECT_APPEARS,
        DRAGON_MOVES,
        DRAGON_COLLECTED,
        PLAYER_COLLECTED
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

        // calculate random starting position. not perfect if plane is non rectangular....

        float posx = Random.Range(outlinePoints[0].x, outlinePoints[1].x);
        float posy = Random.Range(outlinePoints[0].y, outlinePoints[2].y);
        float posz = outlinePoints[0].z;

        Vector3 pos = new Vector3(posx, posy, posz);

        // better raycasting against the plane??

        spawnedDragon = Instantiate(dragon, pos , dragon.transform.rotation);
        oldPos = spawnedDragon.transform.position;
        //spawnedDragon.transform.Rotate(0, 180, 0);
        spawnedDragon.GetComponent<Animation>().Play();

        


    }

    private void Awake()
    {
        

        _arRaycastManager = GetComponent<ARRaycastManager>();
        //_arPlaneManager = GetComponent<ARPlaneManager>();

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
                state = gameState.DRAGON_MOVES;

                break;

            case gameState.DRAGON_MOVES:

                // Move Dragon towards the randomly spawned object --> how to make sure they are not too close?
                
                moveDragon(coinsItem.transform.position - new Vector3(0, 0.2f, 0)); // Workaround because of weird offset...

                // Wait for input from player?
                if (TryTouchItem())
                {
                    state = gameState.PLAYER_COLLECTED;
                }

                // Check if dragon is at position earlier
                if ((spawnedDragon.transform.position + new Vector3(0, 0.2f,0)) == coinsItem.transform.position) // Workaround because of weird offset...
                {

                    coinsItem.SetActive(false);

                    lifes -= 1;

                    state = gameState.DRAGON_COLLECTED;
                }

                break;

            case gameState.DRAGON_COLLECTED:

                // If dragon is at pos of the spawned object take a life and spawn new item if still life left

                if (lifes > 1)
                {
                    // Game over
                }

                // for debug
                state = gameState.OBJECT_APPEARS;

                break;

            case gameState.PLAYER_COLLECTED:

                // If player collected score a point and spawn new item

                // for debug
                state = gameState.OBJECT_APPEARS;

                break;

        }


    }


    void moveDragon(Vector3 pos)
    {



        //Quaternion targetRotation = spawnedDragon.transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(pos);
        targetRotation *= Quaternion.Euler(0, 90, 0);

        //if (oldPos.x > pos.x) // going left
        //{
        //    Debug.Log("New Pos");
        //    Debug.Log(pos.x + "," + pos.y + "," + pos.z);
        //    Debug.Log("Old Pos");
        //    Debug.Log(oldPos.x + "," + oldPos.y + "," + oldPos.z);
        //    Debug.Log("Going Left");
        //    targetRotation = Quaternion.LookRotation(pos);
        //    targetRotation *= Quaternion.Euler(0, -90, 0);
        //} 
        //else if (oldPos.x < pos.x) // going right
        //{
        //    Debug.Log("New Pos");
        //    Debug.Log(pos.x + "," + pos.y + "," + pos.z);
        //    Debug.Log("Old Pos");
        //    Debug.Log(oldPos.x + "," + oldPos.y + "," + oldPos.z);
        //    Debug.Log("Going Right");
        //    targetRotation = Quaternion.LookRotation(pos);
        //    targetRotation *= Quaternion.Euler(0, 90, 0);
        //}

        spawnedDragon.transform.position = Vector3.MoveTowards(spawnedDragon.transform.position, pos, speedDragon * Time.deltaTime);
        spawnedDragon.transform.rotation = Quaternion.Slerp(spawnedDragon.transform.rotation, targetRotation, 2f * Time.deltaTime);

        oldPos = pos;

    }

    GameObject randomlySpawnItem(GameObject item)
    {
        float posx = Random.Range(outlinePoints[0].x, outlinePoints[1].x);
        float posy = Random.Range(outlinePoints[0].y, outlinePoints[2].y);
        float posz = outlinePoints[0].z;

        Vector3 pos = new Vector3(posx, posy, posz);

        return item = Instantiate(item, pos, Quaternion.LookRotation(Vector3.up)); // seems good
    }

    bool TryTouchItem() // Not work yet?
    {
        if (Input.touchCount > 0)
        {
            Touch theTouch = Input.GetTouch(0);

            if (theTouch.phase == TouchPhase.Began)
            {
                Debug.Log("Got the touch");

                Ray ray = _arCamera.ScreenPointToRay(theTouch.position);
                
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
}
