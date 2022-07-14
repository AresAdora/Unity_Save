using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSilhouete : MonoBehaviour
{
    //타워 메쉬렌더러
    public List<MeshRenderer> towerMesh = new List<MeshRenderer>();

    //타워 색깔
    public List<Color> towerColor = new List<Color>();

    //타워 머터리얼
    public Material towerMat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //타워 실루엣의 상태
    public void TowerUpdate(int i)
    {
        switch(i)
        {
            //타워를 숨김
            case 0:
                {
                    for(int j=0;j<towerMesh.Count;j++)
                    {
                        towerMesh[j].enabled = false;
                    }
                    break;
                }
            //타워를 설치할 수 있는 상태 
            case 1:
                {
                    for(int j=0;j<towerMesh.Count;j++)
                    {
                        towerMesh[j].enabled = true;
                    }

                    //베이스컬러 변경
                    towerMat.SetColor("_BaseColor", towerColor[0]);

                    //이미션 컬러
                    towerMat.SetColor("_EmissionColor", towerColor[0] * 3.0f);
                    break;
                }
            //타워를 설치할 수 없는 상태 
            case 2:
                {
                    for (int j = 0; j < towerMesh.Count; j++)
                    {
                        towerMesh[j].enabled = true;
                    }

                    //베이스컬러 변경
                    towerMat.SetColor("_BaseColor", towerColor[1]);

                    //이미션 컬러
                    towerMat.SetColor("_EmissionColor", towerColor[1] * 3.0f);
                    break;
                }
        }
    }    
}
