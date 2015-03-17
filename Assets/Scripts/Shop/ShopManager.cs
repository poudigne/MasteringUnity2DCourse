using UnityEngine;
using System.Collections;

public class ShopManager : MonoBehaviour
{

  public Sprite shopOwnerSprite;
  public Vector3 shopOwnerScale;
  public GameObject shopOwnerLocation;
  public GameObject purchasingSection;
  public SpriteRenderer purchaseItemDisplay;
  public ShopSlot[] itemSlots;
  public InventoryItem[] shopItems;
  private static ShopSlot selectedShopSlot;

  private int nextSlotIndex = 0;
 
	// Use this for initialization
	void Start ()
	{
	  var ownerSpriteRenderer = (SpriteRenderer)shopOwnerLocation.GetComponent(typeof(SpriteRenderer));
	  ownerSpriteRenderer.sprite = shopOwnerSprite;
	  ownerSpriteRenderer.transform.localScale = shopOwnerScale;
	  if (itemSlots.Length > 0 && shopItems.Length > 0)
	  {
	    for (int i = 0; i < shopItems.Length; i++)
	    {
	      if (nextSlotIndex > itemSlots.Length) break;
	      itemSlots[nextSlotIndex].AddShopItem(shopItems[i]);
	      itemSlots[nextSlotIndex].manager = this;
	      nextSlotIndex++;
	    }
	  }

	}
	
	// Update is called once per frame
	void Update () {
	
	}


  public void SetShopSelectedItem(ShopSlot slot)
  {
    selectedShopSlot = slot;
    purchaseItemDisplay.sprite = slot.item.sprite;
    purchasingSection.SetActive(true);
  }

  public void ClearSelectedItem()
  {
    selectedShopSlot = null;
    purchaseItemDisplay.sprite = null;
    purchasingSection.SetActive(false);
  }

  public static void PurchaseSelectionItem()
  {
    selectedShopSlot.PurchaseItem();
  }
}
