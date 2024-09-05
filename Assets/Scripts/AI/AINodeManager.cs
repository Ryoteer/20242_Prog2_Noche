using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AINodeManager : MonoBehaviour
{
    [Header("<color=red>AI</color>")]
    [SerializeField] private List<Enemy> _enemies = new();

    private Transform[] _nodes;

    private void Start()
    {
        _nodes = GetComponentsInChildren<Transform>();

        foreach (Enemy enemy in _enemies)
        {
            enemy.NavMeshNodes.AddRange(_nodes);

            enemy.InitalizeAgent();
        }
    }
}
