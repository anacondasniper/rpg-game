using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    [SerializeField] private string battleSceneName = "BattleScene";
    [SerializeField] private CombatantData playerData;
    [SerializeField] private Camera overworldCamera;

    public event Action<WildEncounterData.Entry> OnBattleStart;
    public event Action OnBattleEnd;

    public bool InBattle { get; private set; }

    public CombatantInstance PlayerCombatant { get; private set; }
    public CombatantInstance EnemyCombatant { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartWildBattle(WildEncounterData.Entry enemy)
    {
        if (InBattle) return;
        InBattle = true;

        int level = UnityEngine.Random.Range(enemy.minLevel, enemy.maxLevel + 1);
        EnemyCombatant = new CombatantInstance(enemy.combatantData, level);
        PlayerCombatant = new CombatantInstance(playerData, 20);
        Debug.Log($"Wild {enemy.enemyName} (Lv.{level}) appeared!");

        OnBattleStart?.Invoke(enemy);
        overworldCamera.gameObject.SetActive(false);
        SceneManager.LoadSceneAsync(battleSceneName ,LoadSceneMode.Additive);
    }

    public void EndBattle(bool playerWon)
    {
        InBattle = false;
        OnBattleEnd?.Invoke();
        overworldCamera.gameObject.SetActive(true);
        SceneManager.UnloadSceneAsync(battleSceneName);
    }
}
