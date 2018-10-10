Off Screen Indicator Readme File


Package Content

DemoScenes
-----------
DemoScript.cs - Example code for demos, show how easily you can add a offscreen arrow in realtime via code.

FPSCanvasTest.unity - Demo Scene, where you can see how it works OffScreenIndicator with a FirstPersonCharacter Standard Asset, and displaying the arrows in Canvas.

FPSVRTest.unity - Demo Scene, using VR (Tested in Oculus Rift DK2) instead of Canvas.

ThirdPersonTopViewTest.unity - Demo Scene, using ThirdPersonCharacter with a cenital top view, and displaying the arrows in Canvas.


Docs
----
FAQ.pdf - Frecuently Asqued Questions with the common problems when implementing OffScreen Indicator


Scripts
-------
Core classes.


Scripts / FirstPersonVRCharacter
--------------------------------
You can remove this folder if you are not using VR. In this folder you can find the FirstPersonCharacter Prefab modified to work with VR, and display arrows in world space in a given distance to the viewer (easily configurable). The MouseLook is modified to change only character rotation, and pov camera will be moved only with the VR device.

Scripts / Helpers
-----------------
Convenience methods for debugging.



Sprites
-------
Example sprites used in the demo scenes.

