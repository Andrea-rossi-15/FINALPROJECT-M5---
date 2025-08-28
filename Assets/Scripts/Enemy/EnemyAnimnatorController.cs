using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimnatorController : MonoBehaviour
{
    private Animator _enemyanimator;
    private NavMeshAgent _enemy;
    // Start is called before the first frame update
    void Start()
    {
        _enemyanimator = GetComponent<Animator>();
        _enemy = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float Blend = _enemy.velocity.magnitude;

        _enemyanimator.SetFloat("Blend", Blend);
    }
}
