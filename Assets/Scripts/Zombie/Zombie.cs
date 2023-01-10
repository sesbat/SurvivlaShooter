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
    NavMeshAgent pathFinder; // 경로계산 AI 에이전트
    Animator animator;
    Coroutine moveCoroutine; //움직이는 함수 OnDamage 에서 Death가 true면 중단

    [SerializeField]
    ParticleSystem hitEffect; //피격 파티클

    AliveObjectHealth myTarget; //플레이어
    public EffectSound audio;
    public AudioClip hitClip;
    public AudioClip deathClip;

    //타겟이 존재하는지 존재하지 않는지
    bool isTargeting 
    {
        get { return myTarget != null; }
    }

    Action OnDead; //죽는 애니메이션 끝날때 호출
    float findDis;

    private void Awake()
    {
        targetMask = LayerMask.GetMask("Player");
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

        pathFinder.isStopped = true; //시작할땐 네비게이션x

        OnDead += () => GameManager.instance.AddScore = 100;
        OnDead += () => StopCoroutine(moveCoroutine); //죽을때 함께 호출하는 함수들, 움직이는 코루틴 종료
        OnDead += () => animator.SetTrigger("Death"); //죽는 애니메이션 출력
        OnDead += () => pathFinder.enabled = false; //네비게이션 비활성화
        OnDead += () => { var cols = GetComponents<CapsuleCollider>(); foreach (var col in cols) col.enabled = false; };
        OnDead += () => GetComponent<Rigidbody>().AddForce(Vector3.up * 100);

        moveCoroutine = StartCoroutine(UpdateMove()); //움직이는 코루틴 시작

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
            if (!isDead) //살아있을때
            {
                if (!isTargeting) //타겟이 없다면
                {
                    var players = Physics.OverlapSphere(transform.position, findDis, targetMask); //플레이어들 중에서

                    //일단 0번째로 가져오긴 했는데 멀티플레이할때 가장 가까운거리 가져오는건지 확인 필요
                    if (players.Length != 0)
                    {
                        pathFinder.isStopped = false; //탐색 시작
                        myTarget = players[0].GetComponent<AliveObjectHealth>(); //0번째 플레이어를 타겟으로
                    }
                }
                else //타겟이 있다면
                {
                    pathFinder.SetDestination(myTarget.transform.position); //타겟을 향해 이동
                }
            }
            yield return new WaitForSeconds(0.25f); // 이 코루틴은 0.25초마다 반복
        }
    }

    public override void DieAniamtion() //Death 애니메이션 중에 호출
    {
        Destroy(gameObject, 1f); //1초뒤 삭제 예약
    }

    public void GameOver() //플레이어가 전부 죽으면 호출 예정
    {
        StopCoroutine(moveCoroutine); //탐색 종료
        pathFinder.enabled = false; //네비게이션 비활성화
        animator.SetTrigger("Idle"); //Idle 애니메이션 호출
    }

    //피격시 호출할 함수
    public override bool OnDamage(int dmg, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (isDead) // 이미 뒤진놈은 return False 해줄래
            return false;
        hitEffect.transform.position = hitPoint;
        hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
        hitEffect.Play(); //이펙트 출력 해주고
        base.OnDamage(dmg, hitPoint, hitNormal); //데미지 주고
        audio.PlayOneShot(hitClip);

        if (isDead) //만약 뒤졌다면
        {
            audio.PlayOneShot(deathClip);
            OnDead(); //Action 에 넣은 이벤트(함수) 들 호출.
        }

        return isDead; //살았는지 뒤졌는지 반환
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
