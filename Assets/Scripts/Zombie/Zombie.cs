using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Zombie : AliveObjectHealth
{
    public ZombieData data;

    LayerMask targetMask;
    CapsuleCollider bodyCollider;
    NavMeshAgent pathFinder; // ��ΰ�� AI ������Ʈ
    Animator animator;
    Coroutine moveCoroutine; //�����̴� �Լ� OnDamage ���� Death�� true�� �ߴ�

    [SerializeField]
    ParticleSystem hitEffect; //�ǰ� ��ƼŬ

    AliveObjectHealth myTarget; //�÷��̾�

    //Ÿ���� �����ϴ��� �������� �ʴ���
    bool isTargeting 
    {
        get { return myTarget != null; }
    }

    Action OnDead; //�״� �ִϸ��̼� ������ ȣ��
    float findDis;

    private void Awake()
    {
        targetMask = LayerMask.GetMask("Player");
        bodyCollider = GetComponent<CapsuleCollider>();
        pathFinder = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        hp = data.hp;
        maxHp = data.maxHp;
        pathFinder.speed = speed = data.speed;
        damage = data.damage;
        findDis = data.findDis;

        pathFinder.isStopped = true; //�����Ҷ� �׺���̼�x

        OnDead += () => StopCoroutine(moveCoroutine); //������ �Բ� ȣ���ϴ� �Լ���, �����̴� �ڷ�ƾ ����
        OnDead += () => animator.SetTrigger("Death"); //�״� �ִϸ��̼� ���
        OnDead += () => pathFinder.enabled = false; //�׺���̼� ��Ȱ��ȭ

        moveCoroutine = StartCoroutine(UpdateMove()); //�����̴� �ڷ�ƾ ����

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    if (isHit)
        //    {
        //        OnDamage(damage, Vector3.one, Vector3.one);
        //        Debug.Log("HitMyBody");
        //    }
        //}
    }

    IEnumerator UpdateMove()
    {
        while(true)
        {
            if (!isDead) //���������
            {
                if (!isTargeting) //Ÿ���� ���ٸ�
                {
                    var players = Physics.OverlapSphere(transform.position, findDis, targetMask); //�÷��̾�� �߿���

                    //�ϴ� 0��°�� �������� �ߴµ� ��Ƽ�÷����Ҷ� ���� �����Ÿ� �������°��� Ȯ�� �ʿ�
                    if (players.Length != 0)
                    {
                        pathFinder.isStopped = false; //Ž�� ����
                        myTarget = players[0].GetComponent<AliveObjectHealth>(); //0��° �÷��̾ Ÿ������
                    }
                }
                else //Ÿ���� �ִٸ�
                {
                    pathFinder.SetDestination(myTarget.transform.position); //Ÿ���� ���� �̵�
                }
            }
            yield return new WaitForSeconds(0.25f); // �� �ڷ�ƾ�� 0.25�ʸ��� �ݺ�
        }
    }

    public override void DieAniamtion() //Death �ִϸ��̼� �߿� ȣ��
    {
        bodyCollider.enabled = false; //�ݶ��̴��� ��Ȱ��ȭ ���� ������ ������
        Destroy(gameObject, 1f); //1�ʵ� ���� ����
    }

    [ContextMenu("GameOver")]
    public void GameOver() //�÷��̾ ���� ������ ȣ�� ����
    {
        StopCoroutine(moveCoroutine); //Ž�� ����
        pathFinder.enabled = false; //�׺���̼� ��Ȱ��ȭ
        animator.SetTrigger("Idle"); //Idle �ִϸ��̼� ȣ��
    }

    //�ǰݽ� ȣ���� �Լ�
    public override bool OnDamage(int dmg, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (isDead) // �̹� �������� return False ���ٷ�
            return false;
        hitEffect.transform.position = hitPoint;
        hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
        hitEffect.Play(); //����Ʈ ��� ���ְ�
        base.OnDamage(dmg, hitPoint, hitNormal); //������ �ְ�

        if (isDead) //���� �����ٸ�
        {
            OnDead(); //Action �� ���� �̺�Ʈ(�Լ�) �� ȣ��.
        }

        return isDead; //��Ҵ��� �������� ��ȯ
    }
    private void OnTriggerEnter(Collider other)
    {
        var obj = other.transform.GetComponent<AliveObjectHealth>();
        if (obj != null && obj == myTarget && isHit)
        {
            var hitPoint = other.ClosestPoint(transform.position);
            var hitNormal = (transform.position - other.transform.position).normalized;
            obj.OnDamage(damage, hitPoint, hitNormal);
        }
    }
}
