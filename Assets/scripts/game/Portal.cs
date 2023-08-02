using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
  private Vector3 spawnCenter;
  private Timer waveTimer;
  private PortalConfiguration config;
  private MapManager mapManager;

  public GameObject enemyPrefab;
  public GameObject baseToDestroy;

  // Start is called before the first frame update
  void Start()
  {
    var boxCollider = GetComponent<BoxCollider>();
    spawnCenter = boxCollider.transform.position;

    baseToDestroy = GameObject.Find("Base");

    mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
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

    var mobile = enemy.GetComponent<GoToTarget>();
    config.pathManager.RegisterMobile(mobile);
  }

  void ConfigureEnemy(GameObject enemy)
  {
    GoToTarget behavior = enemy.GetComponent<GoToTarget>();

    var motionConf = new MotionConfiguration();
    motionConf.target = baseToDestroy;
    motionConf.speed = config.waveConf.enemyConf.GetEnemySpeed();
    motionConf.pathManager = config.pathManager;
    motionConf.xRange = mapManager.GetXRange();
    motionConf.yRange = mapManager.GetYRange();

    behavior.Configure(motionConf);

    Threat threat = enemy.GetComponent<Threat>();

    var threatConf = new ThreatConfiguration();
    threatConf.destroyOnArrivalGracePeriod = config.destroyOnArrivalGracePeriod;

    threat.Configure(threatConf);

    Mob mob = enemy.GetComponent<Mob>();

    var mobConf = new MobConfiguration();
    mobConf.health = config.waveConf.enemyConf.GetEnemyHealth();
    mobConf.reward = config.waveConf.enemyConf.reward;

    mob.Configure(mobConf);
  }

  public float TimeToNextWave()
  {
    return waveTimer.Remaining();
  }
}
