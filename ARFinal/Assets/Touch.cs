using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic; // Necessário para usar Listas

public class Touch : MonoBehaviour
{
    // 1. Criamos uma "classe" simples para aparecer no Inspector
    [System.Serializable]
    public struct InfoConfig
    {
        public string tagDoObjeto;   // Ex: "gradil", "balcao", "bandeira"
        public GameObject infoPrefab; // O popup correspondente a essa tag
    }

    // 2. Lista que você vai preencher no Unity
    public List<InfoConfig> listaDeInfos;

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
                // --- LÓGICA DE ABRIR O POPUP CORRETO ---

                // Vamos procurar na nossa lista se o objeto clicado tem uma tag cadastrada
                bool encontrouConfig = false;

                foreach (var config in listaDeInfos)
                {
                    if (hit.transform.CompareTag(config.tagDoObjeto))
                    {
                        encontrouConfig = true;

                        // Se já tem um popup aberto, fecha ele
                        if (popupAtual != null)
                        {
                            Destroy(popupAtual);
                        }

                        // Posição ajustada (pode ajustar esse valor conforme a necessidade)
                        Vector3 pos = hit.point;
                        pos.y += 0.25f;
                        // Dica: Se o balcão for muito alto, talvez precise ajustar o Z ou Y dinamicamente

                        // Cria o popup ESPECÍFICO daquela tag
                        popupAtual = Instantiate(config.infoPrefab, pos, transform.rotation);

                        // Encerra o loop pois já achamos o objeto certo
                        break;
                    }
                }

                // --- LÓGICA DE FECHAR (CLICAR NA PRÓPRIA INFO) ---

                // Se não clicou em um objeto da lista, verifica se clicou no próprio popup para fechar
                if (!encontrouConfig)
                {
                    // Verifica se o objeto clicado é o popup atual ou um filho dele
                    // OU se ele tem a tag de fechar (caso você use tags nos popups)
                    if (popupAtual != null && hit.transform.gameObject == popupAtual)
                    {
                        Destroy(popupAtual);
                        popupAtual = null;
                    }
                    // Mantive sua lógica antiga por segurança caso use a tag "gradilinfo" em todos
                    else if (hit.transform.CompareTag("gradilinfo"))
                    {
                        Destroy(hit.transform.gameObject);
                        popupAtual = null;
                    }
                }
            }
        }
    }
}