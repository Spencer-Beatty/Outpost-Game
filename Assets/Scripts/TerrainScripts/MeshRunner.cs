using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRunner : MonoBehaviour
{
    public TextureData textureData;
    public TerrainData terrainData;
    // Start is called before the first frame update
    public GameObject meshHolderParent;
    public Material[] ground;
    [SerializeField]
    private bool generateMesh = false;
    
    
    private int meshCount = 0;

    private MeshGenerator meshGenerator;
    
   

    private Anchor[] anchorPoints;
    public int numberOfAnchorPoints = 20;
    private float anchorMin;
    private float anchorMax;
    //private List<MeshGenerator> 

    private void Update()
    {
        if(generateMesh)
        {
            generateMesh = false;
            ReGenerateMesh();
        }
        
    }

    private Vector3[] AnchorPositions()
    {
        Vector3[] anchorPositions = new Vector3[anchorPoints.Length];
        int counter = 0;
        foreach(Anchor e in anchorPoints)
        {
            anchorPositions[counter] = e.position;
            counter++;
        }
        return anchorPositions;
    }
    void ReGenerateMesh()
    {
        //clear the old
       for (int i = 0; i< meshHolderParent.transform.childCount; i++)
        {
            GameObject.Destroy( meshHolderParent.transform.GetChild(i).gameObject);

        }
        


        anchorPoints = GenerateRandomAnchorPoints(numberOfAnchorPoints);
        SetAnchorExtremes();
        GetComponent<MeshGenerator>().SetMeshGenerator(terrainData.xLengthMesh, terrainData.zLengthMesh, AnchorPositions());
        meshGenerator = GetComponent<MeshGenerator>();
        textureData.ApplyToMaterial(ground[0]);
        FillGround(terrainData.xLengthMap, terrainData.zLengthMap, Vector3.zero);
      
        textureData.UpdateMeshHeights(ground[0], anchorMin, anchorMax);
    }
    void Start()
    {
        anchorPoints = GenerateRandomAnchorPoints(numberOfAnchorPoints);
        SetAnchorExtremes();
        GetComponent<MeshGenerator>().SetMeshGenerator(terrainData.xLengthMesh, terrainData.zLengthMesh, AnchorPositions());
        meshGenerator = GetComponent<MeshGenerator>();
        textureData.ApplyToMaterial(ground[0]);
        FillGround(terrainData.xLengthMap,terrainData.zLengthMap, Vector3.zero);
        
        
        textureData.UpdateMeshHeights(ground[0], anchorMin, anchorMax);

    }

    private void SetAnchorExtremes()
    {
        anchorMax = 0;
        anchorMin = 0;
        for (int i = 0; i < anchorPoints.Length; i++)
        {
            if (anchorPoints[i].GetHeight() > anchorMax)
            {
                anchorMax = anchorPoints[i].GetHeight();
            }
            else if (anchorPoints[i].GetHeight() < anchorMin)
            {
                anchorMin = anchorPoints[i].GetHeight();
            }
        }
    }

    GameObject GenerateMeshHolder()
    {
        GameObject meshHolder = new GameObject("meshHolder_" + meshCount);
        meshHolder.AddComponent<MeshFilter>();
        meshHolder.AddComponent<MeshRenderer>();
        meshCount++;
        return meshHolder;
    }

    Anchor[] GenerateRandomAnchorPoints(int numberOfAnchors)
    {
        Anchor[] anchors = new Anchor[numberOfAnchors];
        //X walls
        for(int i = 0; i< numberOfAnchors/2; i++)
        {
            float xCoord = Random.Range(-terrainData.xLengthMap, terrainData.xLengthMap);
            float zCoord = Random.Range(-terrainData.zLengthMap, terrainData.zLengthMap);
            float yCoord = Random.Range(terrainData.minHeight, terrainData.maxHeight);
            anchors[i] = new Anchor(new Vector3(xCoord, yCoord, zCoord),1f);
        }
        //Z walls
        for(int i = numberOfAnchors/2; i< numberOfAnchors; i++)
        {
            float xCoord = Random.Range(-terrainData.xLengthMap, terrainData.xLengthMap);
            float zCoord = Random.Range(-terrainData.zLengthMap, terrainData.zLengthMap);
            float yCoord = Random.Range(terrainData.minHeight, terrainData.maxHeight);
            anchors[i] = new Anchor(new Vector3(xCoord, yCoord, zCoord), 1f);
        }
        return anchors;
    }
    private void FillGround(int widthOfArea, int lengthOfArea, Vector3 startPosition)
    {
        for (float j = -lengthOfArea; j < lengthOfArea; j+=terrainData.xLengthMesh)
        {
            for (float i =-widthOfArea; i < widthOfArea; i+=terrainData.zLengthMesh) 
            { 
            
            
            GameObject a = GenerateMeshHolder();
            Vector3 meshPosition = new Vector3(startPosition.x + j,startPosition.y,startPosition.x+i);
            a.GetComponent<MeshFilter>().mesh = meshGenerator.GenerateMesh(meshPosition);
                a.GetComponent<MeshRenderer>().material = ground[0];
                
                

                a.transform.SetParent(meshHolderParent.transform);
            }
        }
    }

    public struct Anchor
    {
        public Vector3 position;
        public float strength;
        public Anchor(Vector3 pPosition, float pStrength)
        {
            position = pPosition;
            strength = pStrength;
        }
        public float GetHeight()
        {
            return position.y;
        }
        
    }

    private void OnDrawGizmos()
    {
        if (anchorPoints != null)
        {
            
            for (int i = 0; i < anchorPoints.Length; i++)
            {
                Gizmos.color = Color.black;
                if (i % 2 == 0)
                {
                    Gizmos.color = Color.red;
                }
                if (i == 1 || i == 0)
                {
                    Gizmos.color = Color.blue;
                }
                Gizmos.DrawCube(AnchorPositions()[i], new Vector3(0.2f, 30f, 0.2f));

            }

        }
    }
}
