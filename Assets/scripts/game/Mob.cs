using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
  private MobConfiguration config;
  private float health;
  private BuildManager buildManager;

  // Start is called before the first frame update
  void Start()
  {
    buildManager = GameObject.Find("BuildManager").GetComponent<BuildManager>();
  }

  // Update is called once per frame
  void Update()
  {
    if (health <= 0.0f)
    {
      Destroy(gameObject);
    }
  }

  void OnDestroy()
  {
    if (health > 0.0f)
    {
      return;
    }

    buildManager.EnemyKilled(config.reward);
  }

  public void Configure(MobConfiguration inConfig)
  {
    config = inConfig;
    health = config.health;
  }

  public void Damage(float damage)
  {
    health -= damage;
    Debug.Log("Mob took " + damage + " damage, " + health + " hp left");
  }
}
