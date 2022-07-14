using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSilhouete : MonoBehaviour
{
    //Ÿ�� �޽�������
    public List<MeshRenderer> towerMesh = new List<MeshRenderer>();

    //Ÿ�� ����
    public List<Color> towerColor = new List<Color>();

    //Ÿ�� ���͸���
    public Material towerMat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Ÿ�� �Ƿ翧�� ����
    public void TowerUpdate(int i)
    {
        switch(i)
        {
            //Ÿ���� ����
            case 0:
                {
                    for(int j=0;j<towerMesh.Count;j++)
                    {
                        towerMesh[j].enabled = false;
                    }
                    break;
                }
            //Ÿ���� ��ġ�� �� �ִ� ���� 
            case 1:
                {
                    for(int j=0;j<towerMesh.Count;j++)
                    {
                        towerMesh[j].enabled = true;
                    }

                    //���̽��÷� ����
                    towerMat.SetColor("_BaseColor", towerColor[0]);

                    //�̹̼� �÷�
                    towerMat.SetColor("_EmissionColor", towerColor[0] * 3.0f);
                    break;
                }
            //Ÿ���� ��ġ�� �� ���� ���� 
            case 2:
                {
                    for (int j = 0; j < towerMesh.Count; j++)
                    {
                        towerMesh[j].enabled = true;
                    }

                    //���̽��÷� ����
                    towerMat.SetColor("_BaseColor", towerColor[1]);

                    //�̹̼� �÷�
                    towerMat.SetColor("_EmissionColor", towerColor[1] * 3.0f);
                    break;
                }
        }
    }    
}
