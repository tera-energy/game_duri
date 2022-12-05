using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DRCamAnim : MonoBehaviour
{
    Animator _anim;
    
    // Start is called before the first frame update
    void Start()
    {
        _anim.SetTrigger("Beginning");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    void ySetAnimState(int nState)
    {
        if (nState <= 0)
        {
            _anim.enabled = false;
        }
    }
}
