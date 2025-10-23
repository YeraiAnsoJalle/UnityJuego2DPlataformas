using UnityEngine;

public class Character : MonoBehaviour
{
    public string characterName;
    public int maxHealth = 100;
    public int currentHealth;

    public Skill[] skills;

    private void Awake()
    {
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

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    public void UseSkill(int index, Character target)
    {
        if (index < 0 || index >= skills.Length) return;

        Skill skill = skills[index];

        if (skill.isHealing)
        {
            Heal(skill.power);
            Debug.Log($"{characterName} usa {skill.skillName} y se cura {skill.power}");
        }
        else
        {
            target.TakeDamage(skill.power);
            Debug.Log($"{characterName} usa {skill.skillName} e inflige {skill.power} de daño");
        }
    }
}
