
public static class BuildingCost
{
  public static int Get(Building building)
  {
    switch (building)
    {
      case Building.Wall:
        return 10;
    }

    return 0;
  }

}
