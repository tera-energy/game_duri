using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeShakeAnim : MonoBehaviour
{
    Animator _anim;

    bool _animPlaying = false;
    [SerializeField] ParticleSystem _parLeaf;

    void yDisableAnimator()
    {
        
    }
    
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _animPlaying = false;
        _anim.StopPlayback();
        _anim.enabled = false;
    }

    public void zPlayTreeShakeAnim()
    {
        if (_animPlaying)
            return;

        _animPlaying = true;
        _anim.enabled = true;
        _anim.SetTrigger("Sway");
        _parLeaf.Play();
    }
}
