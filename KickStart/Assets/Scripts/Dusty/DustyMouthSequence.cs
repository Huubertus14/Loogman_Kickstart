using UnityEngine;

public class DustyMouthSequence :MonoBehaviour{

    //Load all arrays
    private void Start()
    {
        text01 = Resources.LoadAll<Texture>("Dusty/DustyMouthSequence/01");
        //text02 = Resources.LoadAll<Texture>("Dusty/DustyMouthSequence/02");
       // text04 = Resources.LoadAll<Texture>("Dusty/DustyMouthSequence/04");
       // text05 = Resources.LoadAll<Texture>("Dusty/DustyMouthSequence/05");
       // text06 = Resources.LoadAll<Texture>("Dusty/DustyMouthSequence/06");
       // text07 = Resources.LoadAll<Texture>("Dusty/DustyMouthSequence/07");
    }
    
    private Texture[] text01;

    public Texture[] GetText01 => text01;

}
