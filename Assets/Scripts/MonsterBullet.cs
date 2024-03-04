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

    [SerializeField]
    private int hurtAmount;//伤害数值
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBounce == false)
        {
            //对角色造成伤害
            if (collision.tag == "Player")
            {
                Debug.Log("角色");
                EventManager.CallPlayerHurt();
                EventManager.CallOnHurt();
                if (bullet != null)
                {
                    transform.parent.GetComponent<Monster>().RealseBullet(bullet);
                }
            }
            if (collision.tag == "Bounce")
            {
                isBounce = true;
                bulletSpeed *= 1.2f;//反弹加速
                transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + 180);
                Debug.Log("弹反");
                StartCoroutine(ReturnBullet());
            } 
        }
        else
        {
            if(collision.tag=="Monster")
            {
                isBounce = false;
                Debug.Log("对怪物造成伤害");
                //EventManager.CallOnBoxing();
                collision.GetComponent<Monster>().MonsterHurt();
                //对怪物造成伤害
                if (bullet != null&&collision!=null)
                {
                    //collision.GetComponent<HealthBar>().DecreaseHealth(hurtAmount);
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
        transform.Translate(0f,Time.deltaTime * bulletSpeed ,0f);
    }

    
     IEnumerator ReturnBullet()
    {
        yield return new WaitForSeconds(5f);
        if (bullet.gameObject.activeSelf == false)
        {
            isBounce = false;
            bulletSpeed /= 1.2f;
            transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + 180);
        }
    }
}
