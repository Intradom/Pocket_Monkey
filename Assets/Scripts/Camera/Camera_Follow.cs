using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    [SerializeField] private string tag_to_follow = "";

    private GameObject ref_to_follow = null;

    private void Update()
    {
        if (ref_to_follow == null)
        {
            ref_to_follow = GameObject.FindGameObjectWithTag(tag_to_follow);
        }

        float targ_x = ref_to_follow.transform.position.x;
        float targ_y = ref_to_follow.transform.position.y;

        transform.position = new Vector3(targ_x, targ_y, transform.position.z);
    }
}
