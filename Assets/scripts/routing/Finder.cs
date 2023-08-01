using System.Collections.Generic;
using UnityEngine;

public interface Finder
{
  List<Vector2Int> FindAllWithinRadius(Vector2Int pos, float radius);
}
