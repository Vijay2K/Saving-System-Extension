using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extension.SavingSystem
{
    [System.Serializable]
    public class SerializableVector
    {
        private float x;
        private float y;
        private float z;

        public SerializableVector(Vector2 vec)
        {
            this.x = vec.x;
            this.y = vec.y;
        }

        public SerializableVector(Vector3 vec)
        {
            this.x = vec.x;
            this.y = vec.y;
            this.z = vec.z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }

        public Vector2 ToVector2()
        {
            return new Vector2(x, y);
        }
    }

}