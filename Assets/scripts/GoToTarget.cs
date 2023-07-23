using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToTarget : MonoBehaviour
{
  private GameManager gameManager;
  private MotionConfiguration config;
  private bool reachedBase;
  private Timer destroyTimer;
  private Path path;
  private bool hasTarget = false;
  private Vector3 currentTarget;

  private float ARRIVAL_THRESHOLD = 0.001f;

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

    if (!reachedBase)
    {
      UpdateTarget();
    }

    var dir = currentTarget - gameObject.transform.position;
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

  void GeneratePath()
  {
    var start = GetCurrentPosition();
    var end = VectorUtils.ConvertTo2d(config.target.transform.position);

    AStar astar = new AStar(start, end, config.locator);
    path = astar.FindUnboundedPath();
    Debug.Log("Generated path with size " + path.Size());
  }

  Vector2Int GetCurrentPosition()
  {
    return VectorUtils.ConvertTo2d(gameObject.transform.position);
  }

  void UpdateTarget()
  {
    if (hasTarget)
    {
      var currentTarget2d = VectorUtils.ConvertTo2dFloat(currentTarget);
      var pos2d = VectorUtils.ConvertTo2dFloat(gameObject.transform.position);
      var d = Vector2.Distance(currentTarget2d, pos2d);
      if (d > ARRIVAL_THRESHOLD)
      {
        return;
      }

      Debug.Log("Arrived within " + d + " of " + currentTarget + " (pos: " + gameObject.transform.position + ", 2d: " + pos2d + "), moving to next target");
    }

    if (path == null || path.Empty())
    {
      GeneratePath();
    }

    var target = path.Advance();
    currentTarget = VectorUtils.ConvertTo3d(target, config.target.transform.position.z);
    hasTarget = true;

    Debug.Log("Picked next target " + currentTarget);
  }
}
