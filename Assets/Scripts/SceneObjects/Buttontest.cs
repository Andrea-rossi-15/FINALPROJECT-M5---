using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttontest : MonoBehaviour
{
    [SerializeField] GameObject _messageUI;
    [SerializeField] GameObject Wall;
    private bool _isPlayerInside = false;
    private bool _isWallActive = true;

    // Start is called before the first frame update
    void Start()
    {
        _messageUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlayerInside && (Input.GetKeyDown(KeyCode.E)) && _isWallActive)
        {
            Wall.SetActive(false);
            _isWallActive = false;


        }
        else if (_isPlayerInside && (Input.GetKeyDown(KeyCode.E)) && _isWallActive == false)
        {
            Wall.SetActive(true);
            _isWallActive = true;

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
}
