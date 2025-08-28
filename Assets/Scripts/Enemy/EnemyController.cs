using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
public enum EnemyState
{
    IDLE,
    PATROLLING,
    FOLLOW,
}

public class EnemyController : MonoBehaviour
{


    private float _enemySpeed = 2f;
    private float _visionDistance = 10f;
    [SerializeField] Transform _target;
    private EnemyState _currentState;
    private Vector3 _patrolPoint;
    private bool _isFollowing = false;

    void Start()
    {
        ChangeState(EnemyState.IDLE);
        _patrolPoint = GetRandomPatrolPoint();
    }

    void Update()
    {
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
            _isFollowing = true;

        if (_isFollowing)
            ChangeState(EnemyState.FOLLOW);
    }

    void Patrolling()
    {
        MoveTowards(_patrolPoint);

        if (Vector3.Distance(transform.position, _patrolPoint) < 0.5f)
            _patrolPoint = GetRandomPatrolPoint();

        if (CanSeePlayer())
            _isFollowing = true;

        if (_isFollowing)
            ChangeState(EnemyState.FOLLOW);
    }

    void Following()
    {
        MoveTowards(_target.position);

        if (!CanSeePlayer() && Vector3.Distance(transform.position, _target.position) > _visionDistance * 1.5f)
            _isFollowing = false;

        if (!_isFollowing)
            ChangeState(EnemyState.PATROLLING);
    }

    void ChangeState(EnemyState _newState)
    {
        _currentState = _newState;
    }

    void MoveTowards(Vector3 _destination)
    {
        transform.position = Vector3.MoveTowards(transform.position, _destination, _enemySpeed * Time.deltaTime);
        Vector3 direction = (_destination - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(-direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    Vector3 GetRandomPatrolPoint()
    {
        Vector3 _randomDir = Random.insideUnitSphere * 5f;
        _randomDir.y = 0;
        return transform.position + _randomDir;
    }

    bool CanSeePlayer()
    {
        Vector3 direction = (_target.position - transform.position).normalized;
        Ray ray = new Ray(transform.position + Vector3.up * 1f, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _visionDistance))
        {
            if (hit.collider.CompareTag("Player"))
                return true;
        }
        return false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
        }
    }
}
