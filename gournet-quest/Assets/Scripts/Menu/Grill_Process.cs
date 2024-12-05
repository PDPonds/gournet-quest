using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Menu/Process/Grill")]
public class Grill_Process : Process
{
    public GameObject[] ingredients;

    public Grill_Process()
    {
        processType = ProcessType.Grill;
    }

}
