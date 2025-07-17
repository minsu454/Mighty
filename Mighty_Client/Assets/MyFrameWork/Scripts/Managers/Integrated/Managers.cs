using Common.Objects;
using Common.SceneEx;
using UnityEngine;

public sealed class Managers : MonoBehaviour
{
    private static Managers instance;

    public static UIManager UI { get { return instance.uiManager; } }
    public static SoundManager Sound { get { return instance.soundManager; } }
    public static NetworkManager Network { get { return instance.networkManager; } }

    private UIManager uiManager;
    private SoundManager soundManager;
    private NetworkManager networkManager = new NetworkManager();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        GameObject go = new GameObject("Managers");
        instance = go.AddComponent<Managers>();

        DontDestroyOnLoad(go);

        ObjectManager.Init();

        instance.uiManager = CreateManager<UIManager>(go.transform);
        instance.soundManager = CreateManager<SoundManager>(go.transform);

        SceneJobLoader.Init();
        Network.Init();
    }

    /// <summary>
    /// 매니저 생성 함수
    /// </summary>
    private static T CreateManager<T>(Transform parent) where T : Component, IInit
    {
        GameObject go = new GameObject(typeof(T).Name);
        T t = go.AddComponent<T>();
        go.transform.parent = parent;

        t.Init();

        return t;
    }
}