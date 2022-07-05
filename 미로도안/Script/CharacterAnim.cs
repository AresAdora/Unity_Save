using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnim : MonoBehaviour
{
    public AnimationClip MonsterAttack;

    public enum CharacterState
    {
        Idle,Detact,Attack
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

    private void OnDrawGizmos()
    {
        //와이어 스피어를 그린다(추적경로랑 비슷함,(시작 위치, 크기))
        Gizmos.DrawWireSphere(transform.position, searchRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            monsterAnim.SetInteger("MonsterState",1);
            StartCoroutine("MonsterAttackStop");
        }
        switch(characterState)
        {
            case CharacterState.Idle:
                {
                    //몬스터와 플레이어 사이의 거리를 측정
                    float dist = Vector3.Distance(transform.position, target.position);

                    //플레이어가 감지범위에 들어왔을때
                    if(dist<=searchRange)
                    {
                        //달리기 모션 재생
                        monsterAnim.SetInteger("MonsterState",2);
                        characterState = CharacterState.Detact;
                    }
                    break;
                }
            case CharacterState.Detact:
                {
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
                    if(dist<=attackRange)
                    {
                        //몬스터 공격
                        monsterAnim.SetInteger("MonsterState",1);

                        //몬스터 상태를 공격으로 변경
                        characterState = CharacterState.Attack;
                    }

                    //타겟이 탐지범위를 벗어났을때
                    if(dist > searchRange)
                    {
                        //몬스터 동작을 대기 상태로 변경
                        monsterAnim.SetInteger("MonsterState",0);
                        //캐릭터 상태를 대기 상태로 변경
                        characterState = CharacterState.Idle;
                    }
                    break;
                }
            case CharacterState.Attack:
                {
                    //몬스터와 플레이어 사이의 거리를 측정
                    float dist = Vector3.Distance(transform.position, target.position);
                    if(dist>attackRange)
                    {
                        //몬스터 동작을 이동으로 변경
                        monsterAnim.SetInteger("MonsterState",2);
                        characterState = CharacterState.Detact;
                    }
                    break;
                }
        }

        IEnumerator OrcAttackStop()
        {
            //지연 호출(공격 애니메이션 길이(시간))
            yield return new WaitForSeconds(MonsterAttack.length);

            //몬스터 애니메이션을 Idle(0)으로 변경한다.
            monsterAnim.SetInteger("MonsterState",0);
        }
    }
}
