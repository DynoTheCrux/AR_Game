using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField]
    private AudioSource dragonRoar = new AudioSource();

    [SerializeField]
    private AudioSource coinsClink = new AudioSource();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playDragonRoar()
    {
        dragonRoar.PlayOneShot(dragonRoar.clip);
    }

    public void playCoinsClink()
    {
        coinsClink.PlayOneShot(coinsClink.clip);
    }
}
