using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Fighter : MonoBehaviour
{
    public string fighterName;
    public int maxHP;
    public int currentHP;
    public int attackPower;
    public List<Skill> skills;
    public HealthBar healthBar;
    public Animator animator;

    void Start()
    {
        currentHP = maxHP;
        UpdateHealthBar();
    }

    public bool TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            currentHP = 0;
            if (animator) animator.SetTrigger("Die");
            return true;
        }
        else
        {
            if (animator) animator.SetTrigger("Hit");
        }
        return false;
    }

    public void UpdateHealthBar()
    {
        if (healthBar)
            healthBar.SetHealth(currentHP, maxHP);
    }
}
