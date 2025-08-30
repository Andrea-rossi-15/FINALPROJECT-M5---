using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Vector3 moveDirection;

    public float _speed = 3.5f;

    public enum MoveTipe { WASD, ClickToMove };
    public MoveTipe _moveTipe;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _speed;
        _agent.updateRotation = true;
    }

    void Update()
    {
        switch (_moveTipe)
        {
            case MoveTipe.ClickToMove:
                ClickToMove();
                break;

            case MoveTipe.WASD:
                WASDToMove();
                break;
        }
    }

    void ClickToMove()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                _agent.SetDestination(hit.point);
            }
        }
    }

    void WASDToMove()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            Vector3 targetPosition = transform.position + moveDirection;
            _agent.SetDestination(targetPosition);
        }
    }


}




