using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MobileEntity
{
    public delegate void GhostActions(Ghost g);
    public GhostActions onCollisionWithPlayer;    
    private FSM fsm;
    public enum States
    {
        Idle = 0,
        ExitSpawn,
        Patrol,
        Chase,        
        Vulnerable,
        Dead,
        Count
    }
    public enum Events
    {
        OnActivation = 0,
        OnExited,
        OnSight,
        OffSight,
        OnBigDot,
        OffBigDot,
        OnDeath,
        OnRespawn,
        Count
    }

    public bool isVulnerable;

    public Vector2Int direction;

    public Behaviour myBehaviour;
    public List<PathmapTile> path = new List<PathmapTile>();
    public float idleTimer = 3f;
    public float changeDirectionTimer = 2f;
    public float vulnerableTimer = 5f;
    public float deadTimer = 10f;

    public Sprite normalSprite;
    public Sprite vulnerableSprite;
    private SpriteRenderer sr;

    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = normalSprite;

        idleTimer = UnityEngine.Random.Range(0, idleTimer);
        fsm = new FSM((int)States.Count, (int)Events.Count, (int)States.Idle);

        fsm.SetRelation((int)States.Idle, (int)Events.OnActivation, (int)States.ExitSpawn);
        fsm.SetRelation((int)States.ExitSpawn, (int)Events.OnExited, (int)States.Patrol);

        fsm.SetRelation((int)States.Patrol, (int)Events.OnBigDot, (int)States.Vulnerable);
        fsm.SetRelation((int)States.Patrol, (int)Events.OnSight, (int)States.Chase);

        fsm.SetRelation((int)States.Chase, (int)Events.OffSight, (int)States.Patrol);
        fsm.SetRelation((int)States.Chase, (int)Events.OnBigDot, (int)States.Vulnerable);

        fsm.SetRelation((int)States.Vulnerable, (int)Events.OffBigDot, (int)States.Patrol);
        fsm.SetRelation((int)States.Vulnerable, (int)Events.OnDeath, (int)States.Dead);

        fsm.SetRelation((int)States.Dead, (int)Events.OnRespawn, (int)States.Idle);


        isVulnerable = false;
        speed = 30.0f;
        direction = new Vector2Int(0, 1);
    }

    // Update is called once per frame
    public void OnUpdate(MapManager MapManager, PacMan avatar)
    {

        switch((States)fsm.GetState())
        {
            case States.Idle:
                Idle();
                break;
            case States.ExitSpawn:
                ExitSpawn();
                break;
            case States.Patrol:
                Patrol();
                break;
            case States.Chase:
                Chase();
                break;
            case States.Vulnerable:
                Vulnerable();
                break;
            case States.Dead:
                Dead();
                break;
        }
    }

    private void Idle()
    {
        if(timer >= idleTimer)
        {
            timer = 0;
            SetNextTile(MapManager.Get().ghostExitPos);
            fsm.SendEvent((int)Events.OnActivation);
        }
        else
            timer += Time.deltaTime;
    }
    private void ExitSpawn()
    {
        if(!IsAtDestination())
            MoveToDestination();
        else
            fsm.SendEvent((int)Events.OnExited);
    }

    private void Patrol()
    {
        MoveAroundTheMap();
        fsm.SendEvent((int)Events.OnSight);
    }
    void MoveAroundTheMap()
    {
        MovementDirection nextDirection = (MovementDirection)(UnityEngine.Random.Range(0, ((int)MovementDirection.DirectionCount)));
        Vector2Int newDirection = new Vector2Int();
        switch(nextDirection)
        {
            case MovementDirection.Up:
                newDirection.Set(0, 1);
                break;
            case MovementDirection.Down:
                newDirection.Set(0, -1);
                break;
            case MovementDirection.Left:
                newDirection.Set(-1, 0);
                break;
            case MovementDirection.Right:
                newDirection.Set(1, 0);
                break;
        }

        int nextTileX = GetCurrentTileX() + (int)newDirection.x;
        int nextTileY = GetCurrentTileY() + (int)newDirection.y;
        if(MapManager.Get().TileIsValid(nextTileX, nextTileY) && UnityEngine.Random.Range(0, 100) > 90)
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
        path.Clear();
        path = MapManager.Get().GetPath(currentTileX, currentTileY, GameManager2.Get().GetPlayer().currentTileX, GameManager2.Get().GetPlayer().currentTileY);
        MoveToDestination();
    }
    private void Chase()
    {
        if(timer >= changeDirectionTimer)
        {
            if(UnityEngine.Random.Range(0, 100) > 70)
            {
                path.Clear();
                path = MapManager.Get().GetPath(currentTileX, currentTileY, GameManager2.Get().GetPlayer().currentTileX, GameManager2.Get().GetPlayer().currentTileY);
            }
            timer = 0f;
        }
        else
            timer += Time.deltaTime;
        if(IsAtDestination())
        {
            if(path.Count > 0)
            {
                PathmapTile nextTile = path[0];
                path.RemoveAt(0);
                SetNextTile(new Vector2Int(nextTile.posX, nextTile.posY));
            }
            else
            {
                path.Clear();
                path = MapManager.Get().GetPath(currentTileX, currentTileY, GameManager2.Get().GetPlayer().currentTileX, GameManager2.Get().GetPlayer().currentTileY);
            }
        }
        MoveToDestination();
    }

    private void Vulnerable()
    {
        if(timer >= vulnerableTimer)
        {
            sr.sprite = normalSprite;
            timer = 0;
            isVulnerable = false;
            fsm.SendEvent((int)Events.OffBigDot);
        }
        else
            timer += Time.deltaTime;

        MoveAroundTheMap();
    }

    public void Dead()
    {
        if(timer >= deadTimer)
        {
            fsm.SendEvent((int)Events.OnRespawn);
        }
        else
            timer += Time.deltaTime;
    }
    public void SetVulnerable()
    {
        isVulnerable = true;
        timer = 0;
        sr.sprite = vulnerableSprite;
        fsm.SendEvent((int)Events.OnBigDot);        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(isVulnerable)
            {
                onCollisionWithPlayer(this);
                timer = 0;
                fsm.SendEvent((int)Events.OnDeath);
            }
        }
    }
    public void Reset()
    {        
        fsm.ResetState();        
    }
}
