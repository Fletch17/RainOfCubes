using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    [SerializeField] private int _minLifeTime = 2;
    [SerializeField] private int _maxLifeTime = 5;

    private float _lifeTime;
    private float _currentTime = 0;
    private bool _isTouchedLayer = false;
    private Color _defaultColor;
    private Renderer _renderer;
    private Rigidbody _rigidbody;

    public event System.Action<Cube> OnReleased;

    private void Awake()
    {
        _lifeTime = Random.Range(_minLifeTime, _maxLifeTime + 1);
        _renderer = GetComponent<Renderer>();
        _defaultColor = _renderer.material.color;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _currentTime = 0;
        _isTouchedLayer = false;
        _rigidbody.velocity = Vector3.zero;
        _lifeTime = Random.Range(_minLifeTime, _maxLifeTime + 1);
        ChangeColor(_defaultColor);
    }

    private void Update()
    {
        if (_isTouchedLayer)
        {
            _currentTime += Time.deltaTime;

            if (_currentTime >= _lifeTime)
            {
                OnReleased?.Invoke(this);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isTouchedLayer == false)
        {
            if (collision.gameObject.TryGetComponent(out Platform platform))
            {
                _isTouchedLayer = true;
                ChangeColor(Random.ColorHSV());                
            }
        }
    }

    private void ChangeColor(Color color)
    {
        _renderer.material.color = color;
    }
}
