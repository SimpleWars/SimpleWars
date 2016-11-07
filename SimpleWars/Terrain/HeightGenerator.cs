namespace SimpleWars.Terrain
{
    using System;

    public class HeightGenerator
    {

        private const float Amplitude = 40;

        private Random random;

        private int seed;

        public HeightGenerator()
        {
            this.random = new Random();

            this.seed = this.random.Next(1000000000);
        }

        public float GenerateHeight(int x, int z)
        {
            return 1;
        }
    }
}