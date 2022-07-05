using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public AnimationClip MonsterAttack;

    public enum CharacterState
    {
        Idle, Detact, Attack
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

    NavMeshAgent m_enemy = null;

    [SerializeField] Transform[] m_tfWayPoints = null;
    int m_count = 0;

    Transform m_target = null;

    private void OnDrawGizmos()
    {
        //와이어 스피어를 그린다(추적경로랑 비슷함,(시작 위치, 크기))
        Gizmos.DrawWireSphere(transform.position, searchRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void SetTarget(Transform p_target)
    {
        CancelInvoke();
        m_target = p_target;
    }

    public void RemoveTarget()
    {
        m_target = null;
        InvokeRepeating("MoveToNextWayPoint", 0f, 2f);
    }

    void MoveToNextWayPoint()
    {
        if(m_target==null)
        {
            if (m_enemy.velocity == Vector3.zero)
            {
                m_enemy.SetDestination(m_tfWayPoints[m_count++].position);

                if (m_count >= m_tfWayPoints.Length)
                {
                    m_count = 0;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_enemy = GetComponent<NavMeshAgent>();
        InvokeRepeating("MoveToNextWayPoint", 0f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if(m_target!=null)
        {
            m_enemy.SetDestination(m_target.position);
        }
    }
}
