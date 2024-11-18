using UnityEngine;

[CreateAssetMenu(fileName = "CharacterClassConfig", menuName = "Game/CharacterClassConfig")]
public class CharacterClassConfig : ScriptableObject
{
    public string className;         // Nombre de la clase 
    public int maxHealth;            // Vida máxima
    public int attackDamage;         // Daño del ataque
    public float attackRange;        // Rango del ataque
    public float moveSpeed;          // Velocidad de movimiento
    public float attackCooldown;     // Enfriamiento del ataque 
}