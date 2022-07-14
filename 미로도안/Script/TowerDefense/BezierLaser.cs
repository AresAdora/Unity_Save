using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BezierLaser : MonoBehaviour
{
    //레이저 상태
    public enum BezierLaserState
    {
        On, Off
    }
    public BezierLaserState bezierLaserState = BezierLaserState.Off;

    //곡선의 시작점
    public Transform point1;

    //곡선의 정점
    public Transform point2;

    //곡선의 끝점
    public Transform point3;

    public int vertexCount = 12;

    //라인렌더러
    public LineRenderer lineRenderer;

    //레이캐스트 충돌 지점
    private GameObject hitPos;

    //최대 거리
    public float maxRange;

    //시작위치와 끝 위치의 거리차
    public float dist;

    //타워 번호
    public int towerNum = -1;

    //건설할 타워의 실루엣
    public List<GameObject> towerSilhouetePrefab = new List<GameObject>();

    //타워 실루엣
    public GameObject towerSilhouete;

    //건설할 타워 프리팹
    public List<GameObject> towerPrefab = new List<GameObject>();

    //설치 가능한 레인
    public GameObject towerBuildLane;


    private void OnDrawGizmos()
    {
        //point1에서 point2로 향하는 라인을 그려준다.
        Gizmos.color = Color.green;
        Gizmos.DrawLine(point1.position, point2.position);

        //point2에서 point3로 향하는 라인을 그려준다.
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(point2.position, point3.position);

        Gizmos.color = Color.red;
        for(float ratio=0.5f/vertexCount; ratio<1; ratio+=1.0f/vertexCount)
        {
            Gizmos.DrawLine(Vector3.Lerp(point1.position,point2.position,ratio),
                Vector3.Lerp(point2.position,point3.position,ratio));
        }
    }

    internal void TowerBuildOn(BezierLaser bezierLaserR, BezierLaser bezierLaserR1)
    {
        throw new NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        hitPos = new GameObject();
        hitPos.transform.position = new Vector3(transform.position.x, 0, maxRange);
        hitPos.transform.parent = transform;
        hitPos.transform.name = "HitPos";
    }

    // Update is called once per frame
    void Update()
    {
        //point2의 위치
        dist = Vector3.Distance(transform.position, point3.position);
        point2.localPosition = new Vector3(0, dist / 2, dist / 2);

        //point3의 위치
        RaycastHit hit;
        if(Physics.Raycast(transform.position,transform.forward,out hit))
        {
            point3.position = hit.point;

            if (hit.transform.CompareTag("TowerLane"))
            {
                //타워를 설치할 수 있는 곳
                towerSilhouete.GetComponent<TowerSilhouete>().TowerUpdate(1);
                towerSilhouete.transform.position = hit.transform.position;
            }
            else if (hit.transform.CompareTag("EnemyLane"))
            {
                //타워를 설치할 수 없는 곳
                towerSilhouete.GetComponent<TowerSilhouete>().TowerUpdate(2);
                towerSilhouete.transform.position = hit.transform.position;
            }
            else
            {
                towerSilhouete.GetComponent<TowerSilhouete>().TowerUpdate(0);
            }
        }
        else
        {
            RaycastHit hit2;
            if(Physics.Raycast(hitPos.transform.position,Vector3.down,out hit2))
            {
                point3.position = hit2.point;
                //타워를 설치할 수 있는 곳
                if (hit.transform.CompareTag("TowerLane"))
                {
                    //타워 실루엣이 있을 때
                    if(towerSilhouete)
                    {
                        //레이저에 충돌된 레인의 상태
                        switch(hit.transform.GetComponent<TowerLane>().towerLaneState)
                        {
                            //레이저에 충돌된 레인의 상태가 건설 준비 상태일 때
                            case TowerLane.TowerLaneState.BuildReady:
                                {
                                    //타워 실루엣을 초록색(건설가능) 상태로 변경
                                    towerSilhouete.GetComponent<TowerSilhouete>().TowerUpdate(1);
                                    //타워 실루엣의 위치픞 레이저가 감지한 타일의 위치로 변경
                                    towerSilhouete.transform.position = hit.transform.position;
                                    //감지한 타워 레인을 towerBuildLane에 담아준다.
                                    towerBuildLane = hit.transform.gameObject;
                                    break;
                                }
                            case TowerLane.TowerLaneState.BuildOn:
                                {
                                    //타워 실루엣을 빨간색(건설 불가능) 상태로 변경
                                    towerSilhouete.GetComponent<TowerSilhouete>().TowerUpdate(2);
                                    //타워 실루엣의 위치픞 레이저가 감지한 타일의 위치로 변경
                                    towerSilhouete.transform.position = hit.transform.position;
                                    //건설 가능한 타워 초기화
                                    towerBuildLane = null;
                                    break;
                                }
                        }
                    }
                }
                //타워를 설치할 수 없는 곳
                else if (hit.transform.CompareTag("EnemyLane"))
                {
                    //타워 실루엣이 있을 때
                    if(towerSilhouete)
                    {                        
                        towerSilhouete.GetComponent<TowerSilhouete>().TowerUpdate(2);
                        towerSilhouete.transform.position = hit.transform.position;
                        //건설 가능한 타워 초기화
                        towerBuildLane = null;
                    }                  
                }
                else
                {
                    towerSilhouete.GetComponent<TowerSilhouete>().TowerUpdate(0);
                    //건설 가능한 타워 초기화
                    towerBuildLane = null;
                }
            }
        }


        var pointList = new List<Vector3>();
        for(float ratio=0;ratio<=1;ratio+=1.0f/vertexCount)
        {
            var tangentLineVertex1 = Vector3.Lerp(point1.position, point2.position, ratio);
            var tangentLineVectex2 = Vector3.Lerp(point2.position, point3.position, ratio);
            var bezierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVectex2, ratio);
            pointList.Add(bezierPoint);
        }
        lineRenderer.positionCount = pointList.Count;
        lineRenderer.SetPositions(pointList.ToArray());
    }

    //베지어 레이저 켜기
    public void BezierLaserOn(int i)
    {
        //라인렌더러 켜기
        lineRenderer.enabled = true;
        //베지어 레이저 상태를 On
        bezierLaserState = BezierLaserState.On;
        //아이템 번호를 입력 받음
        towerNum = i;
        //타워 실루엣 번호
        TowerSilhoueteOn(i);
    }

    //타워 실루엣 생성
    public void TowerSilhoueteOn(int i)
    {
        //생성된 실루엣이 없을때
        if(!towerSilhouete)
        {
            // i번째 타워 실루엣 생성
            GameObject towerSilhroueteClone = Instantiate(towerSilhouetePrefab[i]);
            // 타워를 안보이게 숨김
            towerSilhroueteClone.GetComponent<TowerSilhouete>().TowerUpdate(0);
            // 생성된 타워의 이름을 i번째 타워의 이름으로 변경
            towerSilhroueteClone.transform.name = towerSilhouetePrefab[i].transform.name;
            // 생성된 타워를 등록한다.
            towerSilhouete = towerSilhroueteClone;
        }
    }

    // 타워 건설
    public void TowerBuildOn(Vector3 pos)
    {
        //타워 생성
        GameObject tower = Instantiate(towerPrefab[towerNum], pos, Quaternion.identity);
        // 베지어 레이저가 충돌중인 레인에 타워가 건설되었음을 알림
        towerBuildLane.GetComponent<TowerLane>().TowerBuildOn();
        //아이템 번호 초기화
        towerNum = -1;
        //타워 실루엣 삭제
        Destroy(towerSilhouete);
        //타워 실루엣 초기화
        towerSilhouete = null;
        //베지어 레이저 Off
        bezierLaserState = BezierLaserState.Off;
        //라인 렌더러 Off
        lineRenderer.enabled = false;
        // 감지된 타워 레인 초기화
        towerBuildLane = null;
        //베지어 레이저 초기화
        BezierLaserReset();
    }

    void BezierLaserReset()
    {
        point1.position = transform.position;
        point2.position = transform.position;
        point3.position = transform.position;
    }
}
