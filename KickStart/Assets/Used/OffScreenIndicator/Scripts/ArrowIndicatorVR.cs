using UnityEngine;
using System.Collections;

/// <summary>
/// Arrow indicator VR.
/// ArrowIndicator implementation for Oculus Rift
/// </summary>
namespace Greyman{
	public class ArrowIndicatorVR : ArrowIndicator {
		
		public override bool onScreen{
			get{
				return _onScreen;
			}
			
			set{
				if(_onScreenNextValue != value){
					_onScreenNextValue = value;
					if(value){
						if(indicator.transition == Indicator.Transition.None){
							_onScreen = value;
							if(indicator.showOnScreen){
								arrow.SetActive(true);
								arrow.GetComponent<SpriteRenderer>().sprite = indicator.onScreenSprite;
								arrow.GetComponent<SpriteRenderer>().color = indicator.onScreenColor;
							} else {
								arrow.SetActive(false);
							}
						} else {
							fadingToOn = true;
							fadingToOff = false;
						}
					} else {
						//no transition effects
						if(indicator.transition == Indicator.Transition.None){
							_onScreen = value;
							if(indicator.showOffScreen){
								arrow.SetActive(true);
								arrow.GetComponent<SpriteRenderer>().sprite = indicator.offScreenSprite;
								arrow.GetComponent<SpriteRenderer>().color = indicator.offScreenColor;
							} else {
								arrow.SetActive(false);
							}
						} else {
							fadingToOn = false;
							fadingToOff = true;
						}
					}
					timeStartLerp = Time.time;
					fadingUp = false;
				}
			}
		}
		
		public override void UpdateEffects(){
			if(fadingToOn || fadingToOff){
				elapsedTime = Time.time - timeStartLerp;
				//tweak elapsedTime when coming from not showable state.
				if((fadingToOn && !indicator.showOffScreen) || (fadingToOff && !indicator.showOnScreen)){
					elapsedTime += indicator.transitionDuration;
				}
				//
				if(elapsedTime < indicator.transitionDuration){
					//fading down
					FadingDownValues();
				} else if(elapsedTime < indicator.transitionDuration * 2){
					//fading up
					if(!fadingUp){
						//flag!
						arrow.GetComponent<SpriteRenderer>().sprite = fadingToOff ? indicator.offScreenSprite : indicator.onScreenSprite;
						arrow.GetComponent<SpriteRenderer>().color = fadingToOff ? indicator.offScreenColor : indicator.onScreenColor;
						_onScreen = _onScreenNextValue;
						fadingUp = true;
					}
					//check showable
					if((onScreen && !indicator.showOnScreen) || (!onScreen && !indicator.showOffScreen)){
						arrow.SetActive(false);
						fadingToOn = false;
						fadingToOff = false;
					} else {
						arrow.SetActive(true);
						//drawfadingup
						FadingUpValues();
					}
				} else {
					//check flag settled
					if(!fadingUp){
						//It can happens when duration values are near to 0 or in a possible game lag
						arrow.GetComponent<SpriteRenderer>().sprite = fadingToOff ? indicator.offScreenSprite : indicator.onScreenSprite;
						arrow.GetComponent<SpriteRenderer>().color = fadingToOff ? indicator.offScreenColor : indicator.onScreenColor;
						_onScreen = _onScreenNextValue;
						fadingUp = true;
					}
					//fadings end
					EndFadingValues();
					fadingToOn = false;
					fadingToOff = false;
				}
			}
		}
		
		/// <summary>
		/// Fadings down.
		/// Just care about alpha or scale
		/// </summary>
		private void FadingDownValues(){
			if(indicator.transition == Indicator.Transition.Fading){
				//alpha stuff
				if(onScreen){
					transColor = indicator.onScreenColor;
				} else {
					transColor = indicator.offScreenColor;
				}
				arrow.GetComponent<SpriteRenderer>().color = Color32.Lerp (transColor,
				                                                           new Color32(System.Convert.ToByte(transColor.r*255),
				            														   System.Convert.ToByte(transColor.g*255),
				            														   System.Convert.ToByte(transColor.b*255), 0),
				                                                           elapsedTime / indicator.transitionDuration);
			}
			if(indicator.transition == Indicator.Transition.Scaling){
				//scale stuff
				arrow.transform.localScale = Vector3.Lerp(VR_scale, Vector3.zero, elapsedTime / indicator.transitionDuration);
			}
		}
		
		/// <summary>
		/// Fadings up.
		/// Just care about alpha or scale
		/// </summary>
		private void FadingUpValues(){
			if(indicator.transition == Indicator.Transition.Fading){
				//alpha stuff
				if(onScreen){
					transColor = indicator.onScreenColor;
				} else {
					transColor = indicator.offScreenColor;
				}
				arrow.GetComponent<SpriteRenderer>().color = Color32.Lerp (new Color32(System.Convert.ToByte(transColor.r*255),
				            															System.Convert.ToByte(transColor.g*255),
				            															System.Convert.ToByte(transColor.b*255), 0),
				                                                           transColor,
				                                                           (elapsedTime - indicator.transitionDuration) / indicator.transitionDuration);
			}
			if(indicator.transition == Indicator.Transition.Scaling){
				//scale stuff
				arrow.transform.localScale = Vector3.Lerp(Vector3.zero, VR_scale, (elapsedTime - indicator.transitionDuration) / indicator.transitionDuration);
			}
		}
		
		/// <summary>
		/// Ends the fadings.
		/// Fadings finished. Set current color
		/// </summary>
		private void EndFadingValues(){
			if(indicator.transition == Indicator.Transition.Fading){
				//alpha stuff
				if(onScreen){//shouldn't be neccesary, but game lag?
					transColor = indicator.onScreenColor;
				} else {
					transColor = indicator.offScreenColor;
				}
				arrow.GetComponent<SpriteRenderer>().color = transColor;
			}
			if(indicator.transition == Indicator.Transition.Scaling){
				//scale stuff
				arrow.transform.localScale = VR_scale;
			}
		}
	}
}