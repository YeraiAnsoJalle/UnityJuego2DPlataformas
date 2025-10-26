using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Character : MonoBehaviour
{
    [Header("Datos del personaje")]
    public string fighterName;
    public int maxHealth = 100;
    public int currentHealth;
    public List<Skill> skills = new List<Skill>();

    private void Start()
    {
        if (currentHealth <= 0)
            currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

    public void UseSkill(int index, Character target)
    {
        if (index < 0 || index >= skills.Count) return;
        Skill skill = skills[index];

        if (skill.isHealing)
        {
            Heal(skill.power);
        }
        else if (target != null)
        {
            target.TakeDamage(skill.power);
        }
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }
}