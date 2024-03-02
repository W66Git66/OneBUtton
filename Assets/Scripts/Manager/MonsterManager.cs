using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class MonsterManager : MonoBehaviour
{
    public GameObject Flower;
    public GameObject Worm;
    private ObjectPool<GameObject> pool;
    float CreateTime = 5f;
    GameObject Monster1;
    public float Xmin, Xmax, Ymin, Ymax;//随机生成怪物
    List<GameObject> list;
    private void Awake()
    {
        pool = new ObjectPool<GameObject>(createFunc, actionOnGet, actionOnRelease, actionOnDestroy, true, 10, 1000);        
 
    }
    private void Start()
    {
        Worm = Resources.Load<GameObject>("Worm");//加载预制体
        Flower = Resources.Load<GameObject>("Flower");

        list = new List<GameObject>() { Worm,Flower };
    }



    public GameObject createFunc()
    {
        var obj = Instantiate<GameObject>(list[Random.Range(0,2)], transform);
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
            temp.transform.position = new Vector2(Random.Range(Xmin, Xmax), Random.Range(Ymin, Ymax));//随机敌人生成在一定范围内
        }
    }
}
