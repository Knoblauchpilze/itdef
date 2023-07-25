using UnityEngine;
using TMPro;

public class BuildManager : MonoBehaviour
{
  private bool hasBuilding = false;
  private Building buildingToBuild;
  private GameObject buildingPrefab;
  private float gold;
  private MapManager mapManager;
  public TextMeshProUGUI goldText;

  void Start()
  {
    gold = GameStateData.GoldFromDifficulty();
    mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();

    UpdateUi();
  }

  void Update()
  {
  }

  public void SpawnBuildingRequest(Vector2Int pos, float groundLevel)
  {
    if (hasBuilding && CanAfford())
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
