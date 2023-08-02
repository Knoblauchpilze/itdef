
using UnityEngine;

public struct PortalConfiguration
{
  public WaveConfiguration waveConf;
  public float spawnIntervalInSeconds;
  public float destroyOnArrivalGracePeriod;
  public PathManager pathManager;

  public Vector3 GenerateSpawnPosition(Vector3 o)
  {
    return o;
  }
}
