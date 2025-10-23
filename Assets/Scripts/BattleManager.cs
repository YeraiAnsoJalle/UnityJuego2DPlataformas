using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    public Character player;
    public Character enemy;

    public Slider playerHealthBar;
    public Slider enemyHealthBar;
    public Text battleText;
    public GameObject actionPanel;

    private bool playerTurn = true;
    private bool battleOver = false;
    private bool actionChosen = false;
    private int chosenSkillIndex = -1;

    private void Start()
    {
        playerHealthBar.maxValue = player.maxHealth;
        enemyHealthBar.maxValue = enemy.maxHealth;
        UpdateUI();
        StartCoroutine(BattleLoop());
    }

    private IEnumerator BattleLoop()
    {
        battleText.text = "¡Comienza la batalla!";
        yield return new WaitForSeconds(1f);

        while (!battleOver)
        {
            if (playerTurn)
            {
                yield return PlayerTurn();
            }
            else
            {
                yield return EnemyTurn();
            }

            UpdateUI();
            yield return new WaitForSeconds(1f);

            if (player.IsDead() || enemy.IsDead())
            {
                battleOver = true;
                battleText.text = player.IsDead() ? "¡Has perdido!" : "¡Has ganado!";
            }
            else
            {
                playerTurn = !playerTurn;
            }

            yield return null;
        }
    }

    private IEnumerator PlayerTurn()
    {
        battleText.text = "Turno del jugador";
        actionChosen = false;
        chosenSkillIndex = -1;

        // Mostrar botones para que el jugador elija acción
        actionPanel.SetActive(true);

        // Espera hasta que el jugador elija
        while (!actionChosen)
        {
            yield return null;
        }

        actionPanel.SetActive(false);

        // Ejecutar acción elegida
        Skill chosenSkill = player.skills[chosenSkillIndex];

        if (chosenSkill.isHealing)
        {
            player.UseSkill(chosenSkillIndex, null);
            battleText.text = $"Jugador usa {chosenSkill.skillName} y se cura {chosenSkill.power}";
        }
        else
        {
            player.UseSkill(chosenSkillIndex, enemy);
            battleText.text = $"Jugador usa {chosenSkill.skillName} e inflige {chosenSkill.power} de daño";
        }

        yield return new WaitForSeconds(1f);
    }

    private IEnumerator EnemyTurn()
    {
        battleText.text = "Turno del enemigo";
        yield return new WaitForSeconds(1f);

        // El enemigo tiene 20% de probabilidad de curarse
        bool willHeal = Random.value < 0.2f && enemy.currentHealth < enemy.maxHealth;

        if (willHeal)
        {
            enemy.Heal(15);
            battleText.text = $"Enemigo se cura 15 puntos";
        }
        else
        {
            int damage = enemy.skills[0].power; // ataque básico
            player.TakeDamage(damage);
            battleText.text = $"Enemigo ataca e inflige {damage} de daño";
        }

        yield return new WaitForSeconds(1f);
    }

    private void UpdateUI()
    {
        playerHealthBar.value = player.currentHealth;
        enemyHealthBar.value = enemy.currentHealth;
    }

    // Estos métodos se conectan a los botones
    public void OnAttackButton()
    {
        chosenSkillIndex = 0; // asumimos skill[0] = ataque
        actionChosen = true;
    }

    public void OnHealButton()
    {
        chosenSkillIndex = 1; // asumimos skill[1] = curación
        actionChosen = true;
    }
}
