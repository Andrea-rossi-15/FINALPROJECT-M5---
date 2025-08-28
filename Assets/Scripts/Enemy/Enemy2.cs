using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    [SerializeField] float _visionDistance = 8f;
    [SerializeField] float _speed = 3f;
    [SerializeField] float _rotationInterval = 2f;
    [SerializeField] Transform _player;

    private Vector3 _currentDirection;
    private float _rotationTimer;
    private bool _isFollowing = false;

    void Start()
    {
        PickRandomDirection();
    }

    void Update()
    {
        _rotationTimer -= Time.deltaTime;
        if (_rotationTimer <= 0)
        {
            PickRandomDirection();
        }

        if (CanSeePlayer())
        {
            _isFollowing = true;
        }

        if (_isFollowing)
        {
            Vector3 dir = (_player.position - transform.position).normalized;
            transform.position += dir * _speed * Time.deltaTime;
            transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * 5f);

            if (Vector3.Distance(transform.position, _player.position) > _visionDistance * 1.5f)
                _isFollowing = false;
        }
        else
        {
            transform.forward = Vector3.Lerp(transform.forward, _currentDirection, Time.deltaTime * 5f);
        }
    }

    void PickRandomDirection()
    {
        _currentDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        _rotationTimer = _rotationInterval;
    }

    bool CanSeePlayer()
    {
        Vector3 direction = (_player.position - transform.position).normalized;
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
