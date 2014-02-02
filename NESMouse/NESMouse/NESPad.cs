using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NESMouse {
    class NESPad {
        private SerialCom serial;

        public NESPad() {
            serial = new SerialCom();
            serial.Setup("COM9",57600);
        }

        public void Start() {
            serial.Open();
        }

        public void Stop(){
            serial.Close();
        }
    }
}
