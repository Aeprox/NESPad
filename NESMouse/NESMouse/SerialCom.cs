using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace NESMouse {
    class SerialCom {
        //vars
        private SerialPort port;

        private string comport;
        private int baudrate;
        private byte options;

        private MKBIO output;
        private byte input;

        //properties
        
        //constr
        public SerialCom() {

        }

        //methodes
        public void Setup(string comport, int baudrate) {
            this.port = new SerialPort(comport, baudrate);
            this.output = new MKBIO();
            this.port.NewLine = "\n";
        }

        public void Open() {
            this.port.Open();
            this.port.DataReceived += Recieved;
            this.port.DiscardInBuffer();

        }

        public void Close() {
            this.port.Close();
            this.port.DataReceived -= Recieved;
        }


        private void Recieved(object sender, System.IO.Ports.SerialDataReceivedEventArgs e) {
            input = byte.Parse(this.port.ReadLine());
            this.output.Verwerk(input);
            port.DiscardOutBuffer();
        }
    }
}
