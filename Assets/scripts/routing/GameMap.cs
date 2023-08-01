
using System.Collections.Generic;
using UnityEngine;

// https://www.w3schools.com/cs/cs_interface.php
public class GameMap : Router, Finder
{
  private HashSet<string> usedCoordinates = new HashSet<string>();
  private List<GoToTarget> movingElements = new List<GoToTarget>();

  public bool Obstructed(Vector2Int pos)
  {
    return usedCoordinates.Contains(Node.Hash(pos));
  }

  public void RegisterMobile(GoToTarget mobile)
  {
    movingElements.Add(mobile);
  }

  public bool WouldObstaclePreventPath(Vector2Int start, Vector2Int end, Vector2Int obstacle, Vector2Int xRange, Vector2Int yRange)
  {
    if (usedCoordinates.Contains(Node.Hash(obstacle)))
    {
      return false;
    }

    usedCoordinates.Add(Node.Hash(obstacle));

    AStar astar = new AStar(start, end, this);
    var path = astar.FindPathWithin(xRange.x, xRange.y, yRange.x, yRange.y);

    usedCoordinates.Remove(Node.Hash(obstacle));

    return path == null || path.Empty();
  }

  public List<Vector2Int> FindAllWithinRadius(Vector2Int pos, float radius)
  {
    return new List<Vector2Int>();
  }

  public void AddPortal(Vector2Int pos)
  {
    usedCoordinates.Add(Node.Hash(pos));
  }

  public void AddTower(Vector2Int pos)
  {
    usedCoordinates.Add(Node.Hash(pos));
  }

  public void AddWall(Vector2Int pos)
  {
    usedCoordinates.Add(Node.Hash(pos));
  }

  public void InvalidatePaths()
  {
    foreach (GoToTarget mobile in movingElements)
    {
      mobile.InvalidatePath();
    }
  }
}
