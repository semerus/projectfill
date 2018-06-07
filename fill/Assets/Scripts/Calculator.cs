using UnityEngine;

public static class Calculator {
    
    public static bool JudgeValidPositions(Vector3 position, MapData map)
    {
        //first check if the position is inside outer
        if (!map.getOuter().IsInsidePolygon(position))
        {
            return false;
        }

        // check if the position is outside of every hole
        SimplePolygon2D[] holes = map.getHoles();
        for (int i = 0; i < holes.Length; i++)
        {
            if (holes[i].IsInsidePolygon(position))
                return false;
        }

        return true;
    }    
}
