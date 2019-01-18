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
    private Texture[] text02;
    private Texture[] text03;
    private Texture[] text04;
    private Texture[] text05;
    private Texture[] text06;
    private Texture[] text07;

    public Texture[] GetText01 => text01;
    public Texture[] GetText02 => text02;
    public Texture[] GetText03 => text03;
    public Texture[] GetText04 => text04;
    public Texture[] GetText05 => text05;
    public Texture[] GetText06 => text06;
    public Texture[] GetText07 => text07;

}
