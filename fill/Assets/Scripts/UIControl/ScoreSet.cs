using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;

public class ScoreSet : MonoBehaviour {
	//	this script is used to display number of guards on screen
	public static int num_scores = 6; // number of scores

	private float low1, low2, low3, high1, high2, high3; // score value
	private int g_low1, g_low2, g_low3, g_high1, g_high2, g_high3; // number of guards used

	ScoreSet(float[] scores, int[] guards){
		if (scores.Length != num_scores) {
			Debug.LogError ("Number of score info does not match the input\n");
		}

		if (guards.Length != num_scores) {
			Debug.LogError ("Number of guards info does not match the input\n");
		}

		low1 = scores [0];
		low2 = scores [1];
		low3 = scores [2];
		high1 = scores [3];
		high2 = scores [4];
		high3 = scores [5];

		g_low1 = guards [0];
		g_low2 = guards [1];
		g_low3 = guards [2];
		g_high1 = guards [3];
		g_high2 = guards [4];
		g_high3 = guards [5];
	}


	public override string ToString(){
		StringBuilder sb = new StringBuilder ();

		sb.Append ("low1: " + low1 + "\n");
		sb.Append ("low2: " + low2 + "\n");
		sb.Append ("low3: " + low3 + "\n");
		sb.Append ("high1: " + high1 + "\n");
		sb.Append ("high2: " + high2 + "\n");
		sb.Append ("high3: " + high3 + "\n");

		sb.Append ("g_low1: " + g_low1 + "\n");
		sb.Append ("g_low2: " + g_low2 + "\n");
		sb.Append ("g_low3: " + g_low3 + "\n");
		sb.Append ("g_high1: " + g_high1 + "\n");
		sb.Append ("g_high2: " + g_high2 + "\n");
		sb.Append ("g_high3: " + g_high3 + "\n");

		return sb.ToString ();
	}
}
