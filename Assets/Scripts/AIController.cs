using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private NavMeshAgent _agent;

    [SerializeField]
    private Transform[] _destinations;
    private int _currentDestinationIndex = 0;

    [SerializeField] Transform player;

    [SerializeField]
    private float minTimeToChangeDestination = 5f;
    [SerializeField]
    private float maxTimeToChangeDestination = 10f;
    private float _timeToChangeDestination;
    private float _timer = 0f;

    bool isChasing;

    void Start()
    {
        isChasing = false;

        _agent = GetComponent<NavMeshAgent>();

        if (_destinations.Length > 0)
        {
            _agent.SetDestination(_destinations[_currentDestinationIndex].position);
        }

        SetRandomChangeInterval();
    }

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _timeToChangeDestination && !isChasing)
        {
            ChangeDestination();
            _timer = 0f;
            SetRandomChangeInterval();
        }

        else
        {
            ChasePlayer();
        }
    }

    void ChangeDestination()
    {
        _currentDestinationIndex = (_currentDestinationIndex + 1) % _destinations.Length;
        _agent.SetDestination(_destinations[_currentDestinationIndex].position);
    }

    void SetRandomChangeInterval()
    {
        _timeToChangeDestination = Random.Range(minTimeToChangeDestination, maxTimeToChangeDestination);
    }

    void ChasePlayer()
    {
        _agent.SetDestination(player.position);
    }
}