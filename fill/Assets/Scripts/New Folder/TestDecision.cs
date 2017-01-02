using System;
using UnityEngine;

public class TestDecision
{
	static void Main(string[] args)
	{
		// Display the number of command line arguments:
		Console.WriteLine("Filled test started");

		FileManager fm = FileManager.getInstance ();
		MapData md = fm.readMap ("../../GameLevels/star"); // path to dir

		if (md == null) {
			Console.WriteLine ("Error map folder not found!");
			return;
		}

		DecisionAlgorithm da = new DecisionAlgorithm (md);
		Vector3[] guards = new Vector3[3];

		guards [0] = new Vector3 ();
		guards [1] = new Vector3 ();
		guards [2] = new Vector3 ();

		da.isFilled (guards);
	}
}