
using System.Collections.Generic;
using UnityEngine;

// https://www.w3schools.com/cs/cs_interface.php
public class GameMap : Router
{
  private HashSet<string> usedCoordinates = new HashSet<string>();
  private List<GoToTarget> movingElements = new List<GoToTarget>();

  public bool Obstructed(Vector2Int pos)
  {
    return usedCoordinates.Contains(Node.Hash(pos));
  }

  public bool WouldObstaclePreventPath(Vector2Int start, Vector2Int end, Vector2Int obstacle)
  {
    return false;
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

  public void RegisterMobile(GoToTarget mobile)
  {
    movingElements.Add(mobile);
  }

  public void InvalidatePaths()
  {
    foreach (GoToTarget mobile in movingElements)
    {
      mobile.InvalidatePath();
    }
  }
}
