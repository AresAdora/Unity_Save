using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gourd : MonoBehaviour
{
    //박 에니메이터
    public Animator gourdAnim;

    //충돌횟수
    public int crashCount;

    //충돌이 시작될때 한번만 호출
    private void OnCollisionEnter(Collision collision)
    {
        //충돌한 물체의 Tag의 Beanbag이라면
        if(collision.gameObject.CompareTag("Beanbag"))
        {
            //충돌횟수를 1씩 증가
            crashCount++;

            if(crashCount>=10)
            {
                //박 열기 애니메이션 재생
                gourdAnim.SetBool("Open", true);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
