using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Offscreen indicator manager VR.
/// Implementation of OffscreenIndicatorManager for Canvas.
/// </summary>
namespace Greyman{
	public class OffScreenIndicatorManagerCanvas : OffScreenIndicatorManager {
		
		//public Indicator[] indicators = new Indicator[3];
		public GameObject indicatorsParentObj;
		public float cameraDistance = 5; //default distance
		public int circleRadius = 100;
		public int border = 10;
		public int indicatorSize = 100;
		private float realBorder;
		private Vector2 referenceResolution;
		private float screenScaleX;
		private float screenScaleY;
		private bool screenScaled =false;
		
		void Start (){
			//Create empty transform
			if(indicatorsParentObj == null || indicatorsParentObj.GetComponent<Canvas>() == null){
				Debug.LogError("OffScreenIndicator Canvas field requieres a Canvas GameObject");
				Debug.Break();
			}

			if(indicatorsParentObj.GetComponent<Canvas>().renderMode == RenderMode.WorldSpace){

			}
			//Get Reference resolution to obtain scale
			if(indicatorsParentObj.GetComponent<CanvasScaler>().uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize){
				referenceResolution = indicatorsParentObj.GetComponent<CanvasScaler>().referenceResolution;
				Vector2 screenResolution = new Vector2(Screen.width, Screen.height);
				screenScaleX = screenResolution.x / referenceResolution.x;
				screenScaleY = screenResolution.y / referenceResolution.y;
				screenScaled = true;
				screenScaled = false;

				Debug.Log("ReferenceResolution = " + referenceResolution.ToString());
				Debug.Log("ScreenResolution = " + screenResolution.ToString());
				Debug.Log("ScreenScaleX = " + screenScaleX.ToString());
				Debug.Log("ScreenScaleY = " + screenScaleY.ToString());
			} else {
				screenScaled = false;
			}
			//indicatorsSize depends on screen scale

			if(screenScaled){
				indicatorSize = Mathf.RoundToInt(indicatorSize * screenScaleX);
			}
			realBorder = (indicatorSize/2f) + border;
		}
		
		void LateUpdate(){
			//update enemies arrows
			foreach(ArrowIndicator arrowIndicator in arrowIndicators){
				UpdateIndicatorPosition(arrowIndicator);
				arrowIndicator.UpdateEffects();
			}
		}
		
		public override void AddIndicator(Transform target, int indicatorID){
			if(indicatorID >= indicators.Length){
				Debug.LogError("Indicator ID not valid. Check Off Screen Indicator Indicators list.");
				return;
			}
			if (!ExistsIndicator(target)){
				ArrowIndicatorCanvas newArrowIndicator = new ArrowIndicatorCanvas();
				newArrowIndicator.target = target;
				newArrowIndicator.arrow = new GameObject();
				newArrowIndicator.arrow.transform.SetParent(indicatorsParentObj.transform);
				newArrowIndicator.arrow.name = "Indicator";
				newArrowIndicator.arrow.transform.SetAsFirstSibling();
				newArrowIndicator.arrow.transform.localScale = Vector3.one;
				newArrowIndicator.arrow.AddComponent<Image>();
				newArrowIndicator.indicator = indicators[indicatorID];
				newArrowIndicator.arrow.GetComponent<Image>().sprite = newArrowIndicator.indicator.offScreenSprite;
				newArrowIndicator.arrow.GetComponent<RectTransform>().sizeDelta = new Vector2(indicatorSize, indicatorSize);
				newArrowIndicator.arrow.GetComponent<Image>().color = newArrowIndicator.indicator.offScreenColor;
				if(!newArrowIndicator.indicator.showOffScreen){
					newArrowIndicator.arrow.SetActive(false);
				}
				newArrowIndicator.onScreen = false;
				arrowIndicators.Add(newArrowIndicator);
			} else {
				Debug.LogWarning ("Target already added: " + target.name);
			}
		}

		public override void RemoveIndicator(Transform target){
			if(ExistsIndicator(target)){
				ArrowIndicator oldArrowTarget = arrowIndicators.Find(x=>x.target == target);
				int id = arrowIndicators.FindIndex(x=>x.target == target);
				arrowIndicators.RemoveAt(id);
				GameObject.Destroy(oldArrowTarget.arrow);
				ArrowIndicator.Destroy(oldArrowTarget);
			} else {
				Debug.LogWarning ("Target no longer exists: " + target.name);
			}
		}
		
		protected override void UpdateIndicatorPosition(ArrowIndicator arrowIndicator, int id = 0){
			Vector3 v2DPos = Camera.main.WorldToScreenPoint(arrowIndicator.target.localPosition + arrowIndicator.indicator.targetOffset);
			float angle;
			bool behindCamera;

			Vector3 heading = arrowIndicator.target.position - Camera.main.transform.position;
			behindCamera = (Vector3.Dot(Camera.main.transform.forward, heading) < 0);

			if(v2DPos.x > Screen.width - realBorder || v2DPos.x < realBorder || v2DPos.y > Screen.height - realBorder || v2DPos.y < realBorder || behindCamera){
				//Debug.Log ("OUT OF SCREEN");
				arrowIndicator.onScreen = false;
				//Cut position on the sides
				angle = Mathf.Atan2(v2DPos.y - (Screen.height/2), v2DPos.x - (Screen.width/2));
				float xCut, yCut;
				if(v2DPos.x - Screen.width/2 > 0){
					//Right side
					xCut = Screen.width/2 - realBorder;
					yCut = xCut * Mathf.Tan(angle);
				} else {
					//Left side
					xCut = -Screen.width/2 + realBorder;
					yCut = xCut * Mathf.Tan(angle);
				}
				//Check cut position up and down
				if(yCut > Screen.height/2 - realBorder){
					//Up
					yCut = Screen.height/2 - realBorder;
					xCut = yCut / Mathf.Tan(angle);
				}
				if(yCut < -Screen.height/2 + realBorder){
					//Down
					yCut = -Screen.height/2 + realBorder;
					xCut = yCut / Mathf.Tan(angle);
				}
				if(behindCamera){
					xCut = -xCut;
					yCut = -yCut;
				}
				if(screenScaled){
					xCut /= screenScaleX;
					yCut /= screenScaleY;
				}
				arrowIndicator.arrow.transform.localPosition = new Vector3(xCut, yCut, 0);
			} else {
				//Debug.Log ("INSIDE OF SCREEN");
				arrowIndicator.onScreen = true;
				float xScaled = v2DPos.x - (Screen.width/2);
				float yScaled = v2DPos.y - (Screen.height/2);
				if(screenScaled){
					xScaled /= screenScaleX;
					yScaled /= screenScaleY;
				}
				arrowIndicator.arrow.transform.localPosition = new Vector3(xScaled, yScaled, 0);
			}

			//rotatearrow
			if((arrowIndicator.onScreen && arrowIndicator.indicator.onScreenRotates) || (!arrowIndicator.onScreen && arrowIndicator.indicator.offScreenRotates)){
				if(behindCamera){
					angle = Mathf.Atan2(-(v2DPos.y - (Screen.height/2)), -(v2DPos.x - (Screen.width/2)));
				} else {
					angle = Mathf.Atan2(v2DPos.y - (Screen.height/2), v2DPos.x - (Screen.width/2));
				}
			} else {
				angle = 90 * Mathf.Deg2Rad;
			}
			arrowIndicator.arrow.transform.localEulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg - 90);
		}
	}
}