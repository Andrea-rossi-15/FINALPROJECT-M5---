using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonCanvas : MonoBehaviour
{
    [SerializeField] GameObject _messageUI;
    private bool _isPlayerInside = false;

    void Start()
    {
        _messageUI.SetActive(false);
    }
    void Update()
    {
        if (_isPlayerInside = true && Input.GetKeyDown(KeyCode.E))
        {
            DoSomething();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = true;
            _messageUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = false;
            _messageUI.SetActive(false);
        }
    }

    public virtual void DoSomething() { }
}
