using System;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
  // https://stackoverflow.com/questions/18895727/c-vector-like-class-in-c-sharp
  private List<Vector2Int> points = new List<Vector2Int>();

  public Path(Vector2Int start)
  {
    Add(start, false);
  }

  public void Add(Vector2Int p, bool duplicate)
  {
    if (Empty())
    {
      points.Add(p);
      return;
    }

    Vector2Int l = End();
    if (!duplicate && l == p)
    {
      return;
    }

    points.Add(p);
  }

  public Vector2Int At(int id)
  {
    if (id > points.Count)
    {
      throw new ArgumentOutOfRangeException("Failed to access point at " + id,
            "Path only defines " + points.Count + " value(s)");
    }

    return points[id];
  }

  public void Clear()
  {
    points.Clear();
  }

  public void Reverse()
  {
    points.Reverse();
  }

  public Vector2Int Begin()
  {
    if (Empty())
    {
      throw new ArgumentOutOfRangeException("Failed to get starting point of empty path");
    }

    return points[0];
  }

  public Vector2Int End()
  {
    if (Empty())
    {
      throw new ArgumentOutOfRangeException("Failed to get ending point of empty path");
    }

    return points[points.Count - 1];
  }

  public int Size()
  {
    return points.Count;
  }

  public bool Empty()
  {
    return points.Count == 0;
  }

  public Vector2Int Advance()
  {
    if (Empty())
    {
      throw new ArgumentOutOfRangeException("Failed to advance on empty path");
    }

    Vector2Int p = points[0];
    points.RemoveAt(0);

    return p;
  }

}
