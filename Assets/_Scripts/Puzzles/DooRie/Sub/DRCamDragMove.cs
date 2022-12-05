using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DRCamDragMove : MonoBehaviour
{
    [SerializeField] private float _minCameraX = 0f, _maxCameraX = 29.0f;
    [SerializeField] private float _minCameraY = 0f, _maxCameraY = 8.0f;

    private float _dragSpeed = 0.01f;
    private float _timeDragStarted;
    private Vector3 _previousPosition;

    public SlingShot _slingShot;

    void Update()
    {
        if (GameManager.xInstance._isGameStarted == true)
        {
            if (_slingShot._slingShotState == SlingshotState.Idle && Input.touchCount == 1 &&
            (TrPuzzleDooRie.xInstance._gameState == DooRieGameState.Playing || TrPuzzleDooRie.xInstance._gameState == DooRieGameState.Start
            || TrPuzzleDooRie.xInstance._gameState == DooRieGameState.Viewing))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _timeDragStarted = Time.time;
                    _dragSpeed = 0f;
                    _previousPosition = Input.mousePosition;
                }
                else if (Input.GetMouseButton(0) && Time.time - _timeDragStarted > 0.005f)
                {
                    //TrPuzzleDooRie.xInstance._gameState = DooRieGameState.Viewing;
                    Vector3 input = Input.mousePosition;
                    float deltaX = (_previousPosition.x - input.x) * _dragSpeed;
                    float deltaY = (_previousPosition.y - input.y) * _dragSpeed;

                    float newX = Mathf.Clamp(transform.position.x + deltaX, _minCameraX, _maxCameraX);
                    float newY = Mathf.Clamp(transform.position.y + deltaY, _minCameraY, _maxCameraY);

                    transform.position = new Vector3(newX, newY, transform.position.z);

                    _previousPosition = input;

                    /*
                    if (_dragSpeed < 0.1f)
                        _dragSpeed += 0.002f;
                    */
                    _dragSpeed = 0.01f;
                }
            }
        }
    }
}
