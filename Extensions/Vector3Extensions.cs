using Alpalis.UtilityServices.Models;
using UnityEngine;

namespace Alpalis
{
    public static class Vector3Extensions
    {
        public static SerializableVector3 Serialize(this Vector3 vector) => new(vector);
    }
}
