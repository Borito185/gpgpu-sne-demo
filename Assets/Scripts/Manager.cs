using Models;
using Tsne;
using UnityEngine;
using Visualizing;

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
            Utils.Utils.Destroy(gameObject);
    }

    public void Start() 
        => Reset();

    public void Update()
    {
        float time = Time.time;

        if (settings.simulate && timeLastStep + (1f / settings.targetRate) < time)
        {
            timeLastStep = time;
            Step();
        }

        Render();
    }
        
    public void Reset()
    {
        PointManager.Reset();
        TSneManager.Reset();
        Render();
    }

    private void Render()
    {
        PointManager.Render();
        FieldVisualizer.Render();
    }

    public void Step() 
        => TSneManager.SimulateStep();

    public void Recenter() 
        => Utils.Utils.RecenterPoints();
}