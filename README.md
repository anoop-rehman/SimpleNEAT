# SimpleNEAT

A simplified implementation of the NEAT (NeuroEvolution of Augmenting Topologies) algorithm, built for compatibility with Unity using .NET Standard 2.0.

## Overview

This is a streamlined version of the NEAT algorithm that focuses on core functionality:

- Gene representation (Node and Connection genes)
- Genome structure and operations (crossover, distance calculation)
- Neural network implementation
- Network testing capabilities

## Features

- Compatible with Unity's .NET Standard 2.0 and C# 7.3
- No dependencies on advanced C# features not supported in Unity
- Contains only essential functionality with minimal logging
- Includes a network tester for validating NEAT networks

## Usage

To use this library in your Unity project:

1. Build the NEAT.dll from this project
2. Copy the DLL to your Unity project's Assets/Plugins folder
3. Reference the NEAT namespace in your C# scripts

## Example: XOR Test

```csharp
using NEAT.Genes;
using NEAT.Genome;
using NEAT.NN;

// Create a simple XOR network
var genome = new Genome(1);

// Add input nodes
var input1 = new NodeGene(1, NodeType.Input);
var input2 = new NodeGene(2, NodeType.Input);
var bias = new NodeGene(3, NodeType.Bias);

// Add hidden node
var hidden = new NodeGene(4, NodeType.Hidden);

// Add output node
var output = new NodeGene(5, NodeType.Output);

// Add all nodes to genome
genome.AddNode(input1);
genome.AddNode(input2);
genome.AddNode(bias);
genome.AddNode(hidden);
genome.AddNode(output);

// Create connections
var conn1 = new ConnectionGene(6, 1, 4, 0.5);  // input1 -> hidden
var conn2 = new ConnectionGene(7, 2, 4, -0.5); // input2 -> hidden
var conn3 = new ConnectionGene(8, 3, 4, 0.1);  // bias -> hidden
var conn4 = new ConnectionGene(9, 4, 5, 0.8);  // hidden -> output

// Add connections to genome
genome.AddConnection(conn1);
genome.AddConnection(conn2);
genome.AddConnection(conn3);
genome.AddConnection(conn4);

// Create a tester and test XOR
var tester = new NEATNetworkTester(genome);
tester.TestXOR();
```

## Building

```
dotnet build -c Release
```

The output DLL will be in the bin/Release/netstandard2.0 directory. 