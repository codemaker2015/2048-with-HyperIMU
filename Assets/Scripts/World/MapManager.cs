using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapManager :MonoBehaviourSingleton<MapManager>
{
    protected override void Awake()
    {
        base.Awake();
        InitPathmap();
        cherry = GameObject.Instantiate(cherryPrefab).GetComponent<Cherry>();
        cherry.gameObject.SetActive(false);
        cherry.OnTimerRunOut += DisableCherry;
    }
    public int tileSize = 22;

    public GameObject smallDotPrefab;
    public GameObject powerDotPrefab;
    public GameObject cherryPrefab;

    public int dotCount = 0;

    public List<SmallDot> smallDots = new List<SmallDot>();
    public List<PowerDot> powerDots = new List<PowerDot>();
    public List<PathmapTile> tiles = new List<PathmapTile>();

    public Cherry cherry;    
    private bool canSpawnCherry = false;
    private float timer = 0f;
    public float cherryTimer = 10f;

    public GameObject smallDotGroup;
    public GameObject powerDotGroup;

    public Vector2 playerStartPos;
    public Vector2 ghostStartPos;
    public Vector2Int ghostExitPos;

    // Update is called once per frame
    void Update()
    {
        SpawnCherry();
    }

    public bool InitPathmap()
    {
        string[] lines = System.IO.File.ReadAllLines("Assets/Data/Map.txt");
        for(int y = 0; y < lines.Length; y++)
        {
            char[] line = lines[y].ToCharArray();
            for(int x = 0; x < line.Length; x++)
            {
                PathmapTile tile = new PathmapTile();
                tile.posX = x;
                tile.posY = -y;
                tiles.Add(tile);
                switch(line[x])
                {
                    case 'x':
                        tile.blocking = true;
                        break;
                    case 'p':
                        playerStartPos = new Vector2(tile.posX, tile.posY);
                        break;
                    case 'g':
                        ghostStartPos = new Vector2(tile.posX, tile.posY);
                        break;
                    case 'G':
                        ghostExitPos = new Vector2Int(tile.posX, tile.posY);
                        break;
                    case '.':
                        SmallDot smallDot = GameObject.Instantiate(smallDotPrefab).GetComponent<SmallDot>();
                        smallDot.transform.SetParent(smallDotGroup.transform);
                        smallDot.SetPosition(new Vector2(x * MapManager.Get().tileSize, -y * MapManager.Get().tileSize));
                        smallDot.OnCollected += OnItemColleted;
                        smallDots.Add(smallDot);
                        dotCount++;
                        break;
                    case 'o':
                        PowerDot powerDot = GameObject.Instantiate(powerDotPrefab).GetComponent<PowerDot>();
                        powerDot.transform.SetParent(powerDotGroup.transform);
                        powerDot.SetPosition(new Vector2(x * MapManager.Get().tileSize, -y * MapManager.Get().tileSize));
                        powerDot.OnCollected += OnItemColleted;
                        powerDots.Add(powerDot);
                        dotCount++;
                        break;
                }
            }
        }
        canSpawnCherry = true;
        return true;
    }


    internal bool TileIsValid(int tileX, int tileY)
    {
        for(int t = 0; t < tiles.Count; t++)
        {
            if(tileX == tiles[t].posX && tileY == tiles[t].posY && !tiles[t].blocking)
                return true;
        }
        return false;
    }

    public List<PathmapTile> GetPath(int currentTileX, int currentTileY, int targetX, int targetY)
    {
        PathmapTile fromTile = GetTile(currentTileX, currentTileY);
        PathmapTile toTile = GetTile(targetX, targetY);

        for(int t = 0; t < tiles.Count; t++)
        {
            tiles[t].visited = false;
        }

        List<PathmapTile> path = new List<PathmapTile>();
        if(Pathfind(fromTile, toTile, path))
        {
            return path;
        }
        return null;
    }

    private bool Pathfind(PathmapTile fromTile, PathmapTile toTile, List<PathmapTile> path)
    {
        fromTile.visited = true;

        if(fromTile.blocking)
            return false;
        path.Add(fromTile);
        if(fromTile == toTile)
            return true;

        List<PathmapTile> neighbours = new List<PathmapTile>();

        PathmapTile up = GetTile(fromTile.posX, fromTile.posY - 1);
        if(up != null && !up.visited && !up.blocking && !path.Contains(up))
            neighbours.Insert(0, up);

        PathmapTile down = GetTile(fromTile.posX, fromTile.posY + 1);
        if(down != null && !down.visited && !down.blocking && !path.Contains(down))
            neighbours.Insert(0, down);

        PathmapTile right = GetTile(fromTile.posX + 1, fromTile.posY);
        if(right != null && !right.visited && !right.blocking && !path.Contains(right))
            neighbours.Insert(0, right);

        PathmapTile left = GetTile(fromTile.posX - 1, fromTile.posY);
        if(left != null && !left.visited && !left.blocking && !path.Contains(left))
            neighbours.Insert(0, left);

        for(int n = 0; n < neighbours.Count; n++)
        {
            PathmapTile tile = neighbours[n];

            path.Add(tile);

            if(Pathfind(tile, toTile, path))
                return true;

            path.Remove(tile);
        }

        return false;
    }

    public PathmapTile GetTile(int tileX, int tileY)
    {
        for(int t = 0; t < tiles.Count; t++)
        {
            if(tileX == tiles[t].posX && tileY == tiles[t].posY)
                return tiles[t];
        }

        return null;
    }

    public void OnItemColleted(Item item)
    {
        if(item.tag == "SmallDot")
        {
            smallDots.Remove((SmallDot)item);
            GameManager2.Get().UpdateScore(item.points);
            dotCount--;
        }
        else if(item.tag == "PowerDot")
        {
            powerDots.Remove((PowerDot)item);
            EnemyManager.Get().SetEnemiesVulnerables();
            GameManager2.Get().UpdateScore(item.points);
            dotCount--;
        }
        else if(item.tag == "Cherry")
        {
            GameManager2.Get().UpdateScore(item.points);
            DisableCherry(item);
        }
        item.gameObject.SetActive(false);

        if(dotCount == 0)
        {
            GameManager2.Get().Win();
        }
    }
    private void DisableCherry(Item t)
    {
        cherry.gameObject.SetActive(false);
        canSpawnCherry = true;
    }
    private void SpawnCherry()
    {
        if(canSpawnCherry)
        {
            if(timer >= cherryTimer)
            {
                PathmapTile tile = tiles[UnityEngine.Random.Range(0, tiles.Count)];
                if(!tile.blocking)
                {
                    cherry.gameObject.SetActive(true);
                    cherry.SetPosition(new Vector2(tile.posX * MapManager.Get().tileSize, tile.posY * MapManager.Get().tileSize));
                    timer = 0f;
                    canSpawnCherry = false;
                }
            }
            else
                timer += Time.deltaTime;
        }
    }

    public void ResetMap()
    {
        foreach(SmallDot dot in smallDots)
            dot.gameObject.SetActive(true);
        foreach(PowerDot dot in powerDots)
            dot.gameObject.SetActive(true);
    }
}

