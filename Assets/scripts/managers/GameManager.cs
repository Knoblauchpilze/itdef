using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  private int lives;

  public TextMeshProUGUI livesText;
  public TextMeshProUGUI stateButtonText;

  public GameObject gameScreen;
  public GameObject gameOverScreen;

  // Start is called before the first frame update
  void Start()
  {
    lives = GameStateData.LivesFromDifficulty();

    gameScreen.gameObject.SetActive(true);
    gameOverScreen.gameObject.SetActive(false);

    UpdateUi();
  }

  // Update is called once per frame
  void Update()
  {
  }

  void UpdateUi()
  {
    livesText.SetText("Lives: " + lives);
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
