
using System.Collections.Generic;
using UnityEngine;

// https://www.w3schools.com/cs/cs_interface.php
public class GameMap : PathManager, Finder
{
  // https://www.shekhali.com/csharp-hashtable-vs-dictionary-vs-hashset/
  private Dictionary<string, GameObject> usedCoordinates = new Dictionary<string, GameObject>();
  private List<GoToTarget> movingElements = new List<GoToTarget>();

  public bool Obstructed(Vector2Int pos)
  {
    return usedCoordinates.ContainsKey(Node.Hash(pos));
  }

  public void RegisterMobile(GoToTarget mobile)
  {
    movingElements.Add(mobile);
  }

  public void UnregisterMobile(GoToTarget mobile)
  {
    Debug.Log("Unregistering mobile");
    movingElements.Remove(mobile);
  }

  public bool WouldObstaclePreventPath(Vector2Int start, Vector2Int end, Vector2Int obstacle, Vector2Int xRange, Vector2Int yRange)
  {
    if (usedCoordinates.ContainsKey(Node.Hash(obstacle)))
    {
      return false;
    }

    usedCoordinates.Add(Node.Hash(obstacle), null);

    AStar astar = new AStar(start, end, this);
    var path = astar.FindPathWithin(xRange.x, xRange.y, yRange.x, yRange.y);

    usedCoordinates.Remove(Node.Hash(obstacle));

    return path == null || path.Empty();
  }

  public List<GameObject> FindAllWithinRadius(Vector2Int pos, float radius)
  {
    var objects = new List<GameObject>();

    // https://stackoverflow.com/questions/141088/how-to-iterate-over-a-dictionary
    foreach (var entry in usedCoordinates)
    {
      var coord = Node.Unhash(entry.Key);
      var delta = Vector2Int.Distance(pos, coord);
      if (delta <= radius)
      {
        objects.Add(entry.Value);
      }
    }

    return objects;
  }

  public void AddPortal(Vector2Int pos, GameObject entity)
  {
    usedCoordinates.Add(Node.Hash(pos), entity);
  }

  public void AddTower(Vector2Int pos, GameObject entity)
  {
    usedCoordinates.Add(Node.Hash(pos), entity);
  }

  public void AddWall(Vector2Int pos, GameObject entity)
  {
    usedCoordinates.Add(Node.Hash(pos), entity);
  }

  public void InvalidatePaths()
  {
    Debug.Log("Invalidating for " + movingElements.Count);
    foreach (GoToTarget mobile in movingElements)
    {
      mobile.InvalidatePath();
    }
  }
}
