using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    public enum PlayerHPState
    {
        None, HPDown
    }
    public PlayerHPState playerHPState = PlayerHPState.None;
    public float hp;
    private float maxHP;
    private float targetHP;

    public Slider hpSlider;
    public float hpSpeed;

    public AudioClip damage;
    AudioSource audioSource;

    private void Awake()
    {
        this.audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        targetHP = hp;
        maxHP = hp;
    }

    // Update is called once per frame
    void Update()
    {
        switch(playerHPState)
        {
            case PlayerHPState.HPDown:
                {
                    hp = Mathf.MoveTowards(hp, targetHP, hpSpeed * Time.deltaTime);
                    float hpPer = hp / maxHP;
                    hpSlider.value = hpPer;
                    if (hp == targetHP)
                    {
                        hpPer = hp / maxHP;
                        hpSlider.value = hpPer;
                        playerHPState = PlayerHPState.None;
                    }
                    break;
                }
        }
    }

    void PlaySound(string action)
    {
        switch (action)
        {
            case "DAMAGE":
                audioSource.clip = damage;
                break;
        }

        audioSource.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
       if(other.transform.name =="Enemy")
        {
            targetHP -= 1.0f;
            playerHPState = PlayerHPState.HPDown;

            PlaySound("DAMAGE");
        }
    }
}
