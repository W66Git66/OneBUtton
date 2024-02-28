using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : ScrollRect
{
    protected float JoystickRadiu = 0f;//ң�˷�Χ�뾶
    protected override void Start()
    {
        base.Start();//ԭstart
        //����ҡ�˷�Χ�뾶
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
