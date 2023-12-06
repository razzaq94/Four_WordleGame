using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBehaviourGameObject<T> : MonoBehaviour where T : MonoBehaviour
{
    #region Properties
    /// <summary>
    /// Whether an Instance has currently been loaded (Instance != null)
    /// </summary>
    public static bool Exists => instance != null;

    /// <summary>
    /// Singleton-Instance for SObject. Auto-Created if currently null
    /// </summary>
    public static T Instance
    {
        get
        {
            return instance;
        }
        protected set { instance = value; }
    }

    /// <summary>
    /// Internal Singleton-Reference
    /// </summary>
    protected static T instance;

    /// <summary>
    /// Whether this Singleton will stay between scene loads. If true, Object will be added to DontDestroyOnLoad
    /// </summary>
    [SerializeField]
    [Tooltip("Whether this Singleton will stay between scene loads. If true, Object will be added to DontDestroyOnLoad")]
    protected bool shouldNotDestroyOnLoad;
    #endregion

    #region Methods
    /// <summary>
    /// Singleton-Setup. Remember to call base.Awake() if overriding
    /// </summary>
    protected virtual void Awake()
    {
        if (!Exists)
        {
            instance = this as T;
        }
        else if (Exists && !instance.Equals(this as T))
        {
            Debug.LogErrorFormat("Singleton<{0}> already exists! Existing Object: {1}. Destroying new object {2}", typeof(T).Name, instance.gameObject.name, gameObject.name);
            Destroy(gameObject);
            return;
        }

        if (shouldNotDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// Singleton-Destruction
    /// </summary>
    protected virtual void OnDestroy()
    {
        if (Exists && instance == this)
            instance = null;
    }
    #endregion
}
