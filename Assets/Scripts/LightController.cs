using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LightController : MonoBehaviour
{
    [Header("Light Properties")]
    [SerializeField] GameObject lightObject;
    [SerializeField] float maxTime;
    [SerializeField] int maxUses;

    [Header("Raycast Properties")]
    [SerializeField] float viewRadius;
    [SerializeField] float viewAngle;

    [Header("Target GameObjects")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject enemy;
    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstacleMask;

    int useCounter;
    bool isActive;

    public NavMeshAgent enemyNavMeshAgent; 
    private bool enemyIsStopped = false;

    void Start()
    {
        useCounter = 0;
        isActive = false;
        lightObject.SetActive(false);
    }

    public IEnumerator LightTimer()
    {
        isActive = true;
        lightObject.SetActive(true);
        yield return new WaitForSeconds(maxTime);
        lightObject.SetActive(false);
        isActive = false;
        useCounter++;
    }

    public IEnumerator LightRaycast()
    {
        Vector3 enemyTarget = (enemy.transform.position - transform.position).normalized;

        if(Vector3.Angle(-transform.up, enemyTarget) < viewAngle/2)
        {
            float distanceToTarget = Vector3.Distance(transform.position, enemy.transform.position);
            if(distanceToTarget < viewRadius)
            {
                if(Physics.Raycast(transform.position, enemyTarget, distanceToTarget, obstacleMask) == false)
                {
                    Debug.Log("Enemy is in light radius.");

                    if (!enemyIsStopped)  
                    {
                        StartCoroutine(StopEnemyMovement());
                    }
                }
            }
        }
        yield return new WaitForSeconds(maxTime);
    }

    public void OnInteract()
    {
        if(!isActive)
        {
            if(useCounter <= maxUses)
            {
                Debug.Log("Light is turned on.");
                StartCoroutine("LightTimer");
                StartCoroutine("LightRaycast");
            }
            else
            {
                Debug.Log("Maximum uses reached.");
            }
        }

        else
        {
            Debug.Log("Light is currently active.");
        }
    }

    IEnumerator StopEnemyMovement()
    {
        enemyIsStopped = true;  

        if (enemyNavMeshAgent != null)
        {
            enemyNavMeshAgent.isStopped = true;
        }

 
        yield return new WaitForSeconds(10f);


        if (enemyNavMeshAgent != null)
        {
            enemyNavMeshAgent.isStopped = false;
        }

        enemyIsStopped = false;  
    }
}
