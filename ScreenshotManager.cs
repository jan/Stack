using System;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotManager : MonoBehaviour
{
    [SerializeField] private GameObject[] hideGameObjects;
    [SerializeField] private GameObject[] showGameObjects;

    private Dictionary<GameObject, bool> originalState = new Dictionary<GameObject, bool>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            StoreState();
            ChangeState();
            TakeScreenshot();
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            RestoreState();
        }
    }

    private void TakeScreenshot()
    {
        DateTime time = DateTime.Now;
        string filename = $"Panda and Crow Puzzle Screenshot {Screen.width}x{Screen.height} {time.ToString("yyyy-MM-dd HH-mm-ss")}.png";
        ScreenCapture.CaptureScreenshot(filename);
    }

    private void StoreState()
    {
        originalState.Clear();

        foreach (GameObject go in hideGameObjects) originalState.Add(go, go.activeSelf);
        foreach (GameObject go in showGameObjects) originalState.Add(go, go.activeSelf);
    }

    private void ChangeState()
    {
        foreach (GameObject go in hideGameObjects) go.SetActive(false);
        foreach (GameObject go in showGameObjects) go.SetActive(true);
    }

    private void RestoreState()
    {
        foreach (KeyValuePair<GameObject, bool> pair in originalState) pair.Key.SetActive(pair.Value);
    }
}
