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
    }
}