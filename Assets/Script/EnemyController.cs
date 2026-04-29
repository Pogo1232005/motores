using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] enemydata data;

    float maxhealth;
    float curHealth;


    [SerializeField] private GameObject target;
    private NavMeshAgent agent;

    [Header("Settings")]
    [SerializeField] float updateRate = 0.2f; // Solo recalcula el camino cada 0.2s

    // Corregido: El parámetro se llama 'transform', pero el campo es 'target'
    // Además, tenías 'target = playerTransform', pero playerTransform no existía.
    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        maxhealth = data.GetMaxHealth();
        curHealth = maxhealth;
        agent = GetComponent<NavMeshAgent>();
    }
    void Move()
    {
        agent.destination = target.transform.position;
    }

    void Update()
    {
        if (target) Move(); //agent.SetDestination(target.transform.position);
    }

    // Opcional: Para que dejen de calcular si mueren o se desactivan
    /*void OnDisable()
    {
        CancelInvoke(nameof(UpdatePath));
    }*/
    public void GetDamage(float amout)
    {
        curHealth = amout;
        if (curHealth <= 0 )
        {
            Destroy(this.gameObject);
        }
    }
}
