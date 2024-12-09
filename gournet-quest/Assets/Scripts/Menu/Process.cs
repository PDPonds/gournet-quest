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
    public ProcessType processType;
    public int process_Time_Sec;

}

