using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public bool alive = false;

    public Vector2 GetPosition()
    {
        return transform.position / MapManager.Get().tileSize;
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }
}
