using UnityEngine;

[System.Serializable]
public class Skill
{
    public string skillName;
    public int power;
    public bool isHealing;
    public string animationTrigger; // Nombre del trigger en el Animator

    public string Use(Fighter user, Fighter target)
    {
        if (isHealing)
        {
            int healAmount = Mathf.Min(power, user.maxHP - user.currentHP);
            user.currentHP += healAmount;
            user.UpdateHealthBar();
            return $"{user.fighterName} usa {skillName} y recupera {healAmount} HP!";
        }
        else
        {
            bool targetDead = target.TakeDamage(power);
            target.UpdateHealthBar();
            if (targetDead)
                return $"{user.fighterName} usa {skillName} y derrota a {target.fighterName}!";
            else
                return $"{user.fighterName} usa {skillName} e inflige {power} de daño.";
        }
    }
}
