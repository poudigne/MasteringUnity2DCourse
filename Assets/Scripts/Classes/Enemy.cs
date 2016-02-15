using UnityEngine;
using System.Collections;

public class Enemy : Entity {

    public EnemyClass enemyClass;

	public override string ToString()
    {
        return "Name : " + this.name + "\n" +
            "Level : " + this.Level + "\n" +
            "Health : " + this.Health + "\n";

    }
}
