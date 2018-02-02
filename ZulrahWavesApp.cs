using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ZulrahWavesApp
{
    public partial class ZulrahWavesApp : Form
    {
        public int rotOneCount = 2;
        public int rotTwoCount = 2;
        public int rotThreeCount = 2;
        public int rotFourCount = 2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            WinKey = 8
        }

        public ZulrahWavesApp()
        {
            InitializeComponent();
            setupFrames();
            regHotKeys();
        }

        public void setupFrames()
        {
            this.Top = 100;
            this.Left = 450;
            pictureBox1.Image = imageListRot1.Images[1];
            pictureBox2.Image = imageListRot2.Images[1];
            pictureBox3.Image = imageListRot3.Images[1];
            pictureBox4.Image = imageListRot4.Images[1];
        }

        public void regHotKeys()
        {
            //reserves and sets up the hotkeys
            RegisterHotKey(this.Handle, 1, (int)KeyModifier.None, Keys.D1.GetHashCode());
            RegisterHotKey(this.Handle, 2, (int)KeyModifier.None, Keys.D2.GetHashCode());
            RegisterHotKey(this.Handle, 3, (int)KeyModifier.None, Keys.D3.GetHashCode());
            RegisterHotKey(this.Handle, 4, (int)KeyModifier.None, Keys.D4.GetHashCode());
            RegisterHotKey(this.Handle, 5, (int)KeyModifier.None, Keys.Escape.GetHashCode());
            RegisterHotKey(this.Handle, 6, (int)KeyModifier.None, Keys.D0.GetHashCode());
        }

        //When the message that the hotkey is pressed is recieved handle
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 0x0312)
            {
                Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);                  // The key of the hotkey that was pressed.
                KeyModifier modifier = (KeyModifier)((int)m.LParam & 0xFFFF);       // The modifier of the hotkey that was pressed.
                int id = m.WParam.ToInt32();                                        // The id of the hotkey that was pressed.
                
                switch (key.ToString())
                {
                    case "D1":
                        HandleHotkey1();
                        GetTaskWindows("1");
                        break;
                    case "D2":
                        HandleHotkey2();
                        GetTaskWindows("2");
                        break;
                    case "D3":
                        HandleHotkey3();
                        GetTaskWindows("3");
                        break;
                    case "D4":
                        HandleHotkey4();
                        GetTaskWindows("4");
                        break;
                    case "Escape":
                        BorderSwitch();
                        GetTaskWindows("{ESC}");
                        break;
                    case "D0":
                        Reset();
                        GetTaskWindows("0");
                        break;
                }

            }
        }

        private void HandleHotkey1()
        {
            pictureBox1.Image = imageListRot1.Images[rotOneCount];
            rotOneCount++;

            switch (rotOneCount)
            {
                case 3:
                    pictureBox3.Image = null;
                    pictureBox4.Image = null;
                    break;
                case 5:
                    pictureBox2.Image = null;
                    break;
                case 11:
                    rotOneCount = 0;
                    break;
            }


        }
        private void HandleHotkey2()
        {
            pictureBox2.Image = imageListRot2.Images[rotTwoCount];
            rotTwoCount++;

            switch (rotTwoCount)
            {
                case 3:
                    pictureBox3.Image = null;
                    pictureBox4.Image = null;
                    break;
                case 5:
                    pictureBox1.Image = null;
                    break;
                case 11:
                    rotTwoCount = 0;
                    break;
            }
        }

        private void HandleHotkey3()
        {
            pictureBox3.Image = imageListRot3.Images[rotThreeCount];
            rotThreeCount++;

            switch (rotThreeCount)
            {
                case 12:
                    rotThreeCount = 0;
                    break;
                case 3:
                    pictureBox1.Image = null;
                    pictureBox2.Image = null;
                    pictureBox4.Image = null;
                    break;
            }


        }
        private void HandleHotkey4()
        {
            pictureBox4.Image = imageListRot4.Images[rotFourCount];
            rotFourCount++;

            switch (rotFourCount)
            {
                case 3:
                    pictureBox1.Image = null;
                    pictureBox2.Image = null;
                    pictureBox3.Image = null;
                    break;
                case 13:
                    rotFourCount = 0;
                    break;
            }

        }
        private void BorderSwitch()
        {
            if (this.FormBorderStyle == FormBorderStyle.None)
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.None;
            }
        }

        private void Reset()
        {
            rotOneCount = 2;
            rotTwoCount = 2;
            rotThreeCount = 2;
            rotFourCount = 2;
            pictureBox1.Image = imageListRot1.Images[1];
            pictureBox2.Image = imageListRot2.Images[1];
            pictureBox3.Image = imageListRot3.Images[1];
            pictureBox4.Image = imageListRot4.Images[1];
        }

        private void Deregister()
        {
            UnregisterHotKey(this.Handle, 1);
            UnregisterHotKey(this.Handle, 2);
            UnregisterHotKey(this.Handle, 3);
            UnregisterHotKey(this.Handle, 4);
            UnregisterHotKey(this.Handle, 5);
            UnregisterHotKey(this.Handle, 6);
        }

        private void ZulrahWaves_FormClosing(object sender, FormClosingEventArgs e)
        {
            Deregister();

        }
        private void GetTaskWindows(string key)
        {
            Deregister();
             // Get the desktopwindow handle
            int nDeshWndHandle = NativeWin32.GetDesktopWindow();
            // Get the first child window
            int nChildHandle = NativeWin32.GetWindow(nDeshWndHandle,
                                NativeWin32.GW_CHILD);

            while (nChildHandle != 0)
            {
                //If the child window is this (SendKeys) application then ignore it.
                if (nChildHandle == this.Handle.ToInt32())
                {
                    nChildHandle = NativeWin32.GetWindow
                        (nChildHandle, NativeWin32.GW_HWNDNEXT);
                }

                // Get only visible windows
                if (NativeWin32.IsWindowVisible(nChildHandle) != 0)
                {
                    StringBuilder sbTitle = new StringBuilder(1024);
                    // Read the Title bar text on the windows to put in combobox
                    NativeWin32.GetWindowText(nChildHandle, sbTitle, sbTitle.Capacity);
                    String sWinTitle = sbTitle.ToString();
                    {
                        if (sWinTitle.Length > 0)
                        {
                                                        NativeWin32.SetForegroundWindow(nChildHandle);
                            SendKeys.Send(key);
                            break;
                        }
                    }
                }
                // Look for the next child.
                nChildHandle = NativeWin32.GetWindow(nChildHandle,
                               NativeWin32.GW_HWNDNEXT);
            }

            regHotKeys();
        }
        private void ZulrahWavesApp_Load(object sender, EventArgs e)
        {

        }
    }
}
