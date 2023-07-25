using System.Collections.Generic;
using UnityEngine;

// https://stackoverflow.com/questions/9257989/defining-type-aliases
// https://stackoverflow.com/questions/166089/what-is-c-sharp-analog-of-c-stdpair
using Link = System.Tuple<string, float>;

public class AStarNodes
{
  private bool sorted = true;
  private List<Node> openNodes = new List<Node>();
  private Dictionary<string, Link> ancestors = new Dictionary<string, Link>();

  public bool Stuck()
  {
    return openNodes.Count == 0;
  }

  public int Opened()
  {
    return openNodes.Count;
  }

  public void Seed(Vector2Int p, float heuristic)
  {
    openNodes.Clear();
    ancestors.Clear();

    openNodes.Add(new Node(p, 0.0f, heuristic));

    // And register this node as its own ancestor.
    string h = Node.Hash(p);
    ancestors[h] = new Link(h, 0.0f);
  }

  public bool Explore(Node child, Vector2Int parent)
  {
    string cHash = Node.Hash(child.Pos());
    string pHash = Node.Hash(parent);

    Link ancestor = new Link("", 0.0f);
    bool exist = ancestors.TryGetValue(cHash, out ancestor);

    // In case the node doesn't exist, we always register
    // it as it's the first time that we can reach it.
    if (!exist)
    {
      ancestors[cHash] = new Link(pHash, child.Cost());
      openNodes.Add(child);
      sorted = false;
    }
    // Otherwise the new association should have a better
    // cost than the currently registered one to be used
    // as the new best link.
    else if (child.Cost() < ancestor.Item2)
    {
      ancestors[cHash] = new Link(pHash, child.Cost());
    }

    return exist;
  }

  public Node PickBest(bool pop)
  {
    if (!sorted)
    {
      // https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.sort?view=net-7.0
      openNodes.Sort(delegate (Node lhs, Node rhs)
      {
        var cLhs = lhs.Cost() + lhs.Heuristic();
        var cRhs = rhs.Cost() + rhs.Heuristic();
        // https://learn.microsoft.com/en-us/dotnet/api/system.icomparable.compareto?view=net-7.0
        if (cLhs < cRhs)
        {
          return -1;
        }
        else if (cLhs > cRhs)
        {
          return 1;
        }
        else
        {
          return 0;
        }
      });
      sorted = true;
    }

    Node best = openNodes[0];
    if (pop)
    {
      openNodes.RemoveAt(0);
    }

    return best;
  }

  public Path Reconstruct(Vector2Int end)
  {
    var outPath = new Path(end);

    string h = Node.Hash(end);

    Link cur = new Link("", 0.0f);
    bool exist = ancestors.TryGetValue(h, out cur);
    bool foundRoot = false;

    while (exist && !foundRoot)
    {
      var p = Node.Unhash(h);

      outPath.Add(p, false);

      foundRoot = (h == cur.Item1);
      h = cur.Item1;
      exist = ancestors.TryGetValue(h, out cur);
    }

    return outPath;
  }

}
