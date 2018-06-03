using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GiraffeStar;

namespace FillClient
{
	public class PlayRoomModule : Module {

		GameObject root;
		GameObject board;

		bool isInitialized;

		public override void OnRegister ()
		{
			base.OnRegister ();
			Init ();
		}

		void Init()
		{
			if (isInitialized) { return; }

			root = GameObject.Find ("Root");
			board = root.FindChildByName ("Board");

			isInitialized = true;
		}
			
		public void SetStage(StageData stageData)
		{
			var points = stageData.OuterVectices;

			Debug.Log ("Set Stage" + stageData.OuterVectices.Count);

			if (points == null) { return; }

			var lineObj = new GameObject ("Line");
			lineObj.transform.SetParent (board.transform);
			var line = lineObj.AddComponent<LineRenderer> ();
			line.startWidth = 0.3f;
			line.endWidth = 0.3f;
			line.positionCount = points.Count + 1;

			points.Add (points [0]);

			var converted = new List<Vector3> ();

			foreach (var point in points) {
				converted.Add (point);
			}

			line.SetPositions(converted.ToArray ());
		}
	}
}