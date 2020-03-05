using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowJavelin : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 throwVector;
    private Vector3 javelinPosition;
    private Vector3 javelinRotation;
    private Vector3 rotateJavelin;
    private bool throwable = true;

    [SerializeField] private float throwStrength = 0;
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject character;
    [SerializeField] private GameObject javelinReference;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rotateJavelin = new Vector3(-1.323f, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        javelinRotation = camera.transform.rotation.eulerAngles;

        throwVector = camera.transform.forward;

        if (throwable)
        {
            if (Input.GetMouseButtonDown(0))
            {
                rb.useGravity = true;
                transform.parent = null;

                for (int i = 0; i < throwStrength * 100; i++)
                {
                    rb.isKinematic = false;
                    rb.AddRelativeForce(new Vector3(0, 0, 1));
                }

                StartCoroutine(ReturnJavelin());   
            }
        }
    }

    IEnumerator ReturnJavelin()
    {
        throwable = false;
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        transform.localEulerAngles = javelinRotation;
        transform.Rotate(rotateJavelin, Space.Self);

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        throwable = true;
        rb.useGravity = false;
        transform.position = javelinReference.transform.position;
        transform.parent = character.transform;
        rb.isKinematic = true;
    }
}
