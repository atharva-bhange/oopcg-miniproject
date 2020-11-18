using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public int wallId;
    public void setTransforAndAngle(Vector3 parentPosition) {
        if (wallId == 0)
        {
            transform.position = new Vector3(parentPosition.x, parentPosition.y + 1.5f, parentPosition.z + 0.5f);
            transform.Rotate(0, 90, 0);
        }

        if (wallId == 1)
        {
            transform.position = new Vector3(parentPosition.x + 0.5f, parentPosition.y + 1.5f, parentPosition.z);
            transform.Rotate(0, 180, 0);
        }

        if (wallId == 2)
        {
            transform.position = new Vector3(parentPosition.x, parentPosition.y + 1.5f, parentPosition.z - 0.5f);
            transform.Rotate(0, 270, 0);
        }
        if (wallId == 3)
        {
            transform.position = new Vector3(parentPosition.x - 0.5f, parentPosition.y + 1.5f, parentPosition.z);
            transform.Rotate(0, 180, 0);
        }
    }
}
