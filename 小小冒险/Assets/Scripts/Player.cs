using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // 定义变量
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float jumpSpeed = 2f;
    [SerializeField] float climbSpeed = 1f;
    [SerializeField] Vector2 DeadDistance = new Vector2(15f,15f);
    float myGravityAtStart;
    // State
    bool isAlive = true;

    // Cashed component references
    Rigidbody2D myrigidbody2D;
    Animator IsRunning;
    Animator IsClimbing;
    Animator IsDead;
    CapsuleCollider2D myBodyCollider2D;
    BoxCollider2D myFeetCollider2D;

    void Start ()
    {
        myrigidbody2D = GetComponent<Rigidbody2D>();
        IsRunning = GetComponent<Animator>();
        IsClimbing = GetComponent<Animator>();
        IsDead = GetComponent<Animator>();
        myBodyCollider2D = GetComponent<CapsuleCollider2D>();
        myFeetCollider2D = GetComponent<BoxCollider2D>();
        myGravityAtStart = myrigidbody2D.gravityScale;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!isAlive){ return; }
        Run();
        FlipSprite();
        Jump();
        Climbing();
        Dead();
    }
    private void Run()
    {
        float MoveControl = Input.GetAxis("Horizontal");
        Vector2 playerMovement = new Vector2(MoveControl * moveSpeed, myrigidbody2D.velocity.y);
        myrigidbody2D.velocity = playerMovement;
        //if (MoveControl < 0)
        //{
        //    flip.flipX = true;
        //}
        //else if(MoveControl > 0)
        //{
        //    flip.flipX = false;
        //}
        bool playerhasHoriztonalSpeed = Mathf.Abs(myrigidbody2D.velocity.x) > 0;
        IsRunning.SetBool("Running", playerhasHoriztonalSpeed);
    }

    private void FlipSprite()
    {
        // 判断x轴上rigibody的速度的绝对值是否大于0；
        bool IsFlip = Mathf.Abs(myrigidbody2D.velocity.x) > 0;
        if (IsFlip)
        {
            // 是的话，local Scale让角色变向
            transform.localScale = new Vector2(Mathf.Sign(myrigidbody2D.velocity.x), 1f);
        }
    }

    private void Jump()
    {
        // 让角色碰到地板只跳一次，按Jump键不会连续跳
        if(!myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("ground")))
        {
            return;
        }
        // 利用getbuttondown而不用getaxis因为，我们不是一直按着跳跃键
        if (Input.GetButtonDown("Jump"))
        {
            // 按照Y轴上跳，所以X轴就为0
            Vector2 playerJumpMovement = new Vector2(0f, jumpSpeed);
            // 碰撞体的速度 = 碰撞体的速度 加 角色跳跃运动
            myrigidbody2D.velocity += playerJumpMovement;
        }
    }

    private void Climbing()
    {
        // 在触碰到楼梯层面时，角色碰撞体触发
        if (!myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            // 不爬楼梯时重力回到初始
            myrigidbody2D.gravityScale = myGravityAtStart;
            IsClimbing.SetBool("Climbing", false);            
            return;
        }
        // 爬楼梯时重力为0
        myrigidbody2D.gravityScale = 0f;
        // 需要不停按照Up和Down键，所以要用Input.GetAxis
        float climbingControl = Input.GetAxis("Vertical");
        Vector2 playerUpDownMovement = new Vector2(myrigidbody2D.velocity.x, climbingControl * climbSpeed);
        myrigidbody2D.velocity = playerUpDownMovement;
        bool isClimbing = Mathf.Abs(myrigidbody2D.velocity.y) > 0;
        IsClimbing.SetBool("Climbing", isClimbing);
    }

    private void Dead()
    {
        if (myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy", "Spikes")))
        {
            isAlive = false;
            IsDead.SetTrigger("Dead");
            // 死后的距离
            GetComponent<Rigidbody2D>().velocity = DeadDistance;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}
