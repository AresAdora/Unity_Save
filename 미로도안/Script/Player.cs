using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    public float speed;

    int count = 0;

    public Text countText;

    public AudioClip item;
    AudioSource audioSource; 
    
    private void Awake()
    {
        this.audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        countText.text = count.ToString();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

        rb.AddForce(movement * speed);
    }

    void PlaySound(string action)
    {
        switch (action)
        {
            case "ITEM":
                audioSource.clip = item;
                break;
        }

        audioSource.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Key"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            countText.text = count.ToString();

            PlaySound("ITEM");
        }
    }
}