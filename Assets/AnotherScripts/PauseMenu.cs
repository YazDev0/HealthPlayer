using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseUI; // �� ��� Canvas �� Panel ������ �����
    private bool isPaused = false;

    void Update()
    {
        // ��� ��� ������ ��� �� Escape
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
        pauseUI.SetActive(true);   // ����� �������
        Time.timeScale = 0f;       // ����� �����
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseUI.SetActive(false);  // ����� �������
        Time.timeScale = 1f;       // ���� �����
        isPaused = false;
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit(); // ���� �� ������ (�� build ���)
    }
 }

