using UnityEngine;
using UnityEngine.InputSystem;

namespace DebugMenuUtility
{
    public class DebugMenuConfiguration : ScriptableObject
    {
        public enum Alignment
        {
            Left,
            Center,
            Right
        }

        public Alignment alignment = Alignment.Right;

        [Header("Size")]
        public int width = 400;
        public int lineHeight = 24;

        [Header("Fonts")]
        public Font customFont;
        public int fontSize = 12;

        [Header("Colors")]
        public Color headerColor = new Color(0.2f, 0, 0, 0.9f);
        public Color bgColor = new Color(0.1f, 0.1f, 0.1f, 0.9f);
        public Color selectedColor = new Color(1, 1, 1, 0.1f);
        public Color textColor = new Color(.8f, .8f, .8f, 1f);
        public Color headerTextColor = Color.white;
        public Color selectedTextColor = Color.white;

        [Header("Input")]
        public InputAction toggle;
        public InputAction up;
        public InputAction down;
        public InputAction left;
        public InputAction right;
        public InputAction enter;

        public void Initialize()
        {
            Debug.Log("Initialize DebugMenuConfiguration");

            toggle = new InputAction("Toggle", InputActionType.Button);
            toggle.AddBinding("<Gamepad>/Select");
            toggle.AddBinding("<Keyboard>/F12");

            up = new InputAction("Up", InputActionType.Button);
            up.AddBinding("<Gamepad>/D-Pad/Up");
            up.AddBinding("<Keyboard>/UpArrow");

            down = new InputAction("Down", InputActionType.Button);
            down.AddBinding("<Gamepad>/D-Pad/Down");
            down.AddBinding("<Keyboard>/DownArrow");

            left = new InputAction("Left", InputActionType.Button);
            left.AddBinding("<Gamepad>/D-Pad/Left");
            left.AddBinding("<Keyboard>/LeftArrow");

            right = new InputAction("Right", InputActionType.Button);
            right.AddBinding("<Gamepad>/D-Pad/Right");
            right.AddBinding("<Keyboard>/RightArrow");

            enter = new InputAction("Enter", InputActionType.Button);
            enter.AddBinding("<Gamepad>/Button South");
            enter.AddBinding("<Keyboard>/Enter");
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Tools/Debug Menu/Create DebugMenu Configuration Asset")]
        static void CreateAsset()
        {
            DebugMenuConfiguration config = CreateInstance<DebugMenuConfiguration>();
            UnityEditor.AssetDatabase.CreateAsset(config, "Assets/Resources/DebugMenuConfiguration.asset");
            UnityEditor.EditorGUIUtility.PingObject(config);
            UnityEditor.Selection.activeObject = config;
            config.Initialize();
        }
#endif
    }
}
