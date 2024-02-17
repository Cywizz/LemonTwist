using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Juicer5000Controller : MonoBehaviour
{
    public float RotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(RotationSpeed * new Vector3(0, 0, 1) * Time.deltaTime);
    }


    //private void OnTriggerEnter2D(Collider2D collision)
    //{        

    //    var lemonController = collision.gameObject.GetComponent<LemonGameController>();

    //    if (lemonController != null)
    //    {
    //        lemonController
            
    //    }

    //}
}
