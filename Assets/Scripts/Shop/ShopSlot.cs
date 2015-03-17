using UnityEngine;
using System.Collections;

public class ShopSlot : MonoBehaviour
{
  public InventoryItem item;
  public ShopManager manager;

  public void AddShopItem(InventoryItem item)
  {
    var spriteRenderer = (SpriteRenderer) GetComponent(typeof (SpriteRenderer));
    spriteRenderer.sprite = item.sprite;
    spriteRenderer.transform.localScale = item.scale;
    this.item = item;
  }

  public void PurchaseItem()
  {
    GameState.currentPlayer.AddInInventory(item);
    item = null;
    var spriteRenderer = (SpriteRenderer)GetComponent(typeof(SpriteRenderer));
    spriteRenderer.sprite = null;
    manager.ClearSelectedItem();
  }

  void OnMouseDown()
  {
    if (item != null)
    {
      manager.SetShopSelectedItem(this);
    }
  }
}
