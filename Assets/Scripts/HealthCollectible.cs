using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public AudioClip audioClip;

    public GameObject effectParticle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("与我们发生碰撞的对象是："+collision);
        RubyController rubyController = collision.GetComponent<RubyController>();
        //当前发生触发检测的游戏物体对象身上有否有RubyController脚本
        if(rubyController!=null)
        {
            //Ruby是否满血
            if (rubyController.Health<rubyController.maxHealth)
            {
                rubyController.ChangeHealth(1);
                rubyController.PlaySound(audioClip);

                Instantiate(effectParticle,transform.position,Quaternion.identity);
                Destroy(gameObject);
            }
            
        }
        
        
    }


}
