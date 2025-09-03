using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
public enum EnemyState
{
    PATROLLING,
    FOLLOWING,
}

public class EnemyController : MonoBehaviour

{
    [Header("Enemy Settings")]
    public float _visionDistance = 10f;
    public float _patrolRadius = 5f;
    public float _patrolTimeout = 5f;
    public float _loseSightTime = 3f;

    private Transform _player;
    private EnemyState _state = EnemyState.PATROLLING;
    private Vector3 _startPos;
    private Vector3 _patrolPoint;
    private float _patrolTimer;
    private float _loseSightTimer = 0f;

    private NavMeshAgent _agent;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _startPos = transform.position;

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) _player = p.transform;

        _patrolPoint = GetRandomPatrolPoint();
        _patrolTimer = 0f;
    }

    void Update()
    {
        if (_player == null) return;

        switch (_state)
        {
            case EnemyState.PATROLLING:
                Patrol();
                break;
            case EnemyState.FOLLOWING:
                Follow();
                break;
        }
    }

    void Patrol()
    {
        if (CanSeePlayer())
        {
            _state = EnemyState.FOLLOWING;
            return;
        }

        _agent.SetDestination(_patrolPoint);
        _patrolTimer += Time.deltaTime;

        if (Vector3.Distance(transform.position, _patrolPoint) < 0.5f || _patrolTimer > _patrolTimeout)
        {
            _patrolPoint = GetRandomPatrolPoint();
            _patrolTimer = 0f;
        }
    }

    void Follow()
    {
        _agent.SetDestination(_player.position);

        if (CanSeePlayer())
        {
            _loseSightTimer = 0f;
        }
        else
        {
            _loseSightTimer += Time.deltaTime;
            if (_loseSightTimer >= _loseSightTime)
            {
                _patrolPoint = GetRandomPatrolPoint();
                _patrolTimer = 0f;
                _state = EnemyState.PATROLLING;
                _loseSightTimer = 0f;
            }
        }
    }

    Vector3 GetRandomPatrolPoint()
    {
        Vector2 randomCircle = Random.insideUnitCircle * _patrolRadius;
        Vector3 point = _startPos + new Vector3(randomCircle.x, 0, randomCircle.y);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(point, out hit, 2f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return _startPos;
    }

    bool CanSeePlayer()
    {
        if (_player == null) return false;

        Vector3 direction = (_player.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, _player.position);

        if (distance <= _visionDistance)
        {
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, direction, out RaycastHit hit, _visionDistance))
            {
                if (hit.collider.CompareTag("Player")) return true;
            }
        }

        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            SceneManager.LoadScene("Level 1");
        }
    }
}
