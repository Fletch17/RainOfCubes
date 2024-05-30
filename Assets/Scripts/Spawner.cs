using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _poolMaxSize;
    [SerializeField] float _repeatRate;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
         createFunc: () => CreateCubeInstance(),
         actionOnGet: (cube) => OnActionGet(cube),
         actionOnRelease: (cube) => cube.gameObject.SetActive(false),
         actionOnDestroy: (cube) => Destroy(cube.gameObject),
         collectionCheck: true,
         defaultCapacity: _poolCapacity,
         maxSize: _poolMaxSize
         );
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0.0f, _repeatRate);
    }

    private void GetCube()
    {
        _pool.Get();
    }

    private void ReleaseCube(Cube cube)
    {
        _pool.Release(cube);
    }

    private Cube CreateCubeInstance()
    {
        Cube cube = Instantiate(_prefab);
        cube.OnReleased += ReleaseCube;
        return cube;
    }

    private Vector3 GetRandomPointInArea()
    {
        float x = Random.Range(transform.position.x - transform.lossyScale.x, transform.position.x + transform.lossyScale.x);
        float y = transform.position.y;
        float z = Random.Range(transform.position.z - transform.lossyScale.z, transform.position.z + transform.lossyScale.z);

        return new Vector3(x, y, z);
    }

    private void OnActionGet(Cube cube)
    {
        cube.transform.position = GetRandomPointInArea();
        cube.GetComponent<Rigidbody>().velocity = Vector3.zero;
        cube.gameObject.SetActive(true);
    }
}
