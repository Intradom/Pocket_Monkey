using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Player : MonoBehaviour
{
    // References
    [SerializeField] private LayerMask mask_ground = 0;
    [SerializeField] private Transform ref_ground_check = null;
    [SerializeField] private Rigidbody2D ref_self_rbody = null;
    [SerializeField] private Transform ref_self_transform = null;
    [SerializeField] private Animator ref_animator_crank = null;
    [SerializeField] private Animator ref_animator_monkey = null;

    [SerializeField] private string tag_manager = "";

    // Parameters
    [SerializeField] private float move_speed = 0f;
    [SerializeField] private float jump_velocity = 0f;
    [SerializeField] private float windup_max = 0f;
    [SerializeField] private float windup_charge_rate_per_second = 0f;
    [SerializeField] private float windup_use_rate_per_second = 0f;
    [SerializeField] private float side_collider_angle_thresh = 0f;

    /*******************************/

    private Vector2 hold_offset;
    private float ground_check_rad = 0f;
    private bool facing_right = true;
    private bool grounded = true;
    private int holdable = 0; // Can enter multiple hold zones, so check greater than zero instead of bool
    private float wind_gauge = 0f;
    private bool held = false;

    private Behavior_Manager script_manager = null;


    public float GetWindGaugePercentage() { return wind_gauge / windup_max; }

    private void Awake()
    {
        script_manager = GameObject.FindGameObjectWithTag(tag_manager).GetComponent<Behavior_Manager>();
    }

    private void Start()
    {
        CircleCollider2D ref_gcheck_collider = ref_ground_check.GetComponent<CircleCollider2D>();
        ground_check_rad = ref_gcheck_collider.radius;

        hold_offset = Vector2.zero;
    }

    private void Update()
    {
        // Variable updates
        ref_animator_crank.SetFloat("cranking", wind_gauge);
        ref_animator_monkey.SetFloat("cranking", wind_gauge);
        ref_animator_monkey.SetFloat("abs_speed", Mathf.Abs(ref_self_rbody.velocity.x));

        // Held, put position to cursor's position
        if (held)
        {
            Behavior_Sounds.Instance.PlayWind();

            // Charge up
            wind_gauge += windup_charge_rate_per_second * Time.deltaTime;
            wind_gauge = (wind_gauge > windup_max) ? windup_max : wind_gauge;

            Vector3 mouse_pos_world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse_pos_world.x += hold_offset.x;
            mouse_pos_world.y += hold_offset.y;
            mouse_pos_world.z = ref_self_transform.position.z;
            ref_self_transform.position = mouse_pos_world;
            ref_self_rbody.velocity = Vector2.zero;
        }
        else
        {
            wind_gauge -= windup_use_rate_per_second * Time.deltaTime;
            wind_gauge = (wind_gauge < 0) ? 0 : wind_gauge;

            if (wind_gauge > 0)
            {
                Behavior_Sounds.Instance.PlayRelease();
            }
            else
            {
                Behavior_Sounds.Instance.StopSFX();
            }
        }

        // Check ground collision
        grounded = Physics2D.OverlapCircle((Vector2)ref_ground_check.position, ground_check_rad, mask_ground);

        /*
        if (!ref_animator.GetBool("grounded") && grounded)
        {
            ref_particles_ground.Play();
            Manager_Sounds.Instance.PlayLand();
        }
        */

        if (Input.GetMouseButtonDown(0) && holdable > 0)
        {
            Vector3 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 1 << LayerMask.NameToLayer("Player"));

            if (hit.collider && wind_gauge == 0)
            {
                held = true;
                hold_offset.x = ref_self_transform.position.x - mouse_pos.x;
                hold_offset.y = ref_self_transform.position.y - mouse_pos.y;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            held = false;
        }
    }

    private void FixedUpdate()
    {
        if (!held && grounded)
        {
            // Horizontal
            float move_hori = move_speed * Time.fixedDeltaTime * (wind_gauge > 0 ? 1 : 0) * (facing_right ? 1 : -1);

            ref_self_rbody.velocity = new Vector2(move_hori, ref_self_rbody.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bounds")
        {
            script_manager.RestartLevel();
        }
        else if (collision.tag == "Hold_zone")
        {
            ++holdable;
        }
        else if (collision.tag == "Hold_zone_out")
        {
            if (held)
            {
                held = false;
                ref_self_transform.position = collision.transform.position;
            }
        }
        else if (collision.tag == "Power_jump")
        {
            ref_self_rbody.AddForce(new Vector2(0, jump_velocity), ForceMode2D.Force);
            Behavior_Sounds.Instance.PlayJump();
            Destroy(collision.gameObject);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Platform")
        {
            float angle = Vector2.SignedAngle(Vector2.right, collision.GetContact(0).normal);
                        
            // Only change direction if this was a sideways collision
            if (Mathf.Abs(angle) < side_collider_angle_thresh || (180 - Mathf.Abs(angle)) < side_collider_angle_thresh)
            {
                FlipSprite();
            }
        }
    }

    private void FlipSprite()
    {
        transform.Rotate(0f, 180f, 0f);
        facing_right = !facing_right;
    }
}