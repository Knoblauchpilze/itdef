using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  private int lives;
  private int gold;

  public TextMeshProUGUI livesText;
  public TextMeshProUGUI goldText;
  public TextMeshProUGUI stateButtonText;

  public GameObject gameScreen;
  public GameObject gameOverScreen;

  // Start is called before the first frame update
  void Start()
  {
    var portals = GameObject.FindGameObjectsWithTag("portal");
    Debug.Log("Difficulty: " + GameStateData.difficulty);
    foreach (GameObject portal in portals)
    {
      ConfigurePortal(portal, GameStateData.difficulty);
    }

    lives = GameStateData.LivesFromDifficulty();
    gold = GameStateData.GoldFromDifficulty();

    gameScreen.gameObject.SetActive(true);
    gameOverScreen.gameObject.SetActive(false);

    UpdateUi();
  }

  // Update is called once per frame
  void Update()
  {
  }

  void ConfigurePortal(GameObject portal, Difficulty difficulty)
  {
    var script = portal.GetComponent<Portal>();
    script.Configure(GeneratePortalConfiguration(difficulty));
  }

  PortalConfiguration GeneratePortalConfiguration(Difficulty difficulty)
  {
    var conf = new PortalConfiguration();
    conf.waveConf = GenerateWaveConfiguration(difficulty);

    conf.spawnIntervalInSeconds = 2.0f;
    conf.destroyOnArrivalGracePeriod = 2.0f;
    conf.minSpawnDistance = 0.5f;
    conf.spawnRadius = 2.2f;

    return conf;
  }

  WaveConfiguration GenerateWaveConfiguration(Difficulty difficulty)
  {
    var conf = new WaveConfiguration();
    conf.enemyConf = GenerateEnemyConfiguration(difficulty);

    conf.minCount = 1;
    conf.maxCount = 1;

    return conf;
  }

  EnemyConfiguration GenerateEnemyConfiguration(Difficulty difficulty)
  {
    var conf = new EnemyConfiguration();
    conf.minHealth = 1;
    conf.maxHealth = 1;

    conf.minSpeed = 1;
    conf.maxSpeed = 1;

    return conf;
  }

  void UpdateUi()
  {
    livesText.SetText("Lives: " + lives);
    goldText.SetText("Gold: " + gold);
  }

  public void EnemyPassedThroughBase()
  {
    --lives;
    UpdateUi();

    if (lives <= 0)
    {
      SetupGameOverScreen();
    }
  }

  public void ToggleGameState()
  {
    if (GameStateData.state == State.Paused)
    {
      GameStateData.state = State.Play;
      stateButtonText.SetText("Pause");
    }
    else if (GameStateData.state == State.Play)
    {
      GameStateData.state = State.Paused;
      stateButtonText.SetText("Play");
    }
  }

  void SetupGameOverScreen()
  {
    GameStateData.state = State.GameOver;

    gameScreen.gameObject.SetActive(false);
    gameOverScreen.gameObject.SetActive(true);
  }

  public void SetupTitleMenuScreen()
  {
    SceneManager.LoadScene("main_menu");
  }
}
