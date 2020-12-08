using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMover : MonoBehaviour
{
    private NavMeshAgent agent = null;

    private Animator playerAnimator = null;

    static readonly int SPEED_PRCT = Animator.StringToHash("SpeedPercent");

    [SerializeField] private float rotateSpeed = 5.0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerAnimator = GetComponent<Animator>();
    }

    private bool orderedToMove = false;
    public void GiveMoveOrder() => orderedToMove = true;

    void Update()
    {
        //----------------------------Look at Mouse------------------------------------
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f)) 
        {
            Vector3 mousePos = hit.point;

            Vector3 dir = mousePos - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f); 
        }
        //-----------------------------------------------------------------------------
        if (hit.collider && orderedToMove)
        {
            orderedToMove = false;

            agent.SetDestination(hit.point);
            agent.isStopped = false;
        }

        float spdPrct = agent.velocity.magnitude / agent.speed;
        playerAnimator.SetFloat(SPEED_PRCT, spdPrct);
        
        if (spdPrct < .2f)
            playerAnimator.speed = spdPrct;
        else 
            playerAnimator.speed = 1.0f;
    }
}
