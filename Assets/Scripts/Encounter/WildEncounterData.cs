using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Wild Encounter Table")]
public class WildEncounterData : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
        public string enemyName;
        public CombatantData combatantData;
        public int minLevel;
        public int maxLevel;
        [Range(0f, 1f)] public float weight;
    }

    public Entry[] entries;

    public Entry Roll()
    {
        float total = 0f;
        foreach (var e in entries) total += e.weight;

        float roll = Random.Range(0f, total);
        float cumulative = 0f;
        foreach (var e in entries)
        {
            cumulative += e.weight;
            if (roll <= cumulative) return e;
        }
        return entries[entries.Length - 1];
    }
}
