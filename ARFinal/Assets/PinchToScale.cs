using UnityEngine;
using UnityEngine.InputSystem; // Necessário para o novo sistema de input

public class PinchToScale : MonoBehaviour
{
    [Header("Configurações de Escala")]
    public float speed = 0.005f; // Sensibilidade do zoom
    public float minScale = 0.5f; // Tamanho mínimo permitido
    public float maxScale = 3.0f; // Tamanho máximo permitido

    // Variável para guardar o tamanho original caso precise resetar (opcional)
    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        // 1. Lógica para Celular (Toque na tela)
        if (Touchscreen.current != null && Touchscreen.current.touches.Count >= 2)
        {
            var touch0 = Touchscreen.current.touches[0];
            var touch1 = Touchscreen.current.touches[1];

            // Verifica se pelo menos um dos dedos se moveu
            if (touch0.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved ||
                touch1.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved)
            {
                // Pega posições atuais
                Vector2 pos0 = touch0.position.ReadValue();
                Vector2 pos1 = touch1.position.ReadValue();

                // Pega posições anteriores (Subtraindo o delta/movimento da posição atual)
                Vector2 prevPos0 = pos0 - touch0.delta.ReadValue();
                Vector2 prevPos1 = pos1 - touch1.delta.ReadValue();

                // Calcula a distância entre os dedos (magnitude)
                float prevMagnitude = (prevPos0 - prevPos1).magnitude;
                float currentMagnitude = (pos0 - pos1).magnitude;

                // A diferença determina se abriu (zoom in) ou fechou (zoom out)
                float difference = currentMagnitude - prevMagnitude;

                ApplyScale(difference);
            }
        }
        // 2. Lógica para Computador (Roda do Mouse) - Para facilitar seus testes no Editor
        else if (Mouse.current != null)
        {
            float scroll = Mouse.current.scroll.y.ReadValue();
            if (scroll != 0)
            {
                // Ajustamos a velocidade do mouse para ficar suave
                ApplyScale(scroll * 0.1f);
            }
        }
    }

    private void ApplyScale(float delta)
    {
        // Aplica a mudança
        Vector3 newScale = transform.localScale + (Vector3.one * delta * speed);

        // Garante que o objeto não fique menor que o mínimo nem maior que o máximo
        newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
        newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
        newScale.z = Mathf.Clamp(newScale.z, minScale, maxScale);

        transform.localScale = newScale;
    }
}