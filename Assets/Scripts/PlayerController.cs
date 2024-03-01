using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Joystick joystick;

    [SerializeField]
    private float speed;//角色移动速度

    private float bounceCooldown=0.5f; //弹反冷却时间
    private float boxingCoolDown = 3f;//拳击持续时间
    private bool isBoxing = true;

    private Vector2 lastJoystickPosition=new Vector2();//保存延迟
    private bool isBounceEventRunning = false;

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

        bounceCooldown -= Time.deltaTime;
        StartCoroutine(BounceEvent(joystick.moveVector));
        if(boxingCoolDown>0)
        {
            boxingCoolDown -= Time.deltaTime;
            isBoxing = false;
        }
        else
        {
            isBoxing = true;
        }
    }
       
    private void Bounce()
    {
        anim.SetTrigger("Kick");
       Debug.Log("Dash triggered!");
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

    IEnumerator BounceEvent(Vector2 direction)
    {
        if (isBounceEventRunning)
        {
            yield break; // 如果有，直接返回，不执行任何操作
        }

        isBounceEventRunning = true;
        
        if (bounceCooldown<0&&direction.magnitude > 100)
        {
            lastJoystickPosition = joystick.moveVector;
                yield return new WaitForSeconds(0.2f);
            if ((lastJoystickPosition.x * joystick.moveVector.x) < 0
            || lastJoystickPosition.y * joystick.moveVector.y < 0)
            {
                if (Vector2.SignedAngle(lastJoystickPosition, joystick.moveVector) > 90f|| Vector2.SignedAngle(lastJoystickPosition, joystick.moveVector) < -90f)
                {
                    Bounce(); // 执行弹反
                    bounceCooldown = 0.5f;
                }
            }
            
        }
        
        isBounceEventRunning = false;
    }
 }
