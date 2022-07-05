using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnim : MonoBehaviour
{
    public AnimationClip MonsterAttack;

    public enum CharacterState
    {
        Idle,Detact,Attack
    }

    public CharacterState characterState = CharacterState.Idle;

    //�ִϸ�����
    public Animator monsterAnim;

    //Ÿ��
    public Transform target;
    //�����Ÿ�
    public float searchRange;
    //�̵��ӵ�
    public float moveSpeed;
    //���ݰŸ�
    public float attackRange;

    private void OnDrawGizmos()
    {
        //���̾� ���Ǿ �׸���(������ζ� �����,(���� ��ġ, ũ��))
        Gizmos.DrawWireSphere(transform.position, searchRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            monsterAnim.SetInteger("MonsterState",1);
            StartCoroutine("MonsterAttackStop");
        }
        switch(characterState)
        {
            case CharacterState.Idle:
                {
                    //���Ϳ� �÷��̾� ������ �Ÿ��� ����
                    float dist = Vector3.Distance(transform.position, target.position);

                    //�÷��̾ ���������� ��������
                    if(dist<=searchRange)
                    {
                        //�޸��� ��� ���
                        monsterAnim.SetInteger("MonsterState",2);
                        characterState = CharacterState.Detact;
                    }
                    break;
                }
            case CharacterState.Detact:
                {
                    //���Ͱ� �÷��̾ �ٶ󺻴�.
                    Vector3 relationPos = new Vector3(target.position.x, 0, target.position.z)
                        - new Vector3(transform.position.x, 0, transform.position.z);
                    Quaternion rotation = Quaternion.LookRotation(relationPos);
                    transform.rotation = rotation;

                    //���Ͱ� �÷��̾ �߰��Ѵ�.
                    transform.position =
                        Vector3.MoveTowards(transform.position,
                        new Vector3(target.position.x, transform.position.y, target.position.z),
                        moveSpeed * Time.deltaTime);

                    //���Ϳ� �÷��̾� ������ �Ÿ��� ����
                    float dist = Vector3.Distance(transform.position, target.position);

                    //Ÿ���� ���ݰ��� �Ÿ��� ������
                    if(dist<=attackRange)
                    {
                        //���� ����
                        monsterAnim.SetInteger("MonsterState",1);

                        //���� ���¸� �������� ����
                        characterState = CharacterState.Attack;
                    }

                    //Ÿ���� Ž�������� �������
                    if(dist > searchRange)
                    {
                        //���� ������ ��� ���·� ����
                        monsterAnim.SetInteger("MonsterState",0);
                        //ĳ���� ���¸� ��� ���·� ����
                        characterState = CharacterState.Idle;
                    }
                    break;
                }
            case CharacterState.Attack:
                {
                    //���Ϳ� �÷��̾� ������ �Ÿ��� ����
                    float dist = Vector3.Distance(transform.position, target.position);
                    if(dist>attackRange)
                    {
                        //���� ������ �̵����� ����
                        monsterAnim.SetInteger("MonsterState",2);
                        characterState = CharacterState.Detact;
                    }
                    break;
                }
        }

        IEnumerator OrcAttackStop()
        {
            //���� ȣ��(���� �ִϸ��̼� ����(�ð�))
            yield return new WaitForSeconds(MonsterAttack.length);

            //���� �ִϸ��̼��� Idle(0)���� �����Ѵ�.
            monsterAnim.SetInteger("MonsterState",0);
        }
    }
}
