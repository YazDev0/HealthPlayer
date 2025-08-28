using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManage : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Text healthText;
    [SerializeField] private Text maxHealthText;

    private void Start()
    {
        //  √ﬂœ √‰ «··⁄»… ‘€«·… »‘ﬂ· ÿ»Ì⁄Ì ⁄‰œ »œ«Ì… √Ì „‘Âœ
        Time.timeScale = 1f;

        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    public void ShowGameOver(float currentHealth, float maxHealth)
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        if (healthText != null)
            healthText.text = "Health: " + currentHealth.ToString();

        if (maxHealthText != null)
            maxHealthText.text = "Max Health: " + maxHealth.ToString();

        // √Êﬁ› «··⁄»…
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // —Ã⁄ «··⁄»…  ‘ €· ÿ»Ì⁄Ì
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
