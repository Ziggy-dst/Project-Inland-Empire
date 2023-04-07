using DG.Tweening;
using UnityEngine;

public class WASDController : MonoBehaviour
{
    public float moveAmount = 0;
    [SerializeField] private SelectOutlineObject selectOutlineObject;

    void FixedUpdate()
    {
       Move();
    }

    private void Move()
    {
        if (!selectOutlineObject.isMovable) return;
        if (Input.GetKey(KeyCode.W)) //press W to move forward
        {
            transform.DOMove(transform.position + transform.forward * moveAmount, 0.05f);
        }

        if(Input.GetKey(KeyCode.S)) //press S to move backward
        {
            transform.DOMove(transform.position - transform.forward * moveAmount, 0.05f);
        }

        if(Input.GetKey(KeyCode.A)) //press A to turn counterclockwise
        {
            transform.DORotate(Vector3.down, 0.05f).SetRelative();
        }

        if(Input.GetKey(KeyCode.D)) //press D to turn clockwise
        {
            transform.DORotate(Vector3.up, 0.05f).SetRelative();
        }
    }
}
