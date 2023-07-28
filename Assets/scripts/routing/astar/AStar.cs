using UnityEngine;

class AstarSettings
{
  public bool hasBounds = false;
  public Vector2Int xRange;
  public Vector2Int yRange;
  public bool hasRadius = false;
  public float radius;

  public bool ShouldConsiderNode(Node node)
  {
    if (!hasBounds)
    {
      return true;
    }

    var pos = node.Pos();
    if (pos.x < xRange.x || pos.x > xRange.y)
    {
      return false;
    }
    if (pos.y < yRange.x || pos.y > yRange.y)
    {
      return false;
    }

    return true;
  }

}

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

  public Path FindPathWithin(int minX, int maxX, int minY, int maxY)
  {
    var settings = new AstarSettings();
    settings.hasBounds = true;
    settings.xRange = new Vector2Int(minX, maxX);
    settings.yRange = new Vector2Int(minY, maxY);

    return FindPath(settings);
  }

  Path FindPath(AstarSettings settings)
  {
    Path outPath;

    AStarNodes nodes = new AStarNodes();
    nodes.Seed(start, Vector2Int.Distance(start, end));

    while (!nodes.Stuck())
    {
      Node current = nodes.PickBest(true);

      if (current.Contains(end))
      {
        if (!Reconstruct(nodes, settings, out outPath))
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

        if (!settings.ShouldConsiderNode(neighbor))
        {
          continue;
        }

        nodes.Explore(neighbor, current.Pos());
      }
    }

    return null;
  }

  bool Reconstruct(AStarNodes nodes, AstarSettings settings, out Path path)
  {
    path = null;
    Path outPath = nodes.Reconstruct(end);
    outPath.Reverse();

    if (outPath.Begin() != start)
    {
      return false;
    }

    if (PathTooFar(outPath, start, settings))
    {
      return false;
    }

    // https://stackoverflow.com/questions/804706/swap-two-variables-without-using-a-temporary-variable
    path = outPath;

    return true;
  }

  bool PathTooFar(Path path, Vector2Int p, AstarSettings settings)
  {
    // In case the `radius` is negative, consider
    // that there's no limit.
    if (!settings.hasRadius)
    {
      return false;
    }

    bool bounded = false;
    int id = 0;

    while (id < path.Size() && bounded)
    {
      bounded = (Vector2Int.Distance(p, path.At(id)) > settings.radius);
      ++id;
    }

    return bounded;
  }

}
