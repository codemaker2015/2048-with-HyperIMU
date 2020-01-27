using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    int[,] fsm;

    int currentState;
    int initialState;

    public FSM(int statesCount, int eventsCount, int initState)
    {
        fsm = new int[statesCount, eventsCount];

        for(int i = 0; i < fsm.GetLength(0); i++)
        {
            for(int j = 0; j < fsm.GetLength(1); j++)
            {
                fsm[i, j] = -1;
            }
        }

        this.initialState = currentState = initState;
    }

    public void SetRelation(int srcState, int evt, int dstState)
    {
        if(srcState < 0 || srcState >= fsm.GetLength(0))
        {
            Debug.LogError("srcState out of range");
            return;
        }

        if(evt < 0 || evt >= fsm.GetLength(1))
        {
            Debug.LogError("evt out of range");
            return;
        }

        if(dstState < 0 || dstState >= fsm.GetLength(0))
        {
            Debug.LogError("dstState out of range");
            return;
        }

        fsm[srcState, evt] = dstState;
    }

    public void SendEvent(int evt)
    {
        if(fsm[currentState, evt] != -1)
            currentState = fsm[currentState, evt];
    }

    public int GetState()
    {
        return currentState;
    }

    public void Start()
    {
        currentState = initialState;
    }
    public void ResetState()
    {
        currentState = fsm[0, 0];
    }
}
