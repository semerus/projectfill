using UnityEngine;
using System.Collections;
using System.IO;
using LitJson;

public class ReadJson : MonoBehaviour {
    private string jsonString;
    private JsonData data;

    public Vector2[] vertices;
    public int lvl = 1;

    // Use this for initialization
    void Start () {
        jsonString = File.ReadAllText(Application.dataPath + "/Resources/Test.json");
        data = JsonMapper.ToObject(jsonString);
        vertices = ReceiveVertices(lvl);

        Debug.Log(vertices[1]+"8, -10");
    }

    Vector2[] ReceiveVertices(int level)
    {
        Vector2[] vertices = new Vector2[data["Shape"][level]["vertices"].Count / 2];
        for (int i = 0; i < vertices.Length; i++)
        {
            double x = (double)data["Shape"][level]["vertices"][2 * i];
            double y = (double)data["Shape"][level]["vertices"][2 * i + 1];
            vertices[i] = new Vector2((float)x, (float)y);
        }
        return vertices;
    }

}
