using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DebugMenuUtility
{
    public class DebugMenu : MonoBehaviour
    {
        public static DebugMenu instance;

        DebugMenuConfiguration config;
        Styles styles;
        bool visible;

        string currentPath;
        string menuName;
        int selected = 0;

        static Dictionary<string, List<DebugMenuItem>> allItems;

        DebugMenuItem[] currentItems;
        string[] subFolders;

        public static event Action onDebugMenuShown;
        public static event Action onDebugMenuHidden;


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            allItems = new Dictionary<string, List<DebugMenuItem>>();
            allItems.Add(string.Empty, new List<DebugMenuItem>());

            Type[] kAllMenuItems = GetConcreteTypes<DebugMenuItem>();

            foreach (var type in kAllMenuItems)
            {
                DebugMenuItemAttribute attribute = type.GetCustomAttribute<DebugMenuItemAttribute>();

                // Only add those with attribute
                if (attribute == null)
                    continue;

                var item = Activator.CreateInstance(type) as DebugMenuItem;
                if (!allItems.ContainsKey(attribute.path))
                    allItems.Add(attribute.path, new List<DebugMenuItem>());

                allItems[attribute.path].Add(item);
            }

            var go = new GameObject("DebugMenu");
            var menu = go.AddComponent<DebugMenu>();
            DontDestroyOnLoad(go);
            instance = menu;
        }

        private void OnEnable()
        {
            instance = this;
            config = Resources.Load("DebugMenuConfiguration") as DebugMenuConfiguration;

            // Fallback to defaults
            if (config == null)
                config = ScriptableObject.CreateInstance<DebugMenuConfiguration>();

            styles = new Styles(config);

            ToggleAction(config.toggle, true);
            ToggleAction(config.up, true);
            ToggleAction(config.down, true);
            ToggleAction(config.left, true);
            ToggleAction(config.right, true);
            ToggleAction(config.enter, true);
        }

        private void OnDisable()
        {
            ToggleAction(config.toggle, false);
            ToggleAction(config.up, false);
            ToggleAction(config.down, false);
            ToggleAction(config.left, false);
            ToggleAction(config.right, false);
            ToggleAction(config.enter, false);
        }

        void ToggleAction(InputAction action, bool enable)
        {
            if (action == null)
                return;
            if (enable && !action.enabled)
                action.Enable();
            else if (!enable && action.enabled)
                action.Disable();
        }

        private void Update()
        {
            if (config.toggle.WasPressedThisFrame())
            {
                visible = !visible;
                selected = 0;
                if (visible)
                    onDebugMenuShown.Invoke();
                else
                    onDebugMenuHidden.Invoke();

            }

            if (visible)
            {
                if (config.down.WasPressedThisFrame())
                {
                    selected++;
                    if (selected >= currentItems.Length)
                        selected = 0;
                }

                if (config.up.WasPressedThisFrame())
                {
                    selected--;
                    if (selected < 0)
                        selected = currentItems.Length - 1;
                }

                if (config.enter.WasPressedThisFrame())
                {
                    currentItems[selected].OnValidate?.Invoke();
                }

                if (config.left.WasPressedThisFrame())
                {
                    currentItems[selected].OnLeft?.Invoke();
                }

                if (config.right.WasPressedThisFrame())
                {
                    currentItems[selected].OnRight?.Invoke();
                }
            }
        }

        private void OnGUI()
        {
            if (!visible || config == null)
                return;

            if (currentItems == null)
                LoadMenu("");

            int count = currentItems.Length;

            GUI.color = config.headerColor;
            Rect r = new Rect(64, 64, config.width, config.lineHeight);
            GUI.DrawTexture(r, Texture2D.whiteTexture);
            GUI.color = config.headerTextColor;
            GUI.Label(r, menuName, styles.header);

            r = new Rect(64, 64 + config.lineHeight, config.width, config.lineHeight * count);
            GUI.color = config.bgColor;
            GUI.DrawTexture(r, Texture2D.whiteTexture);

            for (int i = 0; i < count; i++)
            {
                bool isSelected = (i == selected);
                r = new Rect(64, 64 + (config.lineHeight * (i + 1)), config.width, config.lineHeight);
                if (isSelected)
                {
                    GUI.color = config.selectedColor;
                    GUI.DrawTexture(r, Texture2D.whiteTexture);
                }

                DebugMenuItem mi = currentItems[i];

                GUI.color = isSelected ? config.selectedTextColor : config.textColor;
                GUI.Label(r, mi.label, styles.label);

                string val = $"{(mi.OnLeft != null ? "<" : " ")} {mi.value} {(mi.OnLeft != null ? ">" : " ")}";
                GUI.Label(r, val, styles.value);
            }
        }

        void LoadMenu(string path)
        {
            currentPath = path;
            List<DebugMenuItem> itemList = new List<DebugMenuItem>();

            int depth = path.Length == 0 ? 0 : currentPath.Split('/').Length;

            subFolders = allItems.Keys.Where(o => !string.IsNullOrEmpty(o) && o.Contains(currentPath) && o.Split('/').Length == depth + 1).ToArray();

            if (depth >= 1)
                itemList.Add(new NavigateDebugMenuItem(string.Join('/', currentPath.Split('/').SkipLast(1).ToArray()), "/.."));

            foreach (var f in subFolders)
                itemList.Add(new NavigateDebugMenuItem(f, $"{f}"));

            if (allItems.ContainsKey(path))
                itemList.AddRange(allItems[path]);

            if (string.IsNullOrEmpty(currentPath))
                menuName = "Debug Menu";
            else
                menuName = path.Split('/').LastOrDefault();

            currentItems = itemList.ToArray();
            selected = 0;
        }

        class Styles
        {
            public GUIStyle header;
            public GUIStyle label;
            public GUIStyle value;

            static void SetAllStyleColor(GUIStyle style, Color color)
            {
                style.onActive.textColor = color;
                style.onHover.textColor = color;
                style.onFocused.textColor = color;
                style.onNormal.textColor = color;
                style.active.textColor = color;
                style.hover.textColor = color;
                style.focused.textColor = color;
                style.normal.textColor = color;
            }

            public Styles(DebugMenuConfiguration config)
            {
                header = new GUIStyle();
                if (config.customFont != null)
                    header.font = config.customFont;
                header.fontSize = config.fontSize;
                header.fontStyle = FontStyle.Bold;
                header.alignment = TextAnchor.MiddleCenter;
                SetAllStyleColor(header, Color.white);

                label = new GUIStyle();
                if (config.customFont != null)
                    label.font = config.customFont;
                label.fontSize = config.fontSize;
                label.padding = new RectOffset(8, 8, 2, 2);
                label.alignment = TextAnchor.MiddleLeft;
                SetAllStyleColor(label, Color.white);

                value = new GUIStyle();
                if (config.customFont != null)
                    value.font = config.customFont;
                value.fontSize = config.fontSize;
                value.padding = new RectOffset(8, 8, 2, 2);
                value.alignment = TextAnchor.MiddleRight;
                SetAllStyleColor(value, Color.white);
            }
        }




        class NavigateDebugMenuItem : DebugMenuItem
        {
            string path;
            string name;

            public NavigateDebugMenuItem(string path, string name = "")
            {
                this.path = path;
                if (string.IsNullOrEmpty(name))
                    this.name = path.Split('/').Last();
                else
                    this.name = name;
            }
            public override string label => name;
            public override string value => "(dir)";
            public override Action OnValidate => () => DebugMenu.instance.LoadMenu(path);
        }


        public static Type[] GetConcreteTypes<T>()
        {
            List<Type> types = new List<Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] assemblyTypes = null;

                try
                {
                    assemblyTypes = assembly.GetTypes();
                }
                catch
                {
                    Debug.LogError($"Could not load types from assembly : {assembly.FullName}");
                }

                if (assemblyTypes != null)
                {
                    foreach (Type t in assemblyTypes)
                    {
                        if (typeof(T).IsAssignableFrom(t) && !t.IsAbstract)
                        {
                            types.Add(t);
                        }
                    }
                }

            }
            return types.ToArray();
        }

    }

    [AttributeUsage(AttributeTargets.Class)]
    public class DebugMenuItemAttribute : Attribute
    {
        public string path;
        public DebugMenuItemAttribute(string path)
        {
            this.path = path;
        }
    }


    public abstract class DebugMenuItem
    {
        public abstract string label { get; }
        public virtual string value => string.Empty;

        public virtual Action OnValidate => null;
        public virtual Action OnLeft => null;
        public virtual Action OnRight => null;
    }

    [DebugMenuItem("")]
    class ExitDebugMenuItem : DebugMenuItem
    {
        public override string label => "Exit Game";
        public override Action OnValidate =>
#if UNITY_EDITOR
            () => UnityEditor.EditorApplication.ExitPlaymode();
#else
            () => Application.Quit();
#endif
    }
}
