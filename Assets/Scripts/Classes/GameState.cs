using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public static class GameState
{

  public static Player currentPlayer = ScriptableObject.CreateInstance<Player>();
  public static bool playerReturningHome;
  public static Dictionary<NavigationManager.Worlds, Vector3> lastScenePositions = new Dictionary<NavigationManager.Worlds, Vector3>();


  public static Vector3 GetLastScenePosition(string sceneName)
  {
    var sceneEnum = (NavigationManager.Worlds)Enum.Parse(typeof (NavigationManager.Worlds), sceneName);

    return lastScenePositions.ContainsKey(sceneEnum) ? lastScenePositions[sceneEnum] : Vector3.zero;
  }

  public static void SetLastScenePosition(string sceneName, Vector3 position)
  {
    var sceneEnum = (NavigationManager.Worlds)Enum.Parse(typeof (NavigationManager.Worlds), sceneName);

    if (lastScenePositions.ContainsKey(sceneEnum))
    {
      lastScenePositions[sceneEnum] = position;
    }
    else
    {
      lastScenePositions.Add(sceneEnum, position);
    }
  }
}
