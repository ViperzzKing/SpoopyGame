using UnityEngine;

public class HighlightObjects : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float playerReach = 5f;

    public bool interactable;
    public GameObject currentObject;

    private void Update()
    {
        DrawDebugRay();
        HandleObjectHighlight();
    }

    private void DrawDebugRay()
    {
        Vector3 origin = cam.transform.position;
        Vector3 lookDirection = cam.transform.forward;

        Debug.DrawRay(origin, lookDirection * playerReach, Color.red);
    }

    private void HandleObjectHighlight()
    {
        Vector3 origin = cam.transform.position;
        Vector3 lookDirection = cam.transform.forward;
        RaycastHit objectToHighlight;

        if (Physics.Raycast(origin, lookDirection, out objectToHighlight, playerReach))
        {
            GameObject hitObject = objectToHighlight.collider.gameObject;
            
            if (hitObject != currentObject)
                SwitchHighlight(hitObject);
        }
        else if (currentObject != null)
        {
            ClearHighlight();
        }
    }

    private void SwitchHighlight(GameObject newObject)
    {
        ClearHighlight();

        GameObject highlightChild = FindHighlightChild(newObject);
        if (highlightChild != null)
        {
            interactable = true;
            highlightChild.SetActive(true);
            currentObject = newObject;
        }
        else
        {
            currentObject = null;
        }
    }

    private void ClearHighlight()
    {
        if (currentObject != null)
        {
            GameObject highlightChild = FindHighlightChild(currentObject);
            if (highlightChild != null)
            {
                highlightChild.SetActive(false);
                interactable = false;
            }
            currentObject = null;
        }
    }

    private GameObject FindHighlightChild(GameObject parent)
    {
        // Search through children
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            GameObject highlight = parent.transform.GetChild(i).gameObject;
            if (highlight.CompareTag("Highlight"))
            {
                return highlight;
            }
        }
        return null;
    }
}
