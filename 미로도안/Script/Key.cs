using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip keySound;

    // Start is called before the first frame update
    void Start()
    {
        this.audio = this.gameObject.AddComponent<AudioSource>();
        this.audio.clip = this.keySound;
        this.audio.loop = false;
    }

    // Update is called once per frame
    void Update()
    {
        this.audio.Play();
    }
}
