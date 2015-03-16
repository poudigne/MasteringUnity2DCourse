using UnityEngine;
using System.Collections;

public class NavigationPrompt : MonoBehaviour
{

  private bool showDialog;

  void OnCollisionEnter2D(Collision2D col)
  {
    if (NavigationManager.CanNavigate(tag))
      DialogVisible(true);
  }
  void OnCollisionExit2D(Collision2D col)
  {
    DialogVisible(false);
  }

  void OnTriggerEnter2D(Collider2D col)
  {
    
    if (NavigationManager.CanNavigate(this.tag))
    {
      DialogVisible(true);
    }
  }

  void OnGUI()
  {
    if (showDialog)
    {
      GUI.BeginGroup(new Rect(Screen.width / 2 - 150, 50, 300, 250));
      GUI.Box(new Rect(0, 0, 300, 250), "");
      GUI.Label(new Rect(15, 10, 300, 68), string.Format("Do you want to travel to {0}?", NavigationManager.GetRouteInfo(tag)));

      if (GUI.Button(new Rect(55, 100, 180, 40), "Travel"))
      {
        DialogVisible(false);
        NavigationManager.NavigateTo(tag);
      }
      if (GUI.Button(new Rect(55, 150, 180, 40), "Stay"))
      {
        DialogVisible(false);
      }
      GUI.EndGroup();
    }
  }

  void DialogVisible(bool visibility)
  {
    showDialog = visibility;
    MessagingManager.Instance.BroadcastUIEvent(visibility);
  }

}
