using System;
using UnityEngine;
using System.Collections.Generic;

public class MapData
{
	private SimplePolygon2D outer;
	private SimplePolygon2D[] holes;
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

	public List<Edge> getTotalEdges(){
		List<Edge> toRet = outer.getEdges ();
		for (int i = 0; i < holes.Length; i++) {
			toRet.AddRange (holes [i].getEdges ());
		}
		return toRet;
	}
}