using UnityEngine;
using UnityEditor;
using System.Collections;

public class InventoryItemAssetsCreator : MonoBehaviour {

  [MenuItem("Assets/Create/PSoft/Inventory Item")]
  public static void CreateAsset()
  {
    CustomAssetUtility.CreateAsset<InventoryItem>();
  }
}
