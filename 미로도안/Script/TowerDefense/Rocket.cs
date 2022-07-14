using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    //로켓 이동 속도
    public float rocketSpeed;

    //로켓 선회 속도
    public float rocketRotateSpeed;
    
    //타겟
    public Transform target;

    //공격력
    public float atkPoint;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("RocketDestroy");
    }

    // Update is called once per frame
    void Update()
    {
        //로켓의 이동
        transform.Translate(Vector3.forward*rocketSpeed*Time.deltaTime);

        if(target)
        {
            //로켓이 타겟을 향해 바라봄
            Vector3 relationPos = new Vector3(target.position.x,target.GetComponent<CapsuleCollider>().height/2,target.position.z) - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relationPos);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rocketRotateSpeed * Time.deltaTime);
        }
    }

    IEnumerator RocketDestroy()
    {
        yield return new WaitForSeconds(2.0f);
        //로켓파괴
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //충돌체의 Tag가 Enemy라면
        if (other.transform.CompareTag("Enemy"))
        {
            Debug.Log("Enemy");
            //적에게 공격대미지를 부여함
            other.GetComponent<Enemy>().EnemyHPDown(atkPoint);

            //로켓 소멸
            Destroy(gameObject);
        }

        //로켓이 바닥에 부딪쳤을때
        if(other.transform.name=="Ground")
        {
            //로켓 소멸
            Destroy(gameObject);
        }
    }

    //타겟 갱신
    public void TargetUpdate(Transform t)
    {
        target = t;
    }
}
