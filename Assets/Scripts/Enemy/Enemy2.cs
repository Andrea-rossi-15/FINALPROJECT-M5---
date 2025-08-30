using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum EnemyState2
{
    IDLE,
    PATROLLING,
    FOLLOW,
}

public class Enemy2 : MonoBehaviour
{
    [SerializeField] private float _enemySpeed = 2f;
    [SerializeField] private float _visionDistance = 10f;
    [SerializeField] private float _rotationSpeed = 30f;
    [SerializeField] private Transform _target;

    private EnemyState2 _currentState;

    void Start()
    {
        if (_target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                _target = player.transform;
        }

        ChangeState(EnemyState2.IDLE);
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
            case EnemyState2.IDLE:
                Idle();
                break;
            case EnemyState2.PATROLLING:
                Patrolling();
                break;
            case EnemyState2.FOLLOW:
                Following();
                break;
        }
    }

    void Idle()
    {
        if (CanSeePlayer())
            ChangeState(EnemyState2.FOLLOW);
        else
            ChangeState(EnemyState2.PATROLLING);
    }

    void Patrolling()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);

        if (CanSeePlayer())
            ChangeState(EnemyState2.FOLLOW);
    }

    void Following()
    {
        if (_target == null) return;

        MoveTowards(_target.position);

        float distanceToPlayer = Vector3.Distance(transform.position, _target.position);

        if (!CanSeePlayer() || distanceToPlayer > _visionDistance * 1.5f)
            ChangeState(EnemyState2.PATROLLING);
    }

    void ChangeState(EnemyState2 newState)
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
            if (Physics.Raycast(transform.position + Vector3.up * 1.5f, direction, out RaycastHit hit, _visionDistance))
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
