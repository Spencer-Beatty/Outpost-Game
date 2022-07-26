using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grids : MonoBehaviour
{

    public bool displayGridGizmos = true;
    public LayerMask unwalkable;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public TerrainType[] walkableRegions;
    public int objectProximityPenalty = 10;
    public int waypointMovementPenalty = 10;
    LayerMask walkableMask;
    Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>();
    Node[,] grid;

    float nodeDiameter;
    int gridSizex, gridSizey;

    int penaltyMin = int.MaxValue;
    int penaltyMax = int.MinValue;

    
    // Start is called before the first frame update
    void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizex = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizey = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        foreach (TerrainType region in walkableRegions)
        {
            walkableMask.value = walkableMask |=region.terrainMask.value;
            walkableRegionsDictionary.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);
        }

        CreateGrid();
    }
    public int MaxSize
    {
        get
        {
            return gridSizex * gridSizey;
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizex - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizey - 1)*percentY);

        return grid[x, y];

    }
    void BlurPenaltyMap(int blurSize)
    {
        int kernalSize = blurSize * 2 + 1;
        int kernalExtents = (kernalSize - 1) / 2;

        int[,] penaltiesHorizontalPass = new int[gridSizex, gridSizey];
        int[,] penaltiesVerticalPass = new int[gridSizex, gridSizey];

        for (int y = 0; y < gridSizey; y++)
        {
            for (int x = -kernalExtents; x <= kernalExtents; x++)
            {
                int SampleX = Mathf.Clamp(x, 0, kernalExtents);
                penaltiesHorizontalPass[0, y] += grid[SampleX, y].movementPenalty;
            }
            for (int x = 1; x < gridSizex; x++)
            {
                int RemoveIndex = Mathf.Clamp(x - kernalExtents - 1, 0, gridSizex);
                int addIndex = Mathf.Clamp(x + kernalExtents, 0, gridSizex - 1);

                penaltiesHorizontalPass[x, y] = penaltiesHorizontalPass[x - 1, y] - grid[RemoveIndex, y].movementPenalty + grid[addIndex, y].movementPenalty;
            }
        }
        for (int x = 0; x < gridSizex; x++)
        {
            for (int y = -kernalExtents; y <= kernalExtents; y++)
            {
                int SampleY = Mathf.Clamp(y, 0, kernalExtents);
                penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, SampleY];


            }

            int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, 0] / (kernalSize * kernalSize));
            grid[x, 0].movementPenalty = blurredPenalty;
            for (int y = 1; y < gridSizey; y++)
            {
                {
                    int RemoveIndex = Mathf.Clamp(y - kernalExtents - 1, 0, gridSizex);
                    int addIndex = Mathf.Clamp(y + kernalExtents, 0, gridSizey - 1);

                    penaltiesVerticalPass[x, y] = penaltiesVerticalPass[x, y - 1] - penaltiesHorizontalPass[x, RemoveIndex] + penaltiesHorizontalPass[x, addIndex];
                    blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, y] / (kernalSize * kernalSize));
                    grid[x, y].movementPenalty = blurredPenalty;

                    if (blurredPenalty > penaltyMax)
                    {
                        penaltyMax = blurredPenalty;
                    }
                    if(blurredPenalty< penaltyMin)
                    {
                        penaltyMin = blurredPenalty;
                    }
                }
            }
        }

    }
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        for(int i = -1; i <=1; i++)
        {
            for(int j = -1; j <=1; j++)
            {
                if (i == 0 && j == 0)
                    continue;
                int checkX = node.gridX + i;
                int checkY = node.gridY + j;
                if (checkX >= 0 && checkX < gridSizex && checkY >= 0 && checkY < gridSizey)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
                
            }
        }
        return neighbours;
    }
    

    
    void CreateGrid()
    {
        grid = new Node[gridSizex, gridSizey];
        Vector3 worldBottemLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        for (int x = 0; x<gridSizex; x++)
        {
            for (int y = 0; y < gridSizey; y++)
            {
                Vector3 worldPoint = worldBottemLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkable));
                int movementPenaly = 0;

                //raycast
                
                    Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 100, walkableMask)) {
                        walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenaly);
                    }
                if (!walkable) {
                    movementPenaly += objectProximityPenalty;
                        }
                
                grid[x, y] = new Node(walkable, worldPoint, x, y, movementPenaly);
            }
        }
        BlurPenaltyMap(3);
    }
    
     void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if (grid != null && displayGridGizmos) 
        {
            
            foreach(Node n in grid)
            {
                Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(penaltyMin, penaltyMax, n.movementPenalty));
                Gizmos.color = (n.walkable) ? Gizmos.color : Color.red;

                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}
[System.Serializable]
public class TerrainType
{
    public LayerMask terrainMask;
    public int terrainPenalty;
}