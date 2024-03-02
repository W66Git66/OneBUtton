using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Monster : MonoBehaviour
{
    public GameObject monster;
    private ObjectPool<GameObject> pool;
    float CreateTime = 5f;
    GameObject Monster1;
    public float Xmin, Xmax, Ymin, Ymax;//随机生成怪物
    private void Awake()
    {
        pool = new ObjectPool<GameObject>(createFunc, actionOnGet, actionOnRelease, actionOnDestroy, true, 10, 1000);
        monster = Resources.Load<GameObject>("Cube");//加载预制体Triangle
    }

     public GameObject createFunc()
    {
        var obj = Instantiate<GameObject>(monster, transform);
        return obj;
    }

    private void actionOnGet(GameObject obj)
    {
        obj.gameObject.SetActive(true);
    }

    private void actionOnRelease(GameObject obj)
    {
        obj.gameObject.SetActive(false);
    }
    
    private void actionOnDestroy(GameObject obj)
    {
        Destroy(obj);
    }

    void Update()
    {
        CreateTime -= Time.deltaTime;
        if (CreateTime <= 0)
        {
            CreateTime = Random.Range(3, 10);//随机倒计时3-10秒后再次生成

            GameObject temp=pool.Get();
            Monster1.transform.position = new Vector2(Random.Range(Xmin, Xmax), Random.Range(Ymin, Ymax));//随机敌人生成在一定范围内
        }
    }
}
