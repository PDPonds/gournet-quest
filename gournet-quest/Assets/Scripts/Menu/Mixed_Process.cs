using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MixedType
{
    Salt, Pepper
}

[CreateAssetMenu(menuName = "Menu/Process/Mixed")]
public class Mixed_Process : Process
{
    public MixedType[] mixedTypes;
    public GameObject[] ingredients;

    public Mixed_Process()
    {
        processType = ProcessType.Mixed;
    }

}
