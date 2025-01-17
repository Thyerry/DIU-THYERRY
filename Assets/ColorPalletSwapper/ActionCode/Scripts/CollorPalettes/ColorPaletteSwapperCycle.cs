﻿using UnityEngine;

namespace ActionCode.ColorPalettes
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ColorPaletteSwapper))]
    public sealed class ColorPaletteSwapperCycle : MonoBehaviour
    {
        public KeyCode swapKey = KeyCode.N;
        public ColorPaletteSwapper swapper;
        public ColorPalette[] palettes;

        public bool init;
        private int _palletIndex = -1;

        private void Reset()
        {
            swapper = GetComponent<ColorPaletteSwapper>();
        }

        public void SwapPalette()
        {
            if (palettes.Length == 0) return;

            _palletIndex = (_palletIndex + 1) % palettes.Length;
            swapper.SwitchPalette(palettes[_palletIndex]);
            init = false;
        }

        public void AnimationSwapPallet(int paletteId)
        {
            swapper.SwitchPalette(palettes[paletteId]);
        }
        
    }
}