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
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBattleMusic();
        }

        if (PlayerData.currentHealth > 0)
        {
            player.currentHealth = PlayerData.currentHealth;
        }

        if (BattleData.enemyToLoad != null)
        {
            enemy.fighterName = BattleData.enemyToLoad.name;
            enemy.maxHealth = BattleData.enemyToLoad.maxHealth;
            enemy.currentHealth = BattleData.enemyToLoad.maxHealth;

            if (enemy.skills.Count == 0)
            {
                enemy.skills.Add(new Skill
                {
                    skillName = "Ataque",
                    power = BattleData.enemyToLoad.attackPower,
                    isHealing = false
                });
            }

            Debug.Log($"Enemigo creado: {enemy.fighterName}, Vida: {enemy.maxHealth}, Ataque: {BattleData.enemyToLoad.attackPower}");
        }
        else
        {
            Debug.LogError("No hay datos de enemigo para cargar en la batalla");
        }

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
                yield return StartCoroutine(PlayerTurn());
            else
                yield return StartCoroutine(EnemyTurn());

            UpdateUI();
            yield return new WaitForSeconds(0.5f);

            if (player.IsDead() || enemy.IsDead())
            {
                battleOver = true;
                state = player.IsDead() ? BattleState.LOST : BattleState.WON;
                yield return StartCoroutine(EndBattle());
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
            player.Heal(chosenSkill.power);
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

        if (enemy.skills.Count > 0)
        {
            Skill enemySkill = enemy.skills[0];
            enemy.UseSkill(0, player);
            battleText.text = $"{enemy.fighterName} usa {enemySkill.skillName} e inflige {enemySkill.power} de daño";
        }

        yield return new WaitForSeconds(1f);
    }

    private void UpdateUI()
    {
        playerHealthBar.value = player.currentHealth;
        enemyHealthBar.value = enemy.currentHealth;
    }

    private IEnumerator EndBattle()
    {
        if (state == BattleState.WON)
        {
            battleText.text = "¡Has ganado!";
            BattleSessionData.defeatedEnemies.Add(BattleData.enemyToLoad.uniqueID);

            PlayerData.currentHealth = player.currentHealth;
            yield return new WaitForSeconds(2f);

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayGameMusic();
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("Juego");
        }
        else if (state == BattleState.LOST)
        {
            battleText.text = "Has sido derrotado...";
            yield return new WaitForSeconds(2f);

            SceneManager.LoadScene("GameOver");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Juego")
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerObj.transform.position = PlayerPositionManager.lastPosition;
                Character playerChar = playerObj.GetComponent<Character>();
                if (playerChar != null)
                {
                    playerChar.currentHealth = PlayerData.currentHealth;
                }
            }
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

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