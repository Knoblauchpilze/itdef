using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToTarget : MonoBehaviour
{
  private GameManager gameManager;
  private MotionConfiguration config;
  private bool reachedBase;
  private Timer destroyTimer;
  private Path path = new Path();
  private bool hasTarget = false;
  private Vector3 currentTarget;

  private float ARRIVAL_THRESHOLD = 0.005f;

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

    UpdateTarget();

    var dir = currentTarget - gameObject.transform.position;
    var d = dir.magnitude;
    dir.Normalize();
    var delta = Mathf.Min(config.speed * Time.deltaTime, d);

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

  Vector2Int GetCurrentPosition()
  {
    return VectorUtils.ConvertTo2d(gameObject.transform.position);
  }

  void UpdateTarget()
  {
    if (hasTarget && !CurrentTargetIsReached())
    {
      return;
    }

    if (path.Empty())
    {
      GeneratePath();
    }

    MoveToNextTargetOnPath();
  }

  bool CurrentTargetIsReached()
  {
    var currentTarget2d = VectorUtils.ConvertTo2dFloat(currentTarget);
    var pos2d = VectorUtils.ConvertTo2dFloat(gameObject.transform.position);
    var d = Vector2.Distance(currentTarget2d, pos2d);
    return d <= ARRIVAL_THRESHOLD;
  }

  void GeneratePath()
  {
    if (reachedBase)
    {
      return;
    }

    var start = GetCurrentPosition();
    var end = VectorUtils.ConvertTo2d(config.target.transform.position);

    AStar astar = new AStar(start, end, config.locator);
    path = astar.FindUnboundedPath();
    Debug.Log("Generated path with size " + path.Size());
    for (int i = 0; i < path.Size(); ++i)
    {
      Debug.Log("Point " + i + " is: " + path.At(i));
    }
  }

  void MoveToNextTargetOnPath()
  {
    if (path.Empty())
    {
      return;
    }

    Debug.Log("Arrived within " + ARRIVAL_THRESHOLD + " of " + currentTarget + " (pos: " + gameObject.transform.position + "), moving to next target");

    var target = path.Advance();
    currentTarget = VectorUtils.ConvertTo3d(target, config.target.transform.position.z);
    hasTarget = true;

    Debug.Log("Picked next target " + currentTarget);
  }
}
