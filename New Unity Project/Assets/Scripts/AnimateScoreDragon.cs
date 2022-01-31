using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateScoreDragon : MonoBehaviour
{
    [SerializeField]
    AudioManager myAudio;
    Animation dragonAnimation;


    // Start is called before the first frame update
    void Start()
    {
        myAudio.playDragonRoar();
        dragonAnimation = GetComponent<Animation>();
        dragonAnimation.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
