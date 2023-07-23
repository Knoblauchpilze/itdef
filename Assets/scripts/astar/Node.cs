using System;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
  private Vector2Int pos;
  private float cost;
  private float heuristic;

  public Node(Vector2Int p, float c, float h)
  {
    pos = p;
    cost = c;
    heuristic = h;
  }

  public Vector2Int Pos()
  {
    return pos;
  }

  public float Cost()
  {
    return cost;
  }

  public float Heuristic()
  {
    return heuristic;
  }

  public bool Contains(Vector2Int rhs)
  {
    return pos.x == rhs.x && pos.y == rhs.y;
  }

  public List<Node> GenerateNeighbors(Vector2Int target)
  {
    // https://stackoverflow.com/questions/466946/how-to-initialize-a-listt-to-a-given-size-as-opposed-to-capacity
    List<Node> neighbors = new List<Node>(new Node[Neighbor.Count]);

    Vector2Int np;
    float nCost;
    float nHeuristic;

    np = new Vector2Int(pos.x + 1, pos.y);
    nCost = cost + Vector2Int.Distance(pos, np);
    nHeuristic = Vector2Int.Distance(np, target);
    neighbors[Neighbor.East] = new Node(np, nCost, nHeuristic);

    np = new Vector2Int(pos.x, pos.y + 1);
    nCost = cost + Vector2Int.Distance(pos, np);
    nHeuristic = Vector2Int.Distance(np, target);
    neighbors[Neighbor.North] = new Node(np, nCost, nHeuristic);

    np = new Vector2Int(pos.x - 1, pos.y);
    nCost = cost + Vector2Int.Distance(pos, np);
    nHeuristic = Vector2Int.Distance(np, target);
    neighbors[Neighbor.West] = new Node(np, nCost, nHeuristic);

    np = new Vector2Int(pos.x, pos.y - 1);
    nCost = cost + Vector2Int.Distance(pos, np);
    nHeuristic = Vector2Int.Distance(np, target);
    neighbors[Neighbor.South] = new Node(np, nCost, nHeuristic);

    return neighbors;
  }

  public static string Hash(Vector2Int p)
  {
    return p.x.ToString() + "#" + p.y.ToString();
  }

  public static Vector2Int Unhash(string hash)
  {
    int id = hash.IndexOf('#');
    if (id < 0)
    {
      return new Vector2Int(0, 0);
    }

    string xStr = hash.Substring(0, id);
    string yStr = hash.Substring(id + 1);

    int x = Int32.Parse(xStr);
    int y = Int32.Parse(yStr);

    return new Vector2Int(x, y);
  }
}
