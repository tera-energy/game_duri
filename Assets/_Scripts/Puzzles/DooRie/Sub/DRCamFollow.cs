using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DRCamFollow : MonoBehaviour
{
    [HideInInspector] public Vector3 _startingPosition;
    [HideInInspector] public bool _isFollowing;
    [HideInInspector] public Transform _doorieFollow;
    [SerializeField] private float _minCameraX = 0, _maxCameraX = 29.0f;

    void Awake()
    {
        _startingPosition = transform.position;
    }

    void LateUpdate()
    {
        if (_isFollowing)
        {
            if (_doorieFollow != null)
            {
                var dooriePosition = _doorieFollow.position;
                float x = Mathf.Clamp(dooriePosition.x, _minCameraX, _maxCameraX);
                float y = Mathf.Clamp(dooriePosition.y, 0, 8);
                transform.position = new Vector3(x, y, _startingPosition.z);
                /*
                var cameraTempPos = transform.position;
                cameraTempPos.x = Mathf.Lerp(cameraTempPos.x, x, 5 * Time.deltaTime);
                transform.position = cameraTempPos;
                */

            }
            else
                _isFollowing = false;
        }
    }
}
