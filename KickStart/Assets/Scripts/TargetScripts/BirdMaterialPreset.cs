using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BirdMaterialPreset {

    [SerializeField]
    private Material body, beek;

    public BirdMaterialPreset(Material _body, Material _beek) {
        body = _body;
        beek = _beek;
	}

    public Material GetBeek
    {
        get { return beek; }
    }

    public Material GetBody
    {
        get { return body; }
    }
}
