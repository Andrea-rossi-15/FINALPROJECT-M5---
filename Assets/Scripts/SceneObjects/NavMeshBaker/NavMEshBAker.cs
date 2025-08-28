using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMEshBAker : MonoBehaviour
{
    public NavMeshSurface _surface;

    void Start()
    {
        _surface.BuildNavMesh();
    }

    public void AggiornaNavMesh()
    {
        _surface.BuildNavMesh();
    }
}
