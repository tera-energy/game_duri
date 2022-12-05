using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomAndShirnk : MonoBehaviour
{
    public float _minScale = 5f, _maxScale = 11;
    
    Camera _mainCamera;
    
    float _touchesPrevPosDifference, _touchesCurPosiDifference, _zoomModifier;
    [SerializeField] float _zoomModifierSpeed = 0.1f;

    Vector2 _firstTouchPrevPos, _secondTouchPrevPos;


    void Update()
    {
        if(TrPuzzleDooRie.xInstance._slingShot._slingShotState != SlingshotState.DooRieFlying)
        {
            if (Input.touchCount == 2)
            {
                Touch firstTouch = Input.GetTouch(0);
                Touch secondTouch = Input.GetTouch(1);

                _firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
                _secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

                _touchesPrevPosDifference = (_firstTouchPrevPos - _secondTouchPrevPos).magnitude;
                _touchesCurPosiDifference = (firstTouch.position - secondTouch.position).magnitude;

                _zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * _zoomModifierSpeed;

                if (_touchesPrevPosDifference > _touchesCurPosiDifference)
                {
                    if (Camera.main.orthographicSize >= _maxScale)
                    {
                        Camera.main.orthographicSize = _maxScale;
                        return;
                    }
                    _mainCamera.orthographicSize += _zoomModifier;
                    TrPuzzleDooRie.xInstance._gameState = DooRieGameState.Viewing;
                }
                if (_touchesPrevPosDifference < _touchesCurPosiDifference)
                {
                    if (Camera.main.orthographicSize <= _minScale)
                    {
                        Camera.main.orthographicSize = _minScale;
                        return;
                    }
                    _mainCamera.orthographicSize -= _zoomModifier;
                    TrPuzzleDooRie.xInstance._gameState = DooRieGameState.Viewing;
                }
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (Camera.main.orthographicSize >= _maxScale)
                    return;
                Camera.main.orthographicSize += (Time.deltaTime) * _zoomModifierSpeed;
                TrPuzzleDooRie.xInstance._gameState = DooRieGameState.Viewing;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (Camera.main.orthographicSize <= _minScale)
                    return;
                Camera.main.orthographicSize -= (Time.deltaTime) * _zoomModifierSpeed;
                TrPuzzleDooRie.xInstance._gameState = DooRieGameState.Viewing;
            }
            if (TrPuzzleDooRie.xInstance._gameState != DooRieGameState.Viewing && TrPuzzleDooRie.xInstance._gameState != DooRieGameState.Beginning
                && Camera.main.orthographicSize != _minScale)
            {
                // Camera.main.orthographicSize -= Time.deltaTime * 2f;

                if (Camera.main.orthographicSize < _minScale)
                    Camera.main.orthographicSize = _minScale;
            }
        }
        
    }

    void Awake()
    {
        _mainCamera = GetComponent<Camera>();
        
    }
}
