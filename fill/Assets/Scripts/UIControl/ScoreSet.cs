using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System;

public class ScoreSet : MonoBehaviour {
	//	this script is used to display number of guards on screen
	public static int num_scores = 6; // number of scores, must be multiple of 2

	private float[] low, high; // score value
	private int[] g_low, g_high; // number of guards used

	public ScoreSet(float[] scores, int[] guards){
		if (scores.Length != num_scores) {
			Debug.LogError ("Number of score info does not match the input\n");
		}

		if (guards.Length != num_scores) {
			Debug.LogError ("Number of guards info does not match the input\n");
		}

		low = new float[num_scores / 2];
		high = new float[num_scores / 2];
		g_low = new int[num_scores / 2];
		g_high = new int[num_scores / 2];

		int i = 0;
		for (; i < num_scores / 2; i++) {
			low [i] = scores [i];
			g_low [i] = guards [i];
		}

		int j = 0;
		for (; i < num_scores; i++, j++) {
			high [j] = scores [i];
			g_high [j] = guards [i];
		}
	}

	public override string ToString(){
		StringBuilder sb = new StringBuilder ();

		for (int i = 0; i < low.Length; i++) {
			sb.Append ("low" + (i + 1) + ": " + low [i] + "\n");
			sb.Append ("g_low" + (i + 1) + ": " + g_low [i] + "\n");
		}

		for (int i = 0; i < high.Length; i++) {
			sb.Append ("high" + (i + 1) + ": " + high [i] + "\n");
			sb.Append ("g_high" + (i + 1) + ": " + g_high [i] + "\n");
		}

		return sb.ToString ();
	}

	// returns the score in string 
	public float getScore(string highlow, int rank){
		if (rank <= 0 || rank > num_scores / 2)
			throw new ArgumentException();
		
		if (highlow == "High") {
			return high [rank - 1];
		} else if (highlow == "Low") {
			return low [rank - 1];
		} else
			throw new ArgumentException();
	}

	// returns the number of guards used to get the score 
	public int getGuards(string highlow, int rank){
		if (rank <= 0 || rank > num_scores / 2)
			throw new ArgumentException();

		if (highlow == "High") {
			return g_high [rank - 1];
		} else if (highlow == "Low") {
			return g_low [rank - 1];
		} else
			throw new ArgumentException();
	}
}
