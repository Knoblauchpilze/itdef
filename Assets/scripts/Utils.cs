using UnityEngine;

public static class VectorUtils
{
  public static Vector2Int ConvertTo2d(Vector3 vec)
  {
    int x = Mathf.RoundToInt(vec.x);
    int y = Mathf.RoundToInt(vec.y);

    return new Vector2Int(x, y);
  }

  public static Vector2 ConvertTo2dFloat(Vector3 vec)
  {
    return new Vector2(vec.x, vec.y);
  }

  public static Vector3 ConvertTo3d(Vector2Int vec, float z)
  {
    return new Vector3(vec.x, vec.y, z);
  }
}
