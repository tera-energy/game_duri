using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DRThrow : MonoBehaviour
{
    public DooRieState _doorieState { set; get; }

    private TrailRenderer _lineRenderer;
    [SerializeField] List<TrailRenderer> _trailRendererList = new List<TrailRenderer>();
    private Rigidbody2D _rigi;
    private CircleCollider2D _collider;

    [HideInInspector] public bool _drThrown;

    [SerializeField] LayerMask _treeMask;

    [SerializeField] AudioClip _drFlyClip;
    private AudioSource _audioSource;

    [SerializeField] ParticleSystem _parFly;
    


    void yInitializeVariables()
    {
        _lineRenderer = GetComponent<TrailRenderer>();
        _rigi = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();
        _audioSource = GetComponent<AudioSource>();

        if (_lineRenderer)
        {
            _lineRenderer.enabled = false;
            _lineRenderer.sortingLayerName = "Foreground";
        }
        if (_trailRendererList != null && _trailRendererList.Count > 0)
        {
            foreach (TrailRenderer trailRenderer in _trailRendererList)
                trailRenderer.enabled = false;
        }

        _collider.radius = GameVariavles._doorieColliderRadiusBig;

        _doorieState = DooRieState.BeforeThrown;
    }

    public void zOnThrow()
    {
        //_audioSource.Play();
        if (_drFlyClip)
        { /*TrAudio_*/}
        if (_lineRenderer)
        { /*_lineRenderer.emitting = true; */ }
        if (_trailRendererList != null && _trailRendererList.Count > 0)
        {
            foreach (TrailRenderer trailRenderer in _trailRendererList)
                trailRenderer.enabled = true;
        }
        if (_parFly)
            _parFly.gameObject.SetActive(true);

            _rigi.isKinematic = false;
            _collider.radius = GameVariavles._doorieColliderRadiusNormal;
            _doorieState = DooRieState.Thrown;
        SlingShot.xInstance._isThrowReady = false;
        }

    IEnumerator yDestroyAfterDelay(float delay)
    {
        yield return TT.WaitForSeconds(delay);
        Destroy(gameObject);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Fruit" || collision.gameObject.tag == "Ground")
        {
            if (_parFly)
                _parFly.Stop();
        }
        if (collision.gameObject.tag == "Fruit" && transform.position.y > -2f)
        {
            var treeArea = Physics2D.Raycast(transform.position, Vector2.zero, 0, _treeMask);
            if (treeArea.collider)
            {
                var treeShakeAnim = treeArea.collider.gameObject.GetComponent<TreeShakeAnim>();
                if (treeShakeAnim)
                    treeShakeAnim.zPlayTreeShakeAnim();
            }
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (_doorieState == DooRieState.Thrown && _rigi.velocity.sqrMagnitude <= GameVariavles._minVelocity)
        {
            StartCoroutine(yDestroyAfterDelay(2f));
            SlingShot.xInstance._isThrowReady = true;
        }

        if (_doorieState == DooRieState.Thrown && _rigi.velocity.sqrMagnitude <= GameVariavles._minVelocity * 10)
        {
            if (_lineRenderer)
            {
                _lineRenderer.enabled = false;
            }
            if (_trailRendererList != null && _trailRendererList.Count > 0)
            {
                foreach (TrailRenderer trailRenderer in _trailRendererList)
                    trailRenderer.enabled = false;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (_parFly && _parFly.gameObject.activeInHierarchy)
            _parFly.transform.rotation = Quaternion.Euler(0.0f, 0.0f, gameObject.transform.rotation.z + 90f);
    }
    void Awake()
    {
        yInitializeVariables();
    }
}
