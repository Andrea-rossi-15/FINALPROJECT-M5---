using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public enum EnemyState2
{
    IDLE,
    FOLLOW,
}

public class Enemy2 : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float _visionDistance = 8f;
    public float _loseSightTime = 3f;

    private Transform _player;
    private NavMeshAgent _agent;
    private Vector3 _startPos;
    private float _loseSightTimer = 0f;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _startPos = transform.position;

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) _player = p.transform;
    }

    void Update()
    {
        if (_player == null) return;

        float distance = Vector3.Distance(transform.position, _player.position);

        if (distance <= _visionDistance && _CanSeePlayer())
        {
            _FollowPlayer();
        }
        else
        {
            _ReturnToStart();
        }
    }

    void _FollowPlayer()
    {
        _agent.SetDestination(_player.position);

        if (_CanSeePlayer())
        {
            _loseSightTimer = 0f;
        }
        else
        {
            _loseSightTimer += Time.deltaTime;
            if (_loseSightTimer >= _loseSightTime)
            {
                _loseSightTimer = 0f;
            }
        }
    }

    void _ReturnToStart()
    {
        if (Vector3.Distance(transform.position, _startPos) > 0.5f)
        {
            _agent.SetDestination(_startPos);
        }
        else
        {
            _agent.ResetPath();
        }
    }

    bool _CanSeePlayer()
    {
        if (_player == null) return false;

        Vector3 dir = (_player.position - transform.position).normalized;
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, out RaycastHit hit, _visionDistance))
        {
            return hit.collider.CompareTag("Player");
        }
        return false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            SceneManager.LoadScene("Level 1");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _visionDistance);
    }
}
