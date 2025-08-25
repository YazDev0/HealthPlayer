using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseUI; // ÖÚ åäÇ Canvas Ãæ Panel áŞÇÆãÉ ÇáÈæÒ
    private bool isPaused = false;

    void Update()
    {
        // ÅĞÇ ÖÛØ ÇááÇÚÈ Úáì ÒÑ Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseUI.SetActive(true);   // ÇÙåÇÑ ÇáŞÇÆãÉ
        Time.timeScale = 0f;       // ÇíŞÇİ ÇáæŞÊ
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseUI.SetActive(false);  // ÇÎİÇÁ ÇáŞÇÆãÉ
        Time.timeScale = 1f;       // ÑÌæÚ ÇáæŞÊ
        isPaused = false;
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit(); // íØáÚ ãä ÇááÚÈÉ (İí build İŞØ)
    }
 }

