using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Menu/Process/Grill")]
public class Grill_Process : Process
{
    public GameObject[] ingredients;

    [Range(0f, 1f)] public float grill_length;
    [Range(0f, 1f)] public float grill_increase_position_per_click;
    [Range(0f, 1f)] public float grill_decrease_per_time;
    [Range(0f, 1f)] public float grill_increase_progression_per_time;

    public Grill_Process()
    {
        processType = ProcessType.Grill;
    }

}
