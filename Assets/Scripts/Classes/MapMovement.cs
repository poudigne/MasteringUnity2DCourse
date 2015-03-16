using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

public class MapMovement : MonoBehaviour
{
  public AnimationCurve movementCurve;
  
  private Vector3 startLocation;
  private Vector3 targetLocation;
  private float timer = 0;
  private bool inputActive = true;
  private bool inputReady = true;
  private bool startedTravelling = false;
  private int encounterChange = 100;
  private float encounterDistance = 0;


  private Collider2D collider2D;
  
  void Awake()
  {
    collider2D = (Collider2D)GetComponent(typeof(Collider2D));
    collider2D.enabled = false;

    var lastPosition = GameState.GetLastScenePosition(Application.loadedLevelName);
    if (lastPosition != Vector3.zero)
    {
      transform.position = lastPosition;
    }
  }

  void Start()
  {
    MessagingManager.Instance.SubscribeUIEvent(UpdateInputAction);
  }

  void Update()
  {
    if (inputActive && (Input.GetMouseButtonUp(0) || Input.touchCount == 1))
    {
      startLocation = transform.position.toVector3_2D();
      timer = 0;
      targetLocation = Input.mousePosition.GetScreenPositionFor2D();
      startedTravelling = true;

      var encounterProbability = Random.Range(1, 100);
      if (encounterProbability < encounterChange && !GameState.playerReturningHome)
      {
        encounterDistance = (Vector3.Distance(startLocation, targetLocation)/100)*Random.Range(10, 100);
      }
      else
      {
        encounterDistance = 0;
      }
    }

    if (targetLocation != Vector3.zero && targetLocation != transform.position && targetLocation != startLocation)
    {
      transform.position = Vector3.Lerp(startLocation, targetLocation, movementCurve.Evaluate(timer));
      timer += Time.deltaTime;
    }
    if (startedTravelling && (Vector3.Distance(startLocation, transform.position.toVector3_2D()) > 1.0f))
    {
      collider2D.enabled = true;
      startedTravelling = false;
    }

    if (encounterDistance > 0)
    {
      if (Vector3.Distance(startLocation, transform.position) > encounterDistance)
      {
        targetLocation = Vector3.zero;
        NavigationManager.NavigateTo(NavigationManager.Worlds.Battle);
      }
    }

    if (!inputReady && inputActive)
    {
      targetLocation = transform.position;
    }

    inputActive = inputReady;
  }

  void OnDestroy()
  {
    GameState.SetLastScenePosition(Application.loadedLevelName, transform.position);
  }

  void UpdateInputAction(bool uiVisible)
  {
    inputReady = !uiVisible;
  }

}
