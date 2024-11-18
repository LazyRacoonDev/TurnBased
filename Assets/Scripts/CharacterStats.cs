using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterClassConfig characterClassConfig; // Configuración de la clase

    private int currentHealth;  // Salud actual
    private int attackDamage;   // Daño del ataque
    private float attackRange;  // Rango de ataque
    private float moveSpeed;    // Velocidad de movimiento

    void Start()
    {
        // Aplicar la configuración de la clase
        if (characterClassConfig != null)
        {
            currentHealth = characterClassConfig.maxHealth;
            attackDamage = characterClassConfig.attackDamage;
            attackRange = characterClassConfig.attackRange;
            moveSpeed = characterClassConfig.moveSpeed;
        }
        else
        {
            Debug.LogWarning("CharacterClassConfig not assigned to " + gameObject.name);
        }
    }

    // Método para recibir daño
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Asegurarse de que la salud no sea menor que 0
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    // Método para atacar un objetivo
    public void AttackTarget(CharacterStats targetStats)
    {
        if (targetStats != null && CanAttackTarget(targetStats.transform))
        {
            targetStats.TakeDamage(attackDamage);
            Debug.Log($"{gameObject.name} attacked {targetStats.gameObject.name} for {attackDamage} damage.");
        }
    }

    // Verificar si el objetivo está en rango
    public bool CanAttackTarget(Transform target)
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        return distanceToTarget <= attackRange;
    }

    // Método que se ejecuta al morir
    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
    }
}