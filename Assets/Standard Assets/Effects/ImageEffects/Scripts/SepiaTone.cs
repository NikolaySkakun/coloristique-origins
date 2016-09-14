using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
    [ExecuteInEditMode]
    [AddComponentMenu("Image Effects/Color Adjustments/Sepia Tone")]
    public class SepiaTone : ImageEffectBase
	{
		void Awake()
		{
			shader = Shader.Find("Sepiatone Effect");
		}

        void OnRenderImage (RenderTexture source, RenderTexture destination)
		{
            Graphics.Blit (source, destination, material);
        }

		public void UpdateMaterial(float color)
		{
			material.SetFloat ("_Color", color);
			material.SetFloat ("_Color", color);
			material.SetFloat ("_Color", color);
		}
    }
}
