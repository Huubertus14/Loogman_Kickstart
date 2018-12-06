using UnityEngine;
using UnityEngine.XR.WSA.Input;
using HoloToolkit.Unity.SpatialMapping;


public class CubeSpawn : MonoBehaviour
{

    public GameObject Cube;
    private GestureRecognizer recognizer;
    // Use this for initialization

   public SpatialMappingManager mapMan;

    float tapTimer;
    bool drawValue;
    void Start()
    {
        recognizer = new GestureRecognizer();
        recognizer.SetRecognizableGestures(GestureSettings.Tap);
        recognizer.Tapped += TapHandler;
        recognizer.StartCapturingGestures();
        tapTimer = 0;
        drawValue = false;

        mapMan.DrawVisualMeshes = drawValue;
    }

    // Update is called once per frame
    void Update()
    {
        tapTimer += Time.deltaTime;
    }

    private void TapHandler(TappedEventArgs obj)
    {
        if (tapTimer < 1.2f)
        {
            drawValue = !drawValue;
        }
        else
        {
            //create cube
            CreateCube();
        }
        mapMan.DrawVisualMeshes = drawValue;
        tapTimer = 0;
    }

    private void CreateCube()
    {
        GameObject _cube = Instantiate(Cube,transform.position, Quaternion.identity);
        _cube.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 500);
    }
}
