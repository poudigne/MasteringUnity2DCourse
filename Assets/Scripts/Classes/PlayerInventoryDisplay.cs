using UnityEngine;
using System.Collections;

public class PlayerInventoryDisplay : MonoBehaviour
{

  private bool displayInventory = false;
  private Rect inventoryWindowRect;
  private Vector2 inventoryWindowSize = new Vector2(150, 150);
  private Vector2 inventoryItemIconSize = new Vector2(130, 32);
  private float offsetX = 6;
  private float offsetY = 6;

  void Awake()
  {
    inventoryWindowRect = new Rect(
      Screen.width - inventoryWindowSize.x,
      Screen.height - 40 - inventoryWindowSize.y,
      inventoryWindowSize.x,
      inventoryWindowSize.y);
  }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  void OnGUI()
  {
    if (GUI.Button(new Rect(Screen.width - 40,
      Screen.height - 40,
      40,
      40), "INV"))
    {
      displayInventory = !displayInventory;
    }
    if (displayInventory)
    {
      inventoryWindowRect = GUI.Window(0, inventoryWindowRect, DisplayInventoryWindow, "Inventory");

      inventoryWindowSize = new Vector2(inventoryWindowRect.width,
        inventoryWindowRect.height);
    }
  }

  
  void DisplayInventoryWindow(int windowID)
  {
    var currentX = 0 + offsetX;
    var currentY = 18 + offsetY;

    foreach (var item in GameState.currentPlayer.Inventory)
    {
      Rect texcoords = item.sprite.textureRect;

      texcoords.x /= item.sprite.texture.width;
      texcoords.y /= item.sprite.texture.height;
      texcoords.width /= item.sprite.texture.width;
      texcoords.height /= item.sprite.texture.height;

      GUI.DrawTextureWithTexCoords(new Rect(
        currentX, 
        currentY, 
        item.sprite.textureRect.width,
        item.sprite.textureRect.height), 
        item.sprite.texture,
        texcoords);
      currentX += inventoryItemIconSize.x;

      if (currentX + inventoryItemIconSize.x + offsetX > inventoryWindowSize.x)
      {
        currentX = offsetX;
        currentY += inventoryItemIconSize.y;
        if (currentY + inventoryItemIconSize.y + offsetX > inventoryWindowSize.y)
        {
          return;
        }
      }
    }
  }
}
