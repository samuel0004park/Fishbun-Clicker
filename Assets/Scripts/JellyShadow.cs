using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyShadow : MonoBehaviour
{

    private void Start()
    {
        float yAxisOffset;
        float xScaleOffset = 1.4f;
        BeanFishMove jelly = this.GetComponentInParent<BeanFishMove>();
        switch (jelly.GetBeanFishId())
        {
            case 2:
                yAxisOffset = -1f;
                break;
            case 3:
                yAxisOffset = -1.1f;
                break;
            case 6:
                yAxisOffset = -1f;
                break;
            case 7:
                yAxisOffset = -0.75f;
                xScaleOffset = 2f;
                break;
            case 8:
                yAxisOffset = -0.8f;
                break;
            case 9:
                yAxisOffset = -0.9f;
                xScaleOffset = 2f;
                break;
            case 10:
                yAxisOffset = -1.2f;
                xScaleOffset = 1.2f;
                break;
            case 11:
                yAxisOffset = -1.2f;
                xScaleOffset = 1.3f;
                break;
            default:
                yAxisOffset = -0.8f;
                break;
        }
        Vector3 newPos = new Vector3(0, yAxisOffset, 0);
        transform.localPosition = newPos;
        transform.localScale = new Vector3(xScaleOffset, 1, 1);
    }   
}
