using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour
{
  // publics
  public float xMargin = 1.5f;
  public float yMargin = 1.5f;
  public float xSmooth = 1.5f;
  public float ySmooth = 1.5f;

  public Vector2 maxXandY;
  public Vector2 minXandY;

  public Transform player;

  // privates


  void Awake()
  {
    player = GameObject.Find("Player").transform;
    if (player == null)
    {
      Debug.LogError("Player object not found");
    } 
  }

	// Use this for initialization
	void Start () {
	  
	}

  void FixedUpdate()
  {
    float targetX = transform.position.x;
    float targetY = transform.position.y;

    if (CheckXMargin())
    {
      targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth*Time.fixedDeltaTime);
    }

    if (CheckYMargin())
    {
      targetY = Mathf.Lerp(transform.position.y, player.position.y, ySmooth * Time.fixedDeltaTime);
    }

    targetX = Mathf.Clamp(targetX, minXandY.x, maxXandY.x);
    targetY = Mathf.Clamp(targetY, minXandY.y, maxXandY.y);

    transform.position = new Vector3(targetX, targetY, transform.position.z);
  }

	bool CheckXMargin()
  {
    return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
  }

  bool CheckYMargin()
  {
    return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
  }
}
