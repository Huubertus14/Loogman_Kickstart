// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using UnityEngine;
using EnumStates;

namespace HoloToolkit.Unity.InputModule
{
    /// <summary>
    /// The object cursor can switch between different game objects based on its state.
    /// It simply links the game object to set to active with its associated cursor state.
    /// </summary>
    public class ObjectCursor : Cursor
    {
        [Serializable]
        public struct ObjectCursorDatum
        {
            public string Name;
            public CursorStateEnum CursorState;
            public GameObject CursorObject;
        }

        [SerializeField]
        public ObjectCursorDatum[] CursorStateData;

        /// <summary>
        /// Sprite renderer to change.  If null find one in children
        /// </summary>
        public Transform ParentTransform;

        /// <summary>
        /// On enable look for a sprite renderer on children
        /// </summary>
        protected override void OnEnable()
        {
            if(ParentTransform == null)
            {
                ParentTransform = transform;
            }
            base.OnEnable();
        }

        /// <summary>
        /// Override OnCursorState change to set the correct animation
        /// state for the cursor
        /// </summary>
        /// <param name="state"></param>
        public override void OnCursorStateChange(CursorStateEnum state)
        {
            base.OnCursorStateChange(state);

            Debug.Log(state);

            if (state != CursorStateEnum.Contextual)
            {

                //Set here the current cursor state
                switch (state)
                {
                    case CursorStateEnum.None:
                        GameManager.Instance.SetHandState(HandStates.NotVisible);
                        break;
                    case CursorStateEnum.Observe:
                        GameManager.Instance.SetHandState(HandStates.NotVisible);
                        break;
                    case CursorStateEnum.ObserveHover:
                        break;
                    case CursorStateEnum.Interact:
                        GameManager.Instance.SetHandState(HandStates.Visible);
                        break;
                    case CursorStateEnum.InteractHover:
                        break;
                    case CursorStateEnum.Select:
                        GameManager.Instance.SetHandState(HandStates.Select);
                        break;
                    case CursorStateEnum.Release:
                        GameManager.Instance.SetHandState(HandStates.Release);
                        break;
                    case CursorStateEnum.Contextual:
                        GameManager.Instance.SetHandState(HandStates.NotVisible);
                        break;
                    default:
                        GameManager.Instance.SetHandState(HandStates.NotVisible);
                        break;
                }

                // Hide all children first
                for (int i = 0; i < ParentTransform.childCount; i++)
                {
                    ParentTransform.GetChild(i).gameObject.SetActive(false);
                }

                //Pnly show these things when you are in instructions
                if (GameManager.Instance.gameState == GameStates.Instructions)
                {
                    // Set active any that match the current state
                    for (int i = 0; i < CursorStateData.Length; i++)
                    {
                        if (CursorStateData[i].CursorState == state)
                        {
                            CursorStateData[i].CursorObject.SetActive(true);
                        }
                    }
                }
                else if (GameManager.Instance.gameState == GameStates.Playing)
                {
                    // Set active any that match the current state
                    for (int i = 0; i < CursorStateData.Length; i++)
                    {
                        if (CursorStateData[i].CursorState == state)
                        {
                            CursorStateData[i].CursorObject.SetActive(true);
                        }
                    }
                }
            }
        }
    }
}
