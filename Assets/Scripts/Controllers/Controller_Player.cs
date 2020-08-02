using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Player : MonoBehaviour
{
    // References
    [SerializeField] private LayerMask mask_ground = 0;
    [SerializeField] private Transform ref_ground_check = null;
    [SerializeField] private Rigidbody2D ref_rbody = null;

    // Parameters
    [SerializeField] private float move_speed = 0f;
    [SerializeField] private float jump_velocity = 0f;
    [SerializeField] private float windup_max = 0f;
    [SerializeField] private float windup_charge_rate_per_second = 0f;
    [SerializeField] private float windup_use_rate_per_second = 0f;

    /*******************************/

    private float ground_check_rad = 0f;
    private bool facing_right = true;
    private bool grounded = true;
    private int holdable = 0; // Can enter multiple hold zones, so check greater than zero instead of bool
    private float wind_gauge = 0f;
    private bool held = false;

    private void FlipSprite()
    {
        transform.Rotate(0f, 180f, 0f);
        facing_right = !facing_right;
    }

    private void Start()
    {
        CircleCollider2D ref_gcheck_collider = ref_ground_check.GetComponent<CircleCollider2D>();
        ground_check_rad = ref_gcheck_collider.radius;
    }

    private void Update()
    {
        // Variable updates


        // Check ground collision
        grounded = Physics2D.OverlapCircle((Vector2)ref_ground_check.position, ground_check_rad, mask_ground);
        /*
        if (!ref_animator.GetBool("grounded") && grounded)
        {
            ref_particles_ground.Play();
            Manager_Sounds.Instance.PlayLand();
        }
        ref_animator.SetBool("grounded", grounded);
        */

        /*
        // Jumping, can't handle jumping in FixedUpdate, need to catch all frames due to acting on key down
        if (input_vertical > 0 && grounded && jump_counter >= jump_lock)
        {
            ref_rbody.AddForce(new Vector2(0, jump_velocity), ForceMode2D.Force);
            //Manager_Sounds.Instance.PlayJump();
        }
        */
    }

    private void FixedUpdate()
    {
        // Horizontal
        float move_hori = move_speed * Time.fixedDeltaTime * (wind_gauge > 0 ? 1 : 0) * (facing_right ? 1 : -1);

        ref_rbody.velocity = new Vector2(move_hori, ref_rbody.velocity.y);
        //ref_animator.SetFloat("abs_speed_x", Mathf.Abs(move_hori));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hold_zone")
        {
            ++holdable;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Hold_zone")
        {
            --holdable;
            holdable = (holdable < 0) ? 0 : holdable; // Don't let holdable be less than 0
        }
    }

    private void OnMouseDown()
    {
        RaycastHit2D hit = Physics2D.Raycast(, Vector2.zero);
    }
}