using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.UI;


public class SteamVRNotifierBase
{
    public string buttonName;
    public bool isLeft;
}

public class SteamVRInputVector2Notifier : SteamVRNotifierBase { public Vector2 value; }
public class SteamVRInputBoolNotifier : SteamVRNotifierBase { public bool value; }
public class SteamVRInputSingleNotifier : SteamVRNotifierBase { public float value; }

public class ViveControllerInputTest : MonoBehaviour
{
    public SteamVR_Input_Sources HandType;
    SteamVRInputBoolNotifier boolNotifier = new SteamVRInputBoolNotifier();
    SteamVRInputSingleNotifier singleNotifier = new SteamVRInputSingleNotifier();
    SteamVRInputVector2Notifier vector2Notifier = new SteamVRInputVector2Notifier();
    SteamVR_Input_Sources[] sources;
    SteamVR_ActionSet[] actionSets;


    void Start()
    {
        actionSets = SteamVR_Input.actionSets;
        if (actionSets == null) { actionSets = SteamVR_Input_References.instance.actionSetObjects; }

        //サンプル
        // MessageBroker.Default.Receive<SteamVRInputBoolNotifier>().Subscribe(x => Debug.Log("receive: " + x.buttonName + " value: " + x.value + " isLeft: " + x.isLeft));
        // MessageBroker.Default.Receive<SteamVRInputSingleNotifier>().Subscribe(x => Debug.Log("receive: " + x.buttonName + " value: " + x.value + " isLeft: " + x.isLeft));
        // MessageBroker.Default.Receive<SteamVRInputVector2Notifier>().Subscribe(x => Debug.Log("receive: " + x.buttonName + " value: " + x.value + " isLeft: " + x.isLeft));
    }
    void Update()
    {
        sources = SteamVR_Input_Source.GetUpdateSources();

        if (SteamVR_Input._default.inActions.GrabGrip.GetStateUp(HandType))
        {
            Debug.Log("GrabGrip.GetStateUp");
        }
        if (SteamVR_Input._default.inActions.GrabGrip.GetStateDown(HandType))
        {
            Debug.Log("GrabGrip.GetStateDown");
        }

        if (SteamVR_Input._default.inActions.GrabGrip.GetState(HandType))
        {
            Debug.Log("GrabGrip.GetState = true");
        }
        else
        {
            Debug.Log("GrabGrip.GetState = false");
        }
        for (int sourceIndex = 0; sourceIndex < sources.Length - 1; sourceIndex++)
        {
            var source = sources[sourceIndex];
            for (int actionSetIndex = 0; actionSetIndex < actionSets.Length; actionSetIndex++)
            {
                var actionSet = actionSets[actionSetIndex];
                for (int actionIndex = 0; actionIndex < actionSet.allActions.Length; actionIndex++)
                {
                    var action = actionSet.allActions[actionIndex];
                    if (action.actionSet == null || action.actionSet.IsActive() == false) { continue; }

                    //ボタン系
                    if (action is SteamVR_Action_Boolean)
                    {
                        var actionBoolean = (SteamVR_Action_Boolean)action;
                        boolNotifier.buttonName = action.GetShortName();
                        boolNotifier.value = actionBoolean.GetState(source);
                        boolNotifier.isLeft = sourceIndex == 0;
                        // MessageBroker.Default.Publish(boolNotifier);
                    }

                    //float系 Triggerとか
                    else if (action is SteamVR_Action_Single)
                    {
                        var actionSingle = (SteamVR_Action_Single)action;
                        singleNotifier.buttonName = action.GetShortName();
                        singleNotifier.value = actionSingle.GetAxis(source);
                        Debug.Log(singleNotifier.value);
                        singleNotifier.isLeft = sourceIndex == 0;
                        // MessageBroker.Default.Publish(singleNotifier);
                    }

                    //facePadとか
                    else if (action is SteamVR_Action_Vector2)
                    {
                        var actionVector2 = (SteamVR_Action_Vector2)action;
                        vector2Notifier.buttonName = action.GetShortName();
                        vector2Notifier.value = actionVector2.GetAxis(source);
                        Debug.Log(vector2Notifier.value);
                        vector2Notifier.isLeft = sourceIndex == 0;
                        // MessageBroker.Default.Publish(vector2Notifier);
                    }
                }

            }
        }
    }
}