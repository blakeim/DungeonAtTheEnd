using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotionEngine : MonoBehaviour{

    [SerializeField]private float moveSpeed = 5f;
    private Animator animator;
    private Rigidbody body;
    private bool canMove;
    [SerializeField]private AudioSource footStepsSource;

    void Start(){
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
    }

    void FixedUpdate(){
        
        if(PlayerPrefs.GetInt("InputEnabled") == 1){
            float motionX = Input.GetAxis("Horizontal");     
            float motionY = Input.GetAxis("Vertical");    

            move(motionX, motionY);    
        }
    }

    void move(float motionX, float motionY){
        
        int direction = 0;

        if(motionX > 0) 
            direction = 1;
        else if(motionX < 0)
            direction = -1;
        else 
            direction = 0;

        if(motionY !=0){
            if(!footStepsSource.isPlaying){
                footStepsSource.Play();
            }
            animator.SetInteger("Direction", direction);
            animator.SetBool("Moving", true);
        }
        else{
            animator.SetBool("Moving", false);
            animator.SetInteger("Direction", direction);
        }

        transform.Translate(new Vector3(0, 0, motionY)*Time.deltaTime*moveSpeed, Space.Self);
        transform.Rotate(motionX*Vector3.up * 100 * Time.deltaTime);
    }

    public void SetCanMove(bool moveBool){
        canMove = moveBool;
    }
}
