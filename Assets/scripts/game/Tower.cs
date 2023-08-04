using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
  private TowerConfiguration config;
  private Timer attackTimer;
  private Vector2Int pos;

  // Start is called before the first frame update
  void Start()
  {
    pos = VectorUtils.ConvertTo2dIntTile(gameObject.transform.position);
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
    }
  }

  public void Configure(TowerConfiguration inConfig)
  {
    config = inConfig;
    attackTimer = new Timer(config.reloadTime);
  }

  void TryToAttack()
  {
    var enemy = config.finder.FindClosestWithinRadius(pos, config.range);
    if (enemy == null)
    {
      return;
    }

    AttackEnemy(enemy);

    attackTimer.Reset();
  }

  void AttackEnemy(GameObject enemy)
  {
    var mob = enemy.GetComponent<Mob>();
    mob.Damage(config.damage);
  }
}
