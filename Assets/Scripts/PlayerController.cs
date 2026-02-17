using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private int speed = 10;
    
    public float vertical;
    public float horizontal;
   

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        

        transform.Translate(Vector3.forward * Time.deltaTime * speed * vertical);
        transform.Translate(Vector3.right * Time.deltaTime * speed * horizontal);
        
    }
}
