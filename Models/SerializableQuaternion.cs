using System;
using UnityEngine;

namespace Alpalis.UtilityServices.Models
{
    [Serializable]
    public struct SerializableQuaternion
    {
        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public float W { get; set; }

        public SerializableQuaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public SerializableQuaternion(Quaternion quaternion)
        {
            X = quaternion.x;
            Y = quaternion.y;
            Z = quaternion.z;
            W = quaternion.w;
        }

        public Quaternion Deserialize() => new(X, Y, Z, W);
    }
}
