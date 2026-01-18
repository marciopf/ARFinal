using UnityEngine;
using UnityEngine.InputSystem;

public class Touch : MonoBehaviour
{
    public GameObject gradilPopup;

    // 1. Variável privada para guardar a referência do popup que está na tela
    private GameObject popupAtual;

    void Update()
    {
        bool isPressed = false;
        Vector2 screenPosition = Vector2.zero;

        // Verifica Input (Toque ou Mouse)
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            isPressed = true;
            screenPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        }
        else if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            isPressed = true;
            screenPosition = Mouse.current.position.ReadValue();
        }

        if (isPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.CompareTag("gradil"))
                {
                    // 2. Lógica para limitar a instância:
                    // Se já existe um popup aberto, destrua-o antes de criar o novo.
                    if (popupAtual != null)
                    {
                        Destroy(popupAtual);
                    }

                    Vector3 pos = hit.point;
                    pos.z += 0.25f;
                    pos.y += 0.25f;

                    // 3. Ao instanciar, guardamos o objeto criado na variável 'popupAtual'
                    popupAtual = Instantiate(gradilPopup, pos, transform.rotation);
                }
                else if (hit.transform.CompareTag("gradilinfo"))
                {
                    // Se clicar na própria info para fechar
                    Destroy(hit.transform.gameObject);

                    // Boa prática: limpar a variável de referência
                    popupAtual = null;
                }
            }
            // Opcional: Se quiser fechar o popup ao clicar no "vazio" (fora de qualquer objeto)
            /*
            else if (popupAtual != null) 
            {
                 Destroy(popupAtual);
                 popupAtual = null;
            }
            */
        }
    }
}