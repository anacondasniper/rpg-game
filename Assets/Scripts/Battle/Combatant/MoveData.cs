using UnityEngine;

[CreateAssetMenu(menuName ="RPG/Move Data")]
public class MoveData : ScriptableObject
{
    public string moveName;
    public int power;
    public int pp;
    public string animationTrigger;
}
