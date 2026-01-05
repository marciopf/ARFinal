using UnityEngine;
using UnityEngine.InputSystem; // 1. REQUIRED: Add this namespace

public class Touch : MonoBehaviour
{
    public GameObject gradilPopup;

    void Update()
    {
        // Safety check: ensure a mouse is connected
        if (Mouse.current == null) return;

        // 2. Replace Input.GetMouseButtonDown(0)
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Pressed left button");

            // 3. Replace Input.mousePosition
            Vector2 mousePos = Mouse.current.position.ReadValue();

            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2f);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Debug.Log("hit: " + hit.transform.name);

                if (hit.transform.CompareTag("gradil")) // slightly faster than tag == "string"
                {
                    Vector3 pos = hit.point;
                    pos.z += 0.25f;
                    pos.y += 0.25f;
                    Instantiate(gradilPopup, pos, transform.rotation);
                }
                else if (hit.transform.CompareTag("gradilinfo"))
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }
}