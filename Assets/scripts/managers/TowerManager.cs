using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
  private Finder finder;
  // Start is called before the first frame update
  void Start()
  {
    var manager = GameObject.Find("MapManager").GetComponent<MapManager>();
    finder = manager.GetFinder();
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void RegisterTower(GameObject tower)
  {
    var towerConf = new TowerConfiguration();
    towerConf.damage = TowerProperties.Damage();
    towerConf.range = TowerProperties.Range();
    towerConf.reloadTime = TowerProperties.ReloadTimeInSeconds();
    towerConf.finder = finder;

    var script = tower.GetComponent<Tower>();
    script.Configure(towerConf);
  }
}
