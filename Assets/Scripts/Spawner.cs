using System.Collections;
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
         createFunc: CreateCubeInstance,
         actionOnGet: OnActionGet,
         actionOnRelease: (cube) => cube.gameObject.SetActive(false),
         actionOnDestroy: (cube) => Destroy(cube.gameObject),
         collectionCheck: true,
         defaultCapacity: _poolCapacity,
         maxSize: _poolMaxSize
         );
    }

    private void Start()
    {
        StartCoroutine(sdsds());        
    }

    private IEnumerator sdsds()
    {
        while (true)
        {
            GetCube();
            yield return new WaitForSeconds(_repeatRate);
        }
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
        float positionX = Random.Range(transform.position.x - transform.lossyScale.x, transform.position.x + transform.lossyScale.x);
        float positionY = transform.position.y;
        float positionZ = Random.Range(transform.position.z - transform.lossyScale.z, transform.position.z + transform.lossyScale.z);

        return new Vector3(positionX, positionY, positionZ);
    }

    private void OnActionGet(Cube cube)
    {
        cube.transform.position = GetRandomPointInArea();
        cube.gameObject.SetActive(true);
    }
}
