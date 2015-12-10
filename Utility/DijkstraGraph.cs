// ************************************************************************ 
// File Name:   DijkstraGraph.cs 
// Purpose:    	Finds shortest path from one node to abother
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2015 Bounder Games
// ************************************************************************ 

// ************************************************************************ 
// Imports 
// ************************************************************************ 
using System.Collections.Generic;


// ************************************************************************ 
// Class: DijkstraGraph
// ************************************************************************ 
public class DijkstraGraph {

	
	// ********************************************************************
	// Private Data Members 
	// ********************************************************************
	private Dictionary<string,  Dictionary<string, float>> m_vertices = new Dictionary<string,  Dictionary<string, float>>();


	// ********************************************************************
	// Function:	AddVertex()
	// Purpose:		Adds a vertex and its edges to the graph
	// ********************************************************************
	public void AddVertex(string _name, Dictionary<string, float> _edges)
	{
		m_vertices[_name] = _edges;
	}


	// ********************************************************************
	// Function:	ShortestPath()
	// Purpose:		Finds the shortest path between two vertices
	// ********************************************************************
	public List<string> ShortestPath(string _start, string _finish)
	{
		var previous = new Dictionary<string, string>();
		var distances = new Dictionary<string, float>();
		var nodes = new List<string>();
		
		List<string> path = null;

		foreach (var vertex in m_vertices)
		{
			if (vertex.Key == _start)
			{
				distances[vertex.Key] = 0;
			}
			else
			{
				distances[vertex.Key] = float.MaxValue;
			}
			
			nodes.Add(vertex.Key);
		}
		
		while (nodes.Count != 0)
		{
			nodes.Sort((x,y) => (distances[x].CompareTo(distances[y])));
			
			var smallest = nodes[0];
			nodes.Remove(smallest);
			
			if (smallest == _finish)
			{
				path = new List<string>();
				while (previous.ContainsKey(smallest))
				{
					path.Add(smallest);
					smallest = previous[smallest];
				}
				
				break;
			}
			
			if (distances[smallest] == float.MaxValue)
			{
				break;
			}
			
			foreach (var neighbor in m_vertices[smallest])
			{
				var alt = distances[smallest] + neighbor.Value;

				if (alt < distances[neighbor.Key])
				{
					distances[neighbor.Key] = alt;
					previous[neighbor.Key] = smallest;
				}
			}
		}
		
		return path;
	}

}
