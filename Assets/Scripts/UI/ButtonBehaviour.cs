using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    public void LoadSceneAsync(string sceneName)
    {
        LoadingManager.Instance.LoadSceneAsync(sceneName);
    }
}
