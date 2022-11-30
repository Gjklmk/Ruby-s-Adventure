using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed=3.0f;

    private Rigidbody2D rigidbody2d;

    //轴向控制
    public bool vertical;
    //方向控制
    private int direction = 1;
    //方向改变时间间隔，常量
    public float changeTime = 3.0f;
    //计时器
    private float timer;

    private Animator animator;

    //当前机器人是否故障
    private bool broken;

    public ParticleSystem smokeEffect;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        // animator.SetFloat("MoveX",direction);
        // animator.SetBool("Vertical",vertical);
        PlayMoveAnimation();
        broken = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!broken)
        {
            //已修好，不移动
            return;
        }


        timer -= Time.deltaTime;
        if (timer<0)
        {
            direction = -direction;
            //animator.SetFloat("MoveX",direction);
            PlayMoveAnimation();
            timer = changeTime;
        }

        Vector2 position = rigidbody2d.position;

        if (vertical)//垂直轴向
        {
            position.y =position.y + Time.deltaTime*speed*direction;
        }
        else   //水平轴向
        {
            position.x = position.x + Time.deltaTime*speed*direction;
        }

        rigidbody2d.MovePosition(position);
    }

    //触发检测
    private void OnCollisionEnter2D(Collision2D collision)
    {
        RubyController rubyController = collision.gameObject.GetComponent<RubyController>();
        if (rubyController!=null)
        {
            rubyController.ChangeHealth(-1);
        }
    }

    //控制移动动画的方法
    private void PlayMoveAnimation()
    {
        if (vertical)//垂直轴向动画的控制
        {
            animator.SetFloat("MoveX",0);
            animator.SetFloat("MoveY",direction);
        }
        else  //水平轴向动画控制
        {
            animator.SetFloat("MoveX",direction);
            animator.SetFloat("MoveY",0);
        }

    }
  
    public void Fix()
    {
        broken = false;
        rigidbody2d.simulated = false;
        animator.SetTrigger("Fixed");
    }



}
