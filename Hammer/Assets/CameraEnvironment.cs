using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEnvironment : MonoBehaviour
{
    [SerializeField] private float offset;
    
    Hammer hammer;
    private float newY;
    private Vector3 startingPos;
    
    void Start()
    {
        hammer = FindObjectOfType<Hammer>();
        startingPos = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        //newY = hammer.transform.position.y;
        newY = Camera.main.transform.position.y;
        transform.position = new Vector3(startingPos.x,startingPos.y+newY+offset,startingPos.z);
    }
}
