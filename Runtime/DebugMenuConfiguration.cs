using UnityEngine;
using UnityEngine.InputSystem;

namespace DebugMenuUtility
{
    public class DebugMenuConfiguration : ScriptableObject
    {
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

            toggle = new InputAction();
            toggle.AddBinding(Gamepad.current.selectButton);
            toggle.AddBinding(Keyboard.current.f12Key);

            up = new InputAction();
            up.AddBinding(Gamepad.current.dpad.up);
            up.AddBinding(Keyboard.current.upArrowKey);

            down = new InputAction();
            down.AddBinding(Gamepad.current.dpad.down);
            down.AddBinding(Keyboard.current.downArrowKey);

            left = new InputAction();
            left.AddBinding(Gamepad.current.dpad.left);
            left.AddBinding(Keyboard.current.leftArrowKey);

            right = new InputAction();
            right.AddBinding(Gamepad.current.dpad.right);
            right.AddBinding(Keyboard.current.rightArrowKey);

            enter = new InputAction();
            enter.AddBinding(Gamepad.current.buttonSouth);
            enter.AddBinding(Keyboard.current.enterKey);
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
