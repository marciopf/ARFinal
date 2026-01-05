using UnityEngine;
using UnityEngine.InputSystem; // 1. REQUIRED: Add this namespace

public class Touch : MonoBehaviour
{
    public GameObject gradilPopup;

    void Update()
    {
        bool isPressed = false;
        Vector2 screenPosition = Vector2.zero;

        // 1. Verifica TOQUE NA TELA (Celular)
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            isPressed = true;
            screenPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        }
        // 2. Se não tocou, verifica MOUSE (Computador/Editor)
        else if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            isPressed = true;
            screenPosition = Mouse.current.position.ReadValue();
        }

        // Se houve clique ou toque, faz o Raycast
        if (isPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100)) // Lembre-se: Distância 100 pode ser pouco dependendo da cena
            {
                if (hit.transform.CompareTag("gradil"))
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