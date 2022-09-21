using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _playerNavAgent;
    [SerializeField] private Transform _playerTrans;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private Transform _ground;
    [SerializeField] private Transform _spawnParent;

    [SerializeField] private GameObject _spherePrefab;
    [SerializeField] private GameObject _capsulePrefab;
    [SerializeField] private GameObject _cubePrefab;

    [SerializeField] private float _minTimeToSpawnCube;
    [SerializeField] private float _maxTimeToSpawnCube;


    private float _minX, _maxX, _minZ, _maxZ;
    private float _nextCubeSpawnTime;
    private float _timeSinceLastCubeSpawn;
    private GameObject _currentActiveSphere;
    private GameObject _currentActiveCapsule;


    private void Awake()
    {
        SetConstraints();
        _nextCubeSpawnTime = Random.Range(_minTimeToSpawnCube, _maxTimeToSpawnCube);

        InitScoreables();
    }

    private void Update()
    {
        _timeSinceLastCubeSpawn += Time.deltaTime;

        if(_timeSinceLastCubeSpawn > _nextCubeSpawnTime)
        {
            SpawnCube();
        }
    }

    public void ResetSpawner()
    {
        _timeSinceLastCubeSpawn = 0;
        _nextCubeSpawnTime = Random.Range(_minTimeToSpawnCube, _maxTimeToSpawnCube);
        ClearTheLevel();
        InitScoreables();
    }

    public void InitScoreables()
    {
        SpawnNewObject(Scoreable.ScoreableType.Sphere);
        SpawnNewObject(Scoreable.ScoreableType.Capsule);
    }

    public void SpawnNewObject(Scoreable.ScoreableType type)
    {
        GameObject go = null;
        switch (type)
        {
            case Scoreable.ScoreableType.Sphere:
                go = SpawnObject(_spherePrefab);
                if (go != null) _currentActiveSphere = go;
                break;

            case Scoreable.ScoreableType.Capsule:
                go = SpawnObject(_capsulePrefab);
                if (go != null) _currentActiveCapsule = go;
                break;

            default:
                Debug.LogError("Not specified scoreable type!");
                break;
        }
    }

    public void ClearTheLevel()
    {
        List<Transform> _allSpawned = new List<Transform>();

        for (int i = 0; i < _spawnParent.childCount; i++)
        {
            _allSpawned.Add(_spawnParent.GetChild(i));
        }

        foreach (var s in _allSpawned)
        {
            Destroy(s.gameObject);
        }
    }

    private void SetConstraints()
    {
        _minX = _ground.localScale.x / -2;
        _maxX = _ground.localScale.x / 2;
        _minZ = _ground.localScale.z / -2;
        _maxZ = _ground.localScale.z / 2;
    }

    private GameObject SpawnObject(GameObject toSpawn, int attempts = 10000)
    {
        Vector3 pos = GetFreeRandomPostion(attempts);
        if(pos != Vector3.zero)
        {
            GameObject go = Instantiate(toSpawn, pos, Quaternion.identity);
            go.transform.parent = _spawnParent;

            return go;
        }
        return null;
    }

    private Vector3 GetFreeRandomPostion(int attempts)
    {
        // we use NavMeshObstacle on each Cube to mark a that erea as taken so that we wont be able to spawn in that positon.
        Vector3 randomPos = Vector3.zero;
        for (int i = 0; i < attempts; i++)
        {
            randomPos = new Vector3(Random.Range(_minX, _maxX), 0, Random.Range(_minZ, _maxZ));
            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 1f, NavMesh.AllAreas) 
                && IsNearPlayer(hit.position) == false)
            {
                randomPos = hit.position;
                break;
            }
        }
        return randomPos;
    }

    private bool IsNearPlayer(Vector3 posToCheck)
    {
        return (_playerTrans.position - posToCheck).sqrMagnitude <= 2.2f * 2.2f; // small tolerance
    }

    private void SpawnCube()
    {
        SpawnObject(_cubePrefab, 40); // just in case the play space is filled with Cubes, we dont want stack overflow 

        _nextCubeSpawnTime = Random.Range(_minTimeToSpawnCube, _maxTimeToSpawnCube);
        _timeSinceLastCubeSpawn = 0;

        CheckIfPlayerCanReachOtherScoreables();
    }

    private void CheckIfPlayerCanReachOtherScoreables()
    {
        NavMeshPath pathToSphere = new NavMeshPath();
        _playerNavAgent.CalculatePath(_currentActiveSphere.transform.position, pathToSphere);

        NavMeshPath pathToCapsule = new NavMeshPath();
        _playerNavAgent.CalculatePath(_currentActiveCapsule.transform.position, pathToCapsule);

        if (pathToSphere.status != NavMeshPathStatus.PathComplete && pathToCapsule.status != NavMeshPathStatus.PathComplete)
        {
            _levelManager.LoseTheGame();
        }
    }
}
