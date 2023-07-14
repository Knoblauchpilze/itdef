
using System;

public class Timer
{

  public float accumulated;
  public float threshold;

  public Timer(float inThreshold)
  {
    threshold = inThreshold;
  }

  public bool Accumulate(float delta)
  {
    accumulated += delta;
    return Ready();
  }

  public bool Ready()
  {
    return accumulated >= threshold;
  }

  public float Remaining()
  {
    return Math.Max(0.0f, threshold - accumulated);
  }

  public void Reset()
  {
    accumulated = 0.0f;
  }
}
