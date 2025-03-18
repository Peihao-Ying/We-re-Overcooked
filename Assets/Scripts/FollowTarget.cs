using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Player.Instance.transform.position;
    }
}
