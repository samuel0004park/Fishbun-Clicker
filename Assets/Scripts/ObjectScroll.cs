using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScroll : MonoBehaviour
{
    public float speed = 0.1f;



    void Update()
    {
        Scroll();
    }

    private void Scroll()
    {
        Vector3 offset = new Vector3(speed, 0, 0);
        transform.Translate(offset * Time.deltaTime);
    }
  

}
