using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component
{
    private static T instance;

    public static T Get()
    {
        return instance;
    }

    protected virtual void Awake()
    {
        if (!instance)
        {
            instance = this as T;            
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
