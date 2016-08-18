using UnityEngine;

public class PolygonTester : MonoBehaviour {
	
	void Start () {
		GameObject gameObject1 = new GameObject ("Test1");
		Vector2[] vertices2D = new Vector2[] {
			new Vector2(0,0),
			new Vector2(0,50),
			new Vector2(25, 25)
		};

		doit (vertices2D, gameObject1);

		GameObject gameObject2 = new GameObject ("Test2");
		Vector2[] vertices2D_2 = new Vector2[] {
			new Vector2(50,50),
			new Vector2(50,100),
			new Vector2(0,100)
		};

		doit (vertices2D_2, gameObject2);
	}

	void doit(Vector2[] vertices2D, GameObject m_gameObject)
	{
		Debug.Log("doit(" + vertices2D + ")");

		m_gameObject.AddComponent<MeshRenderer>();
		MeshFilter filter = m_gameObject.AddComponent<MeshFilter>() as MeshFilter;

		// Use the triangulator to get indices for creating triangles
		Triangulator tr = new Triangulator(vertices2D);
		int[] indices = tr.Triangulate();

		// Create the Vector3 vertices
		Vector3[] vertices = new Vector3[vertices2D.Length];
		for (int i=0; i<vertices.Length; i++) {
			vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
		}

		// Create the mesh
		Mesh msh = new Mesh();
		msh.vertices = vertices;
		msh.triangles = indices;
		msh.RecalculateNormals();
		msh.RecalculateBounds();

		// Set up game object with mesh;
		filter.mesh = msh;
	}
}