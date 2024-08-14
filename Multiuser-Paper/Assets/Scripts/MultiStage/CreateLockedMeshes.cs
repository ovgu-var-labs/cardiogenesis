using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLockedMeshes : MonoBehaviour
{
    public ButtonFunctionality buttonFunctionality;
    public GlobalAnimationManager animationManager;
    public GameObject prefab;
    public GameObject meshes;

    public T5Wand t5Wand;
    // Start is called before the first frame update
    void Start()
    {
        animationManager = gameObject.GetComponent<GlobalAnimationManager>();
        buttonFunctionality = FindAnyObjectByType<ButtonFunctionality>();
        t5Wand = FindAnyObjectByType<T5Wand>();
        t5Wand.annotationScript = GetComponentInChildren<ShowAnnotationScript>(includeInactive: true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        meshes = Instantiate(prefab);
        animationManager.animationProgress = 0f;
        buttonFunctionality.AnimationSlider.Value = 0f;
        t5Wand.annotationScript = GetComponentInChildren<ShowAnnotationScript>(includeInactive: true);
    }

    private void OnDisable()
    {
        Destroy(meshes);
        animationManager.animationProgress = 0f;
        buttonFunctionality.AnimationSlider.Value = 0f;
    }
}
