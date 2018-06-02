using UnityEngine;

namespace GiraffeStar
{
    public static class GameObjectEx
    {
        public static GameObject FindChildByName(this GameObject go, string name)
        {
            var children = go.GetComponentsInChildren<Transform>(true);
            foreach (var child in children)
            {
                if(child.name.Equals(name))
                {
                    return child.gameObject;
                }
            }

            return null;
        }
    }
}


