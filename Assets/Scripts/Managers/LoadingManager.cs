using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    #region Singleton
    public static LoadingManager Instance;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [Header("<color=green>Behaviours</color>")]
    [SerializeField] private float _fadeTime = 0.25f;

    [Header("<color=green>UI</color>")]
    [SerializeField] private Image _bgImage;
    [SerializeField] private Image _loadBarBG;
    [SerializeField] private Image _loadBarImage;
    [SerializeField] private TextMeshProUGUI _doneText;

    private bool _isLoading, _isSceneActive;
    private Color _bgColor;

    private void Start()
    {
        _bgColor = _bgImage.color;
        _bgImage.color = new Color(_bgColor.r, _bgColor.g, _bgColor.b, 0.0f);
        _loadBarBG.enabled = false;
        _doneText.text = "";
    }

    public void LoadSceneAsync(string sceneToLoad)
    {
        if (_isLoading) return;

        StartCoroutine(LoadAsync(sceneToLoad));
    }

    public void LoadSceneAdditive(string sceneToLoad)
    {
        if (_isSceneActive) return;

        StartCoroutine(LoadAdditive(sceneToLoad));
    }

    private void Test(AsyncOperation asyncOp)
    {
        Debug.Log($"<color=blue>:)</color>");
    }

    private IEnumerator LoadAsync(string scene)
    {
        _isLoading = true;

        _bgImage.enabled = true;

        float t = 0.0f;

        while(t < 1.0f)
        {
            t += Time.deltaTime / _fadeTime;
            _bgImage.color = new Color(_bgColor.r, _bgColor.g, _bgColor.b, Mathf.Lerp(0.0f, 1.0f, t));
            yield return null;
        }

        _loadBarBG.enabled = true;

        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(scene);

        asyncOp.allowSceneActivation = false;

        while(asyncOp.progress < 0.9f)
        {
            _loadBarImage.fillAmount = asyncOp.progress / 0.9f;
            yield return null;
        }

        _loadBarBG.enabled = false;
        _loadBarImage.enabled = false;

        _doneText.text = $"Press any button.";

        while (!Input.anyKey)
        {
            yield return null;
        }

        _doneText.text = "";

        asyncOp.allowSceneActivation = true;

        t = 0.0f;

        while (t < 1.0f)
        {
            t += Time.deltaTime / _fadeTime;
            _bgImage.color = new Color(_bgColor.r, _bgColor.g, _bgColor.b, Mathf.Lerp(1.0f, 0.0f, t));
            yield return null;
        }

        _bgImage.enabled = false;

        _isLoading = false;
    }

    private IEnumerator LoadAdditive(string scene)
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        while (!asyncOp.isDone)
        {
            yield return null;
        }

        asyncOp.completed += Test;

        _isSceneActive = true;
    }
}
