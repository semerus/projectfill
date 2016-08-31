using System;
using UnityEngine;

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


	/*Getters*/
	public SimplePolygon2D getOuter(){
		return outer;
	}

	public SimplePolygon2D[] getHoles(){
		return holes;
	}
}