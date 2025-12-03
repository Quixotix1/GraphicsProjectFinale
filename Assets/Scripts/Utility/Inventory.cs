using System.Collections.Generic;

public class Inventory
{
    public static Inventory Instance = new Inventory();

    public List<int> keys = new();
}
