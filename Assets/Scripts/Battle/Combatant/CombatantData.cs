using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Combatant Data")]
public class CombatantData : ScriptableObject
{
    public string combatantName;
    public int baseHp;
    public int baseAttack;
    public int baseDefense;
    public int baseSpeed;
    public MoveData[] moves;
    public GameObject modelPrefab;
    public RuntimeAnimatorController animatorController;
}
