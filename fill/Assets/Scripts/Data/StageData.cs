using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class StageData 
{
	[JsonProperty]
	public string Name { get; private set; }

	[JsonProperty]
	public int Id { get; private set; }

	[JsonProperty]
	public List<Vector2> OuterVectices { get; private set; }

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
		OuterVectices = outerVertices;
		//LineColor = Color.blue;
	}

    public StageData()
    {
        Name = "NewMap";
        Id = 1;
        OuterVectices = new List<Vector2>();
        //LineColor = Color.black;
    }
}
