using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Behavior_Manager : MonoBehaviour
{
    public static Behavior_Manager Instance = null;

    [SerializeField] private Text ref_text_level_name = null;
    [SerializeField] private Slider ref_slider_gauge = null;

    private Behavior_World script_world = null;
    private Controller_Player script_monkey = null;

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void WorldInit()
    {
        script_world = GameObject.FindGameObjectWithTag("World").GetComponent<Behavior_World>();
        script_monkey = GameObject.FindGameObjectWithTag("Monkey").GetComponent<Controller_Player>();

        ref_text_level_name.text = script_world.GetLevelName();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Restart"))
        {
            RestartLevel();
        }

        ref_slider_gauge.value = script_monkey.GetWindGaugePercentage();
    }
}
