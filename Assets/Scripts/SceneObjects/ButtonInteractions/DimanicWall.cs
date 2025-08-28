using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimanicWall : ButtonCanvas
{
    [SerializeField] GameObject DinamicWall;
    private bool _isActive = true;

    void Start()
    {
        gameObject.SetActive(true);
    }

    public override void DoSomething()
    {
        if (_isActive == true)
        {
            DinamicWall.SetActive(false);
            Debug.Log("Spento");
            _isActive = false;
        }
        else if (_isActive == false)
        {
            DinamicWall.SetActive(true);
            Debug.Log("Acceso");
            _isActive = true;
        }
    }
}