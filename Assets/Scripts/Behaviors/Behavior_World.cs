using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_World : MonoBehaviour
{
    [SerializeField] private string level_name = "";

    [SerializeField] private string tag_manager = "";

    public string GetLevelName() { return level_name; }

    private void Awake()
    {
        GameObject.FindGameObjectWithTag(tag_manager).GetComponent<Behavior_Manager>().WorldInit();
    }
}
