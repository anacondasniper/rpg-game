using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private BattleHUD playerHUD;
    [SerializeField] private BattleHUD enemyHUD;
    [SerializeField] private BattleCombatant playerModel;
    [SerializeField] private BattleCombatant enemyModel;
    [SerializeField] private BattleDialogue dialogue;
    private CombatantInstance player;
    private CombatantInstance enemy;
    private BattleState state;

    void Start()
    {
        player = BattleManager.Instance.PlayerCombatant;
        enemy = BattleManager.Instance.EnemyCombatant;
        playerModel.Setup(player);
        enemyModel.Setup(enemy);
        playerHUD.SetData(player);
        enemyHUD.SetData(enemy);
        StartCoroutine(SetupBattle());
    }
    IEnumerator SetupBattle()
    {
        state = BattleState.Start;
        yield return null;
        yield return dialogue.TypeLine($"A wild {enemy.Name} appeared!");
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(PlayerTurn());
    }

    IEnumerator PlayerTurn()
    {
        state = BattleState.PlayerTurn;
        yield return dialogue.TypeLine($"What will\n{player.Name} do?");
        dialogue.ShowActionMenu();
    }

    public void OnMoveSelected(MoveData move)
    {
        if (state != BattleState.PlayerTurn) return;
        StartCoroutine(RunTurn(move));
    }

    public void OnFleeSelected()
    {
        if (state != BattleState.PlayerTurn) return;
        StartCoroutine(TryFlee());
    }

    IEnumerator RunTurn(MoveData playerMove)
    {
        state = BattleState.Resolve;

        yield return playerModel.PlayAttack(playerMove.animationTrigger);
        yield return dialogue.TypeLine($"{player.Name} used {playerMove.moveName}!");

        int dmg = Mathf.Max(1, player.Attack - enemy.Data.baseDefense / 2) * playerMove.power / 10;
        int actual = enemy.TakeDamage(dmg);

        yield return enemyHUD.AnimateHP(enemy);
        yield return dialogue.TypeLine($"It dealt {actual} damage!");

        if (enemy.IsFainted)
        {
            yield return dialogue.TypeLine($"{enemy.Name} fainted!");
            yield return new WaitForSeconds(1.5f);

            BattleManager.Instance.EndBattle(false);
            yield break;
        }

        yield return new WaitForSeconds(0.5f);

        state = BattleState.EnemyTurn;
        MoveData enemyMove = enemy.Data.moves[Random.Range(0, enemy.Data.moves.Length)];

        yield return enemyModel.PlayAttack(enemyMove.animationTrigger);
        yield return dialogue.TypeLine($"{enemy.Name} used {enemyMove.moveName}!");

        int eDmg = Mathf.Max(1, enemy.Attack - player.Data.baseDefense / 2) * enemyMove.power / 10;
        int eActual = player.TakeDamage(eDmg);

        yield return playerHUD.AnimateHP(player);
        yield return dialogue.TypeLine($"It dealt {eActual} damage!");

        if (player.IsFainted)
        {
            yield return dialogue.TypeLine($"{player.Name} fainted!");
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.EndBattle(false);
            yield break;
        }
        StartCoroutine(PlayerTurn());
    }
    IEnumerator TryFlee()
    {
        state = BattleState.Flee;

        yield return dialogue.TypeLine("Got away safely!");
        yield return new WaitForSeconds(1f);
        BattleManager.Instance.EndBattle(false);
    }
}