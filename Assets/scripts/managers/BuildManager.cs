using UnityEngine;
using TMPro;

public class BuildManager : MonoBehaviour
{
  private bool hasBuilding = false;
  private Building buildingToBuild;
  private float gold;
  private GameManager gameManager;
  public TextMeshProUGUI goldText;

  void Start()
  {
    gold = GameStateData.GoldFromDifficulty();
    gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    UpdateUi();
  }

  void Update()
  {
    // https://forum.unity.com/threads/onmousedown-for-right-click.7131/
    if (Input.GetMouseButtonUp(0))
    {
      // https://docs.unity3d.com/ScriptReference/Input-mousePosition.html
      // https://discussions.unity.com/t/mouse-cursor-position-relative-to-the-object/144907
      Debug.Log("clicked at " + Input.mousePosition);
    }
  }

  public void SetBuildingToBuild(Building bToBuild)
  {
    buildingToBuild = bToBuild;
    hasBuilding = true;
    if (hasBuilding)
    {
      Debug.Log("building is now " + buildingToBuild);
    }
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
