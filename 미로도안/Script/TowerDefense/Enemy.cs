using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    //적의 상태
    public enum EnemyState
    {
        None,Move,Finish,KO
    }
    public EnemyState enemyState = EnemyState.Move;

    //적의 HP
    public float enemyHP;

    //적의 전체 HP
    private float maxHP;

    //HP의 백분율
    private float hpPer;

    //진행 루트
    public List<Transform> movePoint = new List<Transform>();
    //진행 루트 번호
    public int moveNum;

    //적의 이동 속도
    public float enemySpeed;

    //적의 애니메이터
    public Animator enemyAnim;

    //가라앉는 시간
    private float koTime;
    //설정할 가라앉는 시간
    private float setKoTime = 1.5f;
    //가라앉을 높이
    float koPos;

    //적 UI
    public Transform enemyUI;
    //HP슬라이더
    public Slider hpSlider;

    // Start is called before the first frame update
    void Start()
    {
        //전체 HP 설정
        maxHP = enemyHP;

        //HP 백분율화
        hpPer = enemyHP / maxHP;

        //달리기 시작
        EnemyAnimUpdate(1);

        //가라앉을 높이 계산
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
                    //이동 경로가 하나라도 있을 때
                    if(movePoint.Count>0)
                    {
                        //타겟을 향해 이동
                        transform.position = Vector3.MoveTowards(transform.position,
                            new Vector3(movePoint[moveNum].position.x, transform.position.y, movePoint[moveNum].position.z),
                            enemySpeed * Time.deltaTime);

                        Vector3 relationPos = new Vector3(movePoint[moveNum].position.x, 0, movePoint[moveNum].position.z)
                         - new Vector3(transform.position.x, 0, transform.position.z);
                        Quaternion rotation = Quaternion.LookRotation(relationPos);
                        transform.rotation = rotation;

                        //타겟과의 거리
                        float dist = Vector3.Distance(transform.position, new Vector3(movePoint[moveNum].position.x, transform.position.y, movePoint[moveNum].position.z));
                        //타겟과의 거리가 0.01 이하 일때
                        if (dist <= 0.01f)
                        {
                            //enemy의 위치를 타겟의 위치로 재위치
                            transform.position = new Vector3(movePoint[moveNum].position.x, transform.position.y, movePoint[moveNum].position.z);

                            if (moveNum < movePoint.Count - 1)
                            {
                                //다음 타겟 목표 지정
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
                        //적이 바닥으로 가라앉음
                        transform.position = Vector3.MoveTowards(transform.position, 
                            new Vector3(transform.position.x, koPos, transform.position.z),Time.deltaTime);

                        //적의 높이가 가라앉을 높이만큼 도달 하였을때
                        if(transform.position.y==koPos)
                        {
                            //적 삭제
                            Destroy(gameObject);
                        }
                    }
                    break;
                }
        }
    }

    //HP감소
    public void EnemyHPDown(float atk)
    {
        //HP감소
        enemyHP -= atk;

        //HP 백분율 계산
        hpPer = enemyHP / maxHP;

        //HP슬라이더에 HP잔여량 표시
        hpSlider.value = hpPer;

        //HP소진
        if (enemyHP <= 0)
        {
            //HP슬라이더 숨김
            hpSlider.gameObject.SetActive(false);
            //캡슐 콜라이더 off
            this.GetComponent<CapsuleCollider>().enabled = false;
            //적 상태를 KO로 변경
            enemyState = EnemyState.KO;
            //KO 애니메이션 실행
            EnemyAnimUpdate(2);
        }
    }

    //애니메이션 변경
    void EnemyAnimUpdate(int i)
    {
        enemyAnim.SetInteger("EnemyState", i);
    }    
}
