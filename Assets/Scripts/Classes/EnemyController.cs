using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    public Enemy enemyProfile;

    private BattleManager battleManager;
    private Animator enemyAI;
    private bool selected;

    GameObject selectionCircle;


    public BattleManager BattleManager
    {
        get { return this.battleManager; }
        set { battleManager = value; }
    }

    void Awake()
    {
        enemyAI = GetComponent<Animator>();
        if (enemyAI == null)
            Debug.LogError("No AI System found.");
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        UpdateAI();
	}

    public void UpdateAI()
    {
        if (enemyAI != null && enemyProfile != null)
        {
            enemyAI.SetInteger("EnemyHealth", enemyProfile.Health);
            enemyAI.SetInteger("PlayerHealth", GameState.currentPlayer.Health);
            enemyAI.SetInteger("EnemiesInBattle", battleManager.EnemyCount);
        }
    }

    IEnumerable SpinObject(GameObject target)
    {
        while (true)
        {
            target.transform.Rotate(0, 0, 180 * Time.deltaTime);
            yield return null;
        }
    }

    void OnMouseDown()
    {
        if (battleManager.CanSelectEnemy)
        {
            var selection = !selected;
            battleManager.ClearSelectedEnemy();
            selected = selection;
            if (selected)
            {
                selectionCircle = (GameObject)GameObject.Instantiate(battleManager.selectionCircle);
                selectionCircle.transform.parent = transform;
                selectionCircle.transform.localPosition = Vector3.zero;
                StartCoroutine("SpinObject", selectionCircle);
                Debug.Log(enemyProfile.ToString());
                battleManager.SelectEnemy(this, enemyProfile.name);
            }
        }
    }

    public void ClearSelection()
    {
        if (selected)
        {
            selected = false;
            if (selectionCircle != null)
                DestroyObject(selectionCircle);
            StopCoroutine("SpinObject");
        }
    }

    

}
