using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class SurfaceReference : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.Surface = GetComponent<NavMeshSurface>();
    }
}
