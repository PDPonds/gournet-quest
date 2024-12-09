using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprinkle_MiniGame_Target : MonoBehaviour
{
    public SprinkleType type;
    float speed;

    public void Setup(SprinkleType type, float speed)
    {
        this.type = type;
        this.speed = speed;
    }

    private void Update()
    {
        lookatCamera();

        if (CookingManager.Instance.isPhase(CookingPhase.Action))
            transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void lookatCamera()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

}
