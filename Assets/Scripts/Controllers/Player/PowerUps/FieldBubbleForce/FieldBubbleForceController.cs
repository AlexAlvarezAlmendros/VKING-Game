using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldBubbleForceController : MonoBehaviour
{
    [Header("Player")]
    private GameObject player;

    [Header("Visuals")]
    public float speedRotation;
    public float divUp;
    public float divForward;
    public float divRight;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = player.transform.position;
        transform.Rotate(speedRotation / divUp * Vector3.up * Time.deltaTime);
        transform.Rotate(speedRotation / divForward * Vector3.forward * Time.deltaTime);
        transform.Rotate(speedRotation / divRight * Vector3.right * Time.deltaTime);
    }
}
