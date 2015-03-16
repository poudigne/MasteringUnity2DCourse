using System.Threading;
using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour
{

  public GameObject[] spawnPoints;
  public GameObject[] enemyPrefabs;
  public AnimationCurve spawnAnimationCurve;

  private int enemyCount;
  private BattlePhase phase;


	// Use this for initialization
	void Start ()
	{
	  enemyCount = Random.Range(1, spawnPoints.Length);
    StartCoroutine(SpawnEnemies());

	  phase = BattlePhase.PlayerAttack;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  IEnumerator SpawnEnemies()
  {

    for (int i = 0; i < enemyCount; i++)
    {
      var newEnemy = (GameObject) Instantiate(enemyPrefabs[0]);
      newEnemy.transform.position = new Vector3(10, -1, 0);

      yield return StartCoroutine(MoveCharacterToPoint(spawnPoints[i], newEnemy));
      newEnemy.transform.parent = spawnPoints[i].transform;
    }
    yield return null;
    
  }

  IEnumerator MoveCharacterToPoint(GameObject destination, GameObject enemy)
  {
    float timer = 0.0f;
    var startPosition = enemy.transform.position;
    if (spawnAnimationCurve.length > 0)
    {
      while (timer < spawnAnimationCurve.keys[spawnAnimationCurve.length - 1].time)
      {
        enemy.transform.position = Vector3.Lerp(startPosition, destination.transform.position,
          spawnAnimationCurve.Evaluate(timer));
        timer += Time.deltaTime;
        yield return new WaitForEndOfFrame();
      }
    }
    else
    {
      enemy.transform.position = destination.transform.position;
    }
  }

  void OnGUI()
  {
    if (phase == BattlePhase.PlayerAttack)
    {
      if (GUI.Button(new Rect(10, 10, 100, 50), "Run away"))
      {
        NavigationManager.NavigateTo("World");
        GameState.playerReturningHome = true;
      }
    }
  }

  enum BattlePhase
  {
    PlayerAttack,
    EnemyAttack
  }
}

