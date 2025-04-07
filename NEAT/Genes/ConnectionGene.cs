using System;

namespace NEAT.Genes
{
    public class ConnectionGene : BaseGene
    {
        public int InputKey { get; private set; }
        public int OutputKey { get; private set; }
        public double Weight { get; set; }

        public ConnectionGene(int key, int inputKey, int outputKey, double weight) : base(key)
        {
            InputKey = inputKey;
            OutputKey = outputKey;
            Weight = weight;
        }

        public override BaseGene Clone()
        {
            var clone = new ConnectionGene(Key, InputKey, OutputKey, Weight)
            {
                Enabled = Enabled
            };
            return clone;
        }

        public override double DistanceTo(BaseGene other)
        {
            if (!(other is ConnectionGene otherConn))
                throw new ArgumentException("Connection distance comparison requires ConnectionGene type");

            if (InputKey != otherConn.InputKey || OutputKey != otherConn.OutputKey)
                return 1.0;  // Different connections are maximally distant

            return Math.Abs(Weight - otherConn.Weight);  // Weight difference for matching connections
        }

        public override string ToString()
        {
            return string.Format("ConnectionGene(key={0}, {1} -> {2}, weight={3:F2}, enabled={4})", 
                Key, InputKey, OutputKey, Weight, Enabled);
        }
    }
} 