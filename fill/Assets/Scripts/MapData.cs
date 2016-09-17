using System;
using UnityEngine;
using System.Collections.Generic;


public class MapData
{
	private string name;
	private int id;
	private SimplePolygon2D outer;
	private SimplePolygon2D[] holes;
	private Color lineColor;
	private Color backgroundColor;
	private Color guardBasicColor;
	private Color guardSelectedColor;
	private Color vgColor;
	private float minX, minY, maxX, maxY;

	/*Constructor*/
	public MapData (SimplePolygon2D outer, SimplePolygon2D[] holes)
	{
		this.outer = outer;
		this.holes = holes;

		minX = float.MaxValue;
		minY = float.MaxValue;
		maxX = float.MinValue;
		maxY = float.MinValue;

		Vector2[] outerVertices = outer.getVertices ();
		for(int i = 0; i < outerVertices.Length; i++){
			if (outerVertices [i].x < minX)
				minX = outerVertices [i].x;
			if (outerVertices [i].y < minY)
				minY = outerVertices [i].y;
			if (outerVertices [i].x > maxX)
				maxX = outerVertices [i].x;
			if (outerVertices [i].y > maxY)
				maxY = outerVertices [i].y;
		}
			
	}

	public MapData (SimplePolygon2D outer, SimplePolygon2D[] holes, string name, int id, Color lineColor,
		Color backgroundColor, Color guardBasicColor, Color guardSelectedColor, Color vgColor)
	{
		this.outer = outer;
		this.holes = holes;
		this.name = name;
		this.id = id;
		this.lineColor = lineColor;
		this.backgroundColor = backgroundColor;
		this.guardBasicColor = guardBasicColor;
		this.guardSelectedColor = guardSelectedColor;
		this.vgColor = vgColor;
	}

	/*Functions*/
	public int getSize(){
		int N = 0;
		N += outer.getVertices().Length;
		for (int i = 0; i < holes.Length; i++) {
			N += holes [i].getVertices ().Length;
		}

		return N;
	}

	public float getMinX(){
		return minX;
	}

	public float getMinY(){
		return minY;
	}

	public float getMaxX(){
		return maxX;
	}

	public float getMaxY(){
		return maxY;
	}

	/*Getters*/
	public SimplePolygon2D getOuter(){
		return outer;
	}

	public SimplePolygon2D[] getHoles(){
		return holes;
	}

	public string getName() {
		return name;
	}

	public int getId() {
		return id;
	}

	public Color getLineColor() {
		return lineColor;
	}

	public Color getBackgroundColor() {
		return backgroundColor;
	}

	public Color getGuardBasicColor() {
		return guardBasicColor;
	}

	public Color getGuardSeletedColor() {
		return guardSelectedColor;
	}

	public Color getVgColor() {
		return vgColor;
	}

	public List<Edge> getTotalEdges(){
		List<Edge> toRet = outer.getEdges ();
		for (int i = 0; i < holes.Length; i++) {
			toRet.AddRange (holes [i].getEdges ());
		}
		return toRet;
	}
}
