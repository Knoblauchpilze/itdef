using UnityEngine;

public interface Router : Locator
{
  void RegisterMobile(GoToTarget mobile);

  bool WouldObstaclePreventPath(Vector2Int start, Vector2Int end, Vector2Int obstacle, Vector2Int xRange, Vector2Int yRange);
}
