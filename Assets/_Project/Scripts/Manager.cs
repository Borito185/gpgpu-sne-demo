using UnityEngine;

namespace _Project.Scripts
{
    [ExecuteInEditMode]
    public class Manager : MonoBehaviour
    {
        public static Manager instance;
        public static Settings Settings => instance.settings;
        
        public Settings settings = new Settings();

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
    }
}
