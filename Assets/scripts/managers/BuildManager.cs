using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildManager : MonoBehaviour
{
  private bool hasBuilding = false;
  private Building buildingToBuild;
  private GameObject buildingPrefab;
  private float gold;
  private MapManager mapManager;
  private List<Portal> portals = new List<Portal>();
  private List<GameObject> bases = new List<GameObject>();

  public TextMeshProUGUI goldText;

  void Start()
  {
    gold = GameStateData.GoldFromDifficulty();
    mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();

    GetAndRegisterPortals();
    GetAndRegisterBases();

    UpdateUi();
  }

  void Update()
  {
  }

  void GetAndRegisterPortals()
  {
    var rawPortals = GameObject.FindGameObjectsWithTag("portal");
    foreach (GameObject rawPortal in rawPortals)
    {
      var portal = rawPortal.GetComponent<Portal>();
      portals.Add(portal);
    }
  }

  void GetAndRegisterBases()
  {
    var rawBases = GameObject.FindGameObjectsWithTag("base");
    foreach (GameObject rawBase in rawBases)
    {
      bases.Add(rawBase);
    }
  }

  public void SpawnBuildingRequest(Vector2Int pos, float groundLevel)
  {
    if (hasBuilding && CanAfford() && AllPortalsAndBasesStillConnected(pos))
    {
      SpawnBuildingAndPay(pos, groundLevel);
    }
  }

  bool CanAfford()
  {
    if (!hasBuilding)
    {
      return false;
    }

    var cost = BuildingCost.Get(buildingToBuild);
    return cost <= gold;
  }

  bool AllPortalsAndBasesStillConnected(Vector2Int pos)
  {
    return false;
  }

  void SpawnBuildingAndPay(Vector2Int pos, float groundLevel)
  {
    var spawned = mapManager.SpawnBuilding(buildingToBuild, buildingPrefab, pos, groundLevel);
    if (!spawned)
    {
      return;
    }

    gold -= BuildingCost.Get(buildingToBuild);
    UpdateUi();
  }

  public void SetBuildingToBuild(Building type, GameObject prefab)
  {
    buildingToBuild = type;
    buildingPrefab = prefab;
    hasBuilding = true;
  }

  public void EnemyKilled(int reward)
  {
    gold += reward;

    UpdateUi();
  }

  void UpdateUi()
  {
    goldText.SetText("Gold: " + gold);
  }
}
