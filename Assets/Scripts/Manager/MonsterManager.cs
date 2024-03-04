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
    private float CreateTime = 1f;

    public float Xmin, Xmax, Ymin, Ymax;//������ɹ���
    List<GameObject> list;
    private void Awake()
    {
        pool = new ObjectPool<GameObject>(createFunc, actionOnGet, actionOnRelease, actionOnDestroy, true, 10, 1000);        
 
    }
    private void Start()
    {
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
            CreateTime = Random.Range(3f, 10f);//�������ʱ3-10����ٴ�����
            GameObject temp=pool.Get();
            temp.transform.position = new Vector2(Random.Range(Xmin, Xmax), Random.Range(Ymin, Ymax));//�������������һ����Χ��
        }
    }

    public void RealseMonster(GameObject gameObject)
    {
        pool.Release(gameObject);
    }
}
