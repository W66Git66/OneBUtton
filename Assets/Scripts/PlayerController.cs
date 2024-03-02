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

    private bool isBounce=false;

    

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        PlayerMove(joystick.moveVector, speed);
        anim.SetFloat("Xmove", joystick.moveVector.x);
        anim.SetFloat("Ymove", joystick.moveVector.y);
        anim.SetFloat("Speed", joystick.moveVector.magnitude);

        StartCoroutine(BounceEvent(joystick.moveVector));
        if(boxingCoolDown>0)
        {
            boxingCoolDown -= Time.deltaTime;
           // isBoxing = false;
        }
        else
        {
           // isBoxing = true;
        }
    }
       
    private void Bounce()
    {
        EventManager.CallOnCameraShake();
        anim.SetTrigger("Kick");
        speed=0.5f*speed;
       Debug.Log("Dash triggered!");
        StartCoroutine(WaitEndCameraShake());
        
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
        yield return new WaitForSeconds(0.7f);
        speed = 2f*speed;
        EventManager.CallOutCameraShake();     
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
                if (Vector2.SignedAngle(lastJoystickPosition, joystick.moveVector) > 90f|| Vector2.SignedAngle(lastJoystickPosition, joystick.moveVector) < -90f)
                {
                    Bounce(); // 执行弹反
                    yield return new WaitForSeconds(1f);
                }
            }
            
        }
        
        isBounceEventRunning = false;
    }
 }
