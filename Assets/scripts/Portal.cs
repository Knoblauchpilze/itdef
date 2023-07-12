using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EnemyConfiguration
{
  public int minHealth;
  public int maxHealth;
  public float minSpeed;
  public float maxSpeed;

  public int getEnemyHealth()
  {
    return Random.Range(minHealth, maxHealth + 1);
  }

  public float getEnemySpeed()
  {
    return Random.Range(minSpeed, maxSpeed);
  }
}

public struct WaveConfiguration
{
  public EnemyConfiguration enemyConf;
  public int minCount;
  public int maxCount;

  public int getWaveSize()
  {
    return Random.Range(minCount, maxCount + 1);
  }
}

public struct PortalConfiguration
{
  public WaveConfiguration waveConf;
  public float spawnIntervalInSeconds;
  public float destroyOnArrivalGracePeriod;
  public float minSpawnDistance;
  public float spawnRadius;

  public Vector3 generateSpawnPosition(Vector3 o)
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

public class Portal : MonoBehaviour
{
  private Vector3 spawnCenter;
  private bool spawnInvoked = false;

  public GameObject enemyPrefab;
  public GameObject baseToDestroy;
  public PortalConfiguration config;

  // Start is called before the first frame update
  void Start()
  {
    var boxCollider = GetComponent<BoxCollider>();
    spawnCenter = boxCollider.transform.position;

    baseToDestroy = GameObject.Find("Base");
  }

  // Update is called once per frame
  void Update()
  {
    if (GameStateData.state == State.Play && !spawnInvoked)
    {
      spawnInvoked = true;
      Invoke("spawnEnemyWave", config.spawnIntervalInSeconds);
    }
    if (GameStateData.state == State.Paused)
    {
      spawnInvoked = false;
    }
  }

  void spawnEnemyWave()
  {
    if (GameStateData.state != State.Play)
    {
      return;
    }

    var waveSize = config.waveConf.getWaveSize();

    for (var id = 0; id < waveSize; ++id)
    {
      spawnEnemy();
    }

    Invoke("spawnEnemyWave", config.spawnIntervalInSeconds);
  }

  void spawnEnemy()
  {
    var health = config.waveConf.enemyConf.getEnemyHealth();
    var pos = config.generateSpawnPosition(spawnCenter);

    var enemy = Instantiate(enemyPrefab, pos, enemyPrefab.transform.rotation);
    configureEnemy(enemy);

  }

  void configureEnemy(GameObject enemy)
  {
    GoToTarget behavior = enemy.GetComponent<GoToTarget>();
    behavior.target = baseToDestroy;
    behavior.config.speed = config.waveConf.enemyConf.getEnemySpeed();
    behavior.config.destroyOnArrivalGracePeriod = config.destroyOnArrivalGracePeriod;
  }
}
