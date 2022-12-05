using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using DG.Tweening;
using TMPro;
using System.Text;
using System;

public class TrPuzzleDooRie : TrPuzzleManager
{
    static TrPuzzleDooRie _instance;
    public static TrPuzzleDooRie xInstance { get { return _instance; } }

    bool _isOnVibration;
    bool _isTimeTenseEffectExec;

    public DRCamFollow _cameraFollow;

    [SerializeField] GameObject _mainCamera;

    public SlingShot _slingShot;

    public DooRieGameState _gameState = DooRieGameState.Beginning;

    private List<GameObject> _doorie;
    public List<GameObject> _fruit;

    private int _totalDooRieCount;
    private int _doorieThrownCount;

    [HideInInspector] public int _totalFruitCount;
    [HideInInspector] public int _fellOffFruitCount;

    [SerializeField] TextMeshProUGUI _txt;

    StringBuilder _stringBuilder = new StringBuilder();

    [SerializeField] GameObject _nextBtn;
    [SerializeField] GameObject _retryBtn;



    private void ySlingShotDooRieThrown()
    {
        _totalDooRieCount--;
        _cameraFollow._isFollowing = true;
    }

    void OnEnable()
    {
        _slingShot._doorieThrown += ySlingShotDooRieThrown;
    }

    void OnDisable()
    {
        _slingShot._doorieThrown -= ySlingShotDooRieThrown;
    }

    void yAnimateDooRieToSlingShot(GameObject doorie)
    {
        var pickedDoorie = _doorie.FirstOrDefault(x => x == doorie);
        pickedDoorie.GetComponent<Rigidbody2D>().isKinematic = true;
        _gameState = DooRieGameState.DooRieMovingToSlingshot;

        pickedDoorie.transform.DOMove(_slingShot._doorieWaitPosition.position, 
            Vector2.Distance(pickedDoorie.transform.position / 10, _slingShot._doorieWaitPosition.position) / 10)
            .OnComplete(() =>
            {
                _gameState = DooRieGameState.Playing;
                _slingShot.enabled = true;

                //slingShot.slingShootLineRenderer1.enabled = true;
                //slingShot.slingShootLineRenderer2.enabled = true;

                _slingShot._doorieToThrow = pickedDoorie;
                _cameraFollow._doorieFollow = pickedDoorie.transform;
            });
    }

    bool yObjectsStoppedMoving()
    {
        foreach ( var item in _fruit.Union(_doorie) )
        {
            if (item != null && item.GetComponent<Rigidbody2D>().velocity.sqrMagnitude > GameVariavles._minVelocity)
                return false;
                
        }
        return true;
    }

    private bool yFruitDestroyed()
    {
        return _totalFruitCount == _fellOffFruitCount;
    }

    private void yAnimateCameraToStartPosition()
    {
        float duration = Vector2.Distance(_mainCamera.transform.position, _cameraFollow._startingPosition) / 10;
        if (duration == 0.0f)
            duration = 0.1f;

        _mainCamera.transform.DOMove(_cameraFollow._startingPosition, duration).OnComplete(()=>
        {
            if (yFruitDestroyed())
            {
                _gameState = DooRieGameState.Won;
            }
            else if (_totalDooRieCount <= _doorieThrownCount)
            {
                _gameState = DooRieGameState.Lost;
            }
            else
            {
                _slingShot._slingShotState = SlingshotState.Idle;
                //yAnimateDooRieToSlingShot();
            }
        });
    }
    IEnumerator yExecTicTok()
    {
        while (_currGameTime > 0 && _currGameTime < 20)
        {
            TrAudio_UI.xInstance.zzPlay_TimerTicTok(0.1f);
            yield return TT.WaitForSeconds(0.5f);
        }
    }

    protected override void yBeforeReadyGame()
    {
        base.yBeforeReadyGame();
        TrAudio_Music.xInstance.zzPlayMain(0.25f);
        _totalDooRieCount = _doorie.Count();
        _isOnVibration = PlayerPrefs.GetInt(TT.strConfigVibrate, 1) == 1 ? true : false;
        _isThridChallengeSame = true;
        
    }

    protected override void yAfterReadyGame()
    {
        base.yAfterReadyGame();
        GameManager.xInstance._isGameStarted = true;
        TrUI_PuzzleNotice.xInstance._goPause = true;


    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!GameManager.xInstance._isGameStarted) return;
        /*if (_currGameTime <= _maxGameTime * 0.25f && !_isTimeTenseEffectExec)
        {
            _isTimeTenseEffectExec = true;

            TrUI_PuzzlePeePee.xInstance.zSetTimerTenseEffect();
            StartCoroutine(yExecTicTok());

        }*/

        TrUI_PuzzleDooRie.xInstance.zSetTimerTenseEffect();

        if (GameManager.xInstance._isGameStarted == true)
        {
            switch (_gameState)
            {
                case DooRieGameState.Start:
                    if (Input.GetMouseButtonUp(0))
                    {
                        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                        if (hit.collider != null && hit.transform.CompareTag("DooRie") && _slingShot._slingShotState != SlingshotState.DooRieFlying && SlingShot.xInstance._isThrowReady == true)
                        {
                            Debug.Log("¾Ó");
                            yAnimateDooRieToSlingShot(hit.collider.gameObject);
                            yAnimateCameraToStartPosition();
                        }
                    }
                    break;

                case DooRieGameState.Playing:
                    if (Input.GetMouseButtonDown(0))
                    {
                        RaycastHit2D hit2 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);
                        if (hit2.collider != null && hit2.transform.CompareTag("DooRie") && hit2.collider.gameObject != _slingShot._doorieToThrow && _slingShot._slingShotState != SlingshotState.DooRieFlying)
                        {
                            var swapPos = new Vector3(hit2.collider.gameObject.transform.position.x, hit2.collider.gameObject.transform.position.y, hit2.collider.gameObject.transform.position.z);
                            var prevDoorie = _slingShot._doorieToThrow;
                            _slingShot._doorieToThrow = null;
                            _slingShot._slingShotState = SlingshotState.Idle;

                            prevDoorie.transform.DOMove(swapPos, 1f);
                            yAnimateDooRieToSlingShot(hit2.collider.gameObject);
                            //yAnimateCameraToStartPosition();
                        }
                    }

                    if (_slingShot._slingShotState == SlingshotState.DooRieFlying && (yObjectsStoppedMoving() || Time.time - _slingShot._timeSinceThrow > 5f))
                    {
                        _slingShot.enabled = false;

                        _slingShot._slingShotLineRendererL.enabled = false;
                        _slingShot._slingShotLineRendererR.enabled = false;

                        _cameraFollow._isFollowing = false;
                        Debug.Log("…³");
                        yAnimateCameraToStartPosition();
                        _gameState = DooRieGameState.Start;
                    }
                    break;
                case DooRieGameState.Viewing:
                    if (Input.GetMouseButtonDown(0))
                    {
                        RaycastHit2D hit2 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                        if (hit2.collider != null && hit2.transform.CompareTag("DooRie"))
                        {
                            if (yFruitDestroyed())
                                _gameState = DooRieGameState.Won;

                            else if (_totalDooRieCount <= _doorieThrownCount)
                            {
                                _gameState = DooRieGameState.Lost;
                            }

                            else if (SlingShot.xInstance._doorieToThrow)
                                _gameState = DooRieGameState.Playing;

                            else if (!SlingShot.xInstance._doorieToThrow)
                                _gameState = DooRieGameState.Start;
                        }
                    }
                    break;
                case DooRieGameState.Won:
                    Debug.Log("ÀÌ±è");
                    zEndGame();
                    break;
                case DooRieGameState.Lost:
                    zEndGame();
                    Debug.Log("Áü");
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

        _gameState = DooRieGameState.Beginning;
        
        _slingShot.enabled = false;

        _slingShot._slingShotLineRendererL.enabled = false;
        _slingShot._slingShotLineRendererR.enabled = false;

        _doorie = new List<GameObject>(GameObject.FindGameObjectsWithTag("DooRie"));
        _fruit = new List<GameObject>(GameObject.FindGameObjectsWithTag("Fruit"));
        if (_fruit != null)
            _totalFruitCount = _fruit.Count;

    }
}
