using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AINodeManager : MonoBehaviour
{
    private Transform[] _nodes;

    private void Start()
    {
        _nodes = GetComponentsInChildren<Transform>();

        foreach (Enemy enemy in GameManager.Instance.Enemies)
        {
            enemy.NavMeshNodes.AddRange(_nodes);

            if(enemy.NavMeshNodes.Count > 0)
            {
                enemy.Initialize();
            }
            else
            {
                Destroy(enemy.gameObject);
            }
        }
    }
}
