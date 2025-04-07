using System;
using System.Collections.Generic;
using NEAT.Genes;
using NEAT.Genome;

namespace NEAT.NN
{
    public class NEATNetworkTester
    {
        private NeuralNetwork _network;
        private double _totalMSE;
        private int _testCount;

        public NEATNetworkTester(Genome.Genome genome)
        {
            _network = new NeuralNetwork(genome);
            _totalMSE = 0;
            _testCount = 0;
        }

        public void TestNetwork(double[] inputs, double[] expectedOutputs)
        {
            // Get actual outputs from network
            double[] actualOutputs = _network.Activate(inputs);
            
            // Calculate Mean Squared Error
            double sumSquaredError = 0;
            for (int i = 0; i < expectedOutputs.Length; i++)
            {
                double error = expectedOutputs[i] - actualOutputs[i];
                sumSquaredError += error * error;
            }
            
            double mse = sumSquaredError / expectedOutputs.Length;
            _totalMSE += mse;
            _testCount++;
            
            // Log the test results
            Console.WriteLine(string.Format("Test {0}: MSE = {1:F6}", _testCount, mse));
            Console.WriteLine(string.Format("  Inputs: [{0}]", string.Join(", ", inputs)));
            Console.WriteLine(string.Format("  Expected: [{0}]", string.Join(", ", expectedOutputs)));
            Console.WriteLine(string.Format("  Actual: [{0}]", string.Join(", ", FormatOutputs(actualOutputs))));
        }
        
        public void TestXOR()
        {
            Console.WriteLine("Testing XOR function:");
            
            // XOR truth table
            TestNetwork(new double[] { 0, 0 }, new double[] { 0 });
            TestNetwork(new double[] { 0, 1 }, new double[] { 1 });
            TestNetwork(new double[] { 1, 0 }, new double[] { 1 });
            TestNetwork(new double[] { 1, 1 }, new double[] { 0 });
            
            PrintResults();
        }
        
        public double GetAverageMSE()
        {
            if (_testCount == 0)
                return 0;
                
            return _totalMSE / _testCount;
        }
        
        public void PrintResults()
        {
            Console.WriteLine(string.Format("\nTesting complete: Average MSE across {0} tests = {1:F6}", _testCount, GetAverageMSE()));
        }
        
        private string[] FormatOutputs(double[] outputs)
        {
            string[] formatted = new string[outputs.Length];
            for (int i = 0; i < outputs.Length; i++)
            {
                formatted[i] = string.Format("{0:F6}", outputs[i]);
            }
            return formatted;
        }
    }
} 