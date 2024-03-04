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
    private bool isBlock=true;//玩家手部被封锁

    private Transform colliderBox;
    private Collider2D Playercollider;//角色碰撞
    private Collider2D BounceCollider;//弹反检测

    public bool isDead;

  

    

    private Animator anim;

    private void OnEnable()
    {
        EventManager.PlayerHurt += Hurt;
        EventManager.OnBoxing += Boxing;
    }

    private void OnDisable()
    {
        EventManager.PlayerHurt -= Hurt;
        EventManager.OnBoxing += Boxing;
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
        anim.SetFloat("Xmove", joystick.moveVector.normalized.x);
        anim.SetFloat("Ymove", joystick.moveVector.normalized.y);
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
        EventManager.CallOnCameraShake();
        BounceCollider.enabled = true;
        yield return new WaitForSeconds(0.3f); //相机              
        //弹反结束
        EventManager.CallOutCameraShake(); 
        yield return new WaitForSeconds(0.4f);      
        speed = 2f*speed; 
        BounceCollider.enabled = false;
        StartCoroutine(GetHurtOrBounce());
    }

    IEnumerator BounceEvent(Vector2 direction)
    {
        if (isBounceEventRunning|| direction.magnitude <= 100)
        {
            yield break; // 如果有，直接返回，不执行任何操作
        }

        isBounceEventRunning = true;
        lastJoystickPosition = joystick.moveVector;
        yield return new WaitForSeconds(0.1f);//记录位置间隔
        if (direction.magnitude > 100)
        {
                
            if ((lastJoystickPosition.x * joystick.moveVector.x) < 0
                 || lastJoystickPosition.y * joystick.moveVector.y < 0)
            {
                if (Vector2.SignedAngle(lastJoystickPosition, joystick.moveVector) > 90f
                    || Vector2.SignedAngle(lastJoystickPosition, joystick.moveVector) < -90f)
                {
                   // Debug.Log();
                    Bounce(); // 执行弹反
                    float rotation = Mathf.Rad2Deg * Mathf.Atan2(joystick.moveVector.y - lastJoystickPosition.y, joystick.moveVector.x - lastJoystickPosition.x);
                    colliderBox.localPosition = joystick.moveVector.normalized ;
                    colliderBox.localRotation =Quaternion.Euler(0,0, rotation);
                    yield return new WaitForSeconds(0.8f);//弹反冷却
                }
            }   
        }
        
        isBounceEventRunning = false;
    }

    private void Hurt()
    {
        StartCoroutine(GetHurtOrBounce());
    }

    IEnumerator GetHurtOrBounce()
    {   
        Playercollider.enabled = false;
        yield return new WaitForSeconds(0.7f);//无敌时间
        Playercollider.enabled = true;
    }

    private void Boxing()
    {
        StartCoroutine(StartBoxing());
    }
    IEnumerator StartBoxing()
    {
        anim.SetTrigger("Boxing");
        isBlock = false;
        yield return new WaitForSeconds(5f);
        isBlock = true;
    }

}
