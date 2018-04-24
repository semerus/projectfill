using UnityEngine;

/// <summary>
/// Edge class that contains both end-points. Although the class is named edge, it is capable of representing line segment and is used as line segment in some cases.
/// </summary>
public class Edge
{
	Vector2 a, b;

	public Edge (Vector2 from, Vector2 to)
	{
		a = from;
		b = to;
	}

	/// <summary>
	/// This method checks if this edge crosses with the parameter edge.
	/// </summary>
	/// <param name="target">The target edge to check whether two line segments cross or not.</param>
	/// <returns>
	/// Returns boolean.
	/// </returns>
	public bool isCross (Edge target)
	{
		Vector2 c, d;
		c = target.a;
		d = target.b;
		if (collinear (a, b, c) ||
		    collinear (a, b, d) ||
		    collinear (c, d, a) ||
		    collinear (c, d, b))
			return false;

		return Xor (Left (a, b, c), Left (a, b, d)) &&
		Xor (Left (c, d, a), Left (c, d, b));
	}

	public bool isCrossEasy (Edge target)
	{
		Vector2 c, d;
		c = target.a;
		d = target.b;

		return Xor (LeftEasy (a, b, c), LeftEasy (a, b, d)) &&
		Xor (LeftEasy (c, d, a), LeftEasy (c, d, b));
	}

	private static bool collinear (Vector2 a, Vector2 b, Vector2 c)
	{
		return (b.x - a.x) * (c.y - a.y) - (c.x - a.x) * (b.y - a.y) == 0;
	}

	private static bool Left (Vector2 a, Vector2 b, Vector2 c)
	{
		return (b.x - a.x) * (c.y - a.y) - (c.x - a.x) * (b.y - a.y) > 0;
	}

	private static bool LeftEasy (Vector2 a, Vector2 b, Vector2 c)
	{
		return (b.x - a.x) * (c.y - a.y) - (c.x - a.x) * (b.y - a.y) >= 0;
	}

	private static bool Xor (bool x, bool y)
	{
		return !x ^ !y;
	}

	public override string ToString ()
	{
		return "Edge[a, b] = " + a + ", " + b;
	}
}