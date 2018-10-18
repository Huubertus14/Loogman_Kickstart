using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VrFox;

public class SceneOrganiser : MonoBehaviour {
    /// <summary>
    /// Allows this class to behave like a singleton
    /// </summary>
    public static SceneOrganiser Instance;
    
    /// <summary>
    /// Current threshold accepted for displaying the label
    /// Reduce this value to display the recognition more often
    /// </summary>
    internal float probabilityThreshold = 0.8f;
    
    /// <summary>
    /// Called on initialization
    /// </summary>
    private void Awake()
    {
        // Use this class instance as singleton
        Instance = this;

        // Add the ImageCapture class to this Gameobject
        gameObject.AddComponent<ImageCapture>();

        // Add the CustomVisionAnalyser class to this Gameobject
        gameObject.AddComponent<CustomVisionAnalyser>();

        // Add the CustomVisionObjects class to this Gameobject
        gameObject.AddComponent<CustomVisionObjects>();
    }


    /// <summary>
    /// Set the Tags as Text of the last label created. 
    /// </summary>
    public void FinaliseLabel(AnalysisRootObject analysisObject)
    {
        if (analysisObject.predictions != null)
        {
            // Sort the predictions to locate the highest one
            List<Prediction> sortedPredictions = new List<Prediction>();
            sortedPredictions = analysisObject.predictions.OrderBy(p => p.probability).ToList();
            Prediction bestPrediction = new Prediction();
            bestPrediction = sortedPredictions[sortedPredictions.Count - 1];
            
            
            foreach (var item in sortedPredictions)
            {
                GameManager.Instance.SendTextMessage(item.tagName, 5, Vector2.zero);
            }

            if (bestPrediction.probability > probabilityThreshold)
            {
               // quadRenderer = quad.GetComponent<Renderer>() as Renderer;
                //Bounds quadBounds = quadRenderer.bounds;

                // Position the label as close as possible to the Bounding Box of the prediction 
                // At this point it will not consider depth
               // lastLabelPlaced.transform.parent = quad.transform;
               // lastLabelPlaced.transform.localPosition = CalculateBoundingBoxPosition(quadBounds, bestPrediction.boundingBox);

                // Set the tag text

                // Cast a ray from the user's head to the currently placed label, it should hit the object detected by the Service.
                // At that point it will reposition the label where the ray HL sensor collides with the object,
                // (using the HL spatial tracking)
                Debug.Log("Repositioning Label");
              
            }
        }
        // Reset the color of the cursor
       // cursor.GetComponent<Renderer>().material.color = Color.green;

        // Stop the analysis process
        ImageCapture.Instance.ResetImageCapture();
    }

    /// <summary>
    /// This method hosts a series of calculations to determine the position 
    /// of the Bounding Box on the quad created in the real world
    /// by using the Bounding Box received back alongside the Best Prediction
    /// </summary>
    /// 
    /*
    public Vector3 CalculateBoundingBoxPosition(Bounds b, BoundingBox boundingBox)
    {
        Debug.Log($"BB: left {boundingBox.left}, top {boundingBox.top}, width {boundingBox.width}, height {boundingBox.height}");

        double centerFromLeft = boundingBox.left + (boundingBox.width / 2);
        double centerFromTop = boundingBox.top + (boundingBox.height / 2);
        Debug.Log($"BB CenterFromLeft {centerFromLeft}, CenterFromTop {centerFromTop}");

        double quadWidth = b.size.normalized.x;
        double quadHeight = b.size.normalized.y;
        Debug.Log($"Quad Width {b.size.normalized.x}, Quad Height {b.size.normalized.y}");

        double normalisedPos_X = (quadWidth * centerFromLeft) - (quadWidth / 2);
        double normalisedPos_Y = (quadHeight * centerFromTop) - (quadHeight / 2);

        return new Vector3((float)normalisedPos_X, (float)normalisedPos_Y, 0);
    }
    */
}
