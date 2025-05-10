using System;
using System.Collections.Generic;
using System.Linq;
using NEAT.Genes;
using NEAT.Genome;

namespace NEAT.NN
{
    // Adapter class that wraps our simplified NeuralNetwork class to maintain compatibility
    // with existing Unity scripts that expect a FeedForwardNetwork
    public class FeedForwardNetwork
    {
        private NeuralNetwork _network;
        
        // Keep these public fields available via reflection for the visualizer
        private Dictionary<int, double> _nodeValues;
        private Dictionary<int, NodeGene> _nodes;
        private Dictionary<int, ConnectionGene> _connections;

        public FeedForwardNetwork(Dictionary<int, NodeGene> nodes, Dictionary<int, ConnectionGene> connections)
        {
            _nodes = new Dictionary<int, NodeGene>();
            _connections = new Dictionary<int, ConnectionGene>();
            _nodeValues = new Dictionary<int, double>();
            
            // Create deep copies of nodes and connections to avoid side effects
            foreach (var node in nodes.Values)
            {
                _nodes[node.Key] = (NodeGene)node.Clone();
            }
            
            foreach (var conn in connections.Values)
            {
                _connections[conn.Key] = (ConnectionGene)conn.Clone();
            }
            
            // Create a genome to pass to the NeuralNetwork constructor
            var genome = new Genome.Genome(0);
            
            // Add all nodes and connections
            foreach (var node in _nodes.Values)
            {
                genome.AddNode((NodeGene)node.Clone());
            }
            
            foreach (var conn in _connections.Values)
            {
                if (conn.Enabled)
                {
                    genome.AddConnection((ConnectionGene)conn.Clone());
                }
            }
            
            // Create the neural network
            _network = new NeuralNetwork(genome);
        }
        
        public double[] Activate(double[] inputs)
        {
            // Get the output from the neural network
            double[] outputs = _network.Activate(inputs);
            
            // Update node values for visualization
            UpdateNodeValues();
            
            return outputs;
        }
        
        // Static factory method that matches the expected API
        public static FeedForwardNetwork Create(Genome.Genome genome)
        {
            return new FeedForwardNetwork(genome.Nodes, genome.Connections);
        }
        
        // Add method to get the genome from the network
        public Genome.Genome GetGenome()
        {
            var genome = new Genome.Genome(0);
            
            // Add all nodes
            foreach (var node in _nodes.Values)
            {
                genome.AddNode((NodeGene)node.Clone());
            }
            
            // Add all connections
            foreach (var conn in _connections.Values)
            {
                if (conn.Enabled)
                {
                    genome.AddConnection((ConnectionGene)conn.Clone());
                }
            }
            
            return genome;
        }
        
        // PUBLIC SERIALIZATION METHODS - Added to fix build serialization issues
        
        // Get all nodes for serialization - avoids reflection in builds
        public Dictionary<int, NodeGene> GetNodes()
        {
            return _nodes;
        }
        
        // Get all connections for serialization - avoids reflection in builds
        public Dictionary<int, ConnectionGene> GetConnections()
        {
            return _connections;
        }
        
        // Helper method to update _nodeValues field with the latest values from the network
        private void UpdateNodeValues()
        {
            // Use reflection to access the private _nodeValues field in NeuralNetwork
            var fieldInfo = _network.GetType().GetField("_nodeValues", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (fieldInfo != null)
            {
                var networkNodeValues = fieldInfo.GetValue(_network) as Dictionary<int, double>;
                if (networkNodeValues != null)
                {
                    // Copy values to our exposed _nodeValues field
                    _nodeValues.Clear();
                    foreach (var kvp in networkNodeValues)
                    {
                        _nodeValues[kvp.Key] = kvp.Value;
                    }
                }
            }
        }
    }
} 