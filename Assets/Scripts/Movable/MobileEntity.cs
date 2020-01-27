using UnityEngine;

public class MobileEntity : Entity
{
    public enum MovementDirection : int
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,
        DirectionCount = 4
    };

    public int currentTileX;
    public int currentTileY;
    public int nextTileX;
    public int nextTileY;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        nextTileX = currentTileX;
        nextTileY = currentTileY;
    }


    public bool IsAtDestination()
    {
        if (currentTileX == nextTileX && currentTileY == nextTileY)
            return true;
        return false;
    }

    public void SetNextTile(Vector2Int nextPos)
    {
        nextTileX = nextPos.x;
        nextTileY = nextPos.y;        
    }

    public int GetCurrentTileX()
    {
        return currentTileX;
    }

    public int GetCurrentTileY()
    {
        return currentTileY;
    }

    public void SetPosition(Vector2Int newPos)
    {
        transform.position = new Vector2(currentTileX * MapManager.Get().tileSize, -currentTileY * MapManager.Get().tileSize);
    }

    public void Respawn(Vector2 respawnLocation)
    {
        alive = true;
        SetPosition(respawnLocation * MapManager.Get().tileSize);
        currentTileX = (int)respawnLocation.x;
        currentTileY = (int)respawnLocation.y;
        nextTileX = currentTileX;
        nextTileY = currentTileY;
    }
    public void MoveToDestination()
    {
        Vector2 destination = new Vector2(nextTileX * MapManager.Get().tileSize, nextTileY * MapManager.Get().tileSize);
        if(destination == Vector2.zero)
            return;

        Vector2 destinationDirection = new Vector2(destination.x - transform.position.x, destination.y - transform.position.y);

        float distanceToMove = Time.deltaTime * speed;

        if(distanceToMove > destinationDirection.magnitude)
        {
            transform.position = destination;
            currentTileX = nextTileX;
            currentTileY = nextTileY;
        }
        else
        {
            destinationDirection.Normalize();
            SetPosition((Vector2)transform.position + destinationDirection * distanceToMove);
        }
    }
}