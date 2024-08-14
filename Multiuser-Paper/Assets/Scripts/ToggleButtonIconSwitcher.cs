using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButtonIconSwitcher : MonoBehaviour
{

    public Sprite IconUnToggled;
    public Sprite IconToggled;
    public SpriteRenderer spriteRenderer;

    public bool IconOneIsActive;

    public MeshRenderer meshRenderer;       // Hintergrund des Buttons
    public Material unPressedMaterial;
    public Material pressedMaterial;

    // Start is called before the first frame update
    void Start()
    {
        // IconOneIsActive = true;
        SetActiveIcon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchBackgroundMaterial(bool isToggled)
    {
        if (isToggled)
            meshRenderer.material = pressedMaterial;
        else
            meshRenderer.material = unPressedMaterial;
    }

    public void SwitchIcons()
    {
        IconOneIsActive = !IconOneIsActive;
        if (IconOneIsActive)
            spriteRenderer.sprite = IconToggled;
        
        if (!IconOneIsActive)
            spriteRenderer.sprite = IconUnToggled;
        
        // SwitchBackgroundMaterial(IconOneIsActive);

    }

    private void SetActiveIcon()
    {
        if (IconOneIsActive)
            spriteRenderer.sprite = IconToggled;


        if (!IconOneIsActive)
            spriteRenderer.sprite = IconUnToggled;

        // SwitchBackgroundMaterial(IconOneIsActive);
    }
}
