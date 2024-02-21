using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Alpalis.UtilityServices.Models
{
    [Serializable]
    public struct SerializableVector3
    {
        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public SerializableVector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public SerializableVector3(Vector3 vector)
        {
            X = vector.x;
            Y = vector.y;
            Z = vector.z;
        }

        public Vector3 Deserialize() => new(X, Y, Z);
    }
}
