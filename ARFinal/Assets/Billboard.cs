using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    // Usamos LateUpdate para garantir que o objeto gire DEPOIS que a câmera se moveu
    void LateUpdate()
    {
        if (cam != null)
        {
            // MÉTODO A: O objeto copia a rotação da câmera (Fica paralelo à tela)
            // Ideal para UI/Textos 2D flutuantes. Evita que o texto fique invertido/espelhado.
            transform.rotation = cam.transform.rotation;

            // MÉTODO B: O objeto olha exatamente para o ponto da câmera
            // transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
        }
    }
}