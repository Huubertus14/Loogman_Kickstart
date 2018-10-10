using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Offscreen indicator manager VR.
/// Implementation of OffscreenIndicatorManager for VR. Tested only on Oculus Rift.
/// </summary>
namespace Greyman{
	public class OffScreenIndicatorManagerVR : OffScreenIndicatorManager {

		//public Indicator[] indicators = new Indicator[3];
		private GameObject indicatorsParentObj;
		public float cameraDistance = 5; //default distance
		public float radius = 2; //default radius
		public float indicatorScale = 0.1f;

		void Start (){

		}

		public void CreateIndicatorsParent(){
			//Create empty transform
			indicatorsParentObj = new GameObject();
			indicatorsParentObj.transform.SetParent(Camera.main.transform);
			indicatorsParentObj.transform.localPosition = new Vector3(0, 0, cameraDistance);
			indicatorsParentObj.transform.localScale = new Vector3(1f, 1f, 1f);
			indicatorsParentObj.transform.localEulerAngles = new Vector3(0, 0, 0);
			indicatorsParentObj.name = "IndicatorsParentObject";
		}

		void LateUpdate(){
			for(int i = 0; i < arrowIndicators.Count; i++){
				UpdateIndicatorPosition(arrowIndicators[i], i);
				arrowIndicators[i].UpdateEffects();
			}
		}

		public override void AddIndicator(Transform target, int indicatorID){
			if(indicatorID >= indicators.Length){
				Debug.LogError("Indicator ID not valid. Check Off Screen Indicator Indicators list.");
				return;
			}
			if (!ExistsIndicator(target)){
				ArrowIndicatorVR newArrowIndicator = new ArrowIndicatorVR();
				newArrowIndicator.target = target;
				newArrowIndicator.indicator = indicators[indicatorID];
				newArrowIndicator.indicatorID = indicatorID;
				newArrowIndicator.arrow = new GameObject();
				newArrowIndicator.arrow.transform.SetParent(indicatorsParentObj.transform);
				newArrowIndicator.arrow.name = "Indicator";
				newArrowIndicator.VR_scale = new Vector3(indicatorScale, indicatorScale, indicatorScale);
				newArrowIndicator.arrow.transform.localScale = newArrowIndicator.VR_scale;
				newArrowIndicator.arrow.AddComponent<SpriteRenderer>();
				newArrowIndicator.arrow.GetComponent<SpriteRenderer>().sprite = newArrowIndicator.indicator.offScreenSprite;
				newArrowIndicator.arrow.GetComponent<SpriteRenderer>().color = newArrowIndicator.indicator.offScreenColor;
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
			}/* else {
				Debug.LogWarning ("Target no longer exists: " + target.name);
			}*/
		}

		protected override void UpdateIndicatorPosition(ArrowIndicator arrowIndicator, int id = 0){
			Vector3 pCam = Camera.main.transform.position;
			Vector3 pPlane = indicatorsParentObj.transform.position;
			//change pPlane according to Z of arrow
			Ray zRay = new Ray(pPlane, pCam-pPlane);
			pPlane = zRay.GetPoint(-0.001f * id);
			//plane to draw things
			Plane plane = new Plane(Vector3.Normalize(pCam-pPlane), pPlane);
			//raycast line to target
			
			Vector3 pTarget = arrowIndicator.target.transform.position + arrowIndicator.indicator.targetOffset;
			Ray rToTarget = new Ray(pCam, pTarget-pCam);
			Vector3 hitPoint; //Point in plane where target hits raycasting camera.
			float distance;
			if (plane.Raycast(rToTarget,out distance)){
				hitPoint = rToTarget.GetPoint(distance);
				if(Vector3.Distance(pPlane, hitPoint) > radius){
					//offscreen
					arrowIndicator.onScreen = false;
					Ray rToArrow = new Ray(pPlane, hitPoint - pPlane);
					arrowIndicator.arrow.transform.position = rToArrow.GetPoint(radius);
				} else {
					//inscreen
					arrowIndicator.onScreen = true;
					arrowIndicator.arrow.transform.position = hitPoint;
				}
				//We do angle stuff in local space *GLOBAL SPACE
				Vector3 plPlane = indicatorsParentObj.transform.localPosition;
				Vector3 plHitPoint = arrowIndicator.arrow.transform.localPosition;
				// plPlane local pos is 0,0 but maybe we move the plane?
				//Apply Head rotation angle
				float angle = (90 - Camera.main.transform.localEulerAngles.z) * Mathf.Deg2Rad;

				if((arrowIndicator.onScreen && arrowIndicator.indicator.onScreenRotates) || (!arrowIndicator.onScreen && arrowIndicator.indicator.offScreenRotates)){
					angle = Mathf.Atan2(plHitPoint.y - plPlane.y, plHitPoint.x - plPlane.x);
				}
				arrowIndicator.arrow.transform.localEulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg - 90);

				//Debug some lines
				if(enableDebug){
					Utils.DrawPlane(Vector3.Normalize(pCam-pPlane), pPlane, radius);
					Debug.DrawRay(rToTarget.origin, rToTarget.direction);
					Debug.DrawLine(pCam, hitPoint, Color.white);
					Debug.DrawLine (hitPoint, pPlane, Color.magenta);
				}
			} else {
				rToTarget = new Ray(pTarget, pCam-pTarget);
				if (plane.Raycast(rToTarget,out distance)){
					hitPoint = rToTarget.GetPoint(distance);
					Ray rToArrow = new Ray(pPlane, hitPoint - pPlane);
					arrowIndicator.arrow.transform.position = rToArrow.GetPoint(-radius);
					arrowIndicator.onScreen = false;

					Vector3 plPlane = indicatorsParentObj.transform.localPosition;
					Vector3 plHitPoint = arrowIndicator.arrow.transform.localPosition;
					float angle = (90 - Camera.main.transform.localEulerAngles.z) * Mathf.Deg2Rad;
					if(arrowIndicator.indicator.offScreenRotates){
						angle = Mathf.Atan2(plHitPoint.y - plPlane.y, plHitPoint.x - plPlane.x);
					}
					arrowIndicator.arrow.transform.localEulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg - 90);
				} else {
					//target-cast is parallel to the plane, using the last indicator position is fine.
				}
			}
		}
	}
}