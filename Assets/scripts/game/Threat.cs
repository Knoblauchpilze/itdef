using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Threat : MonoBehaviour
{
  private GameManager gameManager;
  private ThreatConfiguration config;
  private bool reachedBase;
  private Timer destroyTimer;

  // Start is called before the first frame update
  void Start()
  {
    reachedBase = false;
    gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
  }

  // Update is called once per frame
  void Update()
  {
    if (GameStateData.state != State.Play)
    {
      return;
    }

    if (reachedBase)
    {
      if (destroyTimer.Accumulate(Time.deltaTime))
      {
        DestroyAfterGracePeriod();
      }
    }
  }

  public void ReachedBase()
  {
    reachedBase = true;
    destroyTimer = new Timer(config.destroyOnArrivalGracePeriod);
  }

  public void Configure(ThreatConfiguration inConfig)
  {
    config = inConfig;
  }

  void DestroyAfterGracePeriod()
  {
    gameManager.EnemyPassedThroughBase();
    Destroy(gameObject);
  }
}
