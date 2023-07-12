using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MotionConfiguration
{
  public float speed;
  public float destroyOnArrivalGracePeriod;
}

public class GoToTarget : MonoBehaviour
{
  private GameManager gameManager;

  public GameObject target;
  public MotionConfiguration config;


  // Start is called before the first frame update
  void Start()
  {
    gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
  }

  // Update is called once per frame
  void Update()
  {
    if (GameStateData.state != State.Play)
    {
      return;
    }

    var dir = target.transform.position - transform.position;
    dir.Normalize();
    var delta = config.speed * Time.deltaTime;

    transform.Translate(delta * dir);
  }

  void OnTriggerEnter(Collider other)
  {
    if (other.gameObject == target)
    {
      Invoke("destroyAfterGracePeriod", config.destroyOnArrivalGracePeriod);
    }
  }

  void destroyAfterGracePeriod()
  {
    gameManager.EnemyPassedThroughBase();
    Destroy(gameObject);
  }
}
