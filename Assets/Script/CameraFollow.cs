using Mono.Cecil;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] Transform target;
    [SerializeField] float smoothSpeed;

    Vector3 offset;

    void Start()
    {
        offset = transform.position;
    }


    void Update()
    {
        if(target == null)
        {
            Debug.Log("No target");
        }

        else
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smootedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smootedPosition;
        }
    }
}
