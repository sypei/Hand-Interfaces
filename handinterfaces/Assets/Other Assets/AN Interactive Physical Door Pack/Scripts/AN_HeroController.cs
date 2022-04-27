using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AN_HeroController : MonoBehaviour
{
    [Tooltip("Character settings (rigid body)")]
    public float MoveSpeed = 30f, JumpForce = 200f, Sensitivity = 70f;
    bool jumpFlag = true; // to jump from surface only

    CharacterController character;
    Rigidbody rb;
    Vector3 moveVector;

    Transform Cam;
    float yRotation;

    void Start()
    {
        character = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        Cam = Camera.main.GetComponent<Transform>();

        Cursor.lockState = CursorLockMode.Locked; // freeze cursor on screen centre
        Cursor.visible = false; // invisible cursor
    }

    void Update()
    {
        // camera rotation
        float xmouse = Input.GetAxis("Mouse X") * Time.deltaTime * Sensitivity;
        float ymouse = Input.GetAxis("Mouse Y") * Time.deltaTime * Sensitivity;
        transform.Rotate(Vector3.up * xmouse);
        yRotation -= ymouse;
        yRotation = Mathf.Clamp(yRotation, -85f, 60f);
        Cam.localRotation = Quaternion.Euler(yRotation, 0, 0);

        if (Input.GetButtonDown("Jump") && jumpFlag == true) rb.AddForce(transform.up * JumpForce);
    }

    void FixedUpdate()
    {
        // body moving
        moveVector = transform.forward * MoveSpeed * Input.GetAxis("Vertical") +
            transform.right * MoveSpeed * Input.GetAxis("Horizontal") +
            transform.up * rb.velocity.y;
        rb.velocity = moveVector;

        
    }
    
    private void OnTriggerStay(Collider other)
    {
        jumpFlag = true; // hero can jump
    }

    private void OnTriggerExit(Collider other)
    {
        jumpFlag = false;
    }

}
