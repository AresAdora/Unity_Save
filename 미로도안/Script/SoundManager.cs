using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource gunSound;

    public List<AudioSource> effectSound = new List<AudioSource>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            gunSound.Play() ;
        }
    }

    public void EffectSoundPlay(int i)
    {
        effectSound[i].Play();
    }
}
