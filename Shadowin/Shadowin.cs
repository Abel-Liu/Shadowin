﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Plusal.Windows;
using Shadowin.Implement;
using Shadowin.Properties;

namespace Shadowin
{
    public partial class Shadowin : Form
    {
        public readonly static string Version = "V" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        private const int SizeDifference = 15;
        private const double OpacityDifference = 0.1;
        private readonly Uri BlankUrl = new Uri("about:blank");
        private Screen _currentScreen;
        private int? _currentZoom;

        #region 属性

        /// <summary>
        /// 热键管理器
        /// </summary>
        private HotKeyManager SwHotKeyManager
        {
            get;
            set;
        }

        /// <summary>
        /// 允许自动刷新
        /// </summary>
        private bool RefreshEnabled
        {
            get;
            set;
        }

        #endregion

        public Shadowin()
        {
            InitializeComponent();

            this.Initialize();
        }
        ~Shadowin()
        {
            this.Close();
        }

        private void logoImage_Click(object sender, EventArgs e)
        {
            using (var about = new About())
            {
                //var result = about.ShowDialog();
                //if (result == DialogResult.OK)
                //{
                //    Process.Start("https://github.com/heddaz/shadowin");
                //}
            }
        }
        private void Shadowin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.SwHotKeyManager != null)
            {
                try
                {
                    this.SwHotKeyManager.Dispose();
                }
                catch
                {
                }
                finally
                {
                    this.SwHotKeyManager = null;
                }
            }
        }

        #region 初始化

        private void Initialize()
        {
            #region 热键

            this.SwHotKeyManager = new HotKeyManager(this);

            //增大宽度
            this.SwHotKeyManager.Register(
                (Enumeration.ModifierKeys)Enumeration.FromString<Enumeration.ModifierKeys>(SwGlobal.IncreaseWidthHotKeyModifierKey),
                (Keys)Enumeration.FromString<Keys>(SwGlobal.IncreaseWidthHotKeyKey),
                new HotKeyEventHandler(this.OnIncreaseWidthHotKey)
                );
            //减小宽度
            this.SwHotKeyManager.Register(
                (Enumeration.ModifierKeys)Enumeration.FromString<Enumeration.ModifierKeys>(SwGlobal.DecreaseWidthHotKeyModifierKey),
                (Keys)Enumeration.FromString<Keys>(SwGlobal.DecreaseWidthHotKeyKey),
                new HotKeyEventHandler(this.OnDecreaseWidthHotKey)
                );
            //增大高度
            this.SwHotKeyManager.Register(
                (Enumeration.ModifierKeys)Enumeration.FromString<Enumeration.ModifierKeys>(SwGlobal.IncreaseHeightHotKeyModifierKey),
                (Keys)Enumeration.FromString<Keys>(SwGlobal.IncreaseHeightHotKeyKey),
                new HotKeyEventHandler(this.OnIncreaseHeightHotKey)
                );
            //减小高度
            this.SwHotKeyManager.Register(
                (Enumeration.ModifierKeys)Enumeration.FromString<Enumeration.ModifierKeys>(SwGlobal.DecreaseHeightHotKeyModifierKey),
                (Keys)Enumeration.FromString<Keys>(SwGlobal.DecreaseHeightHotKeyKey),
                new HotKeyEventHandler(this.OnDecreaseHeightHotKey)
                );
            //增大不透明度
            this.SwHotKeyManager.Register(
                (Enumeration.ModifierKeys)Enumeration.FromString<Enumeration.ModifierKeys>(SwGlobal.IncreaseOpacityHotKeyModifierKey),
                (Keys)Enumeration.FromString<Keys>(SwGlobal.IncreaseOpacityHotKeyKey),
                new HotKeyEventHandler(this.OnIncreaseOpacityHotKey)
                );
            //减小不透明度
            this.SwHotKeyManager.Register(
                (Enumeration.ModifierKeys)Enumeration.FromString<Enumeration.ModifierKeys>(SwGlobal.DecreaseOpacityHotKeyModifierKey),
                (Keys)Enumeration.FromString<Keys>(SwGlobal.DecreaseOpacityHotKeyKey),
                new HotKeyEventHandler(this.OnDecreaseOpacityHotKey)
                );
            //显示隐藏
            this.SwHotKeyManager.Register(
                (Enumeration.ModifierKeys)Enumeration.FromString<Enumeration.ModifierKeys>(SwGlobal.ShowHideHotKeyModifierKey),
                (Keys)Enumeration.FromString<Keys>(SwGlobal.ShowHideHotKeyKey),
                new HotKeyEventHandler(this.OnShowHideHotKey)
                );
            //退出
            this.SwHotKeyManager.Register(
                (Enumeration.ModifierKeys)Enumeration.FromString<Enumeration.ModifierKeys>(SwGlobal.ExitHotKeyModifierKey),
                (Keys)Enumeration.FromString<Keys>(SwGlobal.ExitHotKeyKey),
                new HotKeyEventHandler(this.OnExitHotKey)
                );

            #endregion

            #region 窗体

            this.toolTip.ToolTipTitle = SwGlobal.Title;
            this.Size = Settings.Default.FormSize;

            #endregion

            #region 刷新定时器

            if (SwGlobal.RefreshInterval > 0)
            {
                refreshTimer.Interval = SwGlobal.RefreshInterval;
                this.RefreshEnabled = true;
            }
            else
            {
                this.RefreshEnabled = false;
            }

            #endregion
        }

        #endregion

        #region 热键委托

        /// <summary>
        /// 增大宽度
        /// </summary>
        private void OnIncreaseWidthHotKey()
        {
            if (this.Visible)
            {
                this.Width += SizeDifference;
            }
        }
        /// <summary>
        /// 减小宽度
        /// </summary>
        private void OnDecreaseWidthHotKey()
        {
            if (this.Visible)
            {
                this.Width -= SizeDifference;
            }
        }
        /// <summary>
        /// 增大高度
        /// </summary>
        private void OnIncreaseHeightHotKey()
        {
            if (this.Visible)
            {
                this.Height += SizeDifference;
            }
        }
        /// <summary>
        /// 减小高度
        /// </summary>
        private void OnDecreaseHeightHotKey()
        {
            if (this.Visible)
            {
                this.Height -= SizeDifference;
            }
        }
        /// <summary>
        /// 增大不透明度
        /// </summary>
        private void OnIncreaseOpacityHotKey()
        {
            if (this.Visible)
            {
                this.Opacity += OpacityDifference;
            }
        }
        /// <summary>
        /// 减小不透明度
        /// </summary>
        private void OnDecreaseOpacityHotKey()
        {
            if (this.Visible)
            {
                this.Opacity -= OpacityDifference;
            }
        }
        /// <summary>
        /// 显示隐藏
        /// </summary>
        private void OnShowHideHotKey()
        {
            this.Visible = !this.Visible;
        }
        /// <summary>
        /// 退出
        /// </summary>
        private void OnExitHotKey()
        {
            this.Close();
        }

        #endregion

        #region 刷新和显示

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            this.LoadUrl();
        }
        private void Shadowin_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                //this.Shadowin_SizeChanged(sender, e);
                this.LoadUrl();
            }
            else
            {
                //后台歇息
                webBrowser.Url = BlankUrl;
            }

            refreshTimer.Enabled = this.Visible && this.RefreshEnabled;
        }
        private void LoadUrl()
        {
            if (webBrowser.Url == null || webBrowser.Url.Equals(BlankUrl))
            {
                webBrowser.Url = new Uri(SwGlobal.Url);
            }
            else
            {
                webBrowser.Refresh(WebBrowserRefreshOption.Completely);
            }
        }
       
        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // 缩放
            if (!_currentZoom.HasValue)
            {
                _currentZoom = SwGlobal.PageZoom;
                object value = _currentZoom.Value, obj = null;
                try
                {
                    (webBrowser.ActiveXInstance as SHDocVw.WebBrowser)
                        .ExecWB(SHDocVw.OLECMDID.OLECMDID_OPTICAL_ZOOM, SHDocVw.OLECMDEXECOPT.OLECMDEXECOPT_DONTPROMPTUSER, ref value, ref obj);
                }
                catch { }
            }
        }

        #endregion

        private void Shadowin_Load(object sender, EventArgs e)
        {
            // 分屏
            if (_currentScreen == null)
            {
                try
                {
                    _currentScreen = Screen.AllScreens[SwGlobal.ScreenId];
                }
                catch
                {
                    _currentScreen = Screen.PrimaryScreen;
                }
            }

            // 位置
            this.Left = _currentScreen.WorkingArea.X + _currentScreen.WorkingArea.Width - this.Width;
            this.Top = _currentScreen.WorkingArea.Y + _currentScreen.WorkingArea.Bottom - this.Height;
        }
    }
}
