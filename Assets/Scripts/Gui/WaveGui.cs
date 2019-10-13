using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveGui : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] Text nextWaveTimer;
    [SerializeField] Text enemiesRemaining;

    [Header("Options")]
    [SerializeField] float hideDelay = 3.5f;

    public void UpdateTimer(float remainingTime)
    {
        if (!nextWaveTimer.gameObject.activeInHierarchy)
            nextWaveTimer.gameObject.SetActive(true);

        nextWaveTimer.text = remainingTime.ToString("#.#");

        if (remainingTime <= 0f)
        {
            if (hideTimerCo != null)
                StopCoroutine(hideTimerCo);
            hideTimerCo = HideTimer();
            StartCoroutine(hideTimerCo);
        }
    }

    public void UpdateRemaining(int amount)
    {
        if (!enemiesRemaining.gameObject.activeInHierarchy)
            enemiesRemaining.gameObject.SetActive(true);

        enemiesRemaining.text = amount.ToString();

        if (amount <= 0)
        {
            if(hideRemainingCo != null)
                StopCoroutine(hideRemainingCo);
            hideRemainingCo = HideRemaining();
            StartCoroutine(hideRemainingCo);
        }
    }


    IEnumerator hideTimerCo;
    private IEnumerator HideTimer()
    {
        yield return new WaitForSeconds(hideDelay);

        nextWaveTimer.gameObject.SetActive(false);

    }

    IEnumerator hideRemainingCo;
    private IEnumerator HideRemaining()
    {
        yield return new WaitForSeconds(hideDelay);
        enemiesRemaining.gameObject.SetActive(false);
    }
}
