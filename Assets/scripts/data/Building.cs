
public static class BuildingCost
{
  public static int Get(Building building)
  {
    switch (building)
    {
      case Building.Wall:
        return 10;
      case Building.Tower:
        return 20;
    }

    return 0;
  }

}
