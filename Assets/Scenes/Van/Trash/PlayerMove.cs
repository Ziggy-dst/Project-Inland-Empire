using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // If true, diagonal speed (when strafing + moving forward or back) can't exceed normal move speed; otherwise it's about 1.4 times faster
    private bool limitDiagonalSpeed = true;

    // Small amounts of this results in bumping when walking down slopes, but large amounts results in falling too fast
    public float antiBumpFactor = .75f;

    // private bool speedUp = false;

    public GameObject speedlineparticle;

    private Vector3 moveDirection = Vector3.zero;

    public float gravity = 10.0f;

    private CharacterController controller;
    private Transform myTransform;
    public float speed;

    public float walkSpeed = 6.0f;

    private bool grounded = false;

    public Transform targetTrans;




    private void Start()
    {
        controller = GetComponent<CharacterController>();
        myTransform = transform;
        speed = walkSpeed;
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        // If both horizontal and vertical are used simultaneously, limit speed (if allowed), so the total doesn't exceed normal move speed
        float inputModifyFactor = (inputX != 0.0f && inputY != 0.0f && limitDiagonalSpeed) ? .7071f : 1.0f;


        Vector3 tempDir = new Vector3(inputX * inputModifyFactor, -antiBumpFactor, inputY * inputModifyFactor);

/*        if (speedUp && (inputX != 0.0f || inputY != 0.0f))
        {
            speedlineparticle.SetActive(true);
        }
        else
        {
            speedlineparticle.SetActive(false);
        }*/

        moveDirection = new Vector3(inputX * inputModifyFactor, -antiBumpFactor, inputY * inputModifyFactor);

        moveDirection.y = 0;

        moveDirection.y -= gravity * Time.deltaTime;

        grounded = (controller.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;


        targetTrans.position = transform.position;
    }
}
