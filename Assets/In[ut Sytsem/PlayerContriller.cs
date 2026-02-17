using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerContriller : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    private Vector2 m_Move;
    public float r_speed= 90f;
    public float jumpStrength = 5f;
    public Rigidbody rigidbody;
    private bool is_ground = false;



    // Update is called once per frame
    void Update()
    {
        Move(m_Move);
        float m_horizontal = Input.GetAxis("Mouse X");
        float m_vertical = Input.GetAxis("Mouse Y");
        transform.Rotate(Vector3.up * Time.deltaTime * r_speed * m_horizontal );
        transform.Rotate(Vector3.right * Time.deltaTime * r_speed * m_vertical );


        if ( Input.GetKey(KeyCode.Space)& is_ground == true) 
        {
            rigidbody.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            is_ground = false;
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
      m_Move = context.ReadValue<Vector2>();
    }

    private void Move(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01)
            return;
        var scaledMoveSpeed = speed * Time.deltaTime;

        var move = Quaternion.Euler(0 , transform.eulerAngles.y, 0 ) * new Vector3(direction.x,0 ,direction.y );
        transform.position += move * scaledMoveSpeed;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            is_ground = true;
        }
    }





}
