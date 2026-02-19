using UnityEngine;

public class ShowAfterDelay : MonoBehaviour
{
    public float delay = 5f;
    public GameObject target;

    public void Show()
    {
        StartCoroutine(ShowDelayed());
    }

    System.Collections.IEnumerator ShowDelayed()
    {
        yield return new WaitForSeconds(delay);
        target.SetActive(true);
    }
}
