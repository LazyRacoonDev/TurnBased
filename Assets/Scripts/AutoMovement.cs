using UnityEngine;
using UnityEngine.AI;

public class AutoMovement : MonoBehaviour
{    
    public Transform target;           
    private NavMeshAgent agent;        
    private CharacterStats stats;       
    private float stopDistance;        
    private float lastAttackTime;       

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        stats = GetComponent<CharacterStats>();

        if (stats != null && stats.characterClassConfig != null)
        {
            stopDistance = stats.characterClassConfig.attackRange;
            agent.stoppingDistance = stopDistance; // Ajusta NavMeshAgent
        }
        else
        {
            Debug.LogWarning("CharacterStats or CharacterClassConfig not assigned to " + gameObject.name);
        }
    }

    void Update()
    {
        if (target != null)
        {
            // Moverse hacia el objetivo si está fuera del rango de ataque
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget > stopDistance)
            {
                agent.isStopped = false;
                agent.SetDestination(target.position);
            }
            else
            {
                // Detenerse dentro del rango de ataque
                agent.isStopped = true;

                // Intentar atacar si el cooldown ha terminado
                AttackTargetIfInRange();
            }
        }
    }

    void AttackTargetIfInRange()
    {
        // Verificar si el cooldown ha terminado usando attackCooldown del config
        float attackCooldown = stats.characterClassConfig.attackCooldown;
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            // Atacar al objetivo si está en rango
            if (stats.CanAttackTarget(target))
            {
                CharacterStats targetStats = target.GetComponent<CharacterStats>();
                if (targetStats != null)
                {
                    stats.AttackTarget(targetStats);
                    lastAttackTime = Time.time; // Actualizar tiempo del último ataque
                }
            }
        }
    }
}
