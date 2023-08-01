
public enum Difficulty
{
  Easy,
  Medium,
  Hard
}

public enum State
{
  Paused,
  Play,
  GameOver
}

public static class GameStateData
{
  public static Difficulty difficulty;
  public static State state;

  public static int LivesFromDifficulty()
  {
    switch (GameStateData.difficulty)
    {
      case Difficulty.Easy:
        return 2;
      case Difficulty.Medium:
        return 20;
      case Difficulty.Hard:
        return 15;
      default:
        return 0;
    }
  }

  public static int GoldFromDifficulty()
  {
    switch (GameStateData.difficulty)
    {
      case Difficulty.Easy:
        return 4000;
      case Difficulty.Medium:
        return 30;
      case Difficulty.Hard:
        return 20;
      default:
        return 0;
    }
  }
}