using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
  public GameObject titleScreen;
  public GameObject difficultyScreen;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  public void SetupTitleScreen()
  {
    titleScreen.gameObject.SetActive(true);
    difficultyScreen.gameObject.SetActive(false);
  }

  public void SetupDifficultyScreen()
  {
    titleScreen.gameObject.SetActive(false);
    difficultyScreen.gameObject.SetActive(true);
  }

  public void SetupGameScreen(Difficulty difficulty)
  {
    GameStateData.difficulty = difficulty;
    GameStateData.state = State.Paused;

    SceneManager.LoadScene("level");
  }
}
