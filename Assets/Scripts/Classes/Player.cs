using System.Collections.Generic;
using UnityEngine;
public class Player : Entity
{
    public List<InventoryItem> Inventory = new List<InventoryItem>();
    public string[] Skills;
    public int Money;

  public void AddInInventory(InventoryItem item)
  {
    Strength += item.strength;
    Defense += item.defense;
    Inventory.Add(item);
  }
}
