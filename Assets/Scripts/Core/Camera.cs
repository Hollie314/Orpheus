using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target; // theplayer
    public Vector3 offset = new Vector3(0f, 5f, -10f); //offset from the character
    public float followSpeed = 5f; //it should

    void LateUpdate()
    {
        Vector3 desiredPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPos, followSpeed * Time.deltaTime);
    }

}
