using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    [Header("<color=orange>Audio</color>")]
    [SerializeField] private AudioClip[] _stepsSFX;
    [SerializeField] private AudioClip[] _attacksSFX;

    private Player _parent;

    private AudioSource _source;

    private void Start()
    {
        _parent = GetComponentInParent<Player>();

        _source = GetComponent<AudioSource>();
    }

    public void Attack()
    {
        _parent.Attack();
    }

    public void PlayAttackSFX(int i)
    {
        if (_source.isPlaying) _source.Stop();

        _source.clip = _attacksSFX[i];

        _source.Play();
    }

    public void SpearAttack()
    {
        _parent.SpearAttack();
    }

    public void Step()
    {
        if (_source.isPlaying) _source.Stop();

        _source.clip = _stepsSFX[Random.Range(0, _stepsSFX.Length)];

        _source.Play();
    }
}
