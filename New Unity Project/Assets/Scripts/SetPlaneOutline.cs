using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;


[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]


public class SetPlaneOutline : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI showCorner;

    [SerializeField]
    private GameObject indicateCorner;
    [SerializeField]
    private GameObject gameLoopObject;

    private GameObject[] spawnedObjects = new GameObject[4];
    private int i = 0; //count

    private ARRaycastManager _arRaycastManager;
    private ARPlaneManager _arPlaneManager;
    private Vector2 touchPosition;

    public List<Vector3> planeOutlinePoints = new List<Vector3>();

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private bool debugForFinish = false;

    private enum gameCorner
    {
        UpperLeft,
        UpperRight,
        LowerRight,
        LowerLeft,
        AllDone
    } // 0 = upper left, 1 = upper right, 2 = lower right, 3 = lower left
    gameCorner setCorner = gameCorner.UpperLeft;

    public List<Vector3> GetOutlinePoints()
    {
        return planeOutlinePoints;
    }
    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
        _arPlaneManager = GetComponent<ARPlaneManager>();      

    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            Touch theTouch = Input.GetTouch(0);

            if (theTouch.phase == TouchPhase.Ended) // seems to work
            {
                touchPosition = theTouch.position;
                return true;
            }

        }

        touchPosition = default;
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log("SetPlaneOutline: " + showCorner.text);

        // "State machine" on what to show

        switch (setCorner)
        {
            case gameCorner.UpperLeft:
                showCorner.SetText( "Set the upper left Corner"); 
                break;

            case gameCorner.UpperRight:
                showCorner.SetText("Set the upper right Corner");

                break;

            case gameCorner.LowerRight:
                showCorner.SetText("Set the lower right Corner");

                break;

            case gameCorner.LowerLeft:
                showCorner.SetText("Set the lower left Corner");

                break;

            case gameCorner.AllDone:

                //_arPlaneManager.planePrefab = null; // could also work for all future planes...
                _arPlaneManager.planePrefab.SetActive(false);
                _arPlaneManager.SetTrackablesActive(false);
                showCorner.gameObject.SetActive(false);

                // Hide planes again and move on ?
                //foreach (var plane in _arPlaneManager.trackables)
                //{
                //    plane.gameObject.SetActive(false); // not quite right because it stops tracking it? Only disable the visu of the plane? make it transparent?

                //    // Möglicherweise muss plane visualizer auch diabled werden


                //}

                debugForFinish = true;

                foreach (var _object in spawnedObjects)
                {
                    _object.gameObject.SetActive(false);
                }

                // Enable gameobject or script of gameobject for game loop
                gameLoopObject.SetActive(true); // does not start the script object??

                break;
        }



        // "State machine" on input
        if (!debugForFinish)
        {
            if (!TryGetTouchPosition(out Vector2 touchPosition))
            {
                return;
            }

            if (_arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.All)) 
            {
                var hitPos = hits[0].pose;

                GameObject spawnedObject = Instantiate(indicateCorner, hitPos.position, hitPos.rotation);
                spawnedObjects[i] = spawnedObject;
                

                switch (setCorner)
                {
                    case gameCorner.UpperLeft:
                        planeOutlinePoints.Add(hitPos.position);
                        setCorner = gameCorner.UpperRight;
                        i++;
                        break;

                    case gameCorner.UpperRight:
                        planeOutlinePoints.Add(hitPos.position);
                        setCorner = gameCorner.LowerRight;
                        i++;
                        break;

                    case gameCorner.LowerRight:
                        planeOutlinePoints.Add(hitPos.position);
                        setCorner = gameCorner.LowerLeft;
                        i++;
                        break;

                    case gameCorner.LowerLeft:
                        planeOutlinePoints.Add(hitPos.position);
                        setCorner = gameCorner.AllDone;
                        break;

                    case gameCorner.AllDone:

                        // nothing i guess

                        break;



                }


            }
        }

    }


    //IEnumerator WaitSeconds(int Seconds)
    //{
    //    Debug.Log("SetPlaneOutline: " + Time.time);
    //    yield return new WaitForSeconds(Seconds);
    //    Debug.Log("SetPlaneOutline: " + Time.time);
    //}

}


