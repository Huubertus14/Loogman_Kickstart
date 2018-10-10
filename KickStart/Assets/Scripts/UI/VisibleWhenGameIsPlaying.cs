using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrFox;

namespace VrFox
{
    public class VisibleWhenGameIsPlaying : MonoBehaviour
    {
    
        RectTransform rect;
        Vector3 orginScale;
        UIWiggle wig;

        private void Start()
        {
            wig = GetComponent<UIWiggle>();
            rect = GetComponent<RectTransform>();
            orginScale = rect.localScale;

        }

        private void Update()
        {
            if (GameManager.Instance.GameStarted)
            {
                rect.localScale = orginScale;
                wig.enabled = true;
            }
            else
            {
                rect.localScale = Vector3.zero;
                wig.enabled = false;
            }
        }
    }
}