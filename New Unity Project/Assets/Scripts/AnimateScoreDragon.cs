using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateScoreDragon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Animation dragonAnimation = GetComponent<Animation>();
        dragonAnimation.wrapMode = WrapMode.Loop;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
