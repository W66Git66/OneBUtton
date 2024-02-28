using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : ScrollRect
{
    protected float JoystickRadiu = 0f;//Ò£¸Ë·¶Î§°ë¾¶
    protected override void Start()
    {
        base.Start();//Ô­start
        //¼ÆËãÒ¡¸Ë·¶Î§°ë¾¶
        JoystickRadiu=(transform as RectTransform).sizeDelta.x+0.45f;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        var JoystickPosition = this.content.anchoredPosition;

        if(JoystickPosition.magnitude>JoystickRadiu)
        {
            JoystickPosition=JoystickPosition.normalized*JoystickRadiu;
            SetContentAnchoredPosition(JoystickPosition);
        }
    }


}
