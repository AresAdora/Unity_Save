using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gourd : MonoBehaviour
{
    //�� ���ϸ�����
    public Animator gourdAnim;

    //�浹Ƚ��
    public int crashCount;

    //�浹�� ���۵ɶ� �ѹ��� ȣ��
    private void OnCollisionEnter(Collision collision)
    {
        //�浹�� ��ü�� Tag�� Beanbag�̶��
        if(collision.gameObject.CompareTag("Beanbag"))
        {
            //�浹Ƚ���� 1�� ����
            crashCount++;

            if(crashCount>=10)
            {
                //�� ���� �ִϸ��̼� ���
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
