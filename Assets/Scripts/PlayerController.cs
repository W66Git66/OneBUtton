using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Joystick joystick;

    [SerializeField]
    private float speed;//角色移动速度

    private float boxingCoolDown = 3f;//拳击持续时间
    //private bool isBoxing = true;

    private Vector2 lastJoystickPosition=new Vector2();//保存延迟
    private bool isBounceEventRunning = false;

    private float Health=100;
    private float MaxHealth=100;//角色生命值


    private Transform colliderBox;
    private Collider2D Playercollider;//角色碰撞
    private Collider2D BounceCollider;//弹反检测
  

    

    private Animator anim;

    private void OnEnable()
    {
        EventManager.PlayerHurt += Hurt;
    }

    private void OnDisable()
    {
        EventManager.PlayerHurt -= Hurt;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        Playercollider = GetComponent<Collider2D>();//角色碰撞体
        colliderBox=transform.GetChild(0);//反弹挂件
        BounceCollider = colliderBox.GetComponent<Collider2D>();//反弹的碰撞体
    }

    private void Update()
    {
        PlayerMove(joystick.moveVector, speed);
        anim.SetFloat("Xmove", joystick.moveVector.x);
        anim.SetFloat("Ymove", joystick.moveVector.y);
        anim.SetFloat("Speed", joystick.moveVector.magnitude);

        StartCoroutine(BounceEvent(joystick.moveVector));
    }
       
    private void Bounce()
    {
        speed =0.5f*speed;
        StartCoroutine(WaitEndCameraShake());
        StartCoroutine(GetHurtOrBounce());    
    }
    private void PlayerMove(Vector2 moveVector,float speed)
    {
        if (moveVector.x > 0.3 || moveVector.x < -0.3)//向量归一化存在问题，所以尝试用死区来限制
        {
            if (moveVector.y > 0.3 || moveVector.y < -0.3)
            {
                if ((moveVector.x > 1 && moveVector.y > 1)
                    || (moveVector.x > 1 && moveVector.y < -1)
                    || (moveVector.x < -1 && moveVector.y > 1)
                    || (moveVector.x < -1 && moveVector.y < -1))
                {
                    moveVector.x = moveVector.x * (float)Mathf.Sqrt(0.5f);
                    moveVector.y = moveVector.y * (float)Mathf.Sqrt(0.5f);
                }
                    transform.Translate(moveVector.normalized * speed * Time.deltaTime);
            }
        }
    }

    IEnumerator WaitEndCameraShake()
    {
        anim.SetTrigger("Kick");
        yield return new WaitForSeconds(0.4f);
        EventManager.CallOnCameraShake();
        BounceCollider.enabled = true;
        //弹反结束
        yield return new WaitForSeconds(0.2f); 
        EventManager.CallOutCameraShake(); 
        speed = 2f*speed; 
        BounceCollider.enabled = false;
    }

    IEnumerator BounceEvent(Vector2 direction)
    {
        if (isBounceEventRunning)
        {
            yield break; // 如果有，直接返回，不执行任何操作
        }

        isBounceEventRunning = true;
        
        if (direction.magnitude > 100)
        {
            lastJoystickPosition = joystick.moveVector;
                yield return new WaitForSeconds(0.1f);
            if ((lastJoystickPosition.x * joystick.moveVector.x) < 0
                 || lastJoystickPosition.y * joystick.moveVector.y < 0)
            {
                if (Vector2.SignedAngle(lastJoystickPosition, joystick.moveVector) > 90f
                    || Vector2.SignedAngle(lastJoystickPosition, joystick.moveVector) < -90f)
                {
                    Bounce(); // 执行弹反
                    colliderBox.localPosition = joystick.moveVector.normalized ;    
                    yield return new WaitForSeconds(1f);//弹反冷却
                }
            }
            
        }
        
        isBounceEventRunning = false;
    }

    private void Hurt()
    {
        Health -= 10;
        Debug.Log(Health);
        StartCoroutine(GetHurtOrBounce());
    }

    IEnumerator GetHurtOrBounce()
    {   
        Playercollider.enabled = false;
        yield return new WaitForSeconds(2f);//无敌时间
        Playercollider.enabled = true;
    }

}
