using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public AudioSource footsteps;
    public AudioClip screamSFX;

    public float speed = 5;
    private CharacterController controller;
    private Animator anim;

    private bool isMoving;

    private float horizontalMovement;
    private float verticalMovement;
    private Vector3 direction;

    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private bool canMove = true;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {
            MovementCheck();
            AnimationCheck();
        }
    }

    private void MovementCheck()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal"); //Oluþturduðumuz sanal eksenin deðerlerini döndürür.
        verticalMovement = Input.GetAxisRaw("Vertical");

        direction = new Vector3(horizontalMovement, 0, verticalMovement);

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg; // Dönüþlerin keskin olmamasý için
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            controller.Move(direction * speed * Time.deltaTime);
        }
    }

    private void AnimationCheck()
    {
        if(direction != Vector3.zero && !isMoving)
        {
            footsteps.Play();
            isMoving = true;
            anim.SetBool("isRunning", isMoving);
        }
        else if(direction == Vector3.zero && isMoving)
        {
            footsteps.Stop();
            isMoving = false;
            anim.SetBool("isRunning", isMoving);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Zombie"))
        {
            canMove = false;
            UIManager.instance.ShowGameOver(false);
            AudioManager.instance.PlaySFX(screamSFX);
            anim.SetTrigger("isDead");
            footsteps.Stop();
        }
    }
}
