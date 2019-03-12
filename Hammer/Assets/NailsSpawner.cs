using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailsSpawner : MonoBehaviour
{
    [SerializeField] GameObject defaultNail;
    [SerializeField] int numberOfDefaultNails=10;
    [SerializeField,Tooltip("Distance between nails")]
    float Xoffset = 2f;

    private void Awake()
    {
        for (int i = 0; i < numberOfDefaultNails; i++)
        {
            GameObject nail = Instantiate(defaultNail, defaultNail.transform.position + new Vector3(Xoffset * i, 0f, 0f), Quaternion.identity);
            nail.transform.SetParent(this.transform);
        }
    }
}
