using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
  private Button button;
  private GameMenuManager gameMenuManager;

  public Difficulty difficulty;

  // Start is called before the first frame update
  void Start()
  {
    button = GetComponent<Button>();
    gameMenuManager = GameObject.Find("GameMenuManager").GetComponent<GameMenuManager>();

    button.onClick.AddListener(SetDifficulty);
  }

  // Update is called once per frame
  void Update()
  {
  }

  void SetDifficulty()
  {
    gameMenuManager.SetupGameScreen(difficulty);
  }
}
