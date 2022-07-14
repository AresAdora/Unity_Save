using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    //타워 상태
    public enum TowerState
    {
        Idle, Attack
    }

    public TowerState towerState = TowerState.Idle;

    //타워의 적 감지 범위
    public float searchRange;

    //감지된 적
    public GameObject targetEnemy;

    //감지된 적의 거리
    public float targetDist;

    //최소 감지 거리
    public float distMin;

    //타워 헤드
    public Transform towerHead;

    //타워 헤드의 회전 속도
    public float towerHeadSpeed;

    //로켓 발사 위치
    public List<Transform> launchPos = new List<Transform>();

    //로켓 프리팹
    public GameObject rocketPrefab;

    //발사 지연 시간
    private float rocketCoolTime;
    //설정할 발사 지연 시간
    public float setRocketCoolTime;

    //와이어 스피어 그리기
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, searchRange);
    }

    // Start is called before the first frame update
    void Start()
    {
        //최소값을 감지범위로 초기화
        distMin = searchRange;
    }

    // Update is called once per frame
    void Update()
    {
        int layerMask = 1 << 6;
        //SphereCastAll(위치, 반지름, 방향)
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, searchRange, Vector3.up,Mathf.Infinity, layerMask);

        //foreach(var hit in hits)
        //{
        // Debug.Log(hit.transform.name);
        //}

        //타워의 상태

        switch (towerState)
        {
            //대기 상태
            case TowerState.Idle:
                {
                    //적이 최소 하나라도 감지된 상태
                    if (hits.Length > 0)
                    {
                        foreach (var hit in hits)
                        {
                            //감지된 물체가 "Enemy"태그를 가지고 있다면
                            if (hit.transform.CompareTag("Enemy"))
                            {
                                //감지된 적 등록
                                targetEnemy = hit.transform.gameObject;

                                //로켓 생성
                                CreateRocket(targetEnemy.transform);

                                //타워의 상태를 공격 상태로 변경
                                towerState = TowerState.Attack;
                            }
                        }
                    }
                    else
                    {
                        //타워헤드의 회전을 0으로 변경
                        Vector3 relationPos = Vector3.zero - new Vector3(towerHead.transform.rotation.eulerAngles.x, 0, 
                            towerHead.transform.rotation.eulerAngles.z);
                        Quaternion rotation = Quaternion.LookRotation(relationPos);
                        towerHead.rotation = Quaternion.RotateTowards(towerHead.rotation, rotation, towerHeadSpeed * Time.deltaTime);
                    }
                    break;
                }
            //공격 상태
            case TowerState.Attack:
                {
                    //최소 1개 이상의 적이 감지되었을 때
                    if (hits.Length > 0)
                    {
                        //타겟팅 된 적이 있다면
                        if (targetEnemy)
                        {
                            //로켓 쿨타임 시간 시작
                            rocketCoolTime += Time.deltaTime;
                            //로켓 쿨타임이 설정한 시간을 지나면
                            if(rocketCoolTime>=setRocketCoolTime)
                            {
                                //로켓 생성
                                CreateRocket(targetEnemy.transform);
                                //로켓 쿨타임 초기화
                                rocketCoolTime = 0;
                            }


                            //타겟팅한 적을 타워가 바라봄
                            Vector3 relationPos = new Vector3(targetEnemy.transform.position.x, 0, targetEnemy.transform.position.z)
                            - new Vector3(transform.position.x, 0, transform.position.z);
                            Quaternion rotation = Quaternion.LookRotation(relationPos);

                            //타워헤드의 회전 Quaternion.RotateTowards(A의 회전각에서 B의 회전각으로 C의 일정한 속도로 회전한다)
                            towerHead.rotation = Quaternion.RotateTowards(towerHead.rotation, rotation, towerHeadSpeed * Time.deltaTime);

                            //타겟팅한 적의 거리
                            targetDist = Vector3.Distance(transform.position, targetEnemy.transform.position);

                            //타겟팅한 적의 거리가 감지거리보다 클 때 -> 타겟팅한 적이 감지 범위를 벗어났을 때
                            if (targetDist > searchRange)
                            {
                                //최소거리를 감지거리로 초기화
                                distMin = searchRange;
                                foreach (var hit in hits)
                                {
                                    //감지된 적의 거리를 계산함
                                    float enemyDist = Vector3.Distance(transform.position, hit.transform.position);

                                    //감지된 적의 거리가 최소값보다 작을 때
                                    if (enemyDist <= distMin)
                                    {
                                        //최소거리가 갱신됨
                                        distMin = enemyDist;

                                        //적을 리타겟팅함
                                        targetEnemy = hit.transform.gameObject;
                                    }
                                }
                            }
                        }
                        //타겟팅된 적이 없을 때
                        else
                        {
                            //최소거리 초기화
                            distMin = searchRange;

                            foreach (var hit in hits)
                            {
                                //감지된 물체의 Tag가 Enemy라면
                                if (hit.transform.CompareTag("Enemy"))
                                {
                                    //감지된 적의 거리를 계산
                                    float enemyDist = Vector3.Distance(transform.position, hit.transform.position);

                                    //감지된 적의 거리가 최소거리보다 작거나 같아질 때
                                    if (enemyDist <= distMin)
                                    {
                                        //최소 거리를 갱신
                                        distMin = enemyDist;

                                        //타겟팅한 적을 등록
                                        targetEnemy = hit.transform.gameObject;
                                    }
                                }
                            }
                        }
                    }
                    //감지된 적이 하나도 없을 때
                    else
                    {
                        //로켓 발사 지연 시간 초기화
                        rocketCoolTime = 0;

                        //타겟팅한 적을 초기화
                        targetEnemy = null;

                        //타워 상태를 대기 상태로 변경
                        towerState = TowerState.Idle;
                    }
                        break;
                }
        }
    }
    //로켓생성
    public void CreateRocket(Transform t)
    {
        //로켓을 생성
        GameObject rocket = Instantiate(rocketPrefab, launchPos[0].position, launchPos[0].rotation);

        //로켓에 타겟을 지정해줌
        rocket.SendMessage("TargetUpdate", t);
    }
}