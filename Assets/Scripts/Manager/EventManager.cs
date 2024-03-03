using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventManager 
{
    public static event Action Pause;
    public static void CallOnpause()
    {
        Pause?.Invoke();
    }

    //*********��������¼�***********

    public static event Action OnCameraShake;
    public static void CallOnCameraShake()
    {
        OnCameraShake?.Invoke();
    }


    public static event Action OutCameraShake;
    public static void CallOutCameraShake()
    {
        OutCameraShake?.Invoke();
    }

    //***********�����¼�*************

    public static event Action BounceEvent;
    public static void CallBounceEvent()
    {
        BounceEvent?.Invoke();
    }

    //**********��ɫ����**************

    public static event Action PlayerHurt;
    public static void CallPlayerHurt()
    {
        PlayerHurt?.Invoke();
    }


}
