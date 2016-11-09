namespace SimpleWars.Utils
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// The height generator.
    /// </summary>
    public class NoiseGenerator
    {
        /// <summary>
        /// The positive and negative amplitude limit for the height
        /// </summary>
        private const float Amplitude = 70;

        /// <summary>
        /// The octaves (cycles) that the height function will apply
        /// </summary>
        private const int Octaves = 4;

        /// <summary>
        /// The roughness of the produced terrain.
        /// </summary>
        private const float Roughness = 0.2f;

        /// <summary>
        /// Used to initialize random seed if there was none in the constructor
        /// </summary>
        private readonly Random seedRandom;

        /// <summary>
        /// Mersenne twister random generator.
        /// Got it from stack overflow. Works nicely. 
        /// I modified it to make less cycles in cost of randomness. 
        /// Works a lot faster than it used to.
        /// C# default random generator doesn't work for this algorithm
        /// since the seed cannot be set after its initialized
        /// and initializing new random for 30000+ triangles is really slow.
        /// </summary>
        private readonly MersenneTwister random;

        /// <summary>
        /// The seed. Same seed = same world
        /// </summary>
        private readonly uint seed;

        /// <summary>
        /// Initializes a new instance of the <see cref="NoiseGenerator"/> class.
        /// </summary>
        public NoiseGenerator()
        {
            this.seedRandom = new Random();

            this.seed = (uint)this.seedRandom.Next(0, 1000000000);

            this.random = new MersenneTwister(this.seed);
           
            Debug.WriteLine("World seed: " + this.seed);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoiseGenerator"/> class.
        /// </summary>
        /// <param name="seed">
        /// The seed for the noise function. Same seed always produces same results.
        /// </param>
        public NoiseGenerator(uint seed)
        {
            this.seed = seed;
            this.random = new MersenneTwister(this.seed);
        }

        /// <summary>
        /// The generate height.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="z">
        /// The z.
        /// </param>
        /// <returns>
        /// The <see cref="float"/>.
        /// </returns>
        public float GenerateHeight(int x, int z)
        {
            float total = 0;
            float d = (float)Math.Pow(2, Octaves - 1);
            for (int i = 0; i < Octaves; i++)
            {
                float frequency = (float)Math.Pow(2, i) / d;
                float amplitude = (float)Math.Pow(Roughness, i) * Amplitude;
                total += this.GetInterpolatedNoise(x * frequency, z * frequency) * amplitude;
            }
            return total;
        }

        /// <summary>
        /// Smoothens the output from the noise function
        /// </summary>
        /// <param name="x">
        /// The x coord
        /// </param>
        /// <param name="z">
        /// The z coord
        /// </param>
        /// <returns>
        /// The <see cref="float"/>.
        /// </returns>
        private float GetSmoothNoise(int x, int z)
        {
            float corners = (this.GetNoise(x - 1, z - 1) + 
                this.GetNoise(x + 1, z - 1) + 
                this.GetNoise(x - 1, z + 1) + 
                this.GetNoise(x + 1, z + 1)) / 16f;

            float sides = (this.GetNoise(x - 1, z) + 
                this.GetNoise(x + 1, z) + 
                this.GetNoise(x, z - 1) + 
                this.GetNoise(x, z + 1)) / 8f;

            float center = this.GetNoise(x, z) / 4f;

            return corners + sides + center;
        }

        /// <summary>
        /// Gets the noise for the coordinates. 
        /// Returns the same value for the same inputs with the same seed
        /// as it should do.
        /// </summary>
        /// <param name="x">
        /// The x coord
        /// </param>
        /// <param name="z">
        /// The z coord
        /// </param>
        /// <returns>
        /// The <see cref="float"/>.
        /// </returns>
        private float GetNoise(int x, int z)
        {
            this.random.Seed = (uint)(x * 13248 + z * 49245 + this.seed);
            return (float)this.random.NextDouble() * 2 - 1;
        }

        /// <summary>
        /// Cosine interpolation between inputs.
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <param name="blend">
        /// The blend.
        /// </param>
        /// <returns>
        /// The <see cref="float"/>.
        /// </returns>
        private float Interpolate(float a, float b, float blend)
        {
            double theta = blend * Math.PI;
            float f = (float)(1f - Math.Cos(theta)) * 0.5f;
            return a * (1f - f) + b * f;
        }

        /// <summary>
        /// Produces nicely interpolated noise
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="z">
        /// The z.
        /// </param>
        /// <returns>
        /// The <see cref="float"/>.
        /// </returns>
        private float GetInterpolatedNoise(float x, float z)
        {
            int intX = (int)x;
            int intZ = (int)z;

            float fractionX = x - intX;
            float fractionZ = z - intZ;

            float v1 = this.GetSmoothNoise(intX, intZ);
            float v2 = this.GetSmoothNoise(intX + 1, intZ);
            float v3 = this.GetSmoothNoise(intX, intZ + 1);
            float v4 = this.GetSmoothNoise(intX + 1, intZ + 1);
            float i1 = this.Interpolate(v1, v2, fractionX);
            float i2 = this.Interpolate(v3, v4, fractionX);
            return this.Interpolate(i1, i2, fractionZ);
        }
    }
}