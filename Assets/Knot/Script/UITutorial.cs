using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPanelController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject Panel;

    [Header("Time Settings")]
    public float delayBeforeStop = 1.5f;

    void Start()
    {
        if (Panel != null)
        {
            Panel.SetActive(true);
            StartCoroutine(ShowPanelAndWait());

        }

    }
    IEnumerator ShowPanelAndWait()
    {
        
        Panel.SetActive(true);

        yield return new WaitForSeconds(delayBeforeStop);

        Time.timeScale = 0f;
    }
    public void ClosePanel()
    {
        if (Panel != null)
        {
            Panel.SetActive(false);

            Time.timeScale = 1f;
        }
    }
}