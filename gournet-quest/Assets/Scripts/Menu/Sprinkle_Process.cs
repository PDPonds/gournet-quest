using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SprinkleType
{
    Salt, Pepper, GroundChili
}

[CreateAssetMenu(menuName = "Menu/Process/Sprinkle")]
public class Sprinkle_Process : Process
{
    public float sprinkle_target_speed;
    public float sprinkle_delay_per_target;
    public Vector2 sprinkle_min_max_increse_progression;
    public float sprinkle_decrease_progression;
    public float sprinkle_target_progression;

    [Range(0f, 10f)]
    public int success_Rate;

    public SprinkleType[] sprinkleTypes;
    public SprinkleType[] sprinkle_obstacle;

    public GameObject[] ingredients;

    public Sprinkle_Process()
    {
        processType = ProcessType.Sprinkle;
    }

    public float GetIncrease_Progression_Count()
    {
        return Random.Range(sprinkle_min_max_increse_progression.x, sprinkle_min_max_increse_progression.y);
    }

}
