using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Linq;

public class AnimatorValidator : EditorWindow
{
    [MenuItem("Tools/Validate Animator Controllers")]
    public static void ShowWindow()
    {
        GetWindow<AnimatorValidator>("Animator Controller Validator");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Validate All Animator Controllers"))
        {
            ValidateAllAnimatorControllers();
        }
    }

    private void ValidateAllAnimatorControllers()
    {
        string[] guids = AssetDatabase.FindAssets("t:AnimatorController");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);
            if (controller != null)
            {
                ValidateAnimatorController(controller, path);
            }
            else
            {
                Debug.LogError($"Failed to load Animator Controller at path {path}");
            }
        }

        Debug.Log("Validation complete.");
    }

    private void ValidateAnimatorController(AnimatorController controller, string path)
    {
        foreach (AnimatorControllerLayer layer in controller.layers)
        {
            foreach (ChildAnimatorState state in layer.stateMachine.states)
            {
                foreach (AnimatorStateTransition transition in state.state.transitions)
                {
                    foreach (AnimatorCondition condition in transition.conditions)
                    {
                        if (controller.parameters.All(p => p.name != condition.parameter))
                        {
                            Debug.LogError($"Invalid condition parameter '{condition.parameter}' in transition from state '{state.state.name}' in layer '{layer.name}' of Animator Controller '{controller.name}' at path '{path}'");
                        }
                    }
                }
            }
        }
    }
}
