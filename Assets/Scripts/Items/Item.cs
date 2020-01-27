using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Entity
{
    public delegate void ItemActions(Item item);
    public ItemActions OnCollected;
    public int points = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            OnCollected(this);
    }
}
