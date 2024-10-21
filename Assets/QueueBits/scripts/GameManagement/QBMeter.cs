using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class QBMeter : MonoBehaviour
{

    public Transform arrow;
    // Start is called before the first frame update
    private float lucky, unlucky = 0;

    public void Update(bool isLucky){
        if (isLucky)
        {
            lucky++;
        }else
        {
            unlucky++;
        }
        float total = lucky+unlucky;
        float ratio = lucky/total;
        float angle = 140f*ratio - 70f;
        arrow.eulerAngles = new Vector3(0f,0f,angle);
    }

    void Start()
    {
        arrow.eulerAngles = new Vector3(0f,0f,0f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
