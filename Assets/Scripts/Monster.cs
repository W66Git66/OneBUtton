using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class Monster : MonoBehaviour
{
    public GameObject bullet;
    private GameObject player;
    private GameObject monster;
    public GameObject gameController;

    float shootTime = 2f;
    private ObjectPool<GameObject> pool;

    [SerializeField]
    private float maxHp=100;
    [SerializeField]
    private float currentHp=100;

    private void Awake()
    {
        pool = new ObjectPool<GameObject>(createFunc, actionOnGet, actionOnRelease, actionOnDestroy, true, 10, 1000);
        player = GameObject.FindGameObjectWithTag("Player");
        monster = gameObject;
        gameController = GameObject.FindGameObjectWithTag("GameController");
    }


    public GameObject createFunc()
    {
        var obj = Instantiate<GameObject>(bullet, transform);
        return obj;
    }

    private void actionOnGet(GameObject obj)
    {
        obj.gameObject.SetActive(true);
    }

    private void actionOnRelease(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.localPosition = Vector3.zero;
    }

    private void actionOnDestroy(GameObject obj)
    {
        Destroy(obj);
    }



    void Update()
    {
        MonsterShoot();
        if(currentHp<=0)
        {
            if (monster != null)
            {
                transform.parent.GetComponent<MonsterManager>().RealseMonster(monster);
                currentHp = maxHp;
            }
            gameController.GetComponent<GameManager>().CountKillNmuber();
        }
    }

    void MonsterShoot()
    {
        shootTime -= Time.deltaTime;
        if (shootTime <= 0)
        {
            GameObject temp = pool.Get();
            shootTime = 2f;

            StartCoroutine(DestroyBullet(temp));
        }
        
    }

    IEnumerator DestroyBullet(GameObject temp)
    {
        Vector3 direction = player.transform.position - transform.position;
        temp.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction.normalized);
        yield return new WaitForSeconds(10f);//×Óµ¯ÀäÈ´
        if (temp.activeSelf)
        {
            RealseBullet(temp);
        }
    }

    public void RealseBullet (GameObject gameObject)
    {
        pool.Release(gameObject);
    }

    public void MonsterHurt()
    {
        currentHp -= 50;
    }

}
