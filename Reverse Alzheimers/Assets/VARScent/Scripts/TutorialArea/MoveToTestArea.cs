using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTestArea : MonoBehaviour
{
    public Transform TestPos;

    public void MoveToTest()
    {
        gameObject.transform.position = TestPos.position;
    }
}
