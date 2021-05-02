using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAttack", menuName ="Attacks")]
public class MeleeAttack : ScriptableObject
{
    public float damage = 1;
    public float effectRadius = 0.1f;
}
