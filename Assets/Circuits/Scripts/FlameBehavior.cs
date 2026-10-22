using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuits
{
    public class FlameBehavior : MonoBehaviour
    {
        public SpriteRenderer render;
        public Sprite[] sprites;

        private const float FRAME_INTERVAL = 0.125f; 

        private float frameTimer = 0f;
        private int frameIndex = 0;

        private void Start()
        {
            if (render == null || sprites == null || sprites.Length == 0)
            {
                enabled = false;
                return;
            }

            frameIndex = Random.Range(0, sprites.Length-1);
            render.sprite = sprites[frameIndex];
        }

        private void Update()
        {
            frameTimer += Time.deltaTime;

            while (frameTimer >= FRAME_INTERVAL)
            {
                frameTimer -= FRAME_INTERVAL;
                AdvanceFrame();
            }
        }

        private void AdvanceFrame()
        {
            if (sprites.Length == 1)
            {
                return;
            }

            int nextFrameIndex = frameIndex;

            while (nextFrameIndex == frameIndex)
            {
                nextFrameIndex = Random.Range(0, sprites.Length-1);
            }

            frameIndex = nextFrameIndex;
            render.sprite = sprites[frameIndex];
        }
    }
}