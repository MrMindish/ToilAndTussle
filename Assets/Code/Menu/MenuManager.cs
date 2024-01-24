using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Button button;

    public Sprite[] button_sprites;

    public void Start()
    {
        Image buttonImage = this.gameObject.GetComponent<Image>();
    }
    public void Update()
    {
        
    }
    public void OnMouseOver()
    {
        button.GetComponent<Image>().sprite = button_sprites[1];

        button.image.sprite = button_sprites[2];
        Debug.Log("hovered over");
    }

    public void OnMouseEnter()
    {
        button.image.sprite = button_sprites[2];
        Debug.Log("hovered over");
    }


}
