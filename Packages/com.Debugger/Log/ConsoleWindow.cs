using System;
using System.Collections.Generic;
using UnityEngine;
using ZeroFramework.Setting;

namespace ZeroFramework.Debugger
{
    [Serializable]
    public sealed class ConsoleWindow : IDebuggerWindow
    {
        private readonly Queue<LogNode> _logNodes = new Queue<LogNode>();

        private ISettingManager _settingManager = null;
        private Vector2 _logScrollPosition = Vector2.zero;
        private Vector2 _stackScrollPosition = Vector2.zero;
        private int _infoCount = 0;
        private int _warningCount = 0;
        private int _errorCount = 0;
        private int _fatalCount = 0;
        private LogNode _selectedNode = null;
        private bool _lastLockScroll = true;
        private bool _lastInfoFilter = true;
        private bool _lastWarningFilter = true;
        private bool _lastErrorFilter = true;
        private bool _lastFatalFilter = true;

        [SerializeField] private bool _lockScroll = true;
        [SerializeField] private int _maxLine = 100;
        [SerializeField] private bool _infoFilter = true;
        [SerializeField] private bool _warningFilter = true;
        [SerializeField] private bool _errorFilter = true;
        [SerializeField] private bool _fatalFilter = true;
        [SerializeField] private Color32 _infoColor = Color.white;
        [SerializeField] private Color32 _warningColor = Color.yellow;
        [SerializeField] private Color32 _errorColor = Color.red;
        [SerializeField] private Color32 _fatalColor = new Color(0.7f, 0.2f, 0.2f);

        public bool LockScroll
        {
            get => _lockScroll;
            set => _lockScroll = value;
        }

        public int MaxLine
        {
            get => _maxLine;
            set => _maxLine = value;
        }

        public bool InfoFilter
        {
            get => _infoFilter;
            set => _infoFilter = value;
        }

        public bool WarningFilter
        {
            get => _warningFilter;
            set => _warningFilter = value;
        }

        public bool ErrorFilter
        {
            get => _errorFilter;
            set => _errorFilter = value;
        }

        public bool FatalFilter
        {
            get => _fatalFilter;
            set => _fatalFilter = value;
        }

        public int InfoCount => _infoCount;

        public int WarningCount => _warningCount;

        public int ErrorCount => _errorCount;

        public int FatalCount => _fatalCount;

        public Color32 InfoColor
        {
            get => _infoColor;
            set => _infoColor = value;
        }

        public Color32 WarningColor
        {
            get => _warningColor;
            set => _warningColor = value;
        }

        public Color32 ErrorColor
        {
            get => _errorColor;
            set => _errorColor = value;
        }

        public Color32 FatalColor
        {
            get => _fatalColor;
            set => _fatalColor = value;
        }

        public void Initialize(params object[] args)
        {
            _settingManager = Zero.setting;
            if (_settingManager == null)
            {
                Log.Fatal("Setting component is invalid.");
                return;
            }
            
            Application.logMessageReceived += OnLogMessageReceived;
            _lockScroll = _lastLockScroll = _settingManager.GetBool("Debugger.Console.LockScroll", true);
            _infoFilter = _lastInfoFilter = _settingManager.GetBool("Debugger.Console.InfoFilter", true);
            _warningFilter = _lastWarningFilter =
                _settingManager.GetBool("Debugger.Console.WarningFilter", true);
            _errorFilter = _lastErrorFilter = _settingManager.GetBool("Debugger.Console.ErrorFilter", true);
            _fatalFilter = _lastFatalFilter = _settingManager.GetBool("Debugger.Console.FatalFilter", true);
		}

        public void Shutdown()
        {
            Application.logMessageReceived -= OnLogMessageReceived;
            Clear();
        }

        public void OnEnter()
        {
        }

        public void OnLeave()
        {
        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (_lastLockScroll != _lockScroll)
            {
                _lastLockScroll = _lockScroll;
                _settingManager.SetBool("Debugger.Console.LockScroll", _lockScroll);
            }

            if (_lastInfoFilter != _infoFilter)
            {
                _lastInfoFilter = _infoFilter;
                _settingManager.SetBool("Debugger.Console.InfoFilter", _infoFilter);
            }

            if (_lastWarningFilter != _warningFilter)
            {
                _lastWarningFilter = _warningFilter;
                _settingManager.SetBool("Debugger.Console.WarningFilter", _warningFilter);
            }

            if (_lastErrorFilter != _errorFilter)
            {
                _lastErrorFilter = _errorFilter;
                _settingManager.SetBool("Debugger.Console.ErrorFilter", _errorFilter);
            }

            if (_lastFatalFilter != _fatalFilter)
            {
                _lastFatalFilter = _fatalFilter;
                _settingManager.SetBool("Debugger.Console.FatalFilter", _fatalFilter);
            }
        }

        public void OnDraw()
        {
            RefreshCount();

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Clear All", GUILayout.Width(100f)))
                {
                    Clear();
                }

                _lockScroll = GUILayout.Toggle(_lockScroll, "Lock Scroll", GUILayout.Width(90f));
                GUILayout.FlexibleSpace();
                _infoFilter = GUILayout.Toggle(_infoFilter, Utility.Text.Format("Info ({0})", _infoCount),
                    GUILayout.Width(90f));
                _warningFilter = GUILayout.Toggle(_warningFilter,
                    Utility.Text.Format("Warning ({0})", _warningCount), GUILayout.Width(90f));
                _errorFilter = GUILayout.Toggle(_errorFilter, Utility.Text.Format("Error ({0})", _errorCount),
                    GUILayout.Width(90f));
                _fatalFilter = GUILayout.Toggle(_fatalFilter, Utility.Text.Format("Fatal ({0})", _fatalCount),
                    GUILayout.Width(90f));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("box");
            {
                if (_lockScroll)
                {
                    _logScrollPosition.y = float.MaxValue;
                }

                _logScrollPosition = GUILayout.BeginScrollView(_logScrollPosition);
                {
                    bool selected = false;
                    foreach (LogNode logNode in _logNodes)
                    {
                        switch (logNode.LogType)
                        {
                            case LogType.Log:
                                if (!_infoFilter)
                                {
                                    continue;
                                }

                                break;

                            case LogType.Warning:
                                if (!_warningFilter)
                                {
                                    continue;
                                }

                                break;

                            case LogType.Error:
                                if (!_errorFilter)
                                {
                                    continue;
                                }

                                break;

                            case LogType.Exception:
                                if (!_fatalFilter)
                                {
                                    continue;
                                }

                                break;
                        }

                        if (GUILayout.Toggle(_selectedNode == logNode, GetLogString(logNode)))
                        {
                            selected = true;
                            if (_selectedNode != logNode)
                            {
                                _selectedNode = logNode;
                                _stackScrollPosition = Vector2.zero;
                            }
                        }
                    }

                    if (!selected)
                    {
                        _selectedNode = null;
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical("box");
            {
                _stackScrollPosition = GUILayout.BeginScrollView(_stackScrollPosition, GUILayout.Height(100f));
                {
                    if (_selectedNode != null)
                    {
                        Color32 color = GetLogStringColor(_selectedNode.LogType);
                        if (GUILayout.Button(
                                Utility.Text.Format("<color=#{0:x2}{1:x2}{2:x2}{3:x2}><b>{4}</b></color>{6}{6}{5}",
                                    color.r, color.g, color.b, color.a, _selectedNode.LogMessage,
                                    _selectedNode.StackTrack, Environment.NewLine), "label"))
                        {
                            DebuggerComponent.CopyToClipboard(Utility.Text.Format("{0}{2}{2}{1}",
                                _selectedNode.LogMessage,
                                _selectedNode.StackTrack, Environment.NewLine));
                        }
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }

        private void Clear()
        {
            _logNodes.Clear();
        }

        public void RefreshCount()
        {
            _infoCount = 0;
            _warningCount = 0;
            _errorCount = 0;
            _fatalCount = 0;
            foreach (LogNode logNode in _logNodes)
            {
                switch (logNode.LogType)
                {
                    case LogType.Log:
                        _infoCount++;
                        break;

                    case LogType.Warning:
                        _warningCount++;
                        break;

                    case LogType.Error:
                        _errorCount++;
                        break;

                    case LogType.Exception:
                        _fatalCount++;
                        break;
                }
            }
        }

        public void GetRecentLogs(List<LogNode> results)
        {
            if (results == null)
            {
                Log.Error("Results is invalid.");
                return;
            }

            results.Clear();
            foreach (LogNode logNode in _logNodes)
            {
                results.Add(logNode);
            }
        }

        public void GetRecentLogs(List<LogNode> results, int count)
        {
            if (results == null)
            {
                Log.Error("Results is invalid.");
                return;
            }

            if (count <= 0)
            {
                Log.Error("Count is invalid.");
                return;
            }

            int position = _logNodes.Count - count;
            if (position < 0)
            {
                position = 0;
            }

            int index = 0;
            results.Clear();
            foreach (LogNode logNode in _logNodes)
            {
                if (index++ < position)
                {
                    continue;
                }

                results.Add(logNode);
            }
        }

        private void OnLogMessageReceived(string logMessage, string stackTrace, LogType logType)
        {
            if (logType == LogType.Assert)
            {
                logType = LogType.Error;
            }
            
            _logNodes.Enqueue(LogNode.Create(logType, logMessage, stackTrace));
            while (_logNodes.Count > _maxLine)
            {
                ReferencePool.Release(_logNodes.Dequeue());
            }
        }

        private string GetLogString(LogNode logNode)
        {
            Color32 color = GetLogStringColor(logNode.LogType);
            return Utility.Text.Format("<color=#{0:x2}{1:x2}{2:x2}{3:x2}>[{4:HH:mm:ss.fff}][{5}] {6}</color>",
                color.r, color.g, color.b, color.a, logNode.LogTime.ToLocalTime(), logNode.LogFrameCount,
                logNode.LogMessage);
        }

        internal Color32 GetLogStringColor(LogType logType)
        {
            Color32 color = Color.white;
            switch (logType)
            {
                case LogType.Log:
                    color = _infoColor;
                    break;

                case LogType.Warning:
                    color = _warningColor;
                    break;

                case LogType.Error:
                    color = _errorColor;
                    break;

                case LogType.Exception:
                    color = _fatalColor;
                    break;
            }

            return color;
        }
    }
}