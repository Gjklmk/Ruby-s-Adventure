using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    public int maxHealth = 5;//最大生命值
    private int currentHealth;//当前生命值



    // Start is called before the first frame update
    void Start()
    {
        //Application.targetFrameRate = 10;
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //玩家输入监听
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 position = transform.position;
        //Ruby的水平竖直移动
        position.x = position.x + 3*horizontal*Time.deltaTime;
        position.y = position.y + 3*vertical*Time.deltaTime;
        //transform.position = position;
        rigidbody2d.MovePosition(position);
    }

    private void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth+amount,0,maxHealth);
        Debug.Log(currentHealth+"/"+maxHealth);
    }
    
}
