using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    //public Joystick joystick;
    private Rigidbody2D rigidbody2d;
    
    public int speed=3;//ruby的速度
    //Ruby的生命值
    public int maxHealth = 5;//最大生命值
    private int currentHealth;//当前生命值

    public int Health{get {return currentHealth;}}

    //RUby的无敌时间
    public float timeInvincible = 2.0f;//无敌时间常量
    public bool isInvincible;
    public float invincibleTimer;//计时器

    private Vector2 lookDirection = new Vector2(1,0);
    private Animator animator;

    public GameObject projectilePrefab;

    public AudioSource audioSource;

    public AudioSource walkAudioSource;

    public AudioClip playerHit;
    public AudioClip attackSoundClip;
    public AudioClip walkSound;

    private Vector3 respawnPosition;

    public VariableJoystick variableJoystick;

    // Start is called before the first frame update
    void Start()
    {
        //Application.targetFrameRate = 10;
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        //audioSource = GetComponent<AudioSource>();
        respawnPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        //玩家输入监听
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

         //判断如果没有输入再获取摇杆的值
        horizontal = horizontal == 0 ? variableJoystick.Horizontal : horizontal;
        vertical = vertical == 0 ? variableJoystick.Vertical : vertical;

        Vector2 move = new Vector2(horizontal,vertical);
        //当前玩家输入的某个轴向值不为0
        if (!Mathf.Approximately(move.x,0)||!Mathf.Approximately(move.y,0))
        {
            lookDirection.Set(move.x,move.y);
            lookDirection.Normalize();
            if (!walkAudioSource.isPlaying)
            {
                walkAudioSource.clip = walkSound;
                walkAudioSource.Play();
            }
        }
        else
        {
            walkAudioSource.Stop();
        }

        //动画控制
        animator.SetFloat("Look X",lookDirection.x);
        animator.SetFloat("Look Y",lookDirection.y);
        animator.SetFloat("Speed",move.magnitude);

        //移动
        Vector2 position = transform.position;
        //Ruby的水平竖直移动
        // position.x = position.x + speed*horizontal*Time.deltaTime;
        // position.y = position.y + speed*vertical*Time.deltaTime;
        position = position + speed*move*Time.deltaTime;
        //transform.position = position;
        rigidbody2d.MovePosition(position);

        //无敌时间计算
        if (isInvincible)
        {
            invincibleTimer = invincibleTimer - Time.deltaTime;
            if (invincibleTimer<=0)
            {
                isInvincible = false;
            }
        }

        //修理机器人
        if (Input.GetKeyDown(KeyCode.H))
        {
            Launch();

        }


         if (Input.GetKeyDown(KeyCode.T))
        {
            Dialog();
        }
        
        //检测是否与NPC对话
        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //     RaycastHit2D hit=Physics2D.Raycast(rigidbody2d.position+Vector2.up*0.2f,
        //         lookDirection,1.5f,LayerMask.GetMask("NPC"));
        //     if (hit.collider!=null)
        //     {
        //         //Debug.Log("当前射线检测碰撞到的游戏物体是："+hit.collider,gameObject);
        //         NPCDialog npcDialog = hit.collider.GetComponent<NPCDialog>();
        //         if(npcDialog!=null)
        //         {
        //             npcDialog.DisplayDialog();

        //         }


        //     }
        // }

         //摇杆
        // void FixedUpdate()
        //  {
        //     float horizontal = Input.GetAxis("Horizontal");
        //     float vertical = Input.GetAxis("Vertical");

        //     //判断如果没有输入再获取摇杆的值
        //     horizontal = horizontal ==0 ? variableJoystick.Horizontal : horizontal;
        //     vertical = vertical == 0 ? variableJoystick.Vertical : vertical;

            
        //  }
         
        
        
    }

    //检测是否与NPC对话
    public void Dialog()
    {
        RaycastHit2D hit=Physics2D.Raycast(rigidbody2d.position+Vector2.up*0.2f,
                lookDirection,1.5f,LayerMask.GetMask("NPC"));
            if (hit.collider!=null)
            {
                //Debug.Log("当前射线检测碰撞到的游戏物体是："+hit.collider,gameObject);
                NPCDialog npcDialog = hit.collider.GetComponent<NPCDialog>();
                if(npcDialog!=null)
                {
                    npcDialog.DisplayDialog();

                }


            }
    }

    public void ChangeHealth(int amount)
    {
        if (amount<0)
        {
            if (isInvincible)
            { 
                return;
            }
            //受到伤害
            isInvincible = true;
            invincibleTimer = timeInvincible;
            animator.SetTrigger("Hit");
            PlaySound(playerHit);
        }
        currentHealth = Mathf.Clamp(currentHealth+amount,0,maxHealth);
        //Debug.Log(currentHealth+"/"+maxHealth);

        if (currentHealth<=0)
        {
            Respawn();
        }

        UIHealthBar.instance.SetValue(currentHealth/(float)maxHealth);
    }
    
    public void Launch()
    {
        if (!UIHealthBar.instance.hasTask)
        {
            return;
        }
        GameObject projectileObject = Instantiate(projectilePrefab,rigidbody2d.position+Vector2.up*0.5f,Quaternion.identity);
        Projectile projectile=projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection,300);
        animator.SetTrigger("Launch");
        PlaySound(attackSoundClip);
    }

    public void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    private void Respawn()
    {
        ChangeHealth(maxHealth);
        transform.position = respawnPosition;
    }

}
