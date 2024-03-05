using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UDebug = UnityEngine.Debug;

namespace GBG.GameToolkit.Unity.Editor.Python
{
    [CustomPropertyDrawer(typeof(PythonScripts.Content))]
    internal class PythonScriptsContentDrawerAttribute : PropertyDrawer
    {
        // Changes to PythonConsoleWindow.cs: 
        // 
        // + public static event Action<string> onNewOutput;
        // ---
        //   static public void AddToOutput(string input)
        //   {
        // +     if (!string.IsNullOrEmpty(input))
        // +     {
        // +         onNewOutput?.Invoke(input);
        // +     }
        //       
        //       if (s_window)
        //       {
        //           s_window.InternalAddToOutput(input);
        //       }
        //   }
        // See also:
        //  https://forum.unity.com/threads/optimization-suggestion-delay-the-initialization-of-python-scripting-until-it-is-first-called.1496054/


        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            PropertyField gui = new PropertyField(property);
            gui.schedule.Execute(AddExecuteButton);
            //EditorApplication.delayCall += AddExecuteButton;

            return gui;

            void AddExecuteButton()
            {
                Button execButton = new Button(Execute)
                {
                    name = "ExecuteButton",
                    text = "Execute",
                    style =
                        {
                            alignSelf = Align.FlexEnd,
                            height = 17,
                            width = 60,
                            position = Position.Absolute,
                        }
                };
                gui.Add(execButton);
            }

            void Execute()
            {
#if GBG_PY_EX
                    if (!_outputListened)
                    {
                        _outputListened = true;
                        PythonConsoleWindow.onNewOutput += OnNewOutput;
                    }

                    string script = property.FindPropertyRelative(nameof(Content.script)).stringValue;
                    UnityEditor.Scripting.Python.PythonRunner.RunString(script);
#else
                UDebug.LogError("Run python scrip failed.");
                EditorUtility.DisplayDialog("Error", "Please install package 'com.unity.scripting.python' and add scripting define symbol 'GBG_PY_EX' to project settings.", "Ok");
#endif
            }
        }


        private static bool _outputListened;

        private static void OnNewOutput(string output)
        {
            if (output.Equals("\n"))
            {
                return;
            }

            UDebug.Log(output);
        }
    }
}