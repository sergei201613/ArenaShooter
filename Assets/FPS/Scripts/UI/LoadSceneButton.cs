using Sgorey.Unity.Utils.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    public string SceneName = "";
    public bool LoadFromBoot = true;

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject
            && Input.GetButtonDown(GameConstants.k_ButtonNameSubmit))
        {
            LoadTargetScene();
        }
    }

    public void LoadTargetScene()
    {
        if (LoadFromBoot)
            Boot.Load(SceneName);
        else
            SceneManager.LoadScene(SceneName);
    }
}