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
    [SerializeField] private float _patrolDuration = 3f;
    [SerializeField] private Transform _target;

    private EnemyState _currentState;
    private Vector3 _patrolPoint;
    private bool _isFollowing = false;
    private float _patrolTimer = 0f;

    void Start()
    {
        if (_target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                _target = player.transform;
        }

        _patrolPoint = GetRandomPatrolPoint();
        ChangeState(EnemyState.IDLE);
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


        if (CanSeePlayer() && !_isFollowing)
        {
            _isFollowing = true;
            ChangeState(EnemyState.FOLLOW);
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
        {
            _isFollowing = true;
            ChangeState(EnemyState.FOLLOW);
        }
    }

    void Patrolling()
    {
        MoveTowards(_patrolPoint);

        if (Vector3.Distance(transform.position, _patrolPoint) < 0.5f)
            _patrolPoint = GetRandomPatrolPoint();

        if (CanSeePlayer())
        {
            _isFollowing = true;
            ChangeState(EnemyState.FOLLOW);
        }

        _patrolTimer -= Time.deltaTime;
        if (_patrolTimer <= 0f)
            ChangeState(EnemyState.IDLE);
    }

    void Following()
    {
        MoveTowards(_target.position);

        float distanceToPlayer = Vector3.Distance(transform.position, _target.position);
        if (!CanSeePlayer() || distanceToPlayer > _visionDistance * 1.5f)
        {
            _isFollowing = false;
            _patrolTimer = _patrolDuration;
            _patrolPoint = GetRandomPatrolPoint();
            ChangeState(EnemyState.PATROLLING);
        }
    }

    void ChangeState(EnemyState newState)
    {
        _currentState = newState;
        if (_currentState == EnemyState.PATROLLING)
            _patrolTimer = _patrolDuration;
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

    Vector3 GetRandomPatrolPoint()
    {
        Vector3 randomDir = Random.insideUnitSphere * 5f;
        randomDir.y = 0;
        return transform.position + randomDir;
    }

    bool CanSeePlayer()
    {
        if (_target == null) return false;

        Vector3 enemyEye = transform.position + Vector3.up * 1.2f;
        Vector3 playerCenter = _target.position + Vector3.up * 1.2f;
        Vector3 direction = (playerCenter - enemyEye).normalized;
        float distanceToPlayer = Vector3.Distance(enemyEye, playerCenter);

        if (distanceToPlayer <= _visionDistance)
        {
            Ray ray = new Ray(enemyEye, direction);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, _visionDistance))
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _visionDistance);
    }
}
