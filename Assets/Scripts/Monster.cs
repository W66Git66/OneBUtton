using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Monster : MonoBehaviour
{
    private bool isGrow = true;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Growth()
    {
        if (isGrow == true)
        {        
            isGrow = false;
        }
    }  
}
