using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : Character
{

    protected override void Update()
    {
        base.Update();
        Move();
    }

    float GetVelocity(float x)
    {
        //if (x == 0.0f) return 0f;
        //return x > 0.0f ? 1f : -1f;
        return x;
    }

    private void Move()
    {
        rigidbody.velocity = new Vector3(GetVelocity(joystick.Horizontal) * speed + GetVelocity(Input.GetAxis("Horizontal")) * speed,
                                         rigidbody.velocity.y,
                                         GetVelocity(joystick.Vertical) * speed + GetVelocity(Input.GetAxis("Vertical")) * speed);

        Vector3 direct1 = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        Vector3 direct2 = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (Vector3.Distance(direct1, Vector3.zero) > 0.1f) 
            transform.rotation = Quaternion.LookRotation(direct1);
        if (Vector3.Distance(direct2, Vector3.zero) > 0.1f)
            transform.rotation = Quaternion.LookRotation(direct2);
    }

}
