using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class MonsterBullet : MonoBehaviour
{
    public float bulletSpeed;

    private bool isBounce = false;
    private GameObject bullet;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBounce == false)
        {
            //对角色造成伤害
            if (collision.tag == "Player")
            {
                Debug.Log("角色");
                EventManager.CallPlayerHurt();
                if (bullet != null)
                {
                    Debug.Log("111");
                    transform.parent.GetComponent<Monster>().RealseBullet(bullet);
                }
            }
            if (collision.tag == "Bounce")
            {
                isBounce = true;
                bulletSpeed *= 1.2f;//反弹加速
                Debug.Log("弹反");
                StartCoroutine(ReturnBullet());
            } 
        }
        else
        {
            if(collision.tag=="Monster")
            {
                Debug.Log("对怪物造成伤害");
                //对怪物造成伤害
                if (bullet != null)
                {
                    Debug.Log("111");
                    transform.parent.GetComponent<Monster>().RealseBullet(bullet);
                }
            }
        }
    }

    private void Awake()
    {
        bullet =gameObject;
    }
    void Update()
    {
        BulletAttack();
    }

    private void BulletAttack()
    {
           if(isBounce==false)
        {
            transform.Translate(0f,Time.deltaTime * bulletSpeed ,0f);
        }
        else
        {
            transform.Translate(0f,-Time.deltaTime * bulletSpeed ,0f);
            transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z+180);
            isBounce = false;
        }
           
    }

    IEnumerator ReturnBullet()
    {
        yield return new WaitForSeconds(2f);
        if (bullet.gameObject.activeSelf == false)
        {
            isBounce = false;
        }
    }
}
