using System;
using UnityEngine;
using System.Collections.Generic;

public class MapData
{
	private SimplePolygon2D outer;
	private SimplePolygon2D[] holes;

	/*Constructor*/
	public MapData (SimplePolygon2D outer, SimplePolygon2D[] holes)
	{
		this.outer = outer;
		this.holes = holes;		
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