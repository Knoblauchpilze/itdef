using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToTarget : MonoBehaviour
{
  private GameManager gameManager;
  private MotionConfiguration config;
  private bool reachedBase;
  private Threat threat;
  private Path path = new Path();
  private Vector3 currentTarget;

  private float ARRIVAL_THRESHOLD = 0.005f;

  // Start is called before the first frame update
  void Start()
  {
    reachedBase = false;
    gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    threat = gameObject.GetComponent<Threat>();
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
  }

  void OnTriggerEnter(Collider other)
  {
    if (other.gameObject == config.target)
    {
      reachedBase = true;
      threat.ReachedBase();
    }
  }

  public void Configure(MotionConfiguration inConfig)
  {
    config = inConfig;
  }

  public void InvalidatePath()
  {
    path.Clear();
  }

  Vector2Int GetCurrentPosition()
  {
    return VectorUtils.ConvertTo2dIntTile(gameObject.transform.position);
  }

  void UpdateTarget()
  {
    if (!path.Empty() && !CurrentTargetIsReached())
    {
      return;
    }

    if (path.Empty())
    {
      GeneratePath();
      if (!path.Empty())
      {
        var next = path.Peek();
        var pos = GetCurrentPosition();
        if (pos == next)
        {
          path.Advance();
        }
      }
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
    var end = VectorUtils.ConvertTo2dIntTile(config.target.transform.position);

    AStar astar = new AStar(start, end, config.locator);
    path = astar.FindUnboundedPath();
    Debug.Log("Generated path with size " + path.Size());
  }

  void MoveToNextTargetOnPath()
  {
    if (path.Empty())
    {
      return;
    }

    Debug.Log("Arrived within " + ARRIVAL_THRESHOLD + " of " + currentTarget + " (pos: " + gameObject.transform.position + "), moving to next target");

    var target = path.Advance();
    currentTarget = VectorUtils.ConvertTo3dFloat(target, config.target.transform.position.z);

    Debug.Log("Picked next target " + currentTarget);
  }
}
