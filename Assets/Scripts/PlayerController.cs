using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    //write a stringtohash for the animator
    private int isWalkingHash;
    private int isRunningHash;
    // Start is called before the first frame update
    bool move;
    bool running;
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    void Start()
    {
        //define the iswalking hash in the start function
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
    }

    // Update is called once per frame
    private void Update()
    {
        Movement();
        OnAnimatorMove();
        RotateWithView();
    }


    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical);
        direction = Camera.main.transform.TransformDirection(direction);
        characterController.Move(direction * 3 * Time.deltaTime);
        //player be effected by gravity
        characterController.Move(Physics.gravity * Time.deltaTime);
        move = horizontal != 0 || vertical != 0;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            running = true;
            characterController.Move(direction * 6 * Time.deltaTime);
        }
        else
        {
            running = false;
        }
    }

    //animate the player with animator if the player is moving and not running
    private void OnAnimatorMove()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetBool(isWalkingHash, move);
        animator.SetBool(isRunningHash, running && move);
    }
    //rotate the player use quaternion to rotate the player
    private void RotateWithView()
    {
        //wont rotate when the player is not moving
        if (move)
        {
            Vector3 direction = Camera.main.transform.forward;
            direction.y = 0;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 5 * Time.deltaTime);
        }
    }
}
