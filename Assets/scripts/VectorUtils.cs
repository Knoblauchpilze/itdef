using UnityEngine;

public static class VectorUtils
{
  public static Vector2Int ConvertTo2dInt(Vector3 vec)
  {
    int x = Mathf.RoundToInt(vec.x);
    int y = Mathf.RoundToInt(vec.y);

    return new Vector2Int(x, y);
  }

  public static Vector2Int ConvertTo2dIntTile(Vector3 vec)
  {
    int x = Mathf.FloorToInt(vec.x + 0.5f);
    int y = Mathf.FloorToInt(vec.y + 0.5f);

    return new Vector2Int(x, y);
  }

  public static Vector2 ConvertTo2dFloat(Vector3 vec)
  {
    return new Vector2(vec.x, vec.y);
  }

  public static Vector3 ConvertTo3dFloat(Vector2Int vec, float z)
  {
    return new Vector3(vec.x, vec.y, z);
  }
}
