using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshGenerator : MonoBehaviour
{
    // Start is called before the first frame update


    private Vector3[] normals;
    private int[] triangles;
    private Vector3[] vertices;

    private Mesh mesh;
    private int squaresLength;
    private int squaresWidth;
    
    
    private Vector3 topLeftCoord;
    private Vector3[] anchorPoints;
    private int meshcount = 0;

    private float bias = 0.0000001f;
    
    public void SetMeshGenerator(int pSquaresWidth, int pSquaresLength, Vector3[] pAnchorPoints)
    {
        squaresWidth = Mathf.Clamp(pSquaresWidth, 1, 180);
        squaresLength = Mathf.Clamp(pSquaresLength, 1, 180);
        anchorPoints = pAnchorPoints;
        //Set Arrays
        triangles = new int[(squaresWidth * squaresLength) * 6];
        vertices = new Vector3[(squaresLength + 1) * (squaresWidth + 1)];
        normals = new Vector3[(squaresLength + 1) * (squaresWidth + 1)];
    }

   

    /* private void OnEnable()
     {
         mesh = new Mesh
         {
             name = "procedural mesh"
         };


     }*/

    public Mesh GenerateMesh(Vector3 pTopLeftCoord)
    {
        topLeftCoord = pTopLeftCoord;
        mesh = new Mesh
        {
            name = "mesh_" + meshcount
        };
        meshcount++;
        mesh.vertices = CreateVertices(squaresWidth, squaresLength);
        mesh.triangles = CreateTriangles(squaresWidth, squaresLength);
        mesh.normals = CreateNormals(squaresWidth, squaresLength);
        mesh.uv = CreateUVs(squaresWidth, squaresLength);
        mesh.tangents = CreateTangents(squaresWidth, squaresLength);
        
        return mesh;
    }

    // Update is called once per frame


    Vector3[] CreateVertices(int width, int length)
    {
        // width and length refer to how many squares so there is an extra row and colunm
        
        for (int j = 0; j < (length+1); j+=1)
        {
            for (int i = 0; i < (width+1); i+=1)
            {
                float vertJ = topLeftCoord.x + j ;
                float vertI = topLeftCoord.z + i ;
                vertices[j * (width+1) + i] = new Vector3(vertJ, CalculateHeight4(vertJ, vertI), vertI);
            }
        }
        return vertices;

    }

    /*
    float CalculateHeight3(float coordX, float coordZ)
    {
        float height=0;
        float scale = 20f;
        for(int i = 0; i < anchorPoints.Length; i++)
        {
            float distanceXZ = (Mathf.Abs(coordX - anchorPoints[i].x) + Mathf.Abs(coordZ - anchorPoints[i].x) + 1);
            height += Mathf.Clamp((1 / distanceXZ) *scale,0f,1f) * anchorPoints[i].y;
            height += Mathf.Clamp((1 / distanceXZ) *scale,0f,1f) * anchorPoints[i].y;
        }
        return height;
    }*/

    float CalculateHeight4(float coordX, float coordZ)
    {
        float[] distanceArray = new float[anchorPoints.Length];
        float distanceSum = 0f;
        float vertexHeight = 0f;
        for(int i = 0; i< anchorPoints.Length; i++)
        {
            float anchorDistance = getDistance(coordX, coordZ, anchorPoints[i]);
            distanceArray[i] = anchorDistance * anchorPoints[i].y;
            distanceSum += anchorDistance;
        }

        for( int i = 0; i < distanceArray.Length; i++)
        {
            vertexHeight += (distanceArray[i] / distanceSum) ;
        }
        return vertexHeight;
        
    }

    private float getDistance(float coordX, float coordZ, Vector3 anchor)
    {
        float xDist;
        float zDist;
        if(coordX > anchor.x)
        {
            xDist = (coordX - anchor.x);
        }
        else
        {
           xDist = (anchor.x - coordX);
        }
        if(coordZ > anchor.z)
        {
            zDist = (coordZ - anchor.z);
        }
        else
        {
            zDist = (anchor.z - coordZ);
        }
        // TO do manhattan distance which would speed up change to 
        // return xDist + zDist + bias;
        return 1/((xDist * xDist + zDist * zDist)+ bias);
    }
  

    int[] CreateTriangles(int width, int length)
    {
        
        int counter = 0;
        for (int j = 0; j < length; j++)
        {
            for (int i = 0; i < width; i++)
            {
                //first triangle, top left, bottom right, bottom left
                triangles[counter] = i + j * (width+1);
                triangles[counter + 1] = i + 1 + (j + 1) * (width+1);
                triangles[counter + 2] = i + (j + 1) * (width + 1);
                //second triangle, top left, top right, bottom right
                triangles[counter + 3] = i + j * (width + 1);
                triangles[counter + 4] = (i + 1) + j * (width + 1);
                triangles[counter + 5] = i + 1 + (j + 1) * (width + 1);
                //update counter
                counter += 6;
            }
        }
        return triangles;
    }
    Vector3[] CreateNormals(int width, int length)
    {
        
        for (int i = 0; i < (width*length)*2; i++)
        {
            int normalTriangleIndex = i * 3;
            int vertexIndexA = triangles[normalTriangleIndex];
            int vertexIndexB = triangles[normalTriangleIndex + 1];
            int vertexIndexC = triangles[normalTriangleIndex + 2];

            Vector3 triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
            normals[vertexIndexA] += triangleNormal;
            normals[vertexIndexB] += triangleNormal;
            normals[vertexIndexC] += triangleNormal;
        }


        for(int i = 0; i < normals.Length; i++)
        {
            normals[i].Normalize();
        }
        return normals;
    }
    Vector3 SurfaceNormalFromIndices(int indexA, int indexB, int indexC)
    {
        Vector3 pointA = vertices[indexA];
        Vector3 pointB = vertices[indexB];
        Vector3 pointC = vertices[indexC];

        Vector3 sideAB = pointB - pointA;
        Vector3 sideAC = pointC - pointA;
        return Vector3.Cross(sideAB, sideAC).normalized;
    }

    Vector2[] CreateUVs(int width, int length)
    {
        
        Vector2[] uvs = new Vector2[(width+1) * (length+1)];
        float fwidth = 1/(float)(width);
        float fheight = 1/(float)(length);
        
        for(int j = 0; j<length+1; j++)
        {
            
            for(int i = 0; i < width+1; i++)
            {
                
                uvs[i + j * (width+1)]= new Vector2(i * fwidth,  1f-j*fheight);
                
                
            }
        }
        
        return uvs;
    }

    Vector4[] CreateTangents(int width, int length)
    {
        Vector4[] tangents = new Vector4[(length + 1) * (width + 1)];
        for(int i =0; i< tangents.Length;i++)
        {
            tangents[i] = new Vector4(1, 0, 0, 1);
        }
        return tangents;
    }

    
}
