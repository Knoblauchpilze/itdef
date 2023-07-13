using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToTarget : MonoBehaviour
{
  private GameManager gameManager;
  private MotionConfiguration config;
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

    var dir = config.target.transform.position - transform.position;
    dir.Normalize();
    var delta = config.speed * Time.deltaTime;

    transform.Translate(delta * dir);

    if (reachedBase)
    {
      if (destroyTimer.Accumulate(Time.deltaTime))
      {
        DestroyAfterGracePeriod();
      }
    }
  }

  void OnTriggerEnter(Collider other)
  {
    if (other.gameObject == config.target)
    {
      reachedBase = true;
      destroyTimer = new Timer(config.destroyOnArrivalGracePeriod);
    }
  }

  public void Configure(MotionConfiguration inConfig)
  {
    config = inConfig;
  }

  void DestroyAfterGracePeriod()
  {
    gameManager.EnemyPassedThroughBase();
    Destroy(gameObject);
  }
}
