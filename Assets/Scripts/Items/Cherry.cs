using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : Item
{
    public ItemActions OnTimerRunOut;
    private float timer = 0f;
    public float lifeTime = 5f;

    private void Start()
    {
        points = 100;
    }

    private void Update()
    {
        if(timer >= lifeTime)
        {
            timer = 0f;
            OnTimerRunOut(this);            
        }
        else
            timer += Time.deltaTime;
    }
}
