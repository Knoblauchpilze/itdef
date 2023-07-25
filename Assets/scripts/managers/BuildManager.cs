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
    if (hasBuilding)
    {
      mapManager.SpawnBuilding(buildingToBuild, buildingPrefab, pos, groundLevel);
    }
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
