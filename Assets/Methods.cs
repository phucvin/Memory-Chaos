using UnityEngine;
using UnityEngine.SceneManagement;

public class Methods : MonoBehaviour
{
    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayAnimation(AnimationClip clip)
    {
        GetComponent<Animation>().Play(clip.name);
    }
}