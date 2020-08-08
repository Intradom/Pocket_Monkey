using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Behavior_Goal : MonoBehaviour
{
    [SerializeField] private Object ref_next_level = null;

    [SerializeField] private string tag_interactable = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == tag_interactable)
        {
            if (ref_next_level == null)
            {
                int next_build_index = 0;

                next_build_index = SceneManager.GetActiveScene().buildIndex + 1;

                // Check for valid 
                if (next_build_index < SceneManager.sceneCountInBuildSettings)
                {
                    Behavior_Sounds.Instance.StopSFX();
                    SceneManager.LoadScene(next_build_index);
                }
            }
            else
            {
                Behavior_Sounds.Instance.StopSFX();
                SceneManager.LoadScene(ref_next_level.name);
            }   
        }
    }
}
