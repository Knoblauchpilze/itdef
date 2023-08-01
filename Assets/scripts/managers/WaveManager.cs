using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
  private List<Portal> portals;
  private MapManager mapManager;

  public TextMeshProUGUI waveTimerText;

  // Start is called before the first frame update
  void Start()
  {
    mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
    GetAndConfigurePortals();
  }

  // Update is called once per frame
  void Update()
  {
    var minRemainingTime = float.MaxValue;
    foreach (Portal portal in portals)
    {
      var remainingTimeInSeconds = portal.TimeToNextWave();
      if (remainingTimeInSeconds < minRemainingTime)
      {
        minRemainingTime = remainingTimeInSeconds;
      }
    }

    var secondsRemaining = Mathf.FloorToInt(minRemainingTime);
    waveTimerText.SetText("Time: " + secondsRemaining + "s");
  }

  void GetAndConfigurePortals()
  {
    portals = new List<Portal>();

    var rawPortals = GameObject.FindGameObjectsWithTag("portal");
    foreach (GameObject rawPortal in rawPortals)
    {
      var portal = rawPortal.GetComponent<Portal>();
      portal.Configure(GeneratePortalConfiguration(GameStateData.difficulty));
      portals.Add(portal);
    }
  }

  PortalConfiguration GeneratePortalConfiguration(Difficulty difficulty)
  {
    var conf = new PortalConfiguration();
    conf.waveConf = GenerateWaveConfiguration(difficulty);

    conf.spawnIntervalInSeconds = 20.0f;
    conf.destroyOnArrivalGracePeriod = 2.0f;

    conf.router = mapManager.GetRouter();

    return conf;
  }

  WaveConfiguration GenerateWaveConfiguration(Difficulty difficulty)
  {
    var conf = new WaveConfiguration();
    conf.enemyConf = GenerateEnemyConfiguration(difficulty);

    conf.minCount = 1;
    conf.maxCount = 1;

    return conf;
  }

  EnemyConfiguration GenerateEnemyConfiguration(Difficulty difficulty)
  {
    var conf = new EnemyConfiguration();
    conf.minHealth = 1;
    conf.maxHealth = 1;

    conf.minSpeed = 1;
    conf.maxSpeed = 1;

    conf.reward = EnemyReward.Get();

    return conf;
  }

  public void TriggerNextWave()
  {
    foreach (Portal portal in portals)
    {
      portal.SpawnEnemyWave();
    }
  }
}
