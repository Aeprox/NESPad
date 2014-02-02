using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace NESMouse {
    class MKBIO {
        byte cur;
        byte prev;

        private Cursor cursor;

        private bool aIsDown;
        private bool bIsDown;

        private int upCount;
        private int downCount;
        private int leftCount;
        private int rightCount;


        public void Verwerk(byte b) {
            cur = b;
            //01111111 = A
            if ((~b & 128) !=0) {
                if (!aIsDown) {
                    MouseEvent(MouseEventFlags.LeftDown);
                }
                aIsDown = true;
                
            }
            else {
                if (aIsDown) {
                    MouseEvent(MouseEventFlags.LeftUp);
                }
                aIsDown = false;
            }

            //10111111 = B
            if ((~b & 64) != 0) {
                if (!bIsDown) {
                    MouseEvent(MouseEventFlags.RightDown);
                }
                bIsDown = true;
            }
            else {
                if (bIsDown) {
                    MouseEvent(MouseEventFlags.RightUp);
                }
                bIsDown = false;
            }

            //UP = 11110111
            if ((~b & 8) != 0) {
                int yUp;
                upCount++;
                yUp = 20 + upCount * 2;
                Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y - yUp);
            }
            else {
                upCount = 0;
            }

            //DOWN=11111011
            if ((~b & 4) != 0) {
                downCount++;
                Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y + 20);
            }
            else {
                downCount = 0;
            }
            
            //LEFT=11111101
            if ((~b & 2) != 0) {
                leftCount++;
                Cursor.Position = new Point(Cursor.Position.X - 20, Cursor.Position.Y);
            }
            else {
                leftCount = 0;
            }
            //RIGHT=11111110
            if ((~b & 1) != 0) {
                rightCount++;
                Cursor.Position = new Point(Cursor.Position.X +20 , Cursor.Position.Y);
            }
            else {
                rightCount = 0;
            }
            prev = b;

        }

        /* Yeey, source:
         * 
         * http://stackoverflow.com/questions/2416748/how-to-simulate-mouse-click-in-c
         * 
         */
        
        [Flags]
        public enum MouseEventFlags {
            LeftDown = 0x00000002,
            LeftUp = 0x00000004,
            MiddleDown = 0x00000020,
            MiddleUp = 0x00000040,
            Move = 0x00000001,
            Absolute = 0x00008000,
            RightDown = 0x00000008,
            RightUp = 0x00000010
        }

        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out MousePoint lpMousePoint);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        public static void SetCursorPosition(int X, int Y) {
            SetCursorPos(X, Y);
        }

        public static MousePoint GetCursorPosition() {
            MousePoint currentMousePoint;
            var gotPoint = GetCursorPos(out currentMousePoint);
            if (!gotPoint) { currentMousePoint = new MousePoint(0, 0); }
            return currentMousePoint;
        }

        public static void MouseEvent(MouseEventFlags value) {
            MousePoint position = GetCursorPosition();

            mouse_event
                ((int)value,
                 position.X,
                 position.Y,
                 0,
                 0)
                ;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MousePoint {
            public int X;
            public int Y;

            public MousePoint(int x, int y) {
                X = x;
                Y = x;
            }

        }
    }

}
