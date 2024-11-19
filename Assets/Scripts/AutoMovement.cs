using UnityEngine;
using UnityEngine.AI;

public class AutoMovement : MonoBehaviour
{
    public Transform target;           // Objetivo actual
    private NavMeshAgent agent;        // Referencia al NavMeshAgent
    private CharacterStats stats;      // Referencia a las estadísticas del personaje
    private float stopDistance;        // Distancia para detenerse
    private float lastAttackTime;      // Tiempo del último ataque
    private string enemyFactionTag;    // Tag de la facción enemiga

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        stats = GetComponent<CharacterStats>();

        if (stats != null && stats.characterClassConfig != null)
        {
            stopDistance = stats.characterClassConfig.attackRange;
            agent.stoppingDistance = stopDistance;

            // Determinar la facción enemiga en base al tag
            if (gameObject.CompareTag("faction1"))
            {
                enemyFactionTag = "faction2";
            }
            else if (gameObject.CompareTag("faction2"))
            {
                enemyFactionTag = "faction1";
            }
            else
            {
                Debug.LogWarning("Tag del objeto no reconocido: " + gameObject.name);
            }
        }
        else
        {
            Debug.LogWarning("CharacterStats o CharacterClassConfig no asignado a " + gameObject.name);
        }
    }

    void Update()
    {
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            FindTarget();
        }

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

    void FindTarget()
    {
        // Buscar el objetivo más cercano con el tag de la facción enemiga
        GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag(enemyFactionTag);
        float closestDistance = Mathf.Infinity;
        Transform closestTarget = null;

        foreach (GameObject potentialTarget in potentialTargets)
        {
            float distance = Vector3.Distance(transform.position, potentialTarget.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = potentialTarget.transform;
            }
        }

        if (closestTarget != null)
        {
            target = closestTarget;
        }
    }

    void AttackTargetIfInRange()
    {
        float attackCooldown = stats.characterClassConfig.attackCooldown;
        if (Time.time >= lastAttackTime + attackCooldown)
        {
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