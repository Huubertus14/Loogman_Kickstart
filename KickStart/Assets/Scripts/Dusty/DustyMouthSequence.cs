using UnityEngine;

public class DustyMouthSequence : MonoBehaviour
{
    private Texture[] text01;

    public Texture[] GetText01
    {
        get
        {
            if (text01 == null || text01.Length < 1)
            {
                text01 = Resources.LoadAll<Texture>("Dusty/DustyMouthSequence/01");
            }
            return text01;
        }
    }

}
