using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    //���� ����
    public enum EnemyState
    {
        None,Move,Finish,KO
    }
    public EnemyState enemyState = EnemyState.Move;

    //���� HP
    public float enemyHP;

    //���� ��ü HP
    private float maxHP;

    //HP�� �����
    private float hpPer;

    //���� ��Ʈ
    public List<Transform> movePoint = new List<Transform>();
    //���� ��Ʈ ��ȣ
    public int moveNum;

    //���� �̵� �ӵ�
    public float enemySpeed;

    //���� �ִϸ�����
    public Animator enemyAnim;

    //����ɴ� �ð�
    private float koTime;
    //������ ����ɴ� �ð�
    private float setKoTime = 1.5f;
    //������� ����
    float koPos;

    //�� UI
    public Transform enemyUI;
    //HP�����̴�
    public Slider hpSlider;

    // Start is called before the first frame update
    void Start()
    {
        //��ü HP ����
        maxHP = enemyHP;

        //HP �����ȭ
        hpPer = enemyHP / maxHP;

        //�޸��� ����
        EnemyAnimUpdate(1);

        //������� ���� ���
        koPos = transform.position.y - 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 uiPos = new Vector3(Camera.main.transform.position.x,0,Camera.main.transform.position.z)
            - new Vector3(enemyUI.position.x,0,enemyUI.position.z);
        Quaternion uiRot = Quaternion.LookRotation(uiPos);
        enemyUI.rotation = uiRot;

        switch(enemyState)
        {
            case EnemyState.Move:
                {
                    //�̵� ��ΰ� �ϳ��� ���� ��
                    if(movePoint.Count>0)
                    {
                        //Ÿ���� ���� �̵�
                        transform.position = Vector3.MoveTowards(transform.position,
                            new Vector3(movePoint[moveNum].position.x, transform.position.y, movePoint[moveNum].position.z),
                            enemySpeed * Time.deltaTime);

                        Vector3 relationPos = new Vector3(movePoint[moveNum].position.x, 0, movePoint[moveNum].position.z)
                         - new Vector3(transform.position.x, 0, transform.position.z);
                        Quaternion rotation = Quaternion.LookRotation(relationPos);
                        transform.rotation = rotation;

                        //Ÿ�ٰ��� �Ÿ�
                        float dist = Vector3.Distance(transform.position, new Vector3(movePoint[moveNum].position.x, transform.position.y, movePoint[moveNum].position.z));
                        //Ÿ�ٰ��� �Ÿ��� 0.01 ���� �϶�
                        if (dist <= 0.01f)
                        {
                            //enemy�� ��ġ�� Ÿ���� ��ġ�� ����ġ
                            transform.position = new Vector3(movePoint[moveNum].position.x, transform.position.y, movePoint[moveNum].position.z);

                            if (moveNum < movePoint.Count - 1)
                            {
                                //���� Ÿ�� ��ǥ ����
                                moveNum++;
                            }
                        }
                    }
                    break;
                }
            case EnemyState.KO:
                {
                    koTime += Time.deltaTime;
                    if(koTime>=setKoTime)
                    {
                        //���� �ٴ����� �������
                        transform.position = Vector3.MoveTowards(transform.position, 
                            new Vector3(transform.position.x, koPos, transform.position.z),Time.deltaTime);

                        //���� ���̰� ������� ���̸�ŭ ���� �Ͽ�����
                        if(transform.position.y==koPos)
                        {
                            //�� ����
                            Destroy(gameObject);
                        }
                    }
                    break;
                }
        }
    }

    //HP����
    public void EnemyHPDown(float atk)
    {
        //HP����
        enemyHP -= atk;

        //HP ����� ���
        hpPer = enemyHP / maxHP;

        //HP�����̴��� HP�ܿ��� ǥ��
        hpSlider.value = hpPer;

        //HP����
        if (enemyHP <= 0)
        {
            //HP�����̴� ����
            hpSlider.gameObject.SetActive(false);
            //ĸ�� �ݶ��̴� off
            this.GetComponent<CapsuleCollider>().enabled = false;
            //�� ���¸� KO�� ����
            enemyState = EnemyState.KO;
            //KO �ִϸ��̼� ����
            EnemyAnimUpdate(2);
        }
    }

    //�ִϸ��̼� ����
    void EnemyAnimUpdate(int i)
    {
        enemyAnim.SetInteger("EnemyState", i);
    }    
}
