using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;



[RequireComponent(typeof(ARRaycastManager))]
//[RequireComponent(typeof(ARPlaneManager))]

public class GameLoop : MonoBehaviour
{

    [SerializeField]
    private GameObject dragon;

    private GameObject spawnedDragon;

    private ARRaycastManager _arRaycastManager;
    //private ARPlaneManager _arPlaneManager;
    private List<Vector3> outlinePoints = new List<Vector3>();

    private int corner = 0;
    private Vector3 initPosition;



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Now Starting");

        SetPlaneOutline planeOutline = GameObject.FindObjectOfType<SetPlaneOutline>();
        outlinePoints = planeOutline.GetOutlinePoints();

        // calculate random starting positio. not perfect if plane is non rectangular....

        float posx = Random.Range(outlinePoints[0].x, outlinePoints[1].x);
        float posy = Random.Range(outlinePoints[0].y, outlinePoints[2].y);
        float posz = outlinePoints[0].z;

        Vector3 pos = new Vector3(posy, posy, posz);

        // better raycasting against the plane??

        spawnedDragon = Instantiate(dragon, pos , dragon.transform.rotation);
        initPosition = spawnedDragon.transform.position;
        spawnedDragon.transform.Rotate(0, 180, 0);
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

        // Move the dragon around the set plane
        //dragonMover.MoveTo(outlinePoints[0]);


        switch (corner)
        {
            case 0:

                spawnedDragon.transform.position = Vector3.MoveTowards(spawnedDragon.transform.position, outlinePoints[0], 0.5f * Time.deltaTime); // works
                if (spawnedDragon.transform.position == outlinePoints[0])
                {
                    corner = 1;
                }   
                break;
            case 1:
                spawnedDragon.transform.position = Vector3.MoveTowards(spawnedDragon.transform.position, outlinePoints[1], 0.5f * Time.deltaTime); // works
                if (spawnedDragon.transform.position == outlinePoints[1])
                {
                    corner = 2;
                }
                break;
            case 2:
                spawnedDragon.transform.position = Vector3.MoveTowards(spawnedDragon.transform.position, outlinePoints[2], 0.5f * Time.deltaTime); // works
                if (spawnedDragon.transform.position == outlinePoints[2])
                {
                    corner = 3;
                }
                break;
            case 3:
                spawnedDragon.transform.position = Vector3.MoveTowards(spawnedDragon.transform.position, outlinePoints[3], 0.5f * Time.deltaTime); // works
                if (spawnedDragon.transform.position == outlinePoints[3])
                {

                    corner = 0;
                }
                break;

        }




    }

    //void moveDragon(Vector2 pos)
    //{
    //    // Check limits
    //    if (pos.x >= outlinePoints[0].x && pos.y >= outlinePoints[2].y && pos.x <= outlinePoints[1].x && pos.y <= outlinePoints[1].y) 
    //    {
    //        spawnedDragon.transform.position = new Vector2 (transform.position.x + )
    //    }
    //}
}
