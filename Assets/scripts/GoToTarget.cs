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
  public GameObject target;
  public MotionConfiguration config;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
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
    Destroy(gameObject);
  }
}
