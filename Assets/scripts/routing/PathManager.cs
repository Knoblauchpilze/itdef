using UnityEngine;

public interface PathManager : Locator
{
  void RegisterMobile(GameObject mobile);

  void UnregisterMobile(GameObject mobile);

  bool WouldObstaclePreventPath(Vector2Int start, Vector2Int end, Vector2Int obstacle, Vector2Int xRange, Vector2Int yRange);
}
