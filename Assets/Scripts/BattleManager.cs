using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    [Header("Personajes")]
    public Character player;
    public Character enemy;

    [Header("UI")]
    public Slider playerHealthBar;
    public Slider enemyHealthBar;
    public Text battleText;
    public GameObject actionPanel;

    private bool playerTurn = true;
    private bool battleOver = false;
    private bool actionChosen = false;
    private int chosenSkillIndex = -1;

    void Start()
    {
        // Configurar barras de vida
        playerHealthBar.maxValue = player.maxHealth;
        enemyHealthBar.maxValue = enemy.maxHealth;
        UpdateUI();

        // Iniciar batalla
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
            yield return new WaitForSeconds(0.5f);

            if (player.IsDead() || enemy.IsDead())
            {
                battleOver = true;
                battleText.text = player.IsDead() ? "¡Has perdido!" : "¡Has ganado!";
            }
            else
            {
                playerTurn = !playerTurn;
            }
        }
    }

    private IEnumerator PlayerTurn()
    {
        battleText.text = "Turno del jugador";

        // Resetear elección
        actionChosen = false;
        chosenSkillIndex = -1;

        // Mostrar botones
        actionPanel.SetActive(true);

        // Esperar a que el jugador elija acción
        while (!actionChosen)
        {
            yield return null;
        }

        // Ocultar botones
        actionPanel.SetActive(false);

        // Ejecutar acción elegida
        Skill chosenSkill = player.skills[chosenSkillIndex];

        if (chosenSkill.isHealing)
        {
            player.UseSkill(chosenSkillIndex, null);
            battleText.text = $"Jugador usa {chosenSkill.skillName} y se cura {chosenSkill.power} puntos";
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

        // El enemigo puede curarse si tiene menos del 50% de vida (30% de probabilidad)
        bool willHeal = Random.value < 0.3f && enemy.currentHealth < enemy.maxHealth / 2;

        if (willHeal)
        {
            enemy.Heal(15);
            battleText.text = $"Enemigo se cura 15 puntos de vida";
        }
        else
        {
            int damage = enemy.skills[0].power; // Ataque básico
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

    // =======================
    // Métodos para los botones
    // =======================
    public void OnAttackButton()
    {
        chosenSkillIndex = 0; // Skill[0] = Ataque
        actionChosen = true;
    }

    public void OnHealButton()
    {
        chosenSkillIndex = 1; // Skill[1] = Curación
        actionChosen = true;
    }
}
