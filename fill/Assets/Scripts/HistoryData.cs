using UnityEngine;
using System.Collections;

public class HistoryData {
	private HistoryState state;
	private int guardId;
	private Vector3 position;

	public HistoryData (HistoryState state) {
		this.state = state;
	}

	public HistoryData (HistoryState state, int guardId, Vector3 position) {
		this.state = state;
		this.guardId = guardId;
		this.position = position;
	}

	public HistoryState State {
		get {
			return state;
		}
	}

	public int GuardId {
		get {
			return guardId;
		}
	}

	public Vector3 Position {
		get {
			return position;
		}
	}
}
