using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {
    var portals = GameObject.FindGameObjectsWithTag("portal");
    Debug.Log("Difficulty: " + GameStateData.difficulty);
    foreach (GameObject portal in portals)
    {
      configurePortal(portal, GameStateData.difficulty);
    }
  }

  // Update is called once per frame
  void Update()
  {

  }

  void configurePortal(GameObject portal, Difficulty difficulty)
  {
    var script = portal.GetComponent<Portal>();
    script.config = generatePortalConfiguration(difficulty);
  }

  PortalConfiguration generatePortalConfiguration(Difficulty difficulty)
  {
    var conf = new PortalConfiguration();
    conf.waveConf = generateWaveConfiguration(difficulty);

    conf.spawnIntervalInSeconds = 2.0f;
    conf.destroyOnArrivalGracePeriod = 1.0f;
    conf.minSpawnDistance = 0.5f;
    conf.spawnRadius = 2.2f;

    return conf;
  }

  WaveConfiguration generateWaveConfiguration(Difficulty difficulty)
  {
    var conf = new WaveConfiguration();
    conf.enemyConf = generateEnemyConfiguration(difficulty);

    conf.minCount = 1;
    conf.maxCount = 1;

    return conf;
  }

  EnemyConfiguration generateEnemyConfiguration(Difficulty difficulty)
  {
    var conf = new EnemyConfiguration();
    conf.minHealth = 1;
    conf.maxHealth = 1;

    conf.minSpeed = 1;
    conf.maxSpeed = 1;

    return conf;
  }
}
