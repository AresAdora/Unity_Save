using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public List<ParticleSystem> fireParticle = new List<ParticleSystem>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            FireParticleOn();
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            FireParticleOff();
        }
    }

    //��ƼŬ On
    public void FireParticleOn()
    {
        for(int i=0;i<fireParticle.Count;i++)
        {
            fireParticle[i].Play();
        }
    }
    //��ƼŬ Off
    public void FireParticleOff()
    {
        //for(int i=0;i<fireParticle.Count;i++)
        //{
            //fireParticle[i].Stop();
        //}

        for(int i=0;i<fireParticle.Count;i++)
        {
            ParticleSystem.MainModule fireMain = fireParticle[i].main;
            fireMain.loop = false;
            //���� ȣ��
            StartCoroutine(ParticleLoofOff(i,fireMain.duration));
        }
    }

    IEnumerator ParticleLoofOff(int i,float t)
    {
        yield return new WaitForSeconds(t);
        fireParticle[i].Stop();
    }
}
