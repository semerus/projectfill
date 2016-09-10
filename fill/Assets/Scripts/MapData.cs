using System;
using UnityEngine;


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

	/*Constructor*/
	public MapData (SimplePolygon2D outer, SimplePolygon2D[] holes)
	{
		this.outer = outer;
		this.holes = holes;		
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
}