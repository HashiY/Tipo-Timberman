using System.Collections;
using UnityEngine;

//Stop + Shaking + Slow

public class DeadTimeStop : MonoBehaviour
{
    public float slowdownFactor = 0.05f;
    public float slowdownLength = 2f;

    public GameObject cameraShake;
    public AnimationCurve curve;
    public float duration = 1f;

    private bool pause;

    void Update()
    {
        if (pause)
        {
            Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        }
    }

    public void DoSlowMotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }

    public IEnumerator PauseGame(float pauseTime)
    {
        StartCoroutine(Shaking());

        pause = false;
        Time.timeScale = 0.0f;
        float pauseEndTime = Time.realtimeSinceStartup + pauseTime;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        Time.timeScale = 1f;
        pause = true;
        DoSlowMotion();
    }

    IEnumerator Shaking()
    {
        Vector3 startPosition = cameraShake.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            cameraShake.transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }

        cameraShake.transform.position = startPosition;
    }
}
