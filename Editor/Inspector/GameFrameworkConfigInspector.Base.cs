using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ZeroFramework.Editor
{
    public sealed partial class GameFrameworkConfigInspector : GameFrameworkInspector
    {
        private const string NoneOptionName = "<None>";
        private static readonly float[] GameSpeed = new float[] { 0f, 0.01f, 0.1f, 0.25f, 0.5f, 1f, 1.5f, 2f, 4f, 8f };

        private static readonly string[] GameSpeedForDisplay = new string[]
            { "0x", "0.01x", "0.1x", "0.25x", "0.5x", "1x", "1.5x", "2x", "4x", "8x" };

        private SerializedProperty _editorResourceMode = null;
        private SerializedProperty _editorLanguage = null;
        private SerializedProperty _textHelperTypeName = null;
        private SerializedProperty _versionHelperTypeName = null;
        private SerializedProperty _logHelperTypeName = null;
        private SerializedProperty _compressionHelperTypeName = null;
        private SerializedProperty _jsonHelperTypeName = null;
        private SerializedProperty _frameRate = null;
        private SerializedProperty _gameSpeed = null;
        private SerializedProperty _runInBackground = null;
        private SerializedProperty _neverSleep = null;
        private SerializedProperty _runtimeAssemblyNames = null;
        private SerializedProperty _runtimeOrEditorAssemblyNames = null;

        private string[] _textHelperTypeNames = null;
        private int _textHelperTypeNameIndex = 0;
        private string[] _versionHelperTypeNames = null;
        private int _versionHelperTypeNameIndex = 0;
        private string[] _logHelperTypeNames = null;
        private int _logHelperTypeNameIndex = 0;
        private string[] _compressionHelperTypeNames = null;
        private int _compressionHelperTypeNameIndex = 0;
        private string[] _jsonHelperTypeNames = null;
        private int _jsonHelperTypeNameIndex = 0;

        [InspectorConfigInit]
        void BaseInspectorInit()
        {
            _enableFunc.AddFirst(OnBaseEnable);
            _inspectorFunc.AddFirst(OnBaseInspectorGUI);
            _completeFunc.AddFirst(OnBaseComplete);
        }

        void OnBaseEnable()
        {
            _editorResourceMode = serializedObject.FindProperty("editorResourceMode");
            _editorLanguage = serializedObject.FindProperty("editorLanguage");
            _textHelperTypeName = serializedObject.FindProperty("textHelperTypeName");
            _versionHelperTypeName = serializedObject.FindProperty("versionHelperTypeName");
            _logHelperTypeName = serializedObject.FindProperty("logHelperTypeName");
            _compressionHelperTypeName = serializedObject.FindProperty("compressionHelperTypeName");
            _jsonHelperTypeName = serializedObject.FindProperty("jsonHelperTypeName");
            _frameRate = serializedObject.FindProperty("frameRate");
            _gameSpeed = serializedObject.FindProperty("gameSpeed");
            _runInBackground = serializedObject.FindProperty("runInBackground");
            _neverSleep = serializedObject.FindProperty("neverSleep");
            _runtimeAssemblyNames = serializedObject.FindProperty("runtimeAssemblyNames");
            _runtimeOrEditorAssemblyNames = serializedObject.FindProperty("runtimeOrEditorAssemblyNames");

            RefreshBaseTypeName();
        }

        void OnBaseInspectorGUI()
        {
            GameFrameworkConfig t = (GameFrameworkConfig)target;
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.LabelField("Base", EditorStyles.boldLabel);
                _editorResourceMode.boolValue =
                    EditorGUILayout.Toggle("Editor Resource Mode", _editorResourceMode.boolValue);
                EditorGUILayout.PropertyField(_editorLanguage);

                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField("Global Helpers", EditorStyles.boldLabel);

                    int textHelperSelectedIndex =
                        EditorGUILayout.Popup("Text Helper", _textHelperTypeNameIndex, _textHelperTypeNames);
                    if (textHelperSelectedIndex != _textHelperTypeNameIndex)
                    {
                        _textHelperTypeNameIndex = textHelperSelectedIndex;
                        _textHelperTypeName.stringValue = textHelperSelectedIndex <= 0
                            ? null
                            : _textHelperTypeNames[textHelperSelectedIndex];
                    }

                    int versionHelperSelectedIndex = EditorGUILayout.Popup("Version Helper",
                        _versionHelperTypeNameIndex, _versionHelperTypeNames);
                    if (versionHelperSelectedIndex != _versionHelperTypeNameIndex)
                    {
                        _versionHelperTypeNameIndex = versionHelperSelectedIndex;
                        _versionHelperTypeName.stringValue = versionHelperSelectedIndex <= 0
                            ? null
                            : _versionHelperTypeNames[versionHelperSelectedIndex];
                    }

                    int logHelperSelectedIndex =
                        EditorGUILayout.Popup("Log Helper", _logHelperTypeNameIndex, _logHelperTypeNames);
                    if (logHelperSelectedIndex != _logHelperTypeNameIndex)
                    {
                        _logHelperTypeNameIndex = logHelperSelectedIndex;
                        _logHelperTypeName.stringValue = logHelperSelectedIndex <= 0
                            ? null
                            : _logHelperTypeNames[logHelperSelectedIndex];
                    }

                    int compressionHelperSelectedIndex = EditorGUILayout.Popup("Compression Helper",
                        _compressionHelperTypeNameIndex, _compressionHelperTypeNames);
                    if (compressionHelperSelectedIndex != _compressionHelperTypeNameIndex)
                    {
                        _compressionHelperTypeNameIndex = compressionHelperSelectedIndex;
                        _compressionHelperTypeName.stringValue = compressionHelperSelectedIndex <= 0
                            ? null
                            : _compressionHelperTypeNames[compressionHelperSelectedIndex];
                    }

                    int jsonHelperSelectedIndex =
                        EditorGUILayout.Popup("JSON Helper", _jsonHelperTypeNameIndex, _jsonHelperTypeNames);
                    if (jsonHelperSelectedIndex != _jsonHelperTypeNameIndex)
                    {
                        _jsonHelperTypeNameIndex = jsonHelperSelectedIndex;
                        _jsonHelperTypeName.stringValue = jsonHelperSelectedIndex <= 0
                            ? null
                            : _jsonHelperTypeNames[jsonHelperSelectedIndex];
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUI.EndDisabledGroup();

            this._frameRate.intValue = EditorGUILayout.IntSlider("Frame Rate", this._frameRate.intValue, 1, 120);

            EditorGUILayout.BeginVertical("box");
            {
                float gameSpeed = EditorGUILayout.Slider("Game Speed", this._gameSpeed.floatValue, 0f, 8f);
                int selectedGameSpeed =
                    GUILayout.SelectionGrid(GetSelectedGameSpeed(gameSpeed), GameSpeedForDisplay, 5);
                if (selectedGameSpeed >= 0)
                {
                    gameSpeed = GetGameSpeed(selectedGameSpeed);
                }

                this._gameSpeed.floatValue = gameSpeed;
            }
            EditorGUILayout.EndVertical();

            this._runInBackground.boolValue =
                EditorGUILayout.Toggle("Run in Background", this._runInBackground.boolValue);

            this._neverSleep.boolValue = EditorGUILayout.Toggle("Never Sleep", this._neverSleep.boolValue);

            EditorGUILayout.PropertyField(_runtimeAssemblyNames, includeChildren: true);
            EditorGUILayout.PropertyField(_runtimeOrEditorAssemblyNames, includeChildren: true);
        }

        void OnBaseComplete()
        {
            RefreshBaseTypeName();
        }

        void RefreshBaseTypeName()
        {
            List<string> textHelperTypeNames = new List<string>
            {
                NoneOptionName
            };

            textHelperTypeNames.AddRange(Type.GetRuntimeTypeNames(typeof(Utility.Text.ITextHelper)));
            _textHelperTypeNames = textHelperTypeNames.ToArray();
            _textHelperTypeNameIndex = 0;
            if (!string.IsNullOrEmpty(_textHelperTypeName.stringValue))
            {
                _textHelperTypeNameIndex = textHelperTypeNames.IndexOf(_textHelperTypeName.stringValue);
                if (_textHelperTypeNameIndex <= 0)
                {
                    _textHelperTypeNameIndex = 0;
                    _textHelperTypeName.stringValue = null;
                }
            }

            List<string> versionHelperTypeNames = new List<string>
            {
                NoneOptionName
            };

            versionHelperTypeNames.AddRange(Type.GetRuntimeTypeNames(typeof(Version.IVersionHelper)));
            _versionHelperTypeNames = versionHelperTypeNames.ToArray();
            _versionHelperTypeNameIndex = 0;
            if (!string.IsNullOrEmpty(_versionHelperTypeName.stringValue))
            {
                _versionHelperTypeNameIndex = versionHelperTypeNames.IndexOf(_versionHelperTypeName.stringValue);
                if (_versionHelperTypeNameIndex <= 0)
                {
                    _versionHelperTypeNameIndex = 0;
                    _versionHelperTypeName.stringValue = null;
                }
            }

            List<string> logHelperTypeNames = new List<string>
            {
                NoneOptionName
            };

            logHelperTypeNames.AddRange(Type.GetRuntimeTypeNames(typeof(GameFrameworkLog.ILogHelper)));
            _logHelperTypeNames = logHelperTypeNames.ToArray();
            _logHelperTypeNameIndex = 0;
            if (!string.IsNullOrEmpty(_logHelperTypeName.stringValue))
            {
                _logHelperTypeNameIndex = logHelperTypeNames.IndexOf(_logHelperTypeName.stringValue);
                if (_logHelperTypeNameIndex <= 0)
                {
                    _logHelperTypeNameIndex = 0;
                    _logHelperTypeName.stringValue = null;
                }
            }

            List<string> compressionHelperTypeNames = new List<string>
            {
                NoneOptionName
            };

            compressionHelperTypeNames.AddRange(
                Type.GetRuntimeTypeNames(typeof(Utility.Compression.ICompressionHelper)));
            _compressionHelperTypeNames = compressionHelperTypeNames.ToArray();
            _compressionHelperTypeNameIndex = 0;
            if (!string.IsNullOrEmpty(_compressionHelperTypeName.stringValue))
            {
                _compressionHelperTypeNameIndex =
                    compressionHelperTypeNames.IndexOf(_compressionHelperTypeName.stringValue);
                if (_compressionHelperTypeNameIndex <= 0)
                {
                    _compressionHelperTypeNameIndex = 0;
                    _compressionHelperTypeName.stringValue = null;
                }
            }

            List<string> jsonHelperTypeNames = new List<string>
            {
                NoneOptionName
            };

            jsonHelperTypeNames.AddRange(Type.GetRuntimeTypeNames(typeof(Utility.Json.IJsonHelper)));
            _jsonHelperTypeNames = jsonHelperTypeNames.ToArray();
            _jsonHelperTypeNameIndex = 0;
            if (!string.IsNullOrEmpty(_jsonHelperTypeName.stringValue))
            {
                _jsonHelperTypeNameIndex = jsonHelperTypeNames.IndexOf(_jsonHelperTypeName.stringValue);
                if (_jsonHelperTypeNameIndex <= 0)
                {
                    _jsonHelperTypeNameIndex = 0;
                    _jsonHelperTypeName.stringValue = null;
                }
            }
        }

        private float GetGameSpeed(int selectedGameSpeed)
        {
            if (selectedGameSpeed < 0)
            {
                return GameSpeed[0];
            }

            if (selectedGameSpeed >= GameSpeed.Length)
            {
                return GameSpeed[GameSpeed.Length - 1];
            }

            return GameSpeed[selectedGameSpeed];
        }

        private int GetSelectedGameSpeed(float gameSpeed)
        {
            for (int i = 0; i < GameSpeed.Length; i++)
            {
                if (gameSpeed == GameSpeed[i])
                {
                    return i;
                }
            }

            return -1;
        }
    }
}