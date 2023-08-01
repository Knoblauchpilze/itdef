
using UnityEngine;

public struct EnemyConfiguration
{
  public int minHealth;
  public int maxHealth;
  public float minSpeed;
  public float maxSpeed;
  public int reward;


  public int GetEnemyHealth()
  {
    return Random.Range(minHealth, maxHealth + 1);
  }

  public float GetEnemySpeed()
  {
    return Random.Range(minSpeed, maxSpeed);
  }
}
