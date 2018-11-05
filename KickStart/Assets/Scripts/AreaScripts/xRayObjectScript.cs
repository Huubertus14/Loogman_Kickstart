using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrFox;
using EnumStates;

namespace VrFox
{
    public class xRayObjectScript : MonoBehaviour
    {

        public bool isHit;
        int iterator;

        private MeshRenderer rend;

        private  float durationTo;
        private  float durationFrom;

        // Use this for initialization
        void Start()
        {
            //All init values
            durationFrom = GameManager.Instance.DurationFromImpulse;
            durationTo = GameManager.Instance.DurationToImpusle;

            rend = GetComponent<MeshRenderer>();
            isHit = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HitBySonar(Color.cyan, GameManager.Instance.Player.transform.position);
            }
        }

        public bool IsHit
        {
            get { return isHit; }
            set { value = isHit; }
        }

        public void HitBySonar(Color _color, Vector3 _playerPosition)
        {
            iterator = 0;

            if (!isHit)
            {
                rend.material.SetVector("_SonarWaveVector", _playerPosition);
                rend.material.SetColor("_SonarWaveColor", _color);
                StartCoroutine(FadeInOut());
            }
            else if (rend.material.GetColor("_SonarWaveColor") != _color)
            {
                Color temp = rend.material.GetColor("_SonarWaveColor") + _color;
                temp.r = Mathf.Clamp(temp.r, 0f, 1f);
                temp.g = Mathf.Clamp(temp.g, 0f, 1f);
                temp.b = Mathf.Clamp(temp.b, 0f, 1f);
                rend.material.SetColor("_SonarWaveColor", temp);
            }
        }
        
        IEnumerator FadeInOut()
        {
            isHit = true;
            rend.material.EnableKeyword("VISIBLE");
            for (int i = 0; i < durationTo; i++)
            {
                rend.material.SetFloat("_SonarStep", (float)i / durationTo);
                yield return new WaitForSeconds(1f / durationTo);
            }

            yield return null;

            rend.material.DisableKeyword("VISIBLE");
            while (iterator < durationFrom)
            {
                rend.material.SetFloat("_SonarStep", (float)iterator / durationFrom);

                iterator++;
                yield return new WaitForSeconds(1f / durationFrom);
            }

            isHit = false;

            yield return null;
        }
    }
}