using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRequester : MonoBehaviour
{
    [Header("<color=orange>Audio</color>")]
    [SerializeField] private AudioClip _clipToPlay;

    private void Start()
    {
        AudioManager.Instance.PlayClip(_clipToPlay);
    }
}
