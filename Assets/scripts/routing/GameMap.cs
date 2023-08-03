
using System.Collections.Generic;
using UnityEngine;

class MovingElement
{
  public GoToTarget behavior;
  public GameObject gameObject;

  public MovingElement(GameObject inObject)
  {
    gameObject = inObject;
    var inBehavior = inObject.GetComponent<GoToTarget>();
    if (inBehavior != null)
    {
      behavior = inBehavior;
    }
  }
}

// https://www.w3schools.com/cs/cs_interface.php
public class GameMap : PathManager, Finder
{
  // https://www.shekhali.com/csharp-hashtable-vs-dictionary-vs-hashset/
  private Dictionary<string, GameObject> usedCoordinates = new Dictionary<string, GameObject>();
  private List<MovingElement> movingElements = new List<MovingElement>();

  public bool Obstructed(Vector2Int pos)
  {
    return usedCoordinates.ContainsKey(Node.Hash(pos));
  }

  public void RegisterMobile(GameObject mobile)
  {
    var entry = new MovingElement(mobile);
    movingElements.Add(entry);
  }

  public void UnregisterMobile(GameObject mobile)
  {
    movingElements.RemoveAll(m => m.gameObject == mobile);
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
    var pos2d = new Vector2(pos.x, pos.y);

    foreach (var entry in movingElements)
    {
      var coord = VectorUtils.ConvertTo2dFloat(entry.gameObject.transform.position);
      var delta = Vector2.Distance(pos2d, coord);
      if (delta <= radius)
      {
        objects.Add(entry.gameObject);
      }
    }

    objects.Sort(delegate (GameObject lhs, GameObject rhs)
    {
      var posLhs = VectorUtils.ConvertTo2dFloat(lhs.transform.position);
      var posRhs = VectorUtils.ConvertTo2dFloat(rhs.transform.position);

      var dLhs = Vector2.Distance(posLhs, pos2d);
      var dRhs = Vector2.Distance(posRhs, pos2d);
      if (dLhs < dRhs)
      {
        return -1;
      }
      else if (dLhs > dRhs)
      {
        return 1;
      }
      else
      {
        return 0;
      }
    });

    return objects;
  }

  public GameObject FindClosestWithinRadius(Vector2Int pos, float radius)
  {
    var objects = FindAllWithinRadius(pos, radius);

    if (objects.Count == 0)
    {
      return null;
    }

    return objects[0];
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
    foreach (MovingElement mobile in movingElements)
    {
      if (mobile.behavior != null)
      {
        mobile.behavior.InvalidatePath();
      }
    }
  }
}
