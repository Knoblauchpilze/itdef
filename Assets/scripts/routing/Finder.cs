using System.Collections.Generic;
using UnityEngine;

public interface Finder
{
  List<GameObject> FindAllWithinRadius(Vector2Int pos, float radius);

  GameObject FindClosestWithinRadius(Vector2Int pos, float radius);
}
