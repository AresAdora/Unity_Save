using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BezierLaser : MonoBehaviour
{
    //������ ����
    public enum BezierLaserState
    {
        On, Off
    }
    public BezierLaserState bezierLaserState = BezierLaserState.Off;

    //��� ������
    public Transform point1;

    //��� ����
    public Transform point2;

    //��� ����
    public Transform point3;

    public int vertexCount = 12;

    //���η�����
    public LineRenderer lineRenderer;

    //����ĳ��Ʈ �浹 ����
    private GameObject hitPos;

    //�ִ� �Ÿ�
    public float maxRange;

    //������ġ�� �� ��ġ�� �Ÿ���
    public float dist;

    //Ÿ�� ��ȣ
    public int towerNum = -1;

    //�Ǽ��� Ÿ���� �Ƿ翧
    public List<GameObject> towerSilhouetePrefab = new List<GameObject>();

    //Ÿ�� �Ƿ翧
    public GameObject towerSilhouete;

    //�Ǽ��� Ÿ�� ������
    public List<GameObject> towerPrefab = new List<GameObject>();

    //��ġ ������ ����
    public GameObject towerBuildLane;


    private void OnDrawGizmos()
    {
        //point1���� point2�� ���ϴ� ������ �׷��ش�.
        Gizmos.color = Color.green;
        Gizmos.DrawLine(point1.position, point2.position);

        //point2���� point3�� ���ϴ� ������ �׷��ش�.
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
        //point2�� ��ġ
        dist = Vector3.Distance(transform.position, point3.position);
        point2.localPosition = new Vector3(0, dist / 2, dist / 2);

        //point3�� ��ġ
        RaycastHit hit;
        if(Physics.Raycast(transform.position,transform.forward,out hit))
        {
            point3.position = hit.point;

            if (hit.transform.CompareTag("TowerLane"))
            {
                //Ÿ���� ��ġ�� �� �ִ� ��
                towerSilhouete.GetComponent<TowerSilhouete>().TowerUpdate(1);
                towerSilhouete.transform.position = hit.transform.position;
            }
            else if (hit.transform.CompareTag("EnemyLane"))
            {
                //Ÿ���� ��ġ�� �� ���� ��
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
                //Ÿ���� ��ġ�� �� �ִ� ��
                if (hit.transform.CompareTag("TowerLane"))
                {
                    //Ÿ�� �Ƿ翧�� ���� ��
                    if(towerSilhouete)
                    {
                        //�������� �浹�� ������ ����
                        switch(hit.transform.GetComponent<TowerLane>().towerLaneState)
                        {
                            //�������� �浹�� ������ ���°� �Ǽ� �غ� ������ ��
                            case TowerLane.TowerLaneState.BuildReady:
                                {
                                    //Ÿ�� �Ƿ翧�� �ʷϻ�(�Ǽ�����) ���·� ����
                                    towerSilhouete.GetComponent<TowerSilhouete>().TowerUpdate(1);
                                    //Ÿ�� �Ƿ翧�� ��ġ�a �������� ������ Ÿ���� ��ġ�� ����
                                    towerSilhouete.transform.position = hit.transform.position;
                                    //������ Ÿ�� ������ towerBuildLane�� ����ش�.
                                    towerBuildLane = hit.transform.gameObject;
                                    break;
                                }
                            case TowerLane.TowerLaneState.BuildOn:
                                {
                                    //Ÿ�� �Ƿ翧�� ������(�Ǽ� �Ұ���) ���·� ����
                                    towerSilhouete.GetComponent<TowerSilhouete>().TowerUpdate(2);
                                    //Ÿ�� �Ƿ翧�� ��ġ�a �������� ������ Ÿ���� ��ġ�� ����
                                    towerSilhouete.transform.position = hit.transform.position;
                                    //�Ǽ� ������ Ÿ�� �ʱ�ȭ
                                    towerBuildLane = null;
                                    break;
                                }
                        }
                    }
                }
                //Ÿ���� ��ġ�� �� ���� ��
                else if (hit.transform.CompareTag("EnemyLane"))
                {
                    //Ÿ�� �Ƿ翧�� ���� ��
                    if(towerSilhouete)
                    {                        
                        towerSilhouete.GetComponent<TowerSilhouete>().TowerUpdate(2);
                        towerSilhouete.transform.position = hit.transform.position;
                        //�Ǽ� ������ Ÿ�� �ʱ�ȭ
                        towerBuildLane = null;
                    }                  
                }
                else
                {
                    towerSilhouete.GetComponent<TowerSilhouete>().TowerUpdate(0);
                    //�Ǽ� ������ Ÿ�� �ʱ�ȭ
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

    //������ ������ �ѱ�
    public void BezierLaserOn(int i)
    {
        //���η����� �ѱ�
        lineRenderer.enabled = true;
        //������ ������ ���¸� On
        bezierLaserState = BezierLaserState.On;
        //������ ��ȣ�� �Է� ����
        towerNum = i;
        //Ÿ�� �Ƿ翧 ��ȣ
        TowerSilhoueteOn(i);
    }

    //Ÿ�� �Ƿ翧 ����
    public void TowerSilhoueteOn(int i)
    {
        //������ �Ƿ翧�� ������
        if(!towerSilhouete)
        {
            // i��° Ÿ�� �Ƿ翧 ����
            GameObject towerSilhroueteClone = Instantiate(towerSilhouetePrefab[i]);
            // Ÿ���� �Ⱥ��̰� ����
            towerSilhroueteClone.GetComponent<TowerSilhouete>().TowerUpdate(0);
            // ������ Ÿ���� �̸��� i��° Ÿ���� �̸����� ����
            towerSilhroueteClone.transform.name = towerSilhouetePrefab[i].transform.name;
            // ������ Ÿ���� ����Ѵ�.
            towerSilhouete = towerSilhroueteClone;
        }
    }

    // Ÿ�� �Ǽ�
    public void TowerBuildOn(Vector3 pos)
    {
        //Ÿ�� ����
        GameObject tower = Instantiate(towerPrefab[towerNum], pos, Quaternion.identity);
        // ������ �������� �浹���� ���ο� Ÿ���� �Ǽ��Ǿ����� �˸�
        towerBuildLane.GetComponent<TowerLane>().TowerBuildOn();
        //������ ��ȣ �ʱ�ȭ
        towerNum = -1;
        //Ÿ�� �Ƿ翧 ����
        Destroy(towerSilhouete);
        //Ÿ�� �Ƿ翧 �ʱ�ȭ
        towerSilhouete = null;
        //������ ������ Off
        bezierLaserState = BezierLaserState.Off;
        //���� ������ Off
        lineRenderer.enabled = false;
        // ������ Ÿ�� ���� �ʱ�ȭ
        towerBuildLane = null;
        //������ ������ �ʱ�ȭ
        BezierLaserReset();
    }

    void BezierLaserReset()
    {
        point1.position = transform.position;
        point2.position = transform.position;
        point3.position = transform.position;
    }
}
