using UnityEngine;

namespace GiraffeStar
{
    public static class LineRendererEx
    {
        public static void SetFullColor(this LineRenderer line, Color color)
        {
            line.startColor = color;
            line.endColor = color;
        }

        public static void SetFullWidth(this LineRenderer line, float width)
        {
            line.startWidth = width;
            line.endWidth = width;
        }
    }
}

