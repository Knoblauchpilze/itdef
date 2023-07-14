using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
  private Vector3 spawnCenter;
  private Timer waveTimer;
  private PortalConfiguration config;

  public GameObject enemyPrefab;
  public GameObject baseToDestroy;

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
    if (GameStateData.state != State.Play)
    {
      return;
    }

    var ready = waveTimer.Accumulate(Time.deltaTime);

    if (ready)
    {
      SpawnEnemyWave();
    }
  }

  public void Configure(PortalConfiguration inConfig)
  {
    config = inConfig;
    waveTimer = new Timer(config.spawnIntervalInSeconds);
  }

  public void SpawnEnemyWave()
  {
    var waveSize = config.waveConf.GetWaveSize();

    for (var id = 0; id < waveSize; ++id)
    {
      SpawnEnemy();
    }

    waveTimer.Reset();
  }

  void SpawnEnemy()
  {
    var health = config.waveConf.enemyConf.GetEnemyHealth();
    var pos = config.GenerateSpawnPosition(spawnCenter);

    var enemy = Instantiate(enemyPrefab, pos, enemyPrefab.transform.rotation);
    ConfigureEnemy(enemy);

  }

  void ConfigureEnemy(GameObject enemy)
  {
    GoToTarget behavior = enemy.GetComponent<GoToTarget>();

    var motionConf = new MotionConfiguration();
    motionConf.target = baseToDestroy;
    motionConf.speed = config.waveConf.enemyConf.GetEnemySpeed();
    motionConf.destroyOnArrivalGracePeriod = config.destroyOnArrivalGracePeriod;

    behavior.Configure(motionConf);
  }

  public float TimeToNextWave()
  {
    return waveTimer.Remaining();
  }
}
