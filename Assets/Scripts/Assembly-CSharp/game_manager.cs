using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    GameObject prefab;
    GameObject instance;
    Vector3 position, rotation, size;
    Material mat;
    string tag;

    Path() { }
    public Path(GameObject pref, Vector3 pos, Vector3 rot, Vector3 size, Material mat, string tag)
    {
        prefab = pref;
        position = pos;
        rotation = rot;
        this.size = size;
        this.mat = mat;
        this.tag = tag;
    }

    //Instantiates a GameObject from Prefab and sets its tag and material
    public void createPath()
    {
        this.instance = MonoBehaviour.Instantiate(prefab);
        this.instance.transform.position = position;
        this.instance.transform.localScale = size;
        this.instance.transform.rotation = Quaternion.Euler(rotation);
        this.instance.tag = tag;

        Renderer instance_ren = this.instance.GetComponent<Renderer>();
        instance_ren.material = mat;
        instance_ren.material.mainTextureScale = new Vector2(size.x,size.y);    //Set tiling of the texture to the size of the path to prevent stretching
    }

    //Destroys the GameObject created above
    public void destroyPath()
    {
        MonoBehaviour.Destroy(instance);
    }
}

public class game_manager : MonoBehaviour
{
    //Prefabs are set via the inspector in Unity editor, rather than the Start method below
    public GameObject path_prefab;  
    public GameObject complete_prefab;
    GameObject complete_object;
    public Dictionary<string, Material> materials = new Dictionary<string, Material>();     //Dictionary was used here to make it easier to refer to the material needed
    public List<Path>[] paths = new List<Path>[3];      //Each list in the array represents a stage
    Vector3[] complete_positions = new Vector3[3] { new Vector3(3.62f, 2.61f, -30.52f), new Vector3(-47.56f, 2.61f, -31.74f), new Vector3(-37.44f, 2.61f, 32.36f) };
    int stage;
    bool touched_bad, stage_complete;
    float delay_time;
    GameObject player;
    player_controller player_cont;
    ui_manager ui_mgr;
    Vector3 respawn_pos = new Vector3(36.9f, 2.02f, -10.58f);
    Vector3 respawn_rot = new Vector3(0f,0f,0f);

    //Populate the materials dictionary
    void initMaterials()
    {
        //Stage 1 Materials
        materials.Add("L_Shape_1", (Material)Resources.Load("Material/l_shape1"));
        materials.Add("Star_2", (Material)Resources.Load("Material/star2"));
        materials.Add("Triangle_1", (Material)Resources.Load("Material/triangle1"));

        //Stage 2 Materials
        materials.Add("Star_1", (Material)Resources.Load("Material/star1"));
        materials.Add("Triangle_3", (Material)Resources.Load("Material/triangle3"));      //Also used in Stage 3
        materials.Add("L_Shape_3", (Material)Resources.Load("Material/l_shape3"));

        //Stage 3 Materials
        materials.Add("Star_3", (Material)Resources.Load("Material/star3"));
        materials.Add("L_Shape_2", (Material)Resources.Load("Material/l_shape2"));

        //materials["Star_3"].mainTextureScale
    }
    
    //Populate the array of lists that holds Paths
    void initPaths()
    {
        //Initialise each list in the array of lists of paths
        for (int i = 0; i < paths.Length; i++)
            paths[i] = new List<Path>();

        //Stage 1
        paths[0].Add(new Path(path_prefab, new Vector3(14.44f, 0.12f, -11.8f), new Vector3(0f, 0f, 0f), new Vector3(2.37f, 1f, 0.2f), materials["L_Shape_1"], "GoodPattern"));
        paths[0].Add(new Path(path_prefab, new Vector3(3.67f, 0.12f, -21.55f), new Vector3(0f, 90f, 0f), new Vector3(1.75f, 1f, 0.2f), materials["L_Shape_1"], "GoodPattern"));
        paths[0].Add(new Path(path_prefab, new Vector3(-11.04f, 0.12f, -8.43f), new Vector3(0f, 0f, 0f), new Vector3(7.5f, 1f, 0.2f), materials["Star_2"], "BadPattern"));
        paths[0].Add(new Path(path_prefab, new Vector3(27.35f, 0.12f, 14.21f), new Vector3(0f, 90f, 0f), new Vector3(3.7f, 1f, 0.2f), materials["Triangle_1"], "BadPattern"));

        //Stage 2
        paths[1].Add(new Path(path_prefab, new Vector3(-24.76f, 0.12f, -24.96f), new Vector3(180f, 0f, -180f), new Vector3(4.9f, 1f, 0.15f), materials["Star_1"], "GoodPattern"));
        paths[1].Add(new Path(path_prefab, new Vector3(-47.99f, 0.12f, -27.76f), new Vector3(0f, -90f, 0f), new Vector3(0.4f, 1f, 0.15f), materials["Star_1"], "GoodPattern"));
        paths[1].Add(new Path(path_prefab, new Vector3(20.37f, 0.12f, -24.95f), new Vector3(180f, 0f, -180f), new Vector3(3f, 1f, 0.15f), materials["Triangle_3"], "BadPattern"));
        paths[1].Add(new Path(path_prefab, new Vector3(34.66f, 0.12f, -28.24f), new Vector3(0f, -90f, 0f), new Vector3(0.5f, 1f, 0.15f), materials["Triangle_3"], "BadPattern"));
        paths[1].Add(new Path(path_prefab, new Vector3(2.65f, 0.12f, 2.71f), new Vector3(0f, 90f, 0f), new Vector3(5f, 1f, 0.15f), materials["L_Shape_3"], "BadPattern"));
        paths[1].Add(new Path(path_prefab, new Vector3(-0.63f, 0.12f, 26.91f), new Vector3(180f, 0f, -180f), new Vector3(0.5f, 1f, 0.15f), materials["L_Shape_3"], "BadPattern"));
        paths[1].Add(new Path(path_prefab, new Vector3(-2.5f, 0.12f, 28.69f), new Vector3(0f, -90f, 0f), new Vector3(0.25f, 1f, 0.15f), materials["L_Shape_3"], "BadPattern"));

        //Stage 3
        paths[2].Add(new Path(path_prefab, new Vector3(-49.88f, 0.12f, -24.27f), new Vector3(0f, -90f, 0f), new Vector3(0.4f, 1f, 0.1f), materials["Triangle_3"], "GoodPattern"));
        paths[2].Add(new Path(path_prefab, new Vector3(-43.9f, 0.12f, -21.78f), new Vector3(180f, 0f, -180f), new Vector3(1.3f, 1f, 0.1f), materials["Triangle_3"], "GoodPattern"));
        paths[2].Add(new Path(path_prefab, new Vector3(-37.91f, 0.12f, 4.75f), new Vector3(0f, -90f, 0f), new Vector3(5.2f, 1f, 0.1f), materials["Triangle_3"], "GoodPattern"));
        paths[2].Add(new Path(path_prefab, new Vector3(-53.32f, 0.12f, -27.98f), new Vector3(180f, 0f, -180f), new Vector3(0.2f, 1f, 0.1f), materials["Star_3"], "BadPattern"));
        paths[2].Add(new Path(path_prefab, new Vector3(-54.81f, 0.12f, -23.97f), new Vector3(0f, -90f, 0f), new Vector3(0.9f, 1f, 0.1f), materials["Star_3"], "BadPattern"));
        paths[2].Add(new Path(path_prefab, new Vector3(-49.82f, 0.12f, -18.97f), new Vector3(180f, 0f, -180f), new Vector3(1.1f, 1f, 0.1f), materials["Star_3"], "BadPattern"));
        paths[2].Add(new Path(path_prefab, new Vector3(-43.79f, 0.12f, 0.28f), new Vector3(0f, -90f, 0f), new Vector3(3.95f, 1f, 0.1f), materials["Star_3"], "BadPattern"));
        paths[2].Add(new Path(path_prefab, new Vector3(-45.31f, 0.12f, 19.54f), new Vector3(180f, 0f, -180f), new Vector3(0.2f, 1f, 0.1f), materials["Star_3"], "BadPattern"));
        paths[2].Add(new Path(path_prefab, new Vector3(-36.37f, 0.12f, -26.96f), new Vector3(180f, 0f, -180f), new Vector3(2f, 1f, 0.1f), materials["L_Shape_2"], "BadPattern"));
        paths[2].Add(new Path(path_prefab, new Vector3(-26.82f, 0.12f, -28.48f), new Vector3(0f, -90f, 0f), new Vector3(0.2f, 1f, 0.1f), materials["L_Shape_2"], "BadPattern"));
    }

    // Start is called before the first frame update
    void Start()
    {
        ui_mgr = GetComponent<ui_manager>();
        player = GameObject.Find("Player");
        player_cont = player.GetComponent<player_controller>();
        stage = 0;
        touched_bad = false;
        stage_complete = false;
        delay_time = 5.0f;

        initMaterials();
        initPaths();
        setupStage();
        player_cont.can_move = false;   //Lock player movement

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void setupStage()
    {
        //Instantiate prefabs, give them materials and give them tags
        foreach (Path p in paths[stage])
        {
            p.createPath();
        }

        complete_object = Instantiate(complete_prefab);
        complete_object.transform.position = complete_positions[stage];
        complete_object.transform.tag = "MoveOn";
    }
    void cleanUpStage()
    {
        foreach (Path p in paths[stage])
            p.destroyPath();

        Destroy(complete_object);
    }

    public void signalTouchedBad() { touched_bad = true; }
        
    public void signalCompletedStage() { stage_complete = true; }

    // Update is called once per frame
    void Update()
    {
        if (!player_cont.can_move)      //Unlock player movement after 5 seconds, following the stage setup
        {
            ui_mgr.showUI(stage);

            if (delay_time > 0.0f)
                delay_time -= Time.deltaTime;
            else
            {
                player_cont.can_move = true;      //Unlock player movement, if it was locked and timer ran out
                delay_time = 5.0f;
                ui_mgr.hideUI();
            }
        }
        else
        {
            if (touched_bad || (stage_complete && stage == 2))
            {
                if (stage > 0)      //No need to clean up and set up if player failed the 1st stage
                {
                    cleanUpStage();
                    stage = 0;
                    setupStage();
                }
                player.transform.position = respawn_pos;
                player.transform.rotation = Quaternion.Euler(respawn_rot);
                touched_bad = false;
                stage_complete = false;
                player_cont.can_move = false;       //Lock player movement
            }
            else if (stage_complete && stage < 2)
            {
                cleanUpStage();
                stage++;
                setupStage();
                stage_complete = false;
                player_cont.can_move = false;       //Lock player movement
            }
        }

        if (Input.GetKeyDown("escape"))
            Application.Quit();
    }
}

//Timer- 5 seconds