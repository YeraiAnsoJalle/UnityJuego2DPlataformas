using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    private enum BattleState { PLAYERTURN, ENEMYTURN, WON, LOST }
    private BattleState state;

    void Start()
    {
        // Cargar vida del jugador desde datos guardados
        if (PlayerData.currentHealth > 0)
        {
            player.currentHealth = PlayerData.currentHealth;
        }

        // Cargar datos del enemigo desde BattleData
        if (BattleData.enemyToLoad != null)
        {
            enemy.fighterName = BattleData.enemyToLoad.name;
            enemy.maxHealth = BattleData.enemyToLoad.maxHealth;
            enemy.currentHealth = BattleData.enemyToLoad.maxHealth;

            // Añadir skill de ataque básico al enemigo si no tiene
            if (enemy.skills.Count == 0)
            {
                enemy.skills.Add(new Skill
                {
                    skillName = "Ataque",
                    power = BattleData.enemyToLoad.attackPower,
                    isHealing = false
                });
            }
        }

        // Configurar barras de vida
        playerHealthBar.maxValue = player.maxHealth;
        enemyHealthBar.maxValue = enemy.maxHealth;
        UpdateUI();

        battleText.text = "¡Comienza la batalla!";
        StartCoroutine(BattleLoop());
    }

    private IEnumerator BattleLoop()
    {
        yield return new WaitForSeconds(1f);

        while (!battleOver)
        {
            if (playerTurn)
                yield return PlayerTurn();
            else
                yield return EnemyTurn();

            UpdateUI();
            yield return new WaitForSeconds(0.5f);

            if (player.IsDead() || enemy.IsDead())
            {
                battleOver = true;
                state = player.IsDead() ? BattleState.LOST : BattleState.WON;
                EndBattle();
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
        actionChosen = false;
        chosenSkillIndex = -1;
        actionPanel.SetActive(true);

        while (!actionChosen)
            yield return null;

        actionPanel.SetActive(false);

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

        // Verificar que el enemigo tenga skills
        if (enemy.skills == null || enemy.skills.Count == 0)
        {
            battleText.text = "Enemigo no tiene habilidades";
            yield break;
        }

        bool willHeal = Random.value < 0.3f && enemy.currentHealth < enemy.maxHealth / 2;

        if (willHeal)
        {
            enemy.Heal(15);
            battleText.text = $"Enemigo se cura 15 puntos de vida";
        }
        else
        {
            int damage = enemy.skills[0].power;
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

    private void EndBattle()
    {
        if (state == BattleState.WON)
        {
            battleText.text = "¡Has ganado!";

            // Guardar enemigo como derrotado en esta sesión
            if (BattleData.enemyToLoad != null && !string.IsNullOrEmpty(BattleData.enemyToLoad.uniqueID))
            {
                BattleSessionData.defeatedEnemies.Add(BattleData.enemyToLoad.uniqueID);
            }
        }
        else if (state == BattleState.LOST)
        {
            battleText.text = "Has sido derrotado...";
        }

        // Guardar vida actual del jugador para mantenerla entre combates
        PlayerData.currentHealth = player.currentHealth;

        StartCoroutine(ReturnToMap());
    }

    private IEnumerator ReturnToMap()
    {
        yield return new WaitForSeconds(2f);

        // Suscribirse al evento de carga de escena
        SceneManager.sceneLoaded += OnSceneLoaded;

        SceneManager.LoadScene("Juego"); // tu escena del mapa
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Juego") return;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            // Restaurar posición
            playerObj.transform.position = PlayerPositionManager.lastPosition;

            // Restaurar vida
            Character playerChar = playerObj.GetComponent<Character>();
            if (playerChar != null)
            {
                playerChar.currentHealth = PlayerData.currentHealth;
            }
        }

        // Desuscribirse del evento
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // =======================
    // Botones
    // =======================
    public void OnAttackButton()
    {
        chosenSkillIndex = 0;
        actionChosen = true;
    }

    public void OnHealButton()
    {
        chosenSkillIndex = 1;
        actionChosen = true;
    }
}