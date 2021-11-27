using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TapPlaneToStart : MonoBehaviour
{
    public GameObject Child;
    private ARPlane CurrentPlane;

    //private GameObject spawnedObject;
    private ARRaycastManager _arRaycastManager;
    private ARPlaneManager _arPlaneManager;
    //private Vector2 touchPosition;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();


    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
        _arPlaneManager = GetComponent<ARPlaneManager>();


    }

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


    // Start is called before the first frame update
    private void Start()
    {
        Child = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {



    }

}
