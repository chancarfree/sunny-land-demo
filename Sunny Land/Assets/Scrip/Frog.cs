using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{

    private Rigidbody2D rb;
    //private Animator Anim; 父级类Enemy已调用动画器，子级不用再调用
    private Collider2D Coll;
    public LayerMask Ground;
    public Transform leftpoint, rightpoint;
    public float Speed;
    public float JumpForce;
    private float LeftX, RightX;
    private bool FaceLeft = true;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        //Anim = GetComponent<Animator>();
        Coll = GetComponent<Collider2D>();
       transform.DetachChildren();//可删除
        LeftX = leftpoint.position.x;
        RightX = rightpoint.position.x;
        Destroy(leftpoint.gameObject);
        Destroy(rightpoint.gameObject);
    }

    
    void Update()
    {
        SwitchAnim();

    }

    void Movement ()
    {

        if (FaceLeft)//面向左
        {
            if (Coll.IsTouchingLayers(Ground))
            {
                Anim.SetBool("jumping", true);
                rb.velocity = new Vector2(-Speed, JumpForce);
            }
            if (transform.position.x < LeftX)//超过左点掉头
            {
                transform.localScale = new Vector3(-1, 1, 1);
                FaceLeft = false;
            }
        }
        else//面向右
        {
            if (Coll.IsTouchingLayers(Ground))
            {
                Anim.SetBool("jumping", true);
                rb.velocity = new Vector2(Speed, JumpForce);
            }
            if (transform.position.x > RightX)//超过右点掉头
            {
                transform.localScale = new Vector3(1, 1, 1);
                FaceLeft = true;
            }
        }

    }

    void SwitchAnim ()
    {
        if (Anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0.1)
            {
                Anim.SetBool("jumping", false);
                Anim.SetBool("falling", true);
            }
        }
        if(Coll.IsTouchingLayers(Ground) && Anim.GetBool("falling"))
        {
            Anim.SetBool("falling", false);
            
        }
    }

    
}
