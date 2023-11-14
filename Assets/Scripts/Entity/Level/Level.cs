using UnityEngine;

namespace Assets.Scripts.Model
{
    [System.Serializable]
    internal class Level
    {
        public string name;
        public Vector3 initialPlayerPosition;

        public override string ToString()
        {
            return $"{{name={name}, initialPlayerPosition={initialPlayerPosition}}}";
        }
    }
}
