using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroll : MonoBehaviour
{
    public float _parallaxFactor;

    private Vector3 _preCameraPos;

    void Start()
    {
        _preCameraPos = Camera.main.transform.position;
    }

    void Update()
    {
        Vector3 delta = Camera.main.transform.position - _preCameraPos;
        delta.y = 0f;
        delta.z = 0f;
        transform.position += delta / _parallaxFactor;
        _preCameraPos = Camera.main.transform.position;
    }
}
