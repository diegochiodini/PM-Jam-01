using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Assertions;

public class Palette
{
    public const int PALETTE_SIZE = 6;

    //private static Color[] palette1 = { new Color(1, 1, 1), new Color(1, 1, 1), new Color(1, 1, 1), new Color(1, 1, 1), new Color(1, 1, 1), new Color(1, 1, 1) };
    //public readonly Palette[] defaultPalette = 
    //{
    //    new Palette(palette1)
    //};

    public Color topBackground;
    public Color bottomBackground;
    public Color bigCircle;
    public Color anchor;
    public Color bullet;
    public Color hitBullet;

    //public Palette(Color topBackground, Color bottomBackground, Color bigCircle, Color anchor, Color bullet, Color hitBullet)
    public Palette(Color[] colors)
    {
        Assert.IsTrue(colors.Length == PALETTE_SIZE, "You must provide 6 colors for a complete palette.");
        topBackground = colors[0];
        bottomBackground = colors[1];
        bigCircle = colors[2];
        anchor = colors[3];
        bullet = colors[4];
        hitBullet = colors[5];
    }
}

public class PaletteBehaviour : MonoBehaviour 
{
    //public Palette palette;
    public Color[] colors;
    private Palette[] palettes;

    private int currentIndex;
    public Palette currentPalette
    {
        get
        {
            return palettes[currentIndex];
        }

        private set { }
    }

    void Awake()
    {
        const int palSize = Palette.PALETTE_SIZE;
        int paletteNumber = colors.Length / palSize;

        palettes = new Palette[paletteNumber];
        for (int i = 0; i < paletteNumber; i++)
        {
            int index = i * palSize;
            Color[] colorPalette = new Color[Palette.PALETTE_SIZE];
            Array.Copy(colors, index, colorPalette, 0, Palette.PALETTE_SIZE);
            Palette pal = new Palette(colorPalette);
            palettes[i] = pal;
        }
    }

    public void SetPaletteForLevel(LevelDescription level)
    {
        currentIndex = level.paletteIndex;
    }

#if UNITY_EDITOR
    public void SetCurrentPalette(int index)
    {
        currentIndex = index;
    }
#endif
}
