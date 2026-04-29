using UnityEngine;

[CreateAssetMenu(fileName = "enemydata", menuName = "Scriptable Objects/enemydata")]
public class enemydata : ScriptableObject
{
    [SerializeField] float MaxHealth;
    [SerializeField] float Damage;

    public float GetMaxHealth() {  return MaxHealth; }
    public float GetDamage() { return Damage; }
}
