using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlingShot : MonoBehaviour
{
    static SlingShot _instance;
    public static SlingShot xInstance { get { return _instance; } }

    private Vector3 _slingShotMiddleVector;

    public SlingshotState _slingShotState;

    [HideInInspector] public GameObject _doorieToThrow;

    public Transform _leftSSOrigin, _rightSSOrigin;
    public Transform _doorieWaitPosition;

    public LineRenderer _slingShotLineRendererL, _slingShotLineRendererR, _trajectoryLineRenderer;

    [HideInInspector] public float _timeSinceThrow;
    public float _throwSpeed;
    public float _pullDistance = 1.5f;

    public delegate void DooRieThrown();
    public event DooRieThrown _doorieThrown;

    public bool _isThrowReady = true;

    private void yInitializeDooRie()
    {
        

        if (_doorieToThrow)
            _doorieToThrow.transform.position = _doorieWaitPosition.position;

        _slingShotState = SlingshotState.Idle;
        
        ySetSSLinerenderersActive(true);
    }

    void ySetSSLinerenderersActive(bool active)
    {
        _slingShotLineRendererL.enabled = active;
        _slingShotLineRendererR.enabled = active;
    }

    void yDisplaySSLineRenderers()
    {
        if (_doorieToThrow)
        {
            _slingShotLineRendererL.SetPosition(1, _doorieToThrow.transform.position);
            _slingShotLineRendererR.SetPosition(1, _doorieToThrow.transform.position);
        }
    }
    
    void ySetTrajectoryLineRendererActivce(bool active)
    {
        _trajectoryLineRenderer.enabled = active;
    }

    void yDisplayTrajectoryLineRenderer(float distance)
    {
        ySetTrajectoryLineRendererActivce(true);

        Vector3 v2 = _slingShotMiddleVector - _doorieToThrow.transform.position;
        int segmentCount = 15;

        Vector2[] segments = new Vector2[segmentCount];
        segments[0] = _doorieToThrow.transform.position;

        Vector2 segVelocity = new Vector2(v2.x, v2.y) * _throwSpeed * distance;

        for (int i = 1; i < segmentCount; i++)
        {
            float time = i * Time.fixedDeltaTime * 5f;
            segments[i] = segments[0] + segVelocity * time + 0.5f * Physics2D.gravity * Mathf.Pow(time, 2);    
        }

        _trajectoryLineRenderer.positionCount = segmentCount;
        for (int i = 0; i < segmentCount; i++)
            _trajectoryLineRenderer.SetPosition(i, segments[i]);
    }

    private void yThrowDooRie(float distance)
    {
        Vector3 velocity = _slingShotMiddleVector - _doorieToThrow.transform.position;
        _doorieToThrow.GetComponent<DRThrow>().zOnThrow();

        _doorieToThrow.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y) * _throwSpeed * distance;

        if (_doorieThrown != null)
            _doorieThrown();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_isThrowReady);
        if (GameManager.xInstance._isGameStarted == true)
        {
            switch (_slingShotState)
            {
                case SlingshotState.Idle:
                    
                    yInitializeDooRie();
                    yDisplaySSLineRenderers();
                    if (Input.GetMouseButtonDown(0))
                    {
                        Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        if (_doorieToThrow && _doorieToThrow.GetComponent<CircleCollider2D>() == Physics2D.OverlapPoint(location))
                        {
                            _slingShotState = SlingshotState.UserPulling;
                        }
                    }
                    break;

                case SlingshotState.UserPulling:
                    yDisplaySSLineRenderers();
                    if (Input.GetMouseButton(0))
                    {
                        Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        location.z = 0;

                        if (Vector3.Distance(location, _slingShotMiddleVector) > _pullDistance)
                        {
                            var maxPosition = (location - _slingShotMiddleVector).normalized * _pullDistance + _slingShotMiddleVector;
                            _doorieToThrow.transform.position = maxPosition;
                        }
                        else
                        {
                            _doorieToThrow.transform.position = location;
                        }
                        var distance = Vector3.Distance(_slingShotMiddleVector, _doorieToThrow.transform.position);
                        yDisplayTrajectoryLineRenderer(distance);
                    }
                    else
                    {
                        ySetTrajectoryLineRendererActivce(false);
                        _timeSinceThrow = Time.time;

                        float distance = Vector3.Distance(_slingShotMiddleVector, _doorieToThrow.transform.position);

                        if (distance > 1)
                        {
                            ySetSSLinerenderersActive(false);
                            _slingShotState = SlingshotState.DooRieFlying;
                            _isThrowReady = false;
                            yThrowDooRie(distance);
                        }
                        else
                        {
                            _doorieToThrow.transform.DOMove(_doorieWaitPosition.position, distance / 10);
                            yInitializeDooRie();
                        }
                    }
                    break;
            }
        }
    }
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _slingShotState = SlingshotState.Idle;
        _slingShotLineRendererL.SetPosition(0, _leftSSOrigin.position);
        _slingShotLineRendererR.SetPosition(0, _rightSSOrigin.position);

        _slingShotMiddleVector = new Vector3((_leftSSOrigin.position.x + _rightSSOrigin.position.x) / 2,
                                                (_leftSSOrigin.position.y + _rightSSOrigin.position.y) / 2, 0);

    }
}
