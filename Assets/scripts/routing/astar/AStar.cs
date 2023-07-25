using UnityEngine;

public class AStar
{
  private Vector2Int start;
  private Vector2Int end;
  private Locator locator;

  public AStar(Vector2Int s, Vector2Int e, Locator loc)
  {
    start = s;
    end = e;
    locator = loc;
  }

  public Path FindUnboundedPath()
  {
    return FindPath(-1.0f);
  }

  Path FindPath(float radius)
  {
    Path outPath;

    AStarNodes nodes = new AStarNodes();
    nodes.Seed(start, Vector2Int.Distance(start, end));

    while (!nodes.Stuck())
    {
      Node current = nodes.PickBest(true);

      if (current.Contains(end))
      {
        if (!Reconstruct(nodes, radius, out outPath))
        {
          return null;
        }

        return outPath;
      }

      var neighbors = current.GenerateNeighbors(end);
      for (int id = 0; id < neighbors.Count; ++id)
      {
        Node neighbor = neighbors[id];

        if (locator.Obstructed(neighbor.Pos()) && !neighbor.Contains(end))
        {
          continue;
        }

        if (radius > 0.0f && Vector2Int.Distance(start, neighbor.Pos()) >= radius)
        {
          continue;
        }

        nodes.Explore(neighbor, current.Pos());
      }
    }

    return null;
  }

  bool Reconstruct(AStarNodes nodes, float radius, out Path path)
  {
    path = null;
    Path outPath = nodes.Reconstruct(end);
    outPath.Reverse();

    if (outPath.Begin() != start)
    {
      return false;
    }

    if (PathTooFar(outPath, start, radius))
    {
      return false;
    }

    // https://stackoverflow.com/questions/804706/swap-two-variables-without-using-a-temporary-variable
    path = outPath;

    return true;
  }

  bool PathTooFar(Path path, Vector2Int p, float d)
  {
    // In case the `radius` is negative, consider
    // that there's no limit.
    if (d < 0.0f)
    {
      return false;
    }

    bool bounded = false;
    int id = 0;

    while (id < path.Size() && bounded)
    {
      bounded = (Vector2Int.Distance(p, path.At(id)) > d);
      ++id;
    }

    return bounded;
  }

}
