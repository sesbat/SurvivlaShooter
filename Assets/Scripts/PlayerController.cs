using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerController : AliveObjectHealth
{
    public Gun gun;
    public EffectSound audio;
    public AudioClip deathClip;
    public AudioClip hitClip;

    private PlayerInput playerInput;
    private Rigidbody playerRigidBody;
    private Animator playerAnimator;

    private Camera camera;

    public float moveSpeed = 10f;
    public float rotateSpeed = 180f;

    public GameObject leftFire;
    // Start is called before the first frame update

    private void Awake()
    {
        hp = maxHp = 100;
    }
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidBody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        camera = GetComponent<Camera>();
        GameManager.instance.SetMaxHp(maxHp);
    }

    private void FixedUpdate() //물리 갱신 주기마다 업데이트
    {
        Move();
        Rotate();

        var dir = new Vector3(playerInput.moveH, 0f, playerInput.moveV);
        playerAnimator.SetFloat("Move", dir.magnitude);
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isSetting)
        {
            return;
        }
        if(playerInput.fire)
        {
            gun.Fire();
        }
        if (playerInput.left_fire && isHitLeft)
        {
            Instantiate(leftFire,gun.gunPivot.position,transform.rotation);
        }
    }
    private void Move()
    {
        var forward = Camera.main.transform.forward;
        forward.y = 0f;
        forward.Normalize();
        var side = Camera.main.transform.right;
        side.y = 0f;
        side.Normalize();

        var dir = forward * playerInput.moveV;
        dir += side * playerInput.moveH;

        if (dir.magnitude > 1)
        {
            dir.Normalize();
        }
        var delta = dir * moveSpeed * Time.deltaTime;

        playerRigidBody.MovePosition(playerRigidBody.position+delta);
    }
    private void Rotate()
    {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(playerInput.mousePos);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground", "Ground")))
        {
            var forward = hit.point - transform.position;
            forward.y = 0;
            forward.Normalize();

            transform.rotation = Quaternion.LookRotation(forward);
        }
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.blue);
    }

    //피격시 호출할 함수
    public override bool OnDamage(int dmg, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (isDead) // 이미 뒤진놈은 return False 해줄래
            return false;
        base.OnDamage(dmg, hitPoint, hitNormal); //데미지 주고ds

        GameManager.instance.SetHP= hp;

        if (isDead) //만약 뒤졌다면
        {
            audio.PlayOneShot(deathClip);
            playerAnimator.SetTrigger("Death");
            GameManager.instance.GameOver();
        }

        audio.PlayOneShot(hitClip);
        StartCoroutine(GameManager.instance.HitPlayerUi());

        return isDead; //살았는지 뒤졌는지 반환
    }

    public override void DieAniamtion()
    {

    }
}
