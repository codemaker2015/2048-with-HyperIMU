using System;
using UnityEngine;

public class PacMan : MobileEntity
{        
    public delegate void PacManActionsActions(PacMan p);
    public PacManActionsActions OnDeathAnimationFinished; 
    public Vector2 direction = new Vector2(1, 0);
    private Animator animator;

	private DateTime time;
	public UDPReceiver reciever;
	private float x,y,z;

    void Start()
    {
        speed = 50.0f;
        animator = GetComponent<Animator>();
		time = DateTime.Now;
		reciever = GameObject.Find ("UDPReceiver").GetComponent<UDPReceiver>();

    }

    // Update is called once per frame
    void Update()
    {
        if(alive)
        {
            //HandleInput();
			HandleInput2 ();
            MoveToDestination();
        }
    }

	public void HandleInput2(){
		Vector2 newDirection = direction;

		try
		{
			string[] coords = reciever.result.Split(',');
			x = float.Parse(coords[0]);
			z = float.Parse(coords[1]);
		}
		catch (Exception err)
		{
			Debug.Log(err.ToString());
		}

		if (z < -3 && (DateTime.Now - time).Seconds > 1) {
			time = DateTime.Now;
			newDirection = new Vector2(-1, 0);
		} else if (z > 3 && (DateTime.Now - time).Seconds > 1) {
			time = DateTime.Now;
			newDirection = new Vector2(1, 0);
		} else if (x > 3 && (DateTime.Now - time).Seconds > 1) {
			time = DateTime.Now;
			newDirection = new Vector2(0, -1);
		} else if (x < -3 && (DateTime.Now - time).Seconds > 1) {
			time = DateTime.Now;
			newDirection = new Vector2(0, 1);
		}
		Move (newDirection);
	}

    public void HandleInput()
    {
        Vector2 newDirection = direction;
        if(Input.GetKey(KeyCode.UpArrow))
        {
            newDirection = new Vector2(0, 1);
        }
        else if(Input.GetKey(KeyCode.DownArrow))
        {
            newDirection = new Vector2(0, -1);
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            newDirection = new Vector2(1, 0);
        }
        else if(Input.GetKey(KeyCode.LeftArrow))
        {
            newDirection = new Vector2(-1, 0);
        }
        Move(newDirection);
    }
    public void Move(Vector2 newDirection)
    {
        int nextTileX = GetCurrentTileX() + (int)newDirection.x;
        int nextTileY = GetCurrentTileY() + (int)newDirection.y;
        if(MapManager.Get().TileIsValid(nextTileX, nextTileY))
        {
            SetNextTile(new Vector2Int(nextTileX, nextTileY));
            direction = newDirection;
        }
        else
        {
            nextTileX = GetCurrentTileX() + (int)direction.x;
            nextTileY = GetCurrentTileY() + (int)direction.y;

            if(MapManager.Get().TileIsValid(nextTileX, nextTileY))
            {
                SetNextTile(new Vector2Int(nextTileX, nextTileY));                
            }
        }
        animator.SetFloat("DirX", direction.x);
        animator.SetFloat("DirY", direction.y);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ghost" && alive)
        {
            alive = false;
            animator.SetFloat("DirX", 0);
            animator.SetFloat("DirY", 0);
            animator.SetTrigger("Dead");
        }
    }
    private void OnDeathAnimationFinishedCallback()
    {
        OnDeathAnimationFinished(this);
    }
    public void Reset()
    {
        direction = new Vector2(0, 1);
        animator.SetFloat("DirX", direction.x);
        animator.SetFloat("DirY", direction.y);
    }
}