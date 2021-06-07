using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    [SerializeField] private GameObject firstBg;
    [SerializeField] private GameObject secondBg;
    [SerializeField] private GameObject thirdBg;


    private void Update()
    {
        firstBg.transform.Translate(new Vector3(0,-0.05f,0));
        secondBg.transform.Translate(new Vector3(0,-0.05f,0));
        thirdBg.transform.Translate(new Vector3(0,-0.05f,0));

        if (firstBg.transform.position.y > 34 && firstBg.transform.position.y < 36)
        {
            thirdBg.transform.position = new Vector3(0, 358, 5);
        }
        
        if (secondBg.transform.position.y > 34 && secondBg.transform.position.y < 36)
        {
            firstBg.transform.position = new Vector3(0, 358, 5);
        }
        
        if (thirdBg.transform.position.y > 34 && thirdBg.transform.position.y < 36)
        {
            secondBg.transform.position = new Vector3(0, 358, 5);
        }
    }
}
