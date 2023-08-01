using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
  private TowerConfiguration config;
  private Timer attackTimer;

  // Start is called before the first frame update
  void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {
    if (GameStateData.state != State.Play)
    {
      return;
    }

    var ready = attackTimer.Accumulate(Time.deltaTime);
    if (ready)
    {
      TryToAttack();
      Debug.Log("Should attack");
    }
  }

  public void Configure(TowerConfiguration inConfig)
  {
    config = inConfig;
    attackTimer = new Timer(config.reloadTime);
  }

  void TryToAttack()
  {
    attackTimer.Reset();
  }
}
