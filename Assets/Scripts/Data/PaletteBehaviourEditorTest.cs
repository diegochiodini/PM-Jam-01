using UnityEngine;
using System.Collections;
using GameUtils;

public class PaletteBehaviourEditorTest : MonoBehaviour 
{
    public PaletteBehaviour palette;

    public VertexColors background;
    public GameObject bigCircle;       
    public GameObject anchor;          
    public GameObject bullet;
    public GameObject hitBullet;       

    public int paletteIndex;
    public bool apply;

#if UNITY_EDITOR

    void OnDrawGizmos()
    {
        if (apply)
        {
            Log.Print("Applying colors...");
            apply = false;
            //palette.SetCurrentPalette(paletteIndex);
            //Palette currentPalette = palette.currentPalette;

            //background.endColor = currentPalette.topBackground;
            //background.startColor = currentPalette.bottomBackground;
            //bigCircle.GetComponent<Renderer>().material.color = currentPalette.bigCircle;
            //anchor.GetComponent<Renderer>().material.color = currentPalette.anchor;
            //bullet.GetComponent<Renderer>().material.color = currentPalette.bullet;
            //hitBullet.GetComponent<Renderer>().material.color = currentPalette.hitBullet;

            Color[] colors = palette.colors;
            background.endColor                                 = colors[(6 * paletteIndex) + 0];
            background.startColor                               = colors[(6 * paletteIndex) + 1];
            background.ApplyColors();
            bigCircle.GetComponent<Renderer>().sharedMaterial.color   = colors[(6 * paletteIndex) + 2];
            anchor.GetComponent<Renderer>().sharedMaterial.color = colors[(6 * paletteIndex) + 3];
            bullet.GetComponent<Renderer>().sharedMaterial.color = colors[(6 * paletteIndex) + 4];
            hitBullet.GetComponent<Renderer>().sharedMaterial.color = colors[(6 * paletteIndex) + 5];
        }
    }

#endif

}
