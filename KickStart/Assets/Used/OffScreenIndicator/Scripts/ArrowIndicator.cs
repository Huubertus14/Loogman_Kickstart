using UnityEngine;
using System.Collections;

/// <summary>
/// Arrow indicator.
/// Stores object representing indicators, determine which indicator to show based on onScreen value and effects that should be applied
/// </summary>
namespace Greyman{
	public abstract class ArrowIndicator : Object {
		public Indicator indicator;
		public int indicatorID;
		public Transform target;
		public GameObject arrow;
		public Vector3 VR_scale;
		protected bool _onScreen;
		protected bool _onScreenNextValue; //convenience field for onScreen transitions
		protected Color transColor; // cache color used during transitions
		/// <summary>
		/// <para>fadingToOn/Off. Magical Concept!.</para>
		/// <para>Fading to somewhere means:</para>
		/// <para>1.fading out the actual sprite in alpha or scale to transparent or quark-size. fadingup=false</para>
		/// <para>2.Setting the enabled status as needed by indicator configuration.set fadingup</para>
		/// <para>3.fading in the actual sprite in alpha or scale to opaque or normal-size.fadingup=true</para>
		/// </summary>
		protected bool fadingToOn = false;
		protected bool fadingToOff = false;
		protected bool fadingUp = false; //flag
		protected float timeStartLerp;
		protected float elapsedTime;
		protected float lerpAmount; //linear interpolation value for fades and scales
		public abstract bool onScreen {get; set;}
		public abstract void UpdateEffects();
	}
}