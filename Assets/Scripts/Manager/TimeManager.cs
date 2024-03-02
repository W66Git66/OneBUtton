using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float secound = 10f;

    private void OnEnable()
    {
        EventManager.Pause += ClickPause;
    }
    void Update()
    {
        if (secound > 0)
        {
            Timing(); //��Ϸ����ʱ
        }
        if (secound <= 0)
        {
            //ִ����Ϸ����
        }
    }
    private void OnDisable()
    {
        EventManager.Pause -= ClickPause;
    }

    private float waitTime;
    private void Timing()
    {
        if (secound <= 0)
        {
            Debug.Log("Time is up");
            return;
        }
        waitTime += Time.deltaTime;
        if (waitTime >= 1)
        {
            secound--;
            // Debug.Log(secound);
            waitTime = 0;
        }
    }

    //��Ϸ��ͣ
    public void OnPause()
    {
        Time.timeScale = 0f;
        Debug.Log("Pause");
    }

    //��Ϸ����
    public void OnProcess()
    {
        Time.timeScale = 1f;
        Debug.Log("Process");
    }

    //��Ϸ��ͣ����
    public void ClickPause()
    {
        if (Time.timeScale == 0f)
        {
            OnProcess();
        }
        else if (Time.timeScale == 1f)
        {
            OnPause();
        }
    }
}
