using System;
using NEAT.Genes;
using NEAT.Genome;
using NEAT.NN;

namespace SimpleNEAT.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("SimpleNEAT Sample - XOR Test");
            Console.WriteLine("----------------------------");
            
            // Test with a standard XOR network
            var standardXOR = CreateStandardXORNetwork();
            
            Console.WriteLine("\nCreated standard XOR genome with:");
            Console.WriteLine($"  Nodes: {standardXOR.Nodes.Count}");
            Console.WriteLine($"  Connections: {standardXOR.Connections.Count}");

            // Create a neural network from the genome
            Console.WriteLine("\nCreating neural network and testing XOR function...");
            var tester = new NEATNetworkTester(standardXOR);
            tester.TestXOR();
            
            // Also show an example of a simple OR network
            Console.WriteLine("\nAs a comparison, testing a simple OR function network:");
            var orNetwork = CreateORNetwork();
            var orTester = new NEATNetworkTester(orNetwork);
            orTester.TestNetwork(new double[] { 0, 0 }, new double[] { 0 });
            orTester.TestNetwork(new double[] { 0, 1 }, new double[] { 1 });
            orTester.TestNetwork(new double[] { 1, 0 }, new double[] { 1 });
            orTester.TestNetwork(new double[] { 1, 1 }, new double[] { 1 });
            orTester.PrintResults();
            
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        private static Genome CreateStandardXORNetwork()
        {
            var genome = new Genome(1);

            // Add nodes in specific order
            var input1 = new NodeGene(1, NodeType.Input);
            var input2 = new NodeGene(2, NodeType.Input);
            var bias = new NodeGene(3, NodeType.Bias);
            var hidden1 = new NodeGene(4, NodeType.Hidden);
            var hidden2 = new NodeGene(5, NodeType.Hidden);
            var output = new NodeGene(6, NodeType.Output);

            genome.AddNode(input1);
            genome.AddNode(input2);
            genome.AddNode(bias);
            genome.AddNode(hidden1);
            genome.AddNode(hidden2);
            genome.AddNode(output);

            // First hidden neuron implements AND
            genome.AddConnection(new ConnectionGene(7, 1, 4, 4.0));   // input1 -> hidden1 
            genome.AddConnection(new ConnectionGene(8, 2, 4, 4.0));   // input2 -> hidden1
            genome.AddConnection(new ConnectionGene(9, 3, 4, -6.0));  // bias -> hidden1 (threshold of about -6)

            // Second hidden neuron implements OR
            genome.AddConnection(new ConnectionGene(10, 1, 5, 4.0));  // input1 -> hidden2
            genome.AddConnection(new ConnectionGene(11, 2, 5, 4.0));  // input2 -> hidden2
            genome.AddConnection(new ConnectionGene(12, 3, 5, -2.0)); // bias -> hidden2 (threshold of about -2)

            // Output implements OR - AND == XOR
            genome.AddConnection(new ConnectionGene(13, 5, 6, 4.0));  // OR -> output
            genome.AddConnection(new ConnectionGene(14, 4, 6, -8.0)); // AND -> output (negative to invert)
            genome.AddConnection(new ConnectionGene(15, 3, 6, 0.0));  // bias -> output (no bias needed here)

            return genome;
        }
        
        private static Genome CreateORNetwork()
        {
            var genome = new Genome(2);

            // Add nodes
            var input1 = new NodeGene(1, NodeType.Input);
            var input2 = new NodeGene(2, NodeType.Input);
            var bias = new NodeGene(3, NodeType.Bias);
            var output = new NodeGene(4, NodeType.Output);

            genome.AddNode(input1);
            genome.AddNode(input2);
            genome.AddNode(bias);
            genome.AddNode(output);

            // OR function has a simple implementation
            genome.AddConnection(new ConnectionGene(5, 1, 4, 5.0));  // input1 -> output
            genome.AddConnection(new ConnectionGene(6, 2, 4, 5.0));  // input2 -> output
            genome.AddConnection(new ConnectionGene(7, 3, 4, -2.5)); // bias -> output (threshold)

            return genome;
        }
    }
} 