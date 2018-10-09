using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Off screen indicator manager.
/// Abstract class, manages arrow indicators, where should be displayed
/// </summary>
namespace Greyman{
	public abstract class OffScreenIndicatorManager : MonoBehaviour {

		public bool enableDebug;

		protected List<ArrowIndicator> arrowIndicators;

		public Indicator[] indicators;
		public abstract void AddIndicator(Transform target, int indicatorID);
		public abstract void RemoveIndicator(Transform target);
		protected abstract void UpdateIndicatorPosition(ArrowIndicator arrowIndicator, int id = 0);

		void Awake(){
			arrowIndicators = new List<ArrowIndicator>();
		}

		protected bool ExistsIndicator(Transform target){
			bool exists = false;
			foreach(ArrowIndicator arrowIndicator in arrowIndicators){
				if(arrowIndicator.target == target){
					exists = true;
				}
			}
			return exists;
		}

		public void CheckFields(){
			foreach( Indicator indicator in indicators){
				if(indicator.onScreenSprite == null){
					indicator.showOnScreen = false;
				} else {
					indicator.showOnScreen = true;
				}
				//Who may be interested in an OffScreen package that doesn't show offscreen? You never know...
				if(indicator.offScreenSprite == null){
					indicator.showOffScreen = false;
				} else {
					indicator.showOffScreen = true;
				}
				if(!indicator.showOnScreen && !indicator.showOffScreen){
					Debug.LogError("You should add at least one Sprite for offScreen or onScreen. Otherwise this Indicator is useless.");
					Debug.Break();
				}
			}
		}
	}
}