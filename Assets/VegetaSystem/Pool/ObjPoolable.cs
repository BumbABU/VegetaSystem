using UnityEngine;

namespace VegetaSystem
{
    public abstract class ObjPoolable : MonoBehaviour
    {
        private string keyPool;
        public bool IsRelease;
        public virtual void Init(string keyPool)
        {
            this.gameObject.SetActive(false);
            this.keyPool = keyPool;
            IsRelease = true;
        }

        public string GetKeyPool()
        {
            return keyPool;
        }

        public abstract void Get();
        public abstract void Release();
    }
}
