using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
      
    //������ ����
    public enum LaserState
    {
        On, Off
    }

    public LaserState laserState = LaserState.On;

    //������ ��Ƽ����
    public Material laserMat;

    //������ �÷�
    public List<Color> laserColor = new List<Color>();

    //�浹 ������Ʈ
    public GameObject hitObject;

    //������ �׸�
    public GameObject laserGrip;

    //�浹�� ��ġ
    public Vector3 hitPoint;

    //������ ����
    public LineRenderer laserLine;

    // Start is called before the first frame update
    void Start()
    {
        //������ �� ����
        laserMat.SetColor("_BaseColor", laserColor[0]);

        //������ �̹̼��÷� �� ����
        laserMat.SetColor("_EmissionColor", laserColor[0] * 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        switch(laserState)
        {
            case LaserState.On:
            {
                //����ĳ��Ʈ
                RaycastHit hit;
                //����ĳ��Ʈ�� ��ü�� �浹������ ( ���� ��ġ, �߻� ����, �浹 )
                if(Physics.Raycast(transform.position, transform.forward, out hit))
                {

                    //������ �浹ü ���
                    hitObject = hit.transform.gameObject;

                    //�浹�� ��ġ�� ��ȯ
                    hitPoint = hit.point;

                    //���� �������� ������ġ ����
                    laserLine.SetPosition(0, transform.position);

                    //���� �������� �� ��ġ ����
                    laserLine.SetPosition(1, hitPoint);

                }

                //��ü�� �浹���� �ʾ�����
                else
                {
                        //���� ��ü�� ������
                        if(!laserGrip)
                        {
                            //������ �浹ü ����
                            hitObject = null;

                            //���η������� ������ġ ����
                            laserLine.SetPosition(0, transform.position);

                            //���η������� ������ġ ����
                            laserLine.SetPosition(1, transform.forward * 100.0f);
                        }
                }
                break;
            }
        }

        if(Input.GetKeyDown(KeyCode.V))
        {
            //������ ���̽��� ����
            laserMat.SetColor("_BaseColor",laserColor[1]);

            //������ �̹̼��÷� �� ����
            laserMat.SetColor("_EmissionColor", laserColor[1]*2.0f);

            if(hitObject.GetComponent<Rigidbody>())
            {
                //�浹ü�� ������ٵ� ������ �ִٸ�
                hitObject.GetComponent<Rigidbody>().isKinematic = true;
            }

            //�浹ü �θ� �������� ���
            hitObject.transform.parent = transform;

            //���� ��ü�� �浹ü�� ���
            laserGrip = hitObject;
        }
        if (Input.GetKeyUp(KeyCode.V))
        {
            //������ ���̽��� ����
            laserMat.SetColor("_BaseColor", laserColor[0]);

            //������ �̹̼��÷� �� ����
            laserMat.SetColor("_EmissionColor", laserColor[0] * 2.0f);

            if (hitObject.GetComponent<Rigidbody>())
            {
                //�浹ü�� ������ٵ� ������ �ִٸ�
                hitObject.GetComponent<Rigidbody>().isKinematic = true;
            }

            //�浹ü �θ� ����
            hitObject.transform.parent = null;

            //���� ��ü ����
            laserGrip = null;
        }
    }

    public void LaserGripUpdate(bool b)
    {
        switch(b)
        {
            case true:
                //������ ���̽��� ����
                laserMat.SetColor("_BaseColor", laserColor[1]);

                //������ �̹̼��÷� �� ����
                laserMat.SetColor("_EmissionColor", laserColor[1] * 2.0f);

                if (hitObject.GetComponent<Rigidbody>())
                {
                    //�浹ü�� ������ٵ� ������ �ִٸ�
                    hitObject.GetComponent<Rigidbody>().isKinematic = true;
                }

                //�浹ü �θ� �������� ���
                hitObject.transform.parent = transform;

                //���� ��ü�� �浹ü�� ���
                laserGrip = hitObject;

                break;

            case false:
                //������ ���̽��� ����
                laserMat.SetColor("_BaseColor", laserColor[0]);

                //������ �̹̼��÷� �� ����
                laserMat.SetColor("_EmissionColor", laserColor[0] * 2.0f);

                if (hitObject.GetComponent<Rigidbody>())
                {
                    //�浹ü�� ������ٵ� ������ �ִٸ�
                    hitObject.GetComponent<Rigidbody>().isKinematic = true;
                }

                //�浹ü �θ� ����
                hitObject.transform.parent = null;

                //���� ��ü ����
                laserGrip = null;

                break;
        }
    }
}
