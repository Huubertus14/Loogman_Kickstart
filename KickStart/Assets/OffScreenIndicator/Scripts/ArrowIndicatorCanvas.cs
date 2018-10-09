using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Arrow indicator Canvas.
/// ArrowIndicator implementation for Canvas
/// </summary>
namespace Greyman{
	public class ArrowIndicatorCanvas : ArrowIndicator {

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
								arrow.GetComponent<Image>().sprite = indicator.onScreenSprite;
								arrow.GetComponent<Image>().color = indicator.onScreenColor;
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
								arrow.GetComponent<Image>().sprite = indicator.offScreenSprite;
								arrow.GetComponent<Image>().color = indicator.offScreenColor;
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
					fadingDownValues();
				} else if(elapsedTime < indicator.transitionDuration * 2){
					//fading up
					if(!fadingUp){
						//flag!
						arrow.GetComponent<Image>().sprite = fadingToOff ? indicator.offScreenSprite : indicator.onScreenSprite;
						arrow.GetComponent<Image>().color = fadingToOff ? indicator.offScreenColor : indicator.onScreenColor;
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
						fadingUpValues();
					}
				} else {
					//check flag settled
					if(!fadingUp){
						//It can happens when duration values are near to 0 or in a possible game lag
						arrow.GetComponent<Image>().sprite = fadingToOff ? indicator.offScreenSprite : indicator.onScreenSprite;
						arrow.GetComponent<Image>().color = fadingToOff ? indicator.offScreenColor : indicator.onScreenColor;
						_onScreen = _onScreenNextValue;
						fadingUp = true;
					}
					//fadings end
					endFadingValues();
					fadingToOn = false;
					fadingToOff = false;
				}
			}
		}

		/// <summary>
		/// Fadings down.
		/// Just care about alpha or scale
		/// </summary>
		private void fadingDownValues(){
			if(indicator.transition == Indicator.Transition.Fading){
				//alpha stuff
				if(onScreen){
					transColor = indicator.onScreenColor;
				} else {
					transColor = indicator.offScreenColor;
				}
				arrow.GetComponent<Image>().color = Color32.Lerp (transColor,
				                                                  new Color32(System.Convert.ToByte(transColor.r*255),
				            													System.Convert.ToByte(transColor.g*255),
				            													System.Convert.ToByte(transColor.b*255), 0),
				                                                  elapsedTime / indicator.transitionDuration);
			}
			if(indicator.transition == Indicator.Transition.Scaling){
				//scale stuff
				arrow.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, elapsedTime / indicator.transitionDuration);
			}
		}

		/// <summary>
		/// Fadings up.
		/// Just care about alpha or scale
		/// </summary>
		private void fadingUpValues(){
			if(indicator.transition == Indicator.Transition.Fading){
				//alpha stuff
				if(onScreen){
					transColor = indicator.onScreenColor;
				} else {
					transColor = indicator.offScreenColor;
				}
				arrow.GetComponent<Image>().color = Color32.Lerp (new Color32(System.Convert.ToByte(transColor.r*255),
				                                                              System.Convert.ToByte(transColor.g*255),
				                                                              System.Convert.ToByte(transColor.b*255), 0),
				                                                  transColor,
				                                                  (elapsedTime - indicator.transitionDuration) / indicator.transitionDuration);
			}
			if(indicator.transition == Indicator.Transition.Scaling){
				//scale stuff
				arrow.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, (elapsedTime - indicator.transitionDuration) / indicator.transitionDuration);
			}
		}

		/// <summary>
		/// Ends the fadings.
		/// Fadings finished. Set current color
		/// </summary>
		private void endFadingValues(){
			if(indicator.transition == Indicator.Transition.Fading){
				//alpha stuff
				if(onScreen){//shouldn't be neccesary, but game lag?
					transColor = indicator.onScreenColor;
				} else {
					transColor = indicator.offScreenColor;
				}
				arrow.GetComponent<Image>().color = transColor;
			}
			if(indicator.transition == Indicator.Transition.Scaling){
				//scale stuff
				arrow.transform.localScale = Vector3.one;
			}
		}
	}
}