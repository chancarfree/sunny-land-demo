using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : Enemy
{
    private Rigidbody2D rb;
    
    //private Animator Anim;
    public float Speed;
    public Transform TopPoint, ButtomPoint;
    private float TopY, ButtomY;
    private bool IsUp = true;

   protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        
        //Anim = GetComponent<Animator>();

        TopY = TopPoint.position.y;
        ButtomY = ButtomPoint.position.y;
        Destroy(TopPoint.gameObject);
        Destroy(ButtomPoint.gameObject);

    }

    
    void Update()
    {
        Movement();
    }

    void Movement() 
    {
        if(IsUp)
        {
            rb.velocity = new Vector2(rb.velocity.x, Speed);
            if (transform.position.y > TopY)
            {
                IsUp = false;
            }

        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, -Speed);
            if (transform.position.y < ButtomY)
            {
                IsUp = true;
            }
        }

        
        
        
        

    }



}
