using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FlashlightCrank : MonoBehaviour
{
    [Header("Raycast Properties")]
    [SerializeField] float viewRadius;
    [SerializeField] float viewAngle;

    [Header("Target GameObjects")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject enemy;
    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstacleMask;

    public GameObject flashlight;
    public float crankDuration = 0.25f;
    private bool isCranking = false;
    private float crankHoldTime = 0f;

    public NavMeshAgent enemyNavMeshAgent;
    private bool enemyIsStopped = false;

    [SerializeField] float stunTimer;

    private void Start()
    {
        flashlight.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            crankHoldTime += Time.deltaTime;

            if (crankHoldTime >= crankDuration && !isCranking)
            {
                StartCoroutine(CrankFlashlight());
                LightRaycast();
            }
        }
        else
        {
            flashlight.SetActive(false);
            crankHoldTime = 0f;
        }
    }

    public void LightRaycast()
    {
        Vector3 enemyTarget = (enemy.transform.position - transform.position).normalized;

        if (Vector3.Angle(transform.forward, enemyTarget) < viewAngle / 2)
        {
            float distanceToTarget = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToTarget < viewRadius)
            {
                if (Physics.Raycast(transform.position, enemyTarget, distanceToTarget, obstacleMask) == false)
                {

                    Debug.Log("Enemy is in light radius.");

                    if (!enemyIsStopped)
                    {
                        StartCoroutine(StopEnemyMovement());
                    }
                }
            }
        }

    }

    IEnumerator CrankFlashlight()
    {
        isCranking = true;
        flashlight.SetActive(true);

        //Debug.Log("Flashlight cranked");

        yield return new WaitForSeconds(crankDuration);

        isCranking = false;
        crankHoldTime = 0f;

    }

    IEnumerator StopEnemyMovement()
    {
        enemyIsStopped = true;

        if (enemyNavMeshAgent != null)
        {
            enemyNavMeshAgent.isStopped = true;
        }


        yield return new WaitForSeconds(stunTimer);


        if (enemyNavMeshAgent != null)
        {
            enemyNavMeshAgent.isStopped = false;
        }

        enemyIsStopped = false;
    }
}
