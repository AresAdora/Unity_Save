using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyDirector : MonoBehaviour
{

    public AnimationClip MonsterAttack;

    public enum CharacterState
    {
        Patrol, Idle, Detact, Attack
    }

    public CharacterState characterState = CharacterState.Idle;

    //�ִϸ�����
    public Animator monsterAnim;

    //Ÿ��
    public Transform target;
    //�����Ÿ�
    public float searchRange;
    //�̵��ӵ�
    public float moveSpeed;
    //���ݰŸ�
    public float attackRange;

    public float dist;
    public float patrolDist;

    private void OnDrawGizmos()
    {
        //���̾� ���Ǿ �׸���(������ζ� �����,(���� ��ġ, ũ��))
        Gizmos.DrawWireSphere(transform.position, searchRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    NavMeshAgent m_enemy = null;

    [SerializeField] Transform[] m_tfWayPoints = null;
    int m_count = 0;

    int randomCount;
    void MoveToNextWayPoint()
    {
        if(m_enemy.velocity==Vector3.zero)
        {
            m_enemy.SetDestination(m_tfWayPoints[m_count++].position);
            if (m_count >= m_tfWayPoints.Length)
                m_count = 0;
        }
    }

    void NextWayPointMove(int i)
    {
        m_enemy.SetDestination(m_tfWayPoints[i].position);
    }


    // Start is called before the first frame update
    void Start()
    {
        m_enemy = GetComponent<NavMeshAgent>();
        //InvokeRepeating("MoveToNextWayPoint", 0f, 2f);

        randomCount = Random.Range(0, m_tfWayPoints.Length);

        //�޸��� ��� ���
        monsterAnim.SetInteger("MonsterState", 2);
        NextWayPointMove(randomCount);
        characterState = CharacterState.Patrol;
    }

    // Update is called once per frame
    void Update()
    {
        //if (m_enemy.velocity == Vector3.zero)
        //{
        //    monsterAnim.SetInteger("MonsterState", 0);
        //}
        //else
        //{
        //    switch (characterState)
        //    {
        //        case CharacterState.Idle:
        //            {
        //                //���Ϳ� �÷��̾� ������ �Ÿ��� ����
        //                float dist = Vector3.Distance(transform.position, target.position);

        //                //�÷��̾ ���������� ��������
        //                if (dist <= searchRange)
        //                {
        //                    //�޸��� ��� ���
        //                    monsterAnim.SetInteger("MonsterState", 2);
        //                    characterState = CharacterState.Detact;
        //                }
        //                else
        //                {
        //                    monsterAnim.SetInteger("MonsterState", 2);
        //                }
        //                break;
        //            }
        //        case CharacterState.Detact:
        //            {
        //                monsterAnim.SetInteger("MonsterState", 2);
        //                //���Ͱ� �÷��̾ �ٶ󺻴�.
        //                Vector3 relationPos = new Vector3(target.position.x, 0, target.position.z)
        //                    - new Vector3(transform.position.x, 0, transform.position.z);
        //                Quaternion rotation = Quaternion.LookRotation(relationPos);
        //                transform.rotation = rotation;

        //                //���Ͱ� �÷��̾ �߰��Ѵ�.
        //                transform.position =
        //                    Vector3.MoveTowards(transform.position,
        //                    new Vector3(target.position.x, transform.position.y, target.position.z),
        //                    moveSpeed * Time.deltaTime);

        //                //���Ϳ� �÷��̾� ������ �Ÿ��� ����
        //                float dist = Vector3.Distance(transform.position, target.position);

        //                //Ÿ���� ���ݰ��� �Ÿ��� ������
        //                if (dist <= attackRange)
        //                {
        //                    //���� ����
        //                    monsterAnim.SetInteger("MonsterState", 1);

        //                    CancelInvoke("MoveToNextWayPoint");

        //                    //���� ���¸� �������� ����
        //                    characterState = CharacterState.Attack;
        //                }

        //                //Ÿ���� Ž�������� �������
        //                if (dist > searchRange)
        //                {
        //                    monsterAnim.SetInteger("MonsterState", 2);
        //                    //ĳ���� ���¸� ��� ���·� ����
        //                    characterState = CharacterState.Idle;
        //                }
        //                break;
        //            }
        //        case CharacterState.Attack:
        //            {
        //                ////���Ϳ� �÷��̾� ������ �Ÿ��� ����
        //                //dist = Vector3.Distance(transform.position, target.position);
        //                //if (dist > attackRange)
        //                //{
        //                //    characterState = CharacterState.Detact;
        //                //}
        //                break;
        //            }
        //    }
        //}

        switch (characterState)
        {
            case CharacterState.Patrol:
                {
                    patrolDist = Vector3.Distance(transform.position, m_tfWayPoints[randomCount].position);
                    dist = Vector3.Distance(transform.position, target.position);
                    //�÷��̾ ���������� ��������
                    if (dist <= searchRange)
                    {
                        //�޸��� ��� ���
                        monsterAnim.SetInteger("MonsterState", 2);
                        characterState = CharacterState.Detact;
                    }
                    else
                    {
                        if (patrolDist <= 0.1f)
                        {
                            transform.position = m_tfWayPoints[randomCount].position;

                            int oldR = randomCount;
                            while (randomCount == oldR)
                            {
                                randomCount = Random.Range(0, m_tfWayPoints.Length);

                                NextWayPointMove(randomCount);
                            }
                        }
                    }
                    break;
                }
            case CharacterState.Idle:
                {
                    //���Ϳ� �÷��̾� ������ �Ÿ��� ����
                    float dist = Vector3.Distance(transform.position, target.position);

                    //�÷��̾ ���������� ��������
                    if (dist <= searchRange)
                    {
                        //�޸��� ��� ���
                        monsterAnim.SetInteger("MonsterState", 2);
                        characterState = CharacterState.Detact;
                    }
                    else
                    {
                        monsterAnim.SetInteger("MonsterState", 2);
                    }
                    break;
                }
            case CharacterState.Detact:
                {
                    monsterAnim.SetInteger("MonsterState", 2);
                    //���Ͱ� �÷��̾ �ٶ󺻴�.
                    Vector3 relationPos = new Vector3(target.position.x, 0, target.position.z)
                        - new Vector3(transform.position.x, 0, transform.position.z);
                    Quaternion rotation = Quaternion.LookRotation(relationPos);
                    transform.rotation = rotation;

                    //���Ͱ� �÷��̾ �߰��Ѵ�.
                    transform.position =
                        Vector3.MoveTowards(transform.position,
                        new Vector3(target.position.x, transform.position.y, target.position.z),
                        moveSpeed * Time.deltaTime);

                    //���Ϳ� �÷��̾� ������ �Ÿ��� ����
                    float dist = Vector3.Distance(transform.position, target.position);

                    //Ÿ���� ���ݰ��� �Ÿ��� ������
                    if (dist <= attackRange)
                    {
                        //���� ����
                        monsterAnim.SetInteger("MonsterState", 1);

                        //CancelInvoke("MoveToNextWayPoint");

                        m_enemy.SetDestination(target.position - (transform.forward * attackRange * 0.75f));
                       //���� ���¸� �������� ����
                       characterState = CharacterState.Attack;
                    }

                    //Ÿ���� Ž�������� �������
                    if (dist > searchRange)
                    {
                        monsterAnim.SetInteger("MonsterState", 2);
                        //ĳ���� ���¸� ��� ���·� ����
                        characterState = CharacterState.Patrol;
                    }
                    break;
                }
            case CharacterState.Attack:
                {
                    //���Ϳ� �÷��̾� ������ �Ÿ��� ����
                    dist = Vector3.Distance(transform.position, target.position);
                    if (dist > attackRange)
                    {
                        characterState = CharacterState.Detact;
                    }
                    break;
                }
        }
    }
}
