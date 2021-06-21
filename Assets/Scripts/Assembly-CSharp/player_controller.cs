using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controller : MonoBehaviour
{
    float player_speed;
    Vector2 mouse_look;
    Vector2 smooth_vector;
    float sensitivity;
    float smoothing;
    public bool can_move;

    GameObject cam;
    game_manager gm;

    // Start is called before the first frame update
    void Start()
    {
        player_speed = 10.0f;
        sensitivity = 5.0f;
        smoothing = 2.0f;
        cam = GameObject.Find("M a i n  c h a r a c t e r. exe");
        gm = GameObject.Find("Game_Master").GetComponent<game_manager>();
    }

    // Update is called once per frame
    void Update()
    {
        float translation = Input.GetAxis("Vertical") * player_speed;
        float straffe = Input.GetAxis("Horizontal") * player_speed;
        translation *= Time.deltaTime;
        straffe *= Time.deltaTime;

        if(can_move)
            transform.Translate(new Vector3(straffe, 0, translation));

        Vector2 mouse_dir = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        mouse_dir = Vector2.Scale(mouse_dir, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smooth_vector.x = Mathf.Lerp(smooth_vector.x, mouse_dir.x, 1.0f / smoothing);
        smooth_vector.y = Mathf.Lerp(smooth_vector.y, mouse_dir.y, 1.0f / smoothing);
        mouse_look += smooth_vector;

        mouse_look.y = Mathf.Clamp(mouse_look.y, -90, 90);

        cam.transform.localRotation = Quaternion.AngleAxis(-mouse_look.y, Vector3.right);
        transform.localRotation = Quaternion.AngleAxis(mouse_look.x, transform.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "BadPattern")
            gm.signalTouchedBad();

        //Signal if touching stage_complete object
        if (other.transform.tag == "MoveOn")
            gm.signalCompletedStage();
    }
}