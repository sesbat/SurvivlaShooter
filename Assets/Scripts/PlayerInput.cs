using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private string moveVName = "Vertical";
    private string moveHName = "Horizontal";
    private string FireName = "Fire1";
    // Start is called before the first frame update
    public float moveV { get; set; }
    public float moveH { get; set; }
    public bool fire { get; set; }
    public Vector3 mousePos { get; set; }

    // Update is called once per frame
    private void Update()
    {
        moveV = Input.GetAxisRaw(moveVName);
        moveH = Input.GetAxisRaw(moveHName);
        fire = Input.GetButton(FireName);
        mousePos = Input.mousePosition;
    }
}
