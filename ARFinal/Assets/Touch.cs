using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Touch : MonoBehaviour
{
    [System.Serializable]
    public struct InfoConfig
    {
        public string tagDoObjeto;
        public GameObject infoPrefab;
    }

    public List<InfoConfig> listaDeInfos;

    // O popup que está aberto atualmente
    private GameObject popupAtual;

    // NOVO: Guarda qual objeto AR (Gradil, Balcao, etc) originou o popup
    private GameObject objetoArFocado;

    void Update()
    {
        // --- 1. VERIFICAÇÃO DE SEGURANÇA (NOVO) ---
        // Se temos um popup aberto, mas o objeto AR dono dele foi desativado (perdeu tracking), fecha o popup.
        if (popupAtual != null && objetoArFocado != null)
        {
            if (!objetoArFocado.activeInHierarchy)
            {
                FecharPopup();
            }
        }
        // ------------------------------------------

        bool isPressed = false;
        Vector2 screenPosition = Vector2.zero;

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
                bool encontrouConfig = false;

                foreach (var config in listaDeInfos)
                {
                    if (hit.transform.CompareTag(config.tagDoObjeto))
                    {
                        encontrouConfig = true;

                        // Fecha anterior se existir
                        if (popupAtual != null) Destroy(popupAtual);

                        Vector3 pos = hit.point;
                        pos.y += 0.25f;

                        popupAtual = Instantiate(config.infoPrefab, pos, transform.rotation);

                        // NOVO: Guardamos quem é o dono desse popup (ex: o objeto do Gradil que foi clicado)
                        objetoArFocado = hit.transform.gameObject;

                        break;
                    }
                }

                if (!encontrouConfig)
                {
                    // Se clicar no próprio popup para fechar ou no botão de fechar
                    if (popupAtual != null && (hit.transform.gameObject == popupAtual || hit.transform.IsChildOf(popupAtual.transform)))
                    {
                        FecharPopup();
                    }
                    else if (hit.transform.CompareTag("gradilinfo")) // Mantendo compatibilidade
                    {
                        Destroy(hit.transform.gameObject);
                        popupAtual = null;
                        objetoArFocado = null;
                    }
                }
            }
        }
    }

    // Criei uma funçãozinha para evitar repetir código de "zerar" as variáveis
    void FecharPopup()
    {
        if (popupAtual != null)
        {
            Destroy(popupAtual);
        }
        popupAtual = null;
        objetoArFocado = null;
    }
}