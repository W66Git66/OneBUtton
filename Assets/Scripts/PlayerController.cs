using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Joystick joystick;

    [SerializeField]
    private float speed;//角色移动速度

    private void Update()
    {
        if (joystick.moveVector.x > 0.3 || joystick.moveVector.x < -0.3)//向量归一化存在问题，所以尝试用死区来限制
        {
            if (joystick.moveVector.y > 0.3 || joystick.moveVector.y < -0.3)
            {
                if((joystick.moveVector.x>1&&joystick.moveVector.y>1)
                    ||(joystick.moveVector.x>1&& joystick.moveVector.y < -1)
                    || (joystick.moveVector.x < -1 && joystick.moveVector.y > 1)
                    || (joystick.moveVector.x < -1 && joystick.moveVector.y < -1))
                {
                    joystick.moveVector.x = joystick.moveVector.x * (float)Mathf.Sqrt(0.5f);
                    joystick.moveVector.y = joystick.moveVector.y * (float)Mathf.Sqrt(0.5f);
                }
                transform.Translate(joystick.moveVector.normalized * speed * Time.deltaTime);
            }
        }
    }
}
