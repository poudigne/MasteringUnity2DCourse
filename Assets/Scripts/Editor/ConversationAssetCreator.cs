using UnityEngine;
using UnityEditor;

public class ConversationAssetCreator : MonoBehaviour {
  [MenuItem("Assets/Create/PSoft/Conversation")]
  public static void CreateAsset()
  {
    CustomAssetUtility.CreateAsset<Conversation>();
  }
}
