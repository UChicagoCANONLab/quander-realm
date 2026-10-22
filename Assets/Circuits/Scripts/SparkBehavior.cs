using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Circuits
{
    public class SparkBehavior : MonoBehaviour
    {
        public SpriteRenderer render;
        public Sprite[] sprites;
        private uint i = 0;
        private System.Random rng = new System.Random();
        float speed = 0;
        float distance = float.MaxValue;

        float sparkNegativeOffset = 20f;

        public static event Action KillAllSparksRequested;

        public static void RequestKillAllSparks()
        {
            KillAllSparksRequested?.Invoke();
        }

        // Start is called before the first frame update

        private void Start()
        {
            i = (uint)rng.Next(sprites.Length);
        }

        public void stepAnimation()
        {
            render.sprite = sprites[i % sprites.Length];
            i++;
        }

        private void Update()
        {
            float currOffset = speed * Time.deltaTime;
            transform.Translate(new Vector3(currOffset, 0));
            if (transform.position.x + sparkNegativeOffset >= distance)
            {
                killSpark();
            }
        }

        private void OnEnable()
        {
            KillAllSparksRequested += HandleKillAllSparksRequested;
        }

        private void OnDisable()
        {
            KillAllSparksRequested -= HandleKillAllSparksRequested;
        }

        private void HandleKillAllSparksRequested()
        {
            killSpark();
        }

        public void killSpark()
        {
            GTimer timer = GetComponent<GTimer>();
            timer.stopTimer();
            Destroy(this.gameObject);

        }

        public void runSpark(float spd, float dist, TimerManager tm)
        {
            GTimer timer = GetComponent<GTimer>();
            timer.timeManager = tm;
            timer.startTimer();

            speed = spd;
            distance = dist;
        }
    }
}