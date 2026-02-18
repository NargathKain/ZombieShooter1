using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Plays a sound when a button is clicked.
/// Attach to any Button GameObject.
///
/// SETUP:
/// 1. Attach to a Button GameObject
/// 2. Assign the click sound
/// </summary>
[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;
    [Range(0f, 1f)]
    [SerializeField] private float volume = 1f;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(PlaySound);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(PlaySound);
    }

    private void PlaySound()
    {
        if (clickSound != null)
        {
            AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position, volume);
        }
    }
}
