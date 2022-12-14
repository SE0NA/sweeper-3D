using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapManager : MonoBehaviour
{
    public void SetMiniMap(Transform pos)
    {
        gameObject.transform.position = new Vector3(pos.transform.position.x
            , gameObject.transform.position.y, pos.transform.position.z);
    }
}
