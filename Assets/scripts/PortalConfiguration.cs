
using UnityEngine;

public struct PortalConfiguration
{
  public WaveConfiguration waveConf;
  public float spawnIntervalInSeconds;
  public float destroyOnArrivalGracePeriod;
  public float minSpawnDistance;
  public float spawnRadius;

  public Vector3 GenerateSpawnPosition(Vector3 o)
  {
    var pos = new Vector3(o.x, o.y, o.z);

    var dMin = -spawnRadius / 2.0f;
    var dMax = spawnRadius / 2.0f;

    var dx = Random.Range(dMin, dMax);
    var dy = Random.Range(dMin, dMax);
    dx += (dx > 0 ? minSpawnDistance : -minSpawnDistance);
    dy += (dy > 0 ? minSpawnDistance : -minSpawnDistance);

    pos.x += dx;
    pos.y += dy;

    return pos;
  }
}
