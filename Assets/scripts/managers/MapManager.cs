using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
  private GameMap gameMap = new GameMap();

  // Start is called before the first frame update
  void Start()
  {
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
      gameMap.AddPortal(portal, VectorUtils.ConvertTo2dIntTile(rawPortal.gameObject.transform.position));
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

  public void SpawnBuilding(Building type, GameObject prefab, Vector2Int pos, float groundLevel)
  {
    if (gameMap.Obstructed(pos))
    {
      return;
    }

    switch (type)
    {
      case Building.Wall:
        gameMap.AddWall(pos);
        break;
    }

    Debug.Log("Spawned " + type + " at " + pos);

    var pos3d = VectorUtils.ConvertTo3dFloat(pos, groundLevel);
    Instantiate(prefab, pos3d, prefab.transform.rotation);

    gameMap.InvalidatePaths();
  }

  public Router GetRouter()
  {
    return gameMap;
  }
}
