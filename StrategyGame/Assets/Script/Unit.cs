using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Handling moving, animations
public class Unit : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;
    private Vector3 targetPosition;

    //Handling that the unit is standing where he is standing not 0,0,0. If delete this units will run to 0,0,0
    private void Awake() 
    {
        targetPosition = transform.position;
    }

    private void Update()
    {        
        float stoppingDistance = 0.1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            //Movement
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

        //Animations
        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed); //rotate when moving
            unitAnimator.SetBool("IsWalking", true);
        } else
        {
            unitAnimator.SetBool("IsWalking", false);
        }


    }

    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
