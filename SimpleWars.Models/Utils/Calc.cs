namespace SimpleWars.Models.Utils
{
    using System;

    using Microsoft.Xna.Framework;

    public static class Calc
    {
        public static Quaternion GetRotationForDirection(Vector3 direction)
        {
            float dot = Vector3.Dot(Vector3.Forward, direction);

            float rotAngle = (float)Math.Acos(dot);
            Vector3 rotAxis = Vector3.Normalize(Vector3.Cross(Vector3.Forward, direction));
            return Quaternion.CreateFromAxisAngle(rotAxis, rotAngle);
        }

        public static Quaternion QuaternionFromEuler(float x, float y, float z)
        {
            float c1 = (float)Math.Cos(x / 2);
            float s1 = (float)Math.Sin(x / 2);
            float c2 = (float)Math.Cos(y / 2);
            float s2 = (float)Math.Sin(y / 2);
            float c3 = (float)Math.Cos(z / 2);
            float s3 = (float)Math.Sin(z / 2);

            Quaternion q = Quaternion.Identity;

            q.W = c1 * c2 * c3 - s1 * s2 * s3;
            q.X = s1 * c2 * c3 + c1 * s2 * s3;
            q.Y = c1 * s2 * c3 - s1 * c2 * s3;
            q.Z = c1 * c2 * s3 + s1 * s2 * c3;

            return q;
        }
    }
}