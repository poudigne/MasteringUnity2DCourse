  using UnityEngine;
using System.Collections;

public class ShopEntry : MonoBehaviour
{

  private bool canEnterShop;

	// Use this for initialization
	void Start () {
	  
	}
	
	// Update is called once per frame
	void Update () {
	  if (canEnterShop && Input.GetKeyDown(KeyCode.UpArrow))
	  {
	    if (NavigationManager.CanNavigate(tag))
	    {
	      NavigationManager.NavigateTo(tag);
	    }
	  }
	}

  void OnTriggerEnter2D(Collider2D col)
  {
    if (col.tag == "Player")
      DialogVisible(true);
    
  }
  void OnTriggerExit2D(Collider2D col)
  {
    if (col.tag == "Player")
      DialogVisible(false);
  }

  void OnGUI()
  {
    if (canEnterShop)
    {
      GUI.BeginGroup(new Rect(Screen.width/2-150,50,300,50));
      GUI.Box(new Rect(0, 0, 300, 250), "");
      GUI.Label(new Rect(15, 10, 300, 68), "Do you want to enter the shop? (press up)");
      GUI.EndGroup();
    }
  }

  void DialogVisible(bool visibility)
  {
    canEnterShop = visibility;
    MessagingManager.Instance.BroadcastUIEvent(visibility);
  }
}
