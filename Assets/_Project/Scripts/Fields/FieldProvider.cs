using UnityEngine;

namespace _Project.Scripts
{
    public class FieldProvider : MonoBehaviour
    {
        public virtual Field GetField()
        {
            return new Field(Vector2.zero, Vector2.one);
        }
    }
}