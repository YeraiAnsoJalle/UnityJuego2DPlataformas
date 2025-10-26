using UnityEngine;

public static class BattleData
{
    public static EnemyData enemyToLoad;
}

[System.Serializable]
public class EnemyData
{
    public string name;
    public int maxHealth;
    public int attackPower;
    public string uniqueID;

    public EnemyData(string name, int maxHealth, int attackPower, string uniqueID)
    {
        this.name = name;
        this.maxHealth = maxHealth;
        this.attackPower = attackPower;
        this.uniqueID = uniqueID;
    }
}
