using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : ScrollRect
{
    protected float JoystickRadiu = 0f;//“£∏À∑∂Œß∞Îæ∂
    public Vector2 moveVector=new Vector2();//“°∏Àœ‡∂‘“∆∂Øæ‡¿Î
    protected override void Start()
    {
        base.Start();//‘≠start

        //º∆À„“°∏À∑∂Œß∞Îæ∂
        JoystickRadiu =(transform as RectTransform).sizeDelta.x+0.45f;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        var JoystickPosition = this.content.anchoredPosition;
        var position = this.content.localPosition;

        if(JoystickPosition.magnitude>JoystickRadiu)
        {
            JoystickPosition=JoystickPosition.normalized*JoystickRadiu;
            SetContentAnchoredPosition(JoystickPosition);
        }
    }

    private void Update()
    {
        moveVector = this.content.localPosition;//“°∏Àœ‡∂‘Œª“∆
    }


}
