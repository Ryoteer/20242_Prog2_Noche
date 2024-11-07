using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void LoadSceneAsync(string sceneName)
    {
        LoadingManager.Instance.LoadSceneAsync(sceneName);
    }
}
