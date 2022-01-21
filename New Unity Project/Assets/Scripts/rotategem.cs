using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotategem : MonoBehaviour
{

    GameObject gemToRotate;


    // Start is called before the first frame update
    void Start()
    {

        //Debug.Log("gem should be rotated");
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(Vector3.up * (180f * Time.deltaTime), Space.Self);
    }
}
