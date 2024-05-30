using UnityEngine;

[RequireComponent(typeof(Renderer),typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    [SerializeField] private int _minLifeTime = 2;
    [SerializeField] private int _maxLifeTime = 5;
    [SerializeField] private string _groundTag = "Ground";

    private float _lifeTime;
    private float _currentTime = 0;
    private bool _isTouchedLayer = false;
    private bool _isColorChanged = false;
    private Color _defaultColor;

    public event System.Action<Cube> OnReleased;

    private void Awake()
    {
        _lifeTime = Random.Range(_minLifeTime, _maxLifeTime + 1);
        _defaultColor = GetComponent<Renderer>().material.color;
    }

    private void OnEnable()
    {
        _currentTime = 0;
        _isTouchedLayer = false;
        _isColorChanged = false;
        ChangeColor(_defaultColor);
        _lifeTime = Random.Range(_minLifeTime, _maxLifeTime + 1);
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
        if (collision.gameObject.CompareTag(_groundTag))
        {
            _isTouchedLayer = true;

            if (_isColorChanged == false)
            {
                ChangeColor(Random.ColorHSV());
                GetComponent<Renderer>().material.color = Random.ColorHSV();
                _isColorChanged = true;
            }
        }
    }

    private void ChangeColor(Color color)
    {
        GetComponent<Renderer>().material.color = color;
    }
}
