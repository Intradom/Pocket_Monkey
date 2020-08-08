using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Ending : MonoBehaviour
{
    [SerializeField] private string tag_manager = "";

    private void Awake()
    {
        GameObject go_manager = GameObject.FindGameObjectWithTag(tag_manager);
        Behavior_Sounds.Instance.StopSFX();

        Destroy(go_manager);
    }
}
