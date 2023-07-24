using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
  private Button button;
  private BuildManager buildManager;

  public Building building;

  // Start is called before the first frame update
  void Start()
  {
    button = GetComponent<Button>();
    buildManager = GameObject.Find("BuildManager").GetComponent<BuildManager>();

    button.onClick.AddListener(SetBuilding);
  }

  // Update is called once per frame
  void Update()
  {
  }

  void SetBuilding()
  {
    buildManager.SetBuildingToBuild(building);
  }
}
