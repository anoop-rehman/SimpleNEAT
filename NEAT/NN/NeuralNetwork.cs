using System;
using System.Collections.Generic;
using System.Linq;
using NEAT.Genes;
using NEAT.Genome;

namespace NEAT.NN
{
    public class NeuralNetwork
    {
        private Dictionary<int, double> _nodeValues;
        private Dictionary<int, NodeGene> _nodes;
        private Dictionary<int, ConnectionGene> _connections;
        private List<int> _inputNodeKeys;
        private List<int> _outputNodeKeys;
        private List<int> _hiddenNodeKeys;
        private List<ConnectionGene> _enabledConnections;

        public NeuralNetwork(Genome.Genome genome)
        {
            _nodes = new Dictionary<int, NodeGene>(genome.Nodes);
            _connections = new Dictionary<int, ConnectionGene>(genome.Connections);
            _nodeValues = new Dictionary<int, double>();
            
            // Categorize nodes by type
            _inputNodeKeys = _nodes.Values.Where(n => n.Type == NodeType.Input).Select(n => n.Key).ToList();
            _outputNodeKeys = _nodes.Values.Where(n => n.Type == NodeType.Output).Select(n => n.Key).ToList();
            _hiddenNodeKeys = _nodes.Values.Where(n => n.Type == NodeType.Hidden).Select(n => n.Key).ToList();
            
            // Get enabled connections
            _enabledConnections = _connections.Values.Where(c => c.Enabled).ToList();
            
            // Sort hidden nodes by layer (if applicable)
            _hiddenNodeKeys = _hiddenNodeKeys.OrderBy(k => _nodes[k].Layer).ToList();
        }
        
        public double[] Activate(double[] inputs)
        {
            if (inputs.Length != _inputNodeKeys.Count)
            {
                throw new ArgumentException(string.Format(
                    "Expected {0} inputs, but got {1}", _inputNodeKeys.Count, inputs.Length));
            }
            
            // Reset node values
            _nodeValues.Clear();
            
            // Set input values
            for (int i = 0; i < _inputNodeKeys.Count; i++)
            {
                _nodeValues[_inputNodeKeys[i]] = inputs[i];
            }
            
            // Process hidden nodes (in layer order if available)
            foreach (var nodeKey in _hiddenNodeKeys)
            {
                ActivateNode(nodeKey);
            }
            
            // Process output nodes
            double[] outputs = new double[_outputNodeKeys.Count];
            for (int i = 0; i < _outputNodeKeys.Count; i++)
            {
                int nodeKey = _outputNodeKeys[i];
                ActivateNode(nodeKey);
                outputs[i] = _nodeValues[nodeKey];
            }
            
            return outputs;
        }
        
        private void ActivateNode(int nodeKey)
        {
            // Skip if node already has a value
            if (_nodeValues.ContainsKey(nodeKey))
                return;
                
            double sum = 0.0;
            bool hasIncomingConnections = false;
            
            // Get the node to access its bias
            NodeGene node = _nodes[nodeKey];
            
            // Find all connections with this node as output
            var incomingConnections = _enabledConnections.Where(c => c.OutputKey == nodeKey);
            
            foreach (var conn in incomingConnections)
            {
                hasIncomingConnections = true;
                
                // Make sure the input node has a value
                if (!_nodeValues.ContainsKey(conn.InputKey))
                {
                    ActivateNode(conn.InputKey);
                }
                
                // Aggregate the weighted inputs
                sum += _nodeValues[conn.InputKey] * conn.Weight;
            }
            
            // If this node has no incoming connections, just use its bias directly
            // This ensures disconnected nodes still produce an output
            if (!hasIncomingConnections)
            {
                // Use the bias (if any) as the node's value
                sum = node.Bias;
            }
            else
            {
                // Add the bias to the weighted sum for normal connected nodes
                sum += node.Bias;
            }
            
            // Apply tanh activation function (ranges from -1 to 1)
            _nodeValues[nodeKey] = Tanh(sum);
        }
        
        private double Sigmoid(double x)
        {
            return 1.0 / (1.0 + Math.Exp(-4.9 * x));
        }
        
        private double Tanh(double x)
        {
            return Math.Tanh(x);
        }
        
        private double ReLU(double x)
        {
            return Math.Max(0, x);
        }
    }
} 