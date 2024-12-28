using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBackground : MonoBehaviour
{
    [Header("Scroll Settings")]
    [SerializeField] private float scrollSpeed = 2f;
    [SerializeField] private float resetPosition = -50f;
    [SerializeField] private float startPosition = 50f;
    
    void Update()
    {
        transform.Translate(Vector3.forward * -scrollSpeed * Time.deltaTime);
        
        if (transform.position.z <= resetPosition)
        {
            Vector3 newPosition = transform.position;
            newPosition.z = startPosition;
            transform.position = newPosition;
        }
    }
}