using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTrigger : MonoBehaviour
{
    [Header("<color=blue>Behaviours</color>")]
    [SerializeField] private string _sceneToLoad = "CaveLevel";

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            LoadingManager.Instance.LoadSceneAdditive(_sceneToLoad);
        }
    }
}
