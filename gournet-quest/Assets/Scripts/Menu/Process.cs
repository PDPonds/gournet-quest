using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProcessType
{
    Mixed, Grill
}

public class Process : ScriptableObject
{
    public string process_Name;
    public string process_Description;
    public ProcessType processType;
}

