using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ui_manager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject ui_canvas_prefab;
    GameObject ui_canvas_instance;
    Texture2D[] shapes = new Texture2D[3];

    void Start()
    {
        shapes[0] = (Texture2D)Resources.Load("Texture2D/l_shape_gui");
        shapes[1] = (Texture2D)Resources.Load("Texture2D/star_gui");
        shapes[2] = (Texture2D)Resources.Load("Texture2D/triangle_gui");
    }
    
    //Instantiate UI elements and set the sprite, or do nothing until hideUI is called if UI elements have been instantiated
    public void showUI(int stage)
    {
        if (ui_canvas_instance == null)     //No need to instantiate if already created
        {
            ui_canvas_instance = Instantiate(ui_canvas_prefab);
            ui_canvas_instance.transform.GetChild(1).GetComponent<Image>().sprite = Sprite.Create(shapes[stage], new Rect(0,0,shapes[stage].width, shapes[stage].height),new Vector2(0.5f, 0.5f));
        }
    }
    public void hideUI()
    {
        Destroy(ui_canvas_instance);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
