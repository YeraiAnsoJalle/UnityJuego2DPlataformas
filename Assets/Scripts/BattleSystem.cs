using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleSystem : MonoBehaviour
{
    public Fighter player;
    public Fighter enemy;

    [Header("UI")]
    public Text dialogueText;
    public Transform skillButtonContainer;
    public Button skillButtonPrefab;

    private enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }
    private BattleState state;

    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        dialogueText.text = "¡Aparece " + enemy.fighterName + "!";
        yield return new WaitForSeconds(1.2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        dialogueText.text = "Tu turno: ¡Elige una habilidad!";
        GenerateSkillButtons();
    }

    void GenerateSkillButtons()
    {
        foreach (Transform child in skillButtonContainer)
            Destroy(child.gameObject);

        foreach (Skill skill in player.skills)
        {
            Button btn = Instantiate(skillButtonPrefab, skillButtonContainer);
            btn.GetComponentInChildren<Text>().text = skill.skillName;
            btn.onClick.AddListener(() => OnSkillButton(skill));
        }
    }

    void OnSkillButton(Skill skill)
    {
        if (state != BattleState.PLAYERTURN) return;
        StartCoroutine(PlayerUseSkill(skill));
    }

    IEnumerator PlayerUseSkill(Skill skill)
    {
        // Animación de ataque
        if (player.animator && !string.IsNullOrEmpty(skill.animationTrigger))
            player.animator.SetTrigger(skill.animationTrigger);

        string result = skill.Use(player, enemy);
        dialogueText.text = result;

        yield return new WaitForSeconds(1.5f);

        if (enemy.currentHP <= 0)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemy.fighterName + " está pensando...";
        yield return new WaitForSeconds(1f);

        Skill skill = enemy.skills[Random.Range(0, enemy.skills.Count)];

        if (enemy.animator && !string.IsNullOrEmpty(skill.animationTrigger))
            enemy.animator.SetTrigger(skill.animationTrigger);

        string result = skill.Use(enemy, player);
        dialogueText.text = result;

        yield return new WaitForSeconds(1.5f);

        if (player.currentHP <= 0)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        foreach (Transform child in skillButtonContainer)
            Destroy(child.gameObject);

        if (state == BattleState.WON)
            dialogueText.text = "¡Has ganado la batalla!";
        else
            dialogueText.text = "Has sido derrotado...";
    }
}
