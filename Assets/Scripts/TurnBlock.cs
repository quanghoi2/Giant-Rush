using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBlock : MonoBehaviour
{
    public float turnSpeed = 5;

    private void Update()
    {
        transform.Rotate(new Vector3(0, turnSpeed * Time.deltaTime, 0));
    }
}
