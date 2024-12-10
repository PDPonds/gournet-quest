using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProcessType
{
    Sprinkle, Grill, Fried, Deep_Fried
}

public class Process : ScriptableObject
{
    public string process_Description;
    public int process_Time_Sec;

    [Header("===== Fail =====")]
    public float decrease_completness_if_fail_action;
    public float decrease_conpletness_if_time_out;

    [Header("===== Process Type =====")]
    public ProcessType processType;

}

