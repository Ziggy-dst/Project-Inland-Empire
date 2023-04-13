using Sirenix.Utilities;
using UnityEngine.EventSystems;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ModifiedDraggableTopBar : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [Tooltip("Whether or not the window can be dragged outside the screen borders")]
    public bool KeepWithinScreen = false;

    private Vector3 mouseOffset;
    private RectTransform parentRect;

    private Vector3[] parentRectCorners = new Vector3[4];

    [SerializeField] private Camera desktopCamera;
    // [SerializeField] private Camera desktopCamera;

    private void Awake()
    {
        parentRect = transform.parent.GetComponent<RectTransform>();
        if (parentRect == null)
        {
            Debug.LogException(new MissingComponentException("Draggable topbar must have a RectTransform parent"), this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // print("parent rect: " + parentRect.rect.position);
        // print("parent pos: " + parentRect.position);
        parentRect = transform.parent.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToWorldPointInRectangle(parentRect, Input.mousePosition, desktopCamera, out Vector3 worldClick);
        // print("mousePosition: " + worldClick);
        // the offset between the mouse rect and the screen rect
        mouseOffset = parentRect.position - worldClick;
        // print("offset: " + mouseOffset);
    }

    public void OnDrag(PointerEventData eventData)
    {
        parentRect = transform.parent.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToWorldPointInRectangle(parentRect, Input.mousePosition, desktopCamera, out Vector3 worldClick);
        parentRect.position = worldClick + mouseOffset;
        // print("after parent: " + parentRect.position);
        if (KeepWithinScreen)
        {
            KeepParentRectInScreen();
        }
    }

    private void KeepParentRectInScreen()
    {
        parentRect.GetWorldCorners(parentRectCorners);
        Vector3 bottomLeftCorner = parentRectCorners[0];
        Vector3 topRightCorner = parentRectCorners[2];

        print("bottom left" + bottomLeftCorner);
        print("top right" + bottomLeftCorner);

        // keep track of how much to move the rect to keep it in the screen
        float xPush = 0f;
        float yPush = 0f;

        // calculate amount to move the rect based on position of rect corners and screen size
        if (bottomLeftCorner.x < 0)
        {
            xPush -= parentRectCorners[0].x;
        }
        if (bottomLeftCorner.y < 0)
        {
            yPush -= parentRectCorners[0].y;
        }

        if (topRightCorner.x > Screen.width)
        {
            xPush += (1200f - topRightCorner.x);
        }
        if (topRightCorner.y > Screen.height)
        {
            yPush += (675f - topRightCorner.y);
        }

        print("xPush " + xPush);
        print("yPush " + yPush);

        // reposition the rect
        if (Mathf.Abs(xPush) > 0f || Mathf.Abs(yPush) > 0f)
        {
            parentRect.position = new Vector3(
                parentRect.position.x + xPush,
                parentRect.position.y + yPush,
                parentRect.position.z
            );
            RectTransformUtility.ScreenPointToWorldPointInRectangle(parentRect, Input.mousePosition, desktopCamera, out Vector3 worldClick);
            print("mousePosition: " + worldClick);
            // reset mouse offset position so dragging is anchored to the new mouse position
            mouseOffset = parentRect.position - worldClick;
        }
    }
}
