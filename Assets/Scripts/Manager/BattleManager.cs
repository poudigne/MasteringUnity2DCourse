using System.Threading;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum BattleState { BeginBattle, Intro, PlayerMove, PlayerAttack, ChangeControl, EnemyAttack, BattleResult, BattleEnd }

public class BattleManager : MonoBehaviour
{
    public GameObject[] spawnPoints;
    public GameObject[] enemyPrefabs;
    public AnimationCurve spawnAnimationCurve;
    public GameObject selectionCircle;

    private int enemyCount;
    private Animator stateManager;
    private Dictionary<int, BattleState> battleStateHash = new Dictionary<int, BattleState>();
    private BattleState currentBattleState;
    private InventoryItem selectedWeapon;
    private string selectedTargetName;
    private EnemyController selectedTarget;
    private bool canSelectEnemy;
    private bool attacking = false;

    // Use this for initialization
    void Start ()
	{
        MessagingManager.Instance.SubscribeInventoryEvent(InventoryItemSelect);

        stateManager = GetComponent<Animator>();
        if (stateManager == null)
            Debug.LogError("No battleManager animator found!");

        enemyCount = UnityEngine.Random.Range(1, spawnPoints.Length);
        StartCoroutine(SpawnEnemies());
        GetAnimationStates();
   	}
	
	// Update is called once per frame
	void Update () {
        currentBattleState = battleStateHash[stateManager.GetCurrentAnimatorStateInfo(0).fullPathHash];

        switch (currentBattleState)
        {
            case BattleState.Intro:
                break;
            case BattleState.PlayerMove:
                break;
            case BattleState.PlayerAttack:
                break;
            case BattleState.ChangeControl:
                break;
            case BattleState.EnemyAttack:
                break;
            case BattleState.BattleResult:
                break;
            case BattleState.BattleEnd:
                break;
            default:
                break;
        }
	}

    IEnumerator SpawnEnemies()
    {
        Debug.Log(enemyCount);
        for (int i = 0; i < enemyCount; i++)
        {
            var newEnemy = (GameObject) Instantiate(enemyPrefabs[0]);
            newEnemy.transform.position = new Vector3(10, -1, 0);

            newEnemy.transform.parent = spawnPoints[i].transform;

            var controller = newEnemy.GetComponent<EnemyController>();
            controller.BattleManager = this;
            var enemyProfile = ScriptableObject.CreateInstance<Enemy>();
            enemyProfile.enemyClass = EnemyClass.Goblin;
            enemyProfile.Level = 1;
            enemyProfile.Damage = 1;
            enemyProfile.Health = 1;
           
            enemyProfile.Name = enemyProfile.enemyClass + " " + i.ToString();

            controller.enemyProfile = enemyProfile;

            yield return StartCoroutine(MoveCharacterToPoint(spawnPoints[i], newEnemy));
        }

        stateManager.SetBool("battleReady", true);
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
        switch (currentBattleState)
        {
            case BattleState.Intro:
                GUI.Box(new Rect((Screen.width / 2) - 150, 50, 300, 50), "Battle between Player and Goblins");
                break;
            case BattleState.PlayerMove:
                if (GUI.Button(new Rect(10,10,100,50), "Run away"))
                {
                    GameState.playerReturningHome = true;
                    NavigationManager.NavigateTo("World");
                }
                if (selectedWeapon == null)
                {
                    GUI.Box(new Rect((Screen.width / 2) - 50, 10, 100, 50), "Select Weapon");
                }
                else if (selectedTarget == null)
                {
                    GUI.Box(new Rect((Screen.width / 2) - 50, 10, 100, 50), "Select target");
                    canSelectEnemy = true;
                }
                else
                {
                    if (GUI.Button(new Rect((Screen.width / 2) - 50, 10, 100, 50), "Attack " + selectedTargetName))
                    {
                        canSelectEnemy = false;
                        stateManager.SetBool("PlayerReady", true);
                        MessagingManager.Instance.BroadcastUIEvent(true);
                    }
                }
                break;
            case BattleState.PlayerAttack:
                if (!attacking)
                {
                    StartCoroutine(AttackTarget());
                }

                break;
            case BattleState.ChangeControl:
                break;
            case BattleState.EnemyAttack:
                break;
            case BattleState.BattleResult:
                break;
            case BattleState.BattleEnd:
                break;
            default:
                break;
        }
    }

    void GetAnimationStates()
    {
        foreach(BattleState state in (BattleState[])Enum.GetValues(typeof(BattleState)))
        {
            battleStateHash.Add(Animator.StringToHash("Base Layer." + state.ToString()), state);
        }
    }

    private void InventoryItemSelect(InventoryItem item)
    {
        selectedWeapon = item;
    }

    public bool CanSelectEnemy
    {
        get { return this.canSelectEnemy; }
    }

    public int EnemyCount
    {
        get { return this.enemyCount; }
    }

    public void SelectEnemy(EnemyController enemy, string name)
    {
        selectedTarget = enemy;
        selectedTargetName = name;
    }

    public void ClearSelectedEnemy()
    {
        if (selectedTarget != null)
        {
            var enemyController = selectedTarget.GetComponent<EnemyController>();
            enemyController.ClearSelection();
            selectedTarget = null;
            selectedTargetName = String.Empty;
        }
    }

    IEnumerator AttackTarget()
    {
        int attacks = 0;
        attacking = true;
        bool attackComplete = false;
        while (!attackComplete)
        {
            GameState.currentPlayer.Attack(selectedTarget.enemyProfile);
            selectedTarget.UpdateAI();
            attacks++;
            if (selectedTarget.enemyProfile.Health < 1 || attacks > GameState.currentPlayer.NoOfAttacks)
            {
                attackComplete = true;
            }
            yield return new WaitForSeconds(1);
        }
    }
}

