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

    [Header("<color=red>Behaviours</color>")]
    [SerializeField] private int _maxHP = 100;

    private int _actualHP;

    private Transform _actualNode;
    private List<Transform> _navMeshNodes = new();
    public List<Transform> NavMeshNodes 
    { 
        get { return _navMeshNodes; } 
        set { _navMeshNodes = value; } 
    }

    private NavMeshAgent _agent;

    private void Awake()
    {
        _actualHP = _maxHP;

        GameManager.Instance.Enemies.Add(this);
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();        
    }

    public void Initialize()
    {
        Debug.Log($"Available nodes (<color=red>{name}</color>): {_navMeshNodes.Count}.");

        _actualNode = GetNewNode();

        _agent.SetDestination(_actualNode.position);
    }

    private void Update()
    {
        if (!_actualNode) return;

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

    public void TakeDamage(int dmg)
    {
        _actualHP -= dmg;

        if(_actualHP <= 0)
        {
            Debug.Log($"<color=red>{name}</color>: oh no *Explota*");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"<color=red>{name}</color>: Parate de manos gil.");
        }
    }
}
