using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
      
    //레이저 상태
    public enum LaserState
    {
        On, Off
    }

    public LaserState laserState = LaserState.On;

    //레이저 머티리얼
    public Material laserMat;

    //레이저 컬러
    public List<Color> laserColor = new List<Color>();

    //충돌 오브젝트
    public GameObject hitObject;

    //레이저 그립
    public GameObject laserGrip;

    //충돌한 위치
    public Vector3 hitPoint;

    //레이저 라인
    public LineRenderer laserLine;

    // Start is called before the first frame update
    void Start()
    {
        //레이저 색 변경
        laserMat.SetColor("_BaseColor", laserColor[0]);

        //레이저 이미션컬러 색 변경
        laserMat.SetColor("_EmissionColor", laserColor[0] * 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        switch(laserState)
        {
            case LaserState.On:
            {
                //레이캐스트
                RaycastHit hit;
                //레이캐스트가 물체에 충돌했을때 ( 시작 위치, 발사 방향, 충돌 )
                if(Physics.Raycast(transform.position, transform.forward, out hit))
                {

                    //레이저 충돌체 등록
                    hitObject = hit.transform.gameObject;

                    //충돌한 위치를 반환
                    hitPoint = hit.point;

                    //라인 렌더러의 시작위치 지정
                    laserLine.SetPosition(0, transform.position);

                    //라인 렌더러의 끝 위치 지정
                    laserLine.SetPosition(1, hitPoint);

                }

                //물체가 충돌하지 않았을때
                else
                {
                        //잡은 물체가 없을때
                        if(!laserGrip)
                        {
                            //레이저 충돌체 해제
                            hitObject = null;

                            //라인렌더러의 시작위치 지정
                            laserLine.SetPosition(0, transform.position);

                            //라인렌더러의 종료위치 지정
                            laserLine.SetPosition(1, transform.forward * 100.0f);
                        }
                }
                break;
            }
        }

        if(Input.GetKeyDown(KeyCode.V))
        {
            //레이저 베이스색 변경
            laserMat.SetColor("_BaseColor",laserColor[1]);

            //레이저 이미션컬러 색 변경
            laserMat.SetColor("_EmissionColor", laserColor[1]*2.0f);

            if(hitObject.GetComponent<Rigidbody>())
            {
                //충돌체가 리지드바디를 가지고 있다면
                hitObject.GetComponent<Rigidbody>().isKinematic = true;
            }

            //충돌체 부모를 레이저로 등록
            hitObject.transform.parent = transform;

            //잡은 물체에 충돌체를 등록
            laserGrip = hitObject;
        }
        if (Input.GetKeyUp(KeyCode.V))
        {
            //레이저 베이스색 변경
            laserMat.SetColor("_BaseColor", laserColor[0]);

            //레이저 이미션컬러 색 변경
            laserMat.SetColor("_EmissionColor", laserColor[0] * 2.0f);

            if (hitObject.GetComponent<Rigidbody>())
            {
                //충돌체가 리지드바디를 가지고 있다면
                hitObject.GetComponent<Rigidbody>().isKinematic = true;
            }

            //충돌체 부모를 해제
            hitObject.transform.parent = null;

            //잡은 물체 해제
            laserGrip = null;
        }
    }

    public void LaserGripUpdate(bool b)
    {
        switch(b)
        {
            case true:
                //레이저 베이스색 변경
                laserMat.SetColor("_BaseColor", laserColor[1]);

                //레이저 이미션컬러 색 변경
                laserMat.SetColor("_EmissionColor", laserColor[1] * 2.0f);

                if (hitObject.GetComponent<Rigidbody>())
                {
                    //충돌체가 리지드바디를 가지고 있다면
                    hitObject.GetComponent<Rigidbody>().isKinematic = true;
                }

                //충돌체 부모를 레이저로 등록
                hitObject.transform.parent = transform;

                //잡은 물체에 충돌체를 등록
                laserGrip = hitObject;

                break;

            case false:
                //레이저 베이스색 변경
                laserMat.SetColor("_BaseColor", laserColor[0]);

                //레이저 이미션컬러 색 변경
                laserMat.SetColor("_EmissionColor", laserColor[0] * 2.0f);

                if (hitObject.GetComponent<Rigidbody>())
                {
                    //충돌체가 리지드바디를 가지고 있다면
                    hitObject.GetComponent<Rigidbody>().isKinematic = true;
                }

                //충돌체 부모를 해제
                hitObject.transform.parent = null;

                //잡은 물체 해제
                laserGrip = null;

                break;
        }
    }
}
