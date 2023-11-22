using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PreviewUI : MonoBehaviour
{
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        StackableManager.OnNextStackableChosen += HandleNextStackableChosen;
    }

    private void OnDisable()
    {
        StackableManager.OnNextStackableChosen -= HandleNextStackableChosen;
    }

    private void HandleNextStackableChosen(Stackable stackable)
    {
        image.sprite = stackable.Sprite;
        image.color = stackable.Color;
    }
}
