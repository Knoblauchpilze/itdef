
using System.Collections.Generic;
using UnityEngine;

// https://www.w3schools.com/cs/cs_interface.php
public class GameMap : Locator
{
  private List<Portal> portals = new List<Portal>();
  private HashSet<string> usedCoordinates = new HashSet<string>();

  public bool Obstructed(Vector2Int pos)
  {
    return usedCoordinates.Contains(Node.Hash(pos));
  }

  public void AddPortal(Portal portal, Vector2Int pos)
  {
    if (usedCoordinates.Add(Node.Hash(pos)))
    {
      portals.Add(portal);
    }
  }
}