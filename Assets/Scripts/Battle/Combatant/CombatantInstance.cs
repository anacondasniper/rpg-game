using UnityEngine;

public class CombatantInstance
{
    public CombatantData Data { get; }
    public string Name { get; }
    public int Level { get; }
    public int MaxHp { get; }
    public int CurrentHp { get; private set; }
    public int Attack { get; }
    public int Defense { get; }
    public int Speed { get; }

    public bool IsFainted => CurrentHp <= 0;

    public CombatantInstance(CombatantData data, int level)
    {
        Data = data;
        Name = data.combatantName;
        Level = level;

        MaxHp = data.baseHp + level * 3;
        Attack = data.baseAttack + level * 2;
        Defense = data.baseDefense + level * 2;
        Speed = data.baseSpeed + level * 1;
        CurrentHp = MaxHp;
    }

    public int TakeDamage(int damage)
    {
        int actual = Mathf.Max(1, damage - Defense / 2);
        CurrentHp = Mathf.Max(0, CurrentHp - actual);
        return actual;
    }
}
