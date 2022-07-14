using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //��ȯ����
    public enum SpawnState
    {
        None, Spawn
    }

    public SpawnState spawnState = SpawnState.None;

    //��ȯ ����Ʈ
    public Transform spawnPoint;

    //��ȯ�� ���� ����
    public int spawnNum;

    //�� ������
    public List<GameObject> enemyPrefab = new List<GameObject>();

    //��ȯ�ð�
    private float spawnTime;
    //���� ��ȯ�ð�
    public float setSpawnTime;

    //��ȯ�� ��
    public List<GameObject> spawnedEnemy = new List<GameObject>();

    //�̵� ����Ʈ
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
                        //�� ����
                        CreateEnemy(spawnNum);
                        //��ȯ �ð� �ʱ�ȭ
                        spawnTime = 0;
                    }
                    break;
                }
        }
    }
    //��ȯ ����
    void SpawnStart()
    {
        //�� ����
        CreateEnemy(spawnNum);

        //��ȯ ���¸� Spawn���� ����
        spawnState = SpawnState.Spawn;
    }

    //�� ��ȯ
    public void CreateEnemy(int i)
    {
        //�� ����(����, ��ġ, ȸ��)
        GameObject enemy = Instantiate(enemyPrefab[i], spawnPoint.position,spawnPoint.rotation);

        for(int j=0;j<movePoint.Count; j++)
        {
            //������ �̵���θ� �˷��ش�.
            enemy.GetComponent<Enemy>().movePoint.Add(movePoint[j]);
        }

        //������ ���� ��Ƶд�.
        spawnedEnemy.Add(enemy);
    }

}
