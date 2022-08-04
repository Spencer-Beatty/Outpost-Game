using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TransportDoor : MonoBehaviour
{
    public string LevelName;
    private void OnMouseDown()
    {
        LoadLevel(LevelName);
    }

    private void OnTriggerEnter(Collider other)
    {
        LoadLevel(LevelName);
    }
    private void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
