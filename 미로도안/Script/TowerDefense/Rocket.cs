using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    //���� �̵� �ӵ�
    public float rocketSpeed;

    //���� ��ȸ �ӵ�
    public float rocketRotateSpeed;
    
    //Ÿ��
    public Transform target;

    //���ݷ�
    public float atkPoint;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("RocketDestroy");
    }

    // Update is called once per frame
    void Update()
    {
        //������ �̵�
        transform.Translate(Vector3.forward*rocketSpeed*Time.deltaTime);

        if(target)
        {
            //������ Ÿ���� ���� �ٶ�
            Vector3 relationPos = new Vector3(target.position.x,target.GetComponent<CapsuleCollider>().height/2,target.position.z) - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relationPos);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rocketRotateSpeed * Time.deltaTime);
        }
    }

    IEnumerator RocketDestroy()
    {
        yield return new WaitForSeconds(2.0f);
        //�����ı�
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //�浹ü�� Tag�� Enemy���
        if (other.transform.CompareTag("Enemy"))
        {
            Debug.Log("Enemy");
            //������ ���ݴ������ �ο���
            other.GetComponent<Enemy>().EnemyHPDown(atkPoint);

            //���� �Ҹ�
            Destroy(gameObject);
        }

        //������ �ٴڿ� �ε�������
        if(other.transform.name=="Ground")
        {
            //���� �Ҹ�
            Destroy(gameObject);
        }
    }

    //Ÿ�� ����
    public void TargetUpdate(Transform t)
    {
        target = t;
    }
}
