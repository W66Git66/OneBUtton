using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int killNumber = 0;
    private int boxNumber = 0;

    private void EndGame()
    {
        //游戏结束的功能
    }

    public void CountKillNmuber()
    {
        killNumber += 1;
        boxNumber += 1;
    }

    public bool IfBoxing()
    {
        if (boxNumber > 6)
        {
            boxNumber = 0;
            return true;
        }
        else return false;
    }
    private void Update()
    {
        //if(killNumber>10)
    }

}
