using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    private   Rigidbody2D rb;
    private Animator anim;
    private bool IsHurt;//默认是false

    public Collider2D Coll;
    public Collider2D DisColl;
    public Transform CellingCheck;
   
    public float Speed;
    public float JumpForce;
    public LayerMask Ground;
    public int Cherry;
    public Text CherryNum;
    public AudioSource JumpAudio,HitAudio,CherryAudio;
  
  

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsHurt)//判断受伤反弹
        {
            Movement();
        }
        SwitchAnim();
    }

    void Movement()
    {
        float horizontalmove = Input.GetAxis("Horizontal");
        float facedirection = Input.GetAxisRaw("Horizontal");
        
        //角色移动
        if (horizontalmove != 0)
        {
            rb.velocity = new Vector2(horizontalmove * Speed * Time.fixedDeltaTime, rb.velocity.y);
            anim.SetFloat("running", Mathf.Abs(facedirection));
        }
        //角色朝向
        if (facedirection != 0)
        {
            transform.localScale = new Vector3(facedirection, 1, 1);
        }
        //角色跳跃
        if (Input.GetButtonDown("Jump") && Coll.IsTouchingLayers(Ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce * Time.fixedDeltaTime);
            anim.SetBool("jumping", true);
            JumpAudio.Play();

        }
        //角色爬行代码调用
        Crouch();

    }
    //角色蹲下
    void Crouch()
    {
       if (!Physics2D.OverlapCircle(CellingCheck.position,0.2f,Ground)) 
        { 
          if (Input.GetButton("Crouch"))
          {
             anim.SetBool("crouching", true);
            DisColl.enabled = false;
          } else 
          {
            anim.SetBool("crouching", false);
            DisColl.enabled = true;
          }
        }
    }

    //角色动画切换
    void SwitchAnim()
    {
        anim.SetBool("idle", false);//设置idle默认为false

        if (rb.velocity.y < 0 && !Coll.IsTouchingLayers(Ground)) 
        {
            anim.SetBool("falling", true);
        }
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0)//角色速率<0时(角色下落中)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }else if (IsHurt)//判断角色受伤动画
        {
            anim.SetBool("hurt", true);
            anim.SetFloat("runing", 0);//令受伤动画播完后直接回到idle状态，而不要再检测rb速率<0.1f
            if (Mathf.Abs(rb.velocity.x) < 0.1f)//判断角色刚体速率<0.1(反弹速度停止)
            {
                anim.SetBool("hurt", false);
                anim.SetBool("idle", true);
                IsHurt = false;
            }
        }
        else if (Coll.IsTouchingLayers(Ground))//角色触碰地面
        {
            anim.SetBool("falling", false);
            anim.SetBool("idle", true);
        }

    }
    //角色与其他gameObject碰撞
    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        //物品收集
        if (collision.tag == "Collection")
        {
            Destroy(collision.gameObject);
            Cherry += 1;
            CherryNum.text = Cherry.ToString();
            CherryAudio.Play();
        }
        //与死亡触发器碰撞
        if (collision.tag == "DeadLine")
        {
            GetComponent<AudioSource>().enabled = false;
            Invoke("Restart", 1f);//场景重置延迟

        }

    }
    //消灭敌人
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" )
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (anim.GetBool("falling"))
            {
                enemy.JumpOn();

                rb.velocity = new Vector2(rb.velocity.x, JumpForce * Time.fixedDeltaTime);
                anim.SetBool("jumping", true);
            }
            //受伤反弹
            else if(transform.position.x < collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-5, rb.velocity.y);//左边反弹
                IsHurt = true;
                HitAudio.Play();
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(5, rb.velocity.y);//右边反弹
                IsHurt = true;
                HitAudio.Play();
            }
        }

    }
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }





}