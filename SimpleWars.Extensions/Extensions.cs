namespace SimpleWars.Extensions
{
    using System;

    using Microsoft.Xna.Framework;

    public static class Extensions
    {
        /// <summary>
        /// Transforms vector3 to quaternion
        /// </summary>
        public static Quaternion ToQuaternion(this Vector3 v)
        {
            float c1 = (float)Math.Cos(v.X / 2);
            float s1 = (float)Math.Sin(v.X / 2);
            float c2 = (float)Math.Cos(v.Y / 2);
            float s2 = (float)Math.Sin(v.Y / 2);
            float c3 = (float)Math.Cos(v.Z / 2);
            float s3 = (float)Math.Sin(v.Z / 2);

            Quaternion q = Quaternion.Identity;

            q.W = c1 * c2 * c3 - s1 * s2 * s3;
            q.X = s1 * c2 * c3 + c1 * s2 * s3;
            q.Y = c1 * s2 * c3 - s1 * c2 * s3;
            q.Z = c1 * c2 * s3 + s1 * s2 * c3;

            return q;
        }

        /// <summary>
        /// Transforms quaternion to vector3
        /// </summary>
        public static Vector3 ToVec3(this Quaternion q)
        {
            float x = (float)
            Math.Atan2(
                -2 * (q.Y * q.Z - q.W * q.X),
                q.W * q.W - q.X * q.X
                - q.Y * q.Y + q.Z * q.Z);

            float y = (float)Math.Asin(2 * (q.X * q.Z + q.W * q.Y));

            float z = (float)
            Math.Atan2(
                -2 * (q.X * q.Y - q.W * q.Z),
                q.W * q.W + q.X * q.X
                - q.Y * q.Y - q.Z * q.Z);

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Transforms vector3 direction to quaternion
        /// </summary>
        public static Quaternion FromDirectionToQuaternion(this Vector3 v)
        {
            float dot = Vector3.Dot(Vector3.Forward, v);

            float rotAngle = (float)Math.Acos(dot);
            Vector3 rotAxis = Vector3.Normalize(Vector3.Cross(Vector3.Forward, v));
            return Quaternion.CreateFromAxisAngle(rotAxis, rotAngle);
        }
    }
}
