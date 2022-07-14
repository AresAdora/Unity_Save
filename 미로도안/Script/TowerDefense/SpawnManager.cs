using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //소환상태
    public enum SpawnState
    {
        None, Spawn
    }

    public SpawnState spawnState = SpawnState.None;

    //소환 포인트
    public Transform spawnPoint;

    //소환할 적의 종류
    public int spawnNum;

    //적 프리팹
    public List<GameObject> enemyPrefab = new List<GameObject>();

    //소환시간
    private float spawnTime;
    //설정 소환시간
    public float setSpawnTime;

    //소환된 적
    public List<GameObject> spawnedEnemy = new List<GameObject>();

    //이동 포인트
    public List<Transform> movePoint = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        Invoke("SpawnStart", 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        switch(spawnState)
        {
            case SpawnState.Spawn:
                {
                    spawnTime += Time.deltaTime;
                    if(spawnTime>=setSpawnTime)
                    {
                        //적 생성
                        CreateEnemy(spawnNum);
                        //소환 시간 초기화
                        spawnTime = 0;
                    }
                    break;
                }
        }
    }
    //소환 시작
    void SpawnStart()
    {
        //적 생성
        CreateEnemy(spawnNum);

        //소환 상태를 Spawn으로 변경
        spawnState = SpawnState.Spawn;
    }

    //적 소환
    public void CreateEnemy(int i)
    {
        //적 생성(원본, 위치, 회전)
        GameObject enemy = Instantiate(enemyPrefab[i], spawnPoint.position,spawnPoint.rotation);

        for(int j=0;j<movePoint.Count; j++)
        {
            //적에게 이동경로를 알려준다.
            enemy.GetComponent<Enemy>().movePoint.Add(movePoint[j]);
        }

        //생성된 적을 담아둔다.
        spawnedEnemy.Add(enemy);
    }

}
