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

    //애니메이터
    public Animator monsterAnim;

    //타겟
    public Transform target;
    //감지거리
    public float searchRange;
    //이동속도
    public float moveSpeed;
    //공격거리
    public float attackRange;

    public float dist;
    public float patrolDist;

    private void OnDrawGizmos()
    {
        //와이어 스피어를 그린다(추적경로랑 비슷함,(시작 위치, 크기))
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

        //달리기 모션 재생
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
        //                //몬스터와 플레이어 사이의 거리를 측정
        //                float dist = Vector3.Distance(transform.position, target.position);

        //                //플레이어가 감지범위에 들어왔을때
        //                if (dist <= searchRange)
        //                {
        //                    //달리기 모션 재생
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
        //                //몬스터가 플레이어를 바라본다.
        //                Vector3 relationPos = new Vector3(target.position.x, 0, target.position.z)
        //                    - new Vector3(transform.position.x, 0, transform.position.z);
        //                Quaternion rotation = Quaternion.LookRotation(relationPos);
        //                transform.rotation = rotation;

        //                //몬스터가 플레이어를 추격한다.
        //                transform.position =
        //                    Vector3.MoveTowards(transform.position,
        //                    new Vector3(target.position.x, transform.position.y, target.position.z),
        //                    moveSpeed * Time.deltaTime);

        //                //몬스터와 플레이어 사이의 거리를 측정
        //                float dist = Vector3.Distance(transform.position, target.position);

        //                //타겟이 공격가능 거리에 있을때
        //                if (dist <= attackRange)
        //                {
        //                    //몬스터 공격
        //                    monsterAnim.SetInteger("MonsterState", 1);

        //                    CancelInvoke("MoveToNextWayPoint");

        //                    //몬스터 상태를 공격으로 변경
        //                    characterState = CharacterState.Attack;
        //                }

        //                //타겟이 탐지범위를 벗어났을때
        //                if (dist > searchRange)
        //                {
        //                    monsterAnim.SetInteger("MonsterState", 2);
        //                    //캐릭터 상태를 대기 상태로 변경
        //                    characterState = CharacterState.Idle;
        //                }
        //                break;
        //            }
        //        case CharacterState.Attack:
        //            {
        //                ////몬스터와 플레이어 사이의 거리를 측정
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
                    //플레이어가 감지범위에 들어왔을때
                    if (dist <= searchRange)
                    {
                        //달리기 모션 재생
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
                    //몬스터와 플레이어 사이의 거리를 측정
                    float dist = Vector3.Distance(transform.position, target.position);

                    //플레이어가 감지범위에 들어왔을때
                    if (dist <= searchRange)
                    {
                        //달리기 모션 재생
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
                    //몬스터가 플레이어를 바라본다.
                    Vector3 relationPos = new Vector3(target.position.x, 0, target.position.z)
                        - new Vector3(transform.position.x, 0, transform.position.z);
                    Quaternion rotation = Quaternion.LookRotation(relationPos);
                    transform.rotation = rotation;

                    //몬스터가 플레이어를 추격한다.
                    transform.position =
                        Vector3.MoveTowards(transform.position,
                        new Vector3(target.position.x, transform.position.y, target.position.z),
                        moveSpeed * Time.deltaTime);

                    //몬스터와 플레이어 사이의 거리를 측정
                    float dist = Vector3.Distance(transform.position, target.position);

                    //타겟이 공격가능 거리에 있을때
                    if (dist <= attackRange)
                    {
                        //몬스터 공격
                        monsterAnim.SetInteger("MonsterState", 1);

                        //CancelInvoke("MoveToNextWayPoint");

                        m_enemy.SetDestination(target.position - (transform.forward * attackRange * 0.75f));
                       //몬스터 상태를 공격으로 변경
                       characterState = CharacterState.Attack;
                    }

                    //타겟이 탐지범위를 벗어났을때
                    if (dist > searchRange)
                    {
                        monsterAnim.SetInteger("MonsterState", 2);
                        //캐릭터 상태를 대기 상태로 변경
                        characterState = CharacterState.Patrol;
                    }
                    break;
                }
            case CharacterState.Attack:
                {
                    //몬스터와 플레이어 사이의 거리를 측정
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
