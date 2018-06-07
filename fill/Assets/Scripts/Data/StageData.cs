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

    [JsonProperty]
    public List<List<Vector2>> InnerGroups { get; private set; }

    [JsonIgnore]
    public SimplePolygon2D OuterPolygon { get; private set; }

    [JsonIgnore]
    public List<SimplePolygon2D> InnerPolygons { get; private set; }
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
	public StageData(string name, int id, List<Vector2> outerVertices, List<List<Vector2>> innerGroups, Color lineColor)
	{
		Name = name;
		Id = id;
		OuterVertices = outerVertices;
        InnerGroups = innerGroups;
        OuterPolygon = SimplePolygon2D.Create(GetCompleteVertices(outerVertices).ToArray());
        if(InnerGroups != null)
        {
            InnerPolygons = new List<SimplePolygon2D>();
            foreach (var inner in innerGroups)
            {
                InnerPolygons.Add(SimplePolygon2D.Create(GetCompleteVertices(inner).ToArray()));
            }
        }
        else
        {
            InnerGroups = new List<List<Vector2>>();
        }
    }

    public StageData()
    {
        Name = "NewMap";
        Id = 1;
        OuterVertices = new List<Vector2>() { new Vector2(-1f,-1f), new Vector2(-1f, 1f), new Vector2(1f, 1f), new Vector2(1f, -1f) };
        OuterPolygon = SimplePolygon2D.Create(GetCompleteVertices(OuterVertices).ToArray());
    }

    public static List<Vector2> GetCompleteVertices(List<Vector2> vectors)
    {
        if(vectors.Count < 1) { return vectors; }
        var complete = new List<Vector2>();
        complete.AddRange(vectors);
        complete.Add(vectors[0]);

        return complete;
    }

    public List<Edge> GetEdges()
    {
        List<Edge> edges = OuterPolygon.GetEdges();
        foreach (var inner in InnerPolygons)
        {
            edges.AddRange(inner.GetEdges());
        }
                
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
        {
            return false;
        }

        foreach (var inner in InnerPolygons)
        {
            if(inner.IsInsidePolygon(vec3))
            {
                return false;
            }
        }        

        return true;
    }
}
