using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


[RequireComponent(typeof(ARRaycastManager))]
public class TapToPlace : MonoBehaviour
{

    public GameObject coinsToSpawn;

    private GameObject spawnedObject;
    private ARRaycastManager _arRaycastManager;
    private Vector2 touchPosition;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();


    //private void Awake()
    //{
    //    _arRaycastManager = GetComponent<ARRaycastManager>();


    //}

    //bool TryGetTouchPosition(out Vector2 touchPosition)
    //{
    //    if (Input.touchCount > 0)
    //    {
    //        touchPosition = Input.GetTouch(0).position;
    //        return true;
    //    }

    //    touchPosition = default;
    //    return false;
    //}


    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //    if(!TryGetTouchPosition(out Vector2 touchPosition))
    //    {
    //        return;
    //    }

    //    if(_arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.All)) // Maybe all is expensive?? Also instantiates on horizontal plane??
    //    {
    //        var hitPos = hits[0].pose;

    //        spawnedObject = Instantiate(coinsToSpawn, hitPos.position, hitPos.rotation);


    //    }

    //}
}
