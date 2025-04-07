using System;

namespace NEAT.Genes
{
    public abstract class BaseGene
    {
        public int Key { get; private set; }
        public bool Enabled { get; set; } = true;

        protected BaseGene(int key)
        {
            Key = key;
        }

        public abstract BaseGene Clone();

        public virtual double DistanceTo(BaseGene other)
        {
            if (Key != other.Key)
                return 1.0;  // Different genes are maximally distant
            return 0.0;      // Same gene has zero distance
        }
    }
} 