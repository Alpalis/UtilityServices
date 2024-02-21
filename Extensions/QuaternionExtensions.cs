using Alpalis.UtilityServices.Models;
using UnityEngine;

namespace Alpalis
{
    public static class QuaternionExtensions
    {
        public static SerializableQuaternion Serialize(this Quaternion quaternion) => new(quaternion);
    }
}
