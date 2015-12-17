using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public class ProceduralGeometry  
{
    public static void AddToObjectCircleMeshAndCollider(GameObject go, float radius, int segments)
    {
        MeshFilter circleMesh = go.AddComponent<MeshFilter>();
        circleMesh.sharedMesh = ProceduralGeometry.CreateCircleMesh(radius, segments);
        CircleCollider2D collider = go.GetComponent<CircleCollider2D>();

        if (collider == null)
        {
            collider = go.AddComponent<CircleCollider2D>();
        }
        collider.radius = radius;
    }

    public static void AddToObjectCircleMesh(GameObject go, float radius, int segments, Vector3 offset)
    {
        MeshFilter circleMesh = (MeshFilter)go.AddComponent<MeshFilter>();
        circleMesh.sharedMesh = ProceduralGeometry.CreateCircleMesh(radius, segments);
        circleMesh.transform.position = offset;
    }

	static public Mesh CreateCircleMesh(float radius = 1f, int segments = 3) 
    {
        Assert.IsTrue(radius > 0f && segments > 2, "You must provide valid radius and number of segments (>2)");
		int numVerts = segments + 1;
		
		Mesh plane = new Mesh();
		Vector3[] verts = new Vector3[numVerts];
		Vector2[] uvs = new Vector2[numVerts];
		int[] tris = new int[(numVerts * 3)];
		
		// The first vert is in the center of the triangle
		verts[0] = Vector3.zero;
		uvs[0] = new Vector2(0.5f, 0.5f);
		
		float angle = 360.0f / (float)(numVerts - 1);
		
		for (int i = 1; i < numVerts; ++i)
        {
			verts[i] = Quaternion.AngleAxis(angle * (float)(i - 1), Vector3.back) *( Vector3.up * radius);
			
			float normedHorizontal = 0.5f + verts[i].x  / (2f * radius);
            float normedVertical = 0.5f + verts[i].y  / (2f * radius);
            uvs[i] = new Vector2(normedHorizontal, normedVertical);
            //Debug.Log(uvs[i].ToString());
		}


		for (int i = 0; i + 2 < numVerts; ++i) 
        {
			int index = i * 3;
			tris[index + 0] = 0;
			tris[index + 1] = i + 1;
			tris[index + 2] = i + 2;
		}
		
		// The last triangle has to wrap around to the first vert so we do this last and outside the lop
		var lastTriangleIndex = tris.Length - 3;
		tris[lastTriangleIndex + 0] = 0;
		tris[lastTriangleIndex + 1] = numVerts - 1;
		tris[lastTriangleIndex + 2] = 1;
		
		plane.vertices = verts;
		plane.triangles = tris;	
		plane.uv = uvs;
		
		plane.RecalculateNormals();
		
		return plane;
	}
}
