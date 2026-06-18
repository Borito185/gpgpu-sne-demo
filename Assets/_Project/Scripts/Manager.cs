using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace _Project.Scripts
{
    [ExecuteInEditMode]
    public class Manager : MonoBehaviour
    {
        public static Manager instance;
        public static Settings Settings => instance.settings;
        
        public Settings settings = new Settings();

        private float timeLastStep = -1;
        
        public void Awake()
        {
            if (instance == null || instance == this)
                instance = this;
            else
                Utils.Destroy(gameObject);
        }

        public void OnValidate()
        {
            Awake();
        }

        public void Start()
        {
            Reset();
        }

        public void Update()
        {
            float time = Time.time;

            if (settings.simulate && timeLastStep + settings.cooldownSeconds < time)
            {
                timeLastStep = time;
                Step();
            }
        }
        
        [Button]
        public void Reset()
        {
            PointManager.Reset();
            TSneManager.Reset();
            Render();
        }

        [Button]
        public void Render()
        {
            PointManager.Render();
            FieldVisualizer.Render();
        }

        [Button]
        public void Step()
        {
            TSneManager.SimulateStep();
            Render();
        }

        [Button]
        public void Step10()
        {
            for (int i = 0; i < 10; i++)
            {
                Step();
            }
        }

        [Button]
        public void Recenter()
        {
            Utils.RecenterPoints();
            Render();
        }
    }
}
