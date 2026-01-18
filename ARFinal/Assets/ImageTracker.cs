using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracker : MonoBehaviour
{
    private ARTrackedImageManager trackedImages;
    public GameObject[] ArPrefabs;

    // Mudança 1: Usar Dictionary para acesso rápido e vínculo direto
    // Chave (string) = Nome da Imagem de Referência
    // Valor (GameObject) = O objeto que instanciamos
    private Dictionary<string, GameObject> spawnedObjects = new Dictionary<string, GameObject>();

    private void Awake()
    {
        trackedImages = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable() => trackedImages.trackablesChanged.AddListener(OnTrackedImagesChanged);
    private void OnDisable() => trackedImages.trackablesChanged.RemoveListener(OnTrackedImagesChanged);

    private void OnTrackedImagesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        // 1. NOVAS IMAGENS DETECTADAS
        foreach (var trackedImage in eventArgs.added)
        {
            string imageName = trackedImage.referenceImage.name;

            foreach (var arPrefab in ArPrefabs)
            {
                // Verifica se o nome da imagem bate com o nome do prefab
                if (imageName == arPrefab.name)
                {
                    var newPrefab = Instantiate(arPrefab, trackedImage.transform);

                    // Opcional: Remover o "(Clone)" do nome para ficar organizado
                    newPrefab.name = imageName;

                    // Adiciona ao dicionário para controle futuro
                    if (!spawnedObjects.ContainsKey(imageName))
                    {
                        spawnedObjects.Add(imageName, newPrefab);
                    }
                }
            }
        }

        // 2. IMAGENS ATUALIZADAS (Aqui acontece a mágica de esconder/mostrar)
        foreach (var trackedImage in eventArgs.updated)
        {
            string imageName = trackedImage.referenceImage.name;

            // Verifica se nós já criamos um objeto para essa imagem
            if (spawnedObjects.TryGetValue(imageName, out GameObject gameObjectRelacionado))
            {
                // Verifica o estado do tracking
                // Tracking = A câmera está vendo a imagem perfeitamente -> SetActive(true)
                // Limited/None = A câmera perdeu a imagem -> SetActive(false)

                bool isVisible = trackedImage.trackingState == TrackingState.Tracking;

                // Só altera se o estado for diferente para economizar processamento
                if (gameObjectRelacionado.activeSelf != isVisible)
                {
                    gameObjectRelacionado.SetActive(isVisible);
                }
            }
        }

        // 3. IMAGENS REMOVIDAS (Opcional: Limpeza de memória se necessário)
        foreach (var trackedImage in eventArgs.removed)
        {
            string imageName = trackedImage.Value.referenceImage.name;
            if (spawnedObjects.ContainsKey(imageName))
            {
                Destroy(spawnedObjects[imageName]);
                spawnedObjects.Remove(imageName);
            }
        }
    }
}