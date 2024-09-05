using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [Header("<color=red>AI</color>")]
    [SerializeField] private float _distanceToChange = 0.5f;

    private Transform _actualNode;
    private List<Transform> _navMeshNodes = new();
    public List<Transform> NavMeshNodes { get { return _navMeshNodes; } set { _navMeshNodes = value; } }

    private NavMeshAgent _agent;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();        
    }

    public void InitalizeAgent()
    {
        Debug.Log($"<color=red>{name}'s nodes</color>: {_navMeshNodes.Count}.");

        _actualNode = GetNewNode();

        _agent.SetDestination(_actualNode.position);
    }

    private void Update()
    {
        if(Vector3.SqrMagnitude(transform.position - _actualNode.position) <= Mathf.Pow(_distanceToChange, 2))
        {
            _actualNode = GetNewNode();
            _agent.SetDestination(_actualNode.position);
        }
    }

    private Transform GetNewNode(Transform lastNode = null)
    {
        if(lastNode == null) return _navMeshNodes[Random.Range(0, _navMeshNodes.Count)];

        Transform newNode = _navMeshNodes[Random.Range(0, _navMeshNodes.Count)];

        while(newNode == lastNode)
        {
            newNode = _navMeshNodes[Random.Range(0, _navMeshNodes.Count)];
        }

        return newNode;
    }
}
