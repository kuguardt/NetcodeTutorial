using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork : MonoBehaviour
{

    private void Update()
    {
        Vector3 moveDIr = new Vector3(0,0,0);

        if (Input.GetKey(KeyCode.W)) moveDIr.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDIr.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDIr.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDIr.x = +1f;

        float moveSpeed = 3f;
        transform.position += moveDIr * moveSpeed * Time.deltaTime;
    }
}
