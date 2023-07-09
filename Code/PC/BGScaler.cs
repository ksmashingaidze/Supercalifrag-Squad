using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class BGScaler : MonoBehaviour
{
    Image bgimage; //Declare a variable to store the 'Image' component of the background image
    RectTransform recttrans; //Declare a variable to enable resizing of the background image
    public float ratio; //Declare a variable to store the image aspect ratio
    // Start is called before the first frame update
    void Start()
    {
        bgimage = GetComponent<Image>(); //Get the background image's 'Image' component
        recttrans = bgimage.rectTransform; //Get the component of the background image that allows easy resizing
        ratio = bgimage.sprite.bounds.size.x / bgimage.sprite.bounds.size.y; //Get the aspect ratio of the background image

    }

    // Update is called once per frame
    void Update()
    {
        if (!recttrans)
            return;

        //Scale the background image to the size of the screen
        if (Screen.height*ratio >= Screen.width)
        {
            recttrans.sizeDelta = new Vector2(Screen.height * ratio, Screen.height);
        }
        else
        {
            recttrans.sizeDelta = new Vector2(Screen.width, Screen.width / ratio);
        }
    }
}
