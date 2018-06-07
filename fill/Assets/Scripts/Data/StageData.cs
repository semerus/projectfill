using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using FillClient;

public class StageData 
{
	[JsonProperty]
	public string Name { get; private set; }

	[JsonProperty]
	public int Id { get; private set; }

	[JsonProperty]
	public List<Vector2> OuterVertices { get; private set; }

    [JsonIgnore]
    public SimplePolygon2D OuterPolygon { get; private set; }
	//[JsonProperty]
	//public Color LineColor { get; private set; }

//	[JsonProperty]
//	public Color BackgroundColor { get; private set; }
//
//	[JsonProperty]
//	public Color GuardBasicColor { get; private set; }

	/*
	private int id;
	private SimplePolygon2D outer;
	private SimplePolygon2D[] holes;
	private Color lineColor;
	private Color backgroundColor;
	private Color guardBasicColor;
	private Color guardSelectedColor;
	private Color vgColor;
	private float minX, minY, maxX, maxY;
	*/
    
    [JsonConstructor]
	public StageData(string name, int id, List<Vector2> outerVertices, Color lineColor)
	{
		Name = name;
		Id = id;
		OuterVertices = outerVertices;
        OuterPolygon = SimplePolygon2D.Create(this.GetCompleteVertices().ToArray());
    }

    public StageData()
    {
        Name = "NewMap";
        Id = 1;
        OuterVertices = new List<Vector2>() { new Vector2(-1f,-1f), new Vector2(-1f, 1f), new Vector2(1f, 1f), new Vector2(1f, -1f) };
        OuterPolygon = SimplePolygon2D.Create(this.GetCompleteVertices().ToArray());
    }

    public List<Vector2> GetCompleteVertices()
    {
        if(OuterVertices.Count < 1) { return OuterVertices; }
        var complete = new List<Vector2>();
        complete.AddRange(OuterVertices);
        complete.Add(OuterVertices[0]);

        return complete;
    }

    public List<Edge> GetEdges()
    {
        List<Edge> edges = OuterPolygon.GetEdges();
        //for (int i = 0; i < holes.Length; i++)
        //{
        //    toRet.AddRange(holes[i].GetEdges());
        //}
        return edges;
    }

    public Vector2[] GetExtremePoints()
    {
        Vector2[] extreme = new Vector2[4];

        extreme[Numbers.NORTH] = new Vector2(0, float.MinValue);
        extreme[Numbers.SOUTH] = new Vector2(0, float.MaxValue);
        extreme[Numbers.EAST] = new Vector2(float.MinValue, 0);
        extreme[Numbers.WEST] = new Vector2(float.MaxValue, 0);

        Vector2[] vertices = OuterPolygon.getVertices();
        for (int i = 0; i < vertices.Length; i++)
        {
            if (extreme[Numbers.NORTH].y < vertices[i].y)
                extreme[Numbers.NORTH] = vertices[i];

            if (extreme[Numbers.SOUTH].y > vertices[i].y)
                extreme[Numbers.SOUTH] = vertices[i];

            if (extreme[Numbers.EAST].x < vertices[i].x)
                extreme[Numbers.EAST] = vertices[i];

            if (extreme[Numbers.WEST].x > vertices[i].x)
                extreme[Numbers.WEST] = vertices[i];
        }

        return extreme;
    }

    public bool IsInsideMap(float x, float y)
    {
        Vector3 vec3 = new Vector3(x, y, 0);
        if (!OuterPolygon.IsInsidePolygon(vec3))
            return false;

        //for (int i = 0; i < holes.Length; i++)
        //{
        //    if (holes[i].IsInsidePolygon(vec3))
        //        return false;
        //}

        return true;
    }
}
