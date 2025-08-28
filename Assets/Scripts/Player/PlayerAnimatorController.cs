using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator _animator;
    private NavMeshAgent _player;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _player = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float speed = _player.velocity.magnitude;

        _animator.SetFloat("Speed", speed);
    }
}
