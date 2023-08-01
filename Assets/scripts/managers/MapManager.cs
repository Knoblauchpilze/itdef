using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
  private GameMap gameMap = new GameMap();
  private Vector2Int xRange;
  private Vector2Int yRange;

  // Start is called before the first frame update
  void Start()
  {
    var ground = GameObject.Find("Ground");
    var collider = ground.GetComponent<MeshCollider>();
    var minGround = collider.bounds.min;
    var maxGround = collider.bounds.max;

    xRange = VectorUtils.ConvertTo2dIntTile(new Vector3(minGround.x, maxGround.x, 0.0f));
    yRange = VectorUtils.ConvertTo2dIntTile(new Vector3(minGround.y, maxGround.y, 0.0f));

    GetAndRegisterPortals();
    GetAndRegisterWalls();
  }

  // Update is called once per frame
  void Update()
  {
  }

  void GetAndRegisterPortals()
  {
    var rawPortals = GameObject.FindGameObjectsWithTag("portal");
    foreach (GameObject rawPortal in rawPortals)
    {
      var portal = rawPortal.GetComponent<Portal>();
      gameMap.AddPortal(VectorUtils.ConvertTo2dIntTile(rawPortal.gameObject.transform.position));
    }
  }

  void GetAndRegisterWalls()
  {
    var rawWalls = GameObject.FindGameObjectsWithTag("wall");
    foreach (GameObject rawWall in rawWalls)
    {
      gameMap.AddWall(VectorUtils.ConvertTo2dIntTile(rawWall.gameObject.transform.position));
    }
  }

  // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/out-parameter-modifier
  public bool SpawnBuilding(Building type, GameObject prefab, Vector2Int pos, float groundLevel, out GameObject instantiated)
  {
    instantiated = null;

    if (gameMap.Obstructed(pos))
    {
      return false;
    }

    switch (type)
    {
      case Building.Wall:
        gameMap.AddWall(pos);
        break;
      case Building.Tower:
        gameMap.AddTower(pos);
        break;
    }

    Debug.Log("Spawned " + type + " at " + pos);

    var pos3d = VectorUtils.ConvertTo3dFloat(pos, groundLevel);
    instantiated = Instantiate(prefab, pos3d, prefab.transform.rotation);

    gameMap.InvalidatePaths();

    return true;
  }

  public Router GetRouter()
  {
    return gameMap;
  }

  public Vector2Int GetXRange()
  {
    return xRange;
  }

  public Vector2Int GetYRange()
  {
    return yRange;
  }
}
