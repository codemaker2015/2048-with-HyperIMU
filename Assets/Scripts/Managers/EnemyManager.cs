using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviourSingleton<EnemyManager>
{
    public GameObject ghostPrefab;
    public List<Ghost> ghosts;
    public Sprite[] ghostSprites;
    public float myGhostGhostCounter;
    public int maxGhost;
    // Start is called before the first frame update
    void Start()
    {
        maxGhost = ghostSprites.Length;
        ghosts = new List<Ghost>();
        for(int g = 0; g < maxGhost; g++)
        {
            ghosts.Add(GameObject.Instantiate(ghostPrefab).GetComponent<Ghost>());
            ghosts[g].Respawn(MapManager.Get().ghostStartPos);
            ghosts[g].transform.SetParent(this.transform);
            ghosts[g].normalSprite = ghostSprites[g];
            ghosts[g].onCollisionWithPlayer += GhostDestroyed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int g = 0; g < ghosts.Count; g++)
        {
            ghosts[g].OnUpdate(MapManager.Get(), GameManager2.Get().GetPlayer());
        }
    }
    void GhostDestroyed(Ghost g)
    {
         GameManager2.Get().GhostDestroyed();
    }
    public void SetEnemiesVulnerables()
    {
        for(int g = 0; g < ghosts.Count; g++)
        {
            ghosts[g].isVulnerable = true;
            ghosts[g].SetVulnerable();
        }
    }
    public void ResetGhosts()
    {
        for(int g = 0; g < maxGhost; g++)
        {            
            ghosts[g].Respawn(MapManager.Get().ghostStartPos);
            ghosts[g].Reset();
        }
    }
}
