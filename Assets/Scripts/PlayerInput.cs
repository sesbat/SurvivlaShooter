using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private string moveVName = "Vertical";
    private string moveHName = "Horizontal";
    // Start is called before the first frame update
    public float moveV { get; set; }
    public float moveH { get; set; }
    public Vector3 mousePos { get; set; }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        moveV = Input.GetAxisRaw(moveVName);
        moveH = Input.GetAxisRaw(moveHName);
        mousePos = Input.mousePosition;
    }
}
