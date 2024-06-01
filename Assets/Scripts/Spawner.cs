using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _poolMaxSize;
    [SerializeField] private float _repeatRate;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
         createFunc: CreateCubeInstance,
         actionOnGet: OnGetting,
         actionOnRelease: (cube) => cube.gameObject.SetActive(false),
         actionOnDestroy: (cube) => Destroy(cube.gameObject),
         collectionCheck: true,
         defaultCapacity: _poolCapacity,
         maxSize: _poolMaxSize);
    }

    private void Start()
    {
        StartCoroutine(RetrieveCubeFromPool());        
    }

    private IEnumerator RetrieveCubeFromPool()
    {
        var delay = new WaitForSeconds(_repeatRate);

        while (true)
        {
            _pool.Get();
            yield return delay;
        }
    }

    private void ReleaseCube(Cube cube)
    {
        _pool.Release(cube);
    }

    private Cube CreateCubeInstance()
    {
        Cube cube = Instantiate(_prefab);
        cube.Released += ReleaseCube;
        return cube;
    }

    private Vector3 GetRandomPointInArea()
    {
        float positionX = Random.Range(transform.position.x - transform.lossyScale.x, transform.position.x + transform.lossyScale.x);
        float positionY = transform.position.y;
        float positionZ = Random.Range(transform.position.z - transform.lossyScale.z, transform.position.z + transform.lossyScale.z);

        return new Vector3(positionX, positionY, positionZ);
    }

    private void OnGetting(Cube cube)
    {
        cube.transform.position = GetRandomPointInArea();
        cube.gameObject.SetActive(true);
    }
}
