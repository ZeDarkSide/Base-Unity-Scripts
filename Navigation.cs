using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Navigation : MonoBehaviour
{

    [Header("Angles")]
    public float radius;
    [Range(0,360)]
    public float angle;


    [Header("Target")]
    private NavMeshAgent agent = null;
    public Transform target;
    public Animator anim;
    public PlayerMovement Player;
    public LayerMask targetMask;
    public LayerMask ObjectMask;


    [Header("Stats")]
    public bool CanSeePlayer;
    public float timeOfLastAttack;
    public bool HasStopped;
    public float timer;
    public bool Isclose = false;



    private void Start()
    {
        
        GetReferences();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "player")
        {
            Isclose = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "player")
        {
            Isclose = false;
        }
    }

    private void Update()
    {
        if (CanSeePlayer == true)
        {
            
            MoveToTarget();
            RotateToTarget();
        }
        else
        {
            Wander();
            anim.SetBool("attack", false);
        }
        
    }

    public void MoveToTarget()
    {
        agent.SetDestination(target.position);
        anim.SetBool("walking", true);
        //RotateToTarget();
        float DisToTarget = Vector3.Distance(target.position, transform.position);
        if (DisToTarget <= agent.stoppingDistance && CanSeePlayer == true)
        {

            anim.SetBool("attack", true);
           
            if (!HasStopped )
            {
                HasStopped = true;
                timeOfLastAttack = Time.time;
            }
            if (Time.time >= timeOfLastAttack + 1.5f)
            {
                timeOfLastAttack = Time.time;
                Player.Health -= 5;
            }
            
        }
        else
        {
            
            anim.SetBool("attack", false);
            if (HasStopped)
            {
                HasStopped = false;
            }
        }

    }

    private void RotateToTarget()
    {
        transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
    }

    void GetReferences()
    {
        
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        StartCoroutine(FOVRoutine());
    }

    public IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            fieldOfViewCheck();
        }
    }

    private void fieldOfViewCheck()
    {
        Collider[] rangechecks = Physics.OverlapSphere(transform.position, radius, targetMask);
        if (rangechecks.Length != 0)
        {
            Transform target = rangechecks[0].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.position, dirToTarget)> angle / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.forward, dirToTarget, distToTarget, ObjectMask)){
                    CanSeePlayer = true;
                }
                else if (Isclose == true)
                {
                    CanSeePlayer = true;
                }
                else
                {
                    CanSeePlayer = false;
                }
            }
            else
            {
                CanSeePlayer = false;
            }
        }
        else if (CanSeePlayer && Isclose == false)
        {
            CanSeePlayer = false;
        }
    }


    public void Wander()
    {
        timer += Time.deltaTime;

        if (timer >= 3f)
        {
            Vector3 newPos = RandomNavSphere(transform.position, 40, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }


    public static Vector3 RandomNavSphere(Vector3 orgin, float dist, int layermask)
    {
        Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist;
        randDirection += orgin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}
