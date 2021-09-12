using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{

    float yRot;
    RaycastHit hit;
    GameObject grabbedOBJ;
    public Transform grabPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        yRot -= Input.GetAxis("Mouse Y");
        yRot = Mathf.Clamp(yRot, -60, 60);
        transform.localRotation = Quaternion.Euler(yRot, 0, 0);

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(transform.position, transform.forward, out hit, 5) && hit.transform.GetComponent<Rigidbody>())
        {
            grabbedOBJ = hit.transform.gameObject;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            grabbedOBJ = null;
        }
        if (grabbedOBJ)
        {
            grabbedOBJ.GetComponent<Rigidbody>().velocity = 10 * (grabPos.position - grabbedOBJ.transform.position);
        }
    }
}
