
using UnityEngine;

public struct WaveConfiguration
{
  public EnemyConfiguration enemyConf;
  public int minCount;
  public int maxCount;

  public int GetWaveSize()
  {
    return Random.Range(minCount, maxCount + 1);
  }
}
