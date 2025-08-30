using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
public enum EnemyState
{
    IDLE,
    PATROLLING,
    FOLLOW,
}

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _enemySpeed = 2f;
    [SerializeField] private float _visionDistance = 10f;
    [SerializeField] private Transform _target;
    [SerializeField] private float _patrolRadius = 5f;
    [SerializeField] private float _patrolTimeout = 5f;

    private EnemyState _currentState;
    private Vector3 _patrolPoint;
    private Vector3 _startPosition;
    private float _patrolTimer;

    void Start()
    {
        if (_target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                _target = player.transform;
        }

        _startPosition = transform.position;
        _patrolPoint = GetRandomPatrolPoint();
        ChangeState(EnemyState.IDLE);
        _patrolTimer = 0f;
    }

    void Update()
    {
        if (_target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                _target = player.transform;
            else
                return;
        }

        switch (_currentState)
        {
            case EnemyState.IDLE:
                Idle();
                break;
            case EnemyState.PATROLLING:
                Patrolling();
                break;
            case EnemyState.FOLLOW:
                Following();
                break;
        }
    }

    void Idle()
    {
        if (CanSeePlayer())
            ChangeState(EnemyState.FOLLOW);
        else
            ChangeState(EnemyState.PATROLLING);
    }

    void Patrolling()
    {
        if (CanSeePlayer())
        {
            ChangeState(EnemyState.FOLLOW);
            return;
        }

        _patrolTimer += Time.deltaTime;
        MoveTowards(_patrolPoint);

        if (Vector3.Distance(transform.position, _patrolPoint) < 0.5f || _patrolTimer > _patrolTimeout)
        {
            _patrolPoint = GetRandomPatrolPoint();
            _patrolTimer = 0f;
        }
    }

    void Following()
    {
        MoveTowards(_target.position);

        float distanceToPlayer = Vector3.Distance(transform.position, _target.position);
        if (!CanSeePlayer() || distanceToPlayer > _visionDistance * 1.5f)
        {
            _patrolPoint = GetRandomPatrolPoint();
            _patrolTimer = 0f;
            ChangeState(EnemyState.PATROLLING);
        }
    }

    Vector3 GetRandomPatrolPoint()
    {
        Vector2 randomCircle = Random.insideUnitCircle * _patrolRadius;
        return _startPosition + new Vector3(randomCircle.x, 0, randomCircle.y);
    }

    void ChangeState(EnemyState newState)
    {
        _currentState = newState;
    }

    void MoveTowards(Vector3 destination)
    {
        Vector3 direction = (destination - transform.position).normalized;
        direction.y = 0;

        transform.position = Vector3.MoveTowards(transform.position, destination, _enemySpeed * Time.deltaTime);

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    bool CanSeePlayer()
    {
        if (_target == null) return false;

        Vector3 direction = (_target.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, _target.position);

        if (distanceToPlayer <= _visionDistance)
        {
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, direction, out RaycastHit hit, _visionDistance))
            {
                if (hit.collider.CompareTag("Player"))
                    return true;
            }
        }
        return false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            SceneManager.LoadScene("Level 1");
        }
    }
}
