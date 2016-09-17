using UnityEngine;
using System.Collections;

public class ThumbnailData {
	private int id;
	private string name;
	private string imgPath;

	public ThumbnailData (int id, string name, string imgPath) {
		this.id = id;
		this.name = name;
		this.imgPath = imgPath;
	}

	public int Id {
		get {
			return id;
		}
	}

	public string Name {
		get {
			return name;
		}
	}

	public string ImgPath {
		get {
			return imgPath;
		}
	}
}
