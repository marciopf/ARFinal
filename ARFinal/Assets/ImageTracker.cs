using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracker : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private ARTrackedImageManager trackedImages;
    public GameObject[] ArPrefabs;

    List<GameObject> ARObjects = new List<GameObject>();

    private void Awake()
    {
        trackedImages = GetComponent<ARTrackedImageManager>();
    }
    private void OnEnable() => trackedImages.trackablesChanged.AddListener(OnTrackedImagesChanged);

    private void OnDisable() => trackedImages.trackablesChanged.RemoveListener(OnTrackedImagesChanged);
   
    private void OnTrackedImagesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        foreach( var trackedImage in eventArgs.added)
        {
            foreach( var arPrefab in ArPrefabs)
            {
                if ( trackedImage.referenceImage.name == arPrefab.name)
                {
                    var newPrefab = Instantiate(arPrefab, trackedImage.transform);
                    ARObjects.Add(newPrefab);

                }
            }
        }
        foreach (var trackedImage in eventArgs.updated)
        {
            foreach (var gameObject in ARObjects)
            {
                if (trackedImage.name == gameObject.name)
                {
                    gameObject.SetActive(trackedImage.trackingState == TrackingState.Tracking);

                }
            }
        }
    }
}
