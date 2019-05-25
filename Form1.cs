using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace IoT_DEMO
{
    public partial class main : Form
    {
        /* Variable for connect TCP client */
        public TcpListener listener = new TcpListener(IPAddress.Any, 32000);
        /* Variable for connect client */
        public int CT_client1 = -1;
        public int CT_client2 = -1;
        public int CT_client3 = -1;

        Slave IOT_Slave1 = new Slave();
        
        public main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            listener.Start();
            Thread newThread1 = new Thread(new ThreadStart(client_connect1));
            newThread1.Start();
            //Thread newThread2 = new Thread(new ThreadStart(client_connect2));
            //newThread2.Start();
            //Thread newThread3 = new Thread(new ThreadStart(client_connect3));
            //newThread3.Start();
        }



        #region Communication with slave 1
        private void client_connect1()
        {
            int length;
            char[] rx_array = new char[128];
            TcpClient client = listener.AcceptTcpClient();
            StreamReader sr = new StreamReader(client.GetStream());
            StreamWriter sw = new StreamWriter(client.GetStream());
            sw.AutoFlush = true;
            while (true)
            {
                if (client.Connected)
                {
                    try
                    {
                        length = client.Available;
                        if (length > 0)
                        {
                            Array.Clear(rx_array, 0, 128);
                            sr.Read(rx_array, 0, length);
                            handle_data_client1(rx_array, length);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }

                    if (CT_client1 == 1)
                    {
                        CT_client1 = -1;
                        sw.Write(IOT_Slave1.ControlOutput());

                    }
                    else if (CT_client1 == 2)
                    {
                        CT_client1 = -1;
                        sw.Write(IOT_Slave1.SetTimeUpdate());
                    }
                    else if (CT_client1 == 3)
                    {
                        CT_client1 = -1;
                        sw.Write(IOT_Slave1.SetRealTime());
                    }
                }
                else
                {
                    client.Close();
                }
                Thread.Sleep(500);
            }
        }
        #endregion

        #region Handle data client 1
        private void handle_data_client1(char[] array, int length)
        {
            if (array[0] == '#')
            {
                if (array[1] == '#')
                {
                    if (array[2] == IOT_Slave1.GetIDByte1())
                    {
                        if (array[3] == IOT_Slave1.GetIDByte2())
                        {
                            /* FUNCTION 1 */
                            if (array[4] == '1')
                            {
                                if (array[5] == 'F')
                                {
                                    pic_Slave1Connect.BackColor = System.Drawing.Color.Green;
                                }
                                else if (array[5] == '0')
                                {
                                    pic_Slave1Connect.BackColor = System.Drawing.Color.Red;
                                }
                            }

                            /* FUNCTION 2 */
                            if(array[4] == '2')
                            {
                                /* Copy to buffer */
                                char[] buffer = new char[15];
                                for(int count = 0; count < 15; count++)
                                {
                                    buffer[count] = array[count + 5];
                                }
                                txt_Slave1Alarm.Text = new string(buffer);
                            }

                            /* FUNCTION 3 */
                            if (array[4] == '3')
                            {
                                IOT_Slave1.SetResponseFlag(3);
                            }

                            /* FUNCTION 4 */
                            if (array[4] == '4')
                            {
                                IOT_Slave1.SetResponseFlag(4);
                            }

                            /* FUNCTION 5 */
                            if (array[4] == '5')
                            {
                                IOT_Slave1.SetResponseFlag(5);
                            }

                            /* FUNCTION 6 */
                            if (array[4] == '6')
                            {
                                IOT_Slave1.UpdateRelay(ascii_to_hex(array[5]));
                                IOT_Slave1.UpdateOutput(Convert.ToUInt16(ascii_to_hex(array[6]) * 16 + ascii_to_hex(array[7])));
                                IOT_Slave1.UpdateAnalog(1, Convert.ToUInt16(ascii_to_hex(array[8]) * 4096 + ascii_to_hex(array[9]) * 256 +
                                                                            ascii_to_hex(array[10]) * 16 + ascii_to_hex(array[11])));
                                IOT_Slave1.UpdateAnalog(2, Convert.ToUInt16(ascii_to_hex(array[12]) * 4096 + ascii_to_hex(array[13]) * 256 +
                                                                            ascii_to_hex(array[14]) * 16 + ascii_to_hex(array[15])));
                                IOT_Slave1.UpdateInputV(Convert.ToUInt16(ascii_to_hex(array[16]) * 16 + ascii_to_hex(array[17])));
                                IOT_Slave1.UpdateInputS(Convert.ToUInt16(ascii_to_hex(array[18]) * 16 + ascii_to_hex(array[19])));
                                IOT_Slave1.UpdateVoltage(1, Convert.ToUInt16(ascii_to_hex(array[20]) * 4096 + ascii_to_hex(array[21]) * 256 +
                                                                            ascii_to_hex(array[22]) * 16 + ascii_to_hex(array[23])));
                                IOT_Slave1.UpdateVoltage(2, Convert.ToUInt16(ascii_to_hex(array[24]) * 4096 + ascii_to_hex(array[25]) * 256 +
                                                                            ascii_to_hex(array[26]) * 16 + ascii_to_hex(array[27])));
                                IOT_Slave1.UpdateVoltage(3, Convert.ToUInt16(ascii_to_hex(array[28]) * 4096 + ascii_to_hex(array[29]) * 256 +
                                                                            ascii_to_hex(array[30]) * 16 + ascii_to_hex(array[31])));
                                IOT_Slave1.UpdateVoltage(4, Convert.ToUInt16(ascii_to_hex(array[32]) * 4096 + ascii_to_hex(array[33]) * 256 +
                                                                            ascii_to_hex(array[34]) * 16 + ascii_to_hex(array[35])));
                                IOT_Slave1.UpdateCurrent(1, Convert.ToUInt16(ascii_to_hex(array[36]) * 4096 + ascii_to_hex(array[37]) * 256 +
                                                                            ascii_to_hex(array[38]) * 16 + ascii_to_hex(array[39])));
                                IOT_Slave1.UpdateCurrent(2, Convert.ToUInt16(ascii_to_hex(array[40]) * 4096 + ascii_to_hex(array[41]) * 256 +
                                                                            ascii_to_hex(array[42]) * 16 + ascii_to_hex(array[43])));
                                IOT_Slave1.UpdateCurrent(3, Convert.ToUInt16(ascii_to_hex(array[44]) * 4096 + ascii_to_hex(array[45]) * 256 +
                                                                            ascii_to_hex(array[46]) * 16 + ascii_to_hex(array[47])));
                                IOT_Slave1.UpdateCurrent(4, Convert.ToUInt16(ascii_to_hex(array[48]) * 4096 + ascii_to_hex(array[49]) * 256 +
                                                                            ascii_to_hex(array[50]) * 16 + ascii_to_hex(array[51])));

                                // update digital output 24v
                                this.btn_Slave1OutPut1.BackColor = IOT_Slave1.GetOutputState(1);
                                this.btn_Slave1OutPut2.BackColor = IOT_Slave1.GetOutputState(2);
                                this.btn_Slave1OutPut3.BackColor = IOT_Slave1.GetOutputState(3);
                                this.btn_Slave1OutPut4.BackColor = IOT_Slave1.GetOutputState(4);
                                this.btn_Slave1OutPut5.BackColor = IOT_Slave1.GetOutputState(5);
                                this.btn_Slave1OutPut6.BackColor = IOT_Slave1.GetOutputState(6);
                                this.btn_Slave1OutPut7.BackColor = IOT_Slave1.GetOutputState(7);
                                this.btn_Slave1OutPut8.BackColor = IOT_Slave1.GetOutputState(8);
                                // update relay
                                this.btn_Slave1Relay1.BackColor = IOT_Slave1.GetRelayState(1);
                                this.btn_Slave1Relay2.BackColor = IOT_Slave1.GetRelayState(2);
                                this.btn_Slave1Relay3.BackColor = IOT_Slave1.GetRelayState(3);
                                this.btn_Slave1Relay4.BackColor = IOT_Slave1.GetRelayState(4);
                                // update input 200V label
                                this.lab_Slave1InputV1.BackColor = IOT_Slave1.GetInputV(1);
                                this.lab_Slave1InputV2.BackColor = IOT_Slave1.GetInputV(2);
                                this.lab_Slave1InputV3.BackColor = IOT_Slave1.GetInputV(3);
                                this.lab_Slave1InputV4.BackColor = IOT_Slave1.GetInputV(4);
                                this.lab_Slave1InputV5.BackColor = IOT_Slave1.GetInputV(5);
                                this.lab_Slave1InputV6.BackColor = IOT_Slave1.GetInputV(6);
                                this.lab_Slave1InputV7.BackColor = IOT_Slave1.GetInputV(7);
                                this.lab_Slave1InputV8.BackColor = IOT_Slave1.GetInputV(8);

                                // update input sensor label
                                this.lab_Slave1InputS1.BackColor = IOT_Slave1.GetInputS(1);
                                this.lab_Slave1InputS2.BackColor = IOT_Slave1.GetInputS(2);
                                this.lab_Slave1InputS3.BackColor = IOT_Slave1.GetInputS(3);
                                this.lab_Slave1InputS4.BackColor = IOT_Slave1.GetInputS(4);

                                // update voltage value
                                this.txt_Slave1Vol1.Invoke(new MethodInvoker(delegate () { txt_Slave1Vol1.Text = String.Format("{0:0.##}", ((IOT_Slave1.GetVoltage(1) - 1952) / 1952) * 10); }));
                                this.txt_Slave1Vol2.Invoke(new MethodInvoker(delegate () { txt_Slave1Vol2.Text = String.Format("{0:0.##}", ((IOT_Slave1.GetVoltage(2) - 1952) / 1952) * 10); }));
                                this.txt_Slave1Vol3.Invoke(new MethodInvoker(delegate () { txt_Slave1Vol3.Text = String.Format("{0:0.##}", ((IOT_Slave1.GetVoltage(3) - 1952) / 1952) * 10); }));
                                this.txt_Slave1Vol4.Invoke(new MethodInvoker(delegate () { txt_Slave1Vol4.Text = String.Format("{0:0.##}", ((IOT_Slave1.GetVoltage(4) - 1952) / 1952) * 10); }));

                                // update curent value
                                this.txt_Slave1Cur1.Invoke(new MethodInvoker(delegate () { txt_Slave1Cur1.Text = String.Format("{0:0.##}", (IOT_Slave1.GetCurrent(1) / 4096) * 20); }));
                                this.txt_Slave1Cur2.Invoke(new MethodInvoker(delegate () { txt_Slave1Cur2.Text = String.Format("{0:0.##}", (IOT_Slave1.GetCurrent(2) / 4096) * 20); }));
                                this.txt_Slave1Cur3.Invoke(new MethodInvoker(delegate () { txt_Slave1Cur3.Text = String.Format("{0:0.##}", (IOT_Slave1.GetCurrent(3) / 4096) * 20); }));
                                this.txt_Slave1Cur4.Invoke(new MethodInvoker(delegate () { txt_Slave1Cur4.Text = String.Format("{0:0.##}", (IOT_Slave1.GetCurrent(4) / 4096) * 20); }));
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Control Slave 1
        private void Btn_Slave1Relay1_Click(object sender, EventArgs e)
        {
            if(btn_Slave1Relay1.BackColor == System.Drawing.Color.Red)
            {
                IOT_Slave1.SetRelay(1);
                btn_Slave1Relay1.BackColor = System.Drawing.Color.Green;
                CT_client1 = 1;
            }
            else if (btn_Slave1Relay1.BackColor == System.Drawing.Color.Green)
            {
                IOT_Slave1.ResetRelay(1);
                btn_Slave1Relay1.BackColor = System.Drawing.Color.Red;
                CT_client1 = 1;
            }
        }

        private void Btn_Slave1Relay2_Click(object sender, EventArgs e)
        {
            if (btn_Slave1Relay2.BackColor == System.Drawing.Color.Red)
            {
                IOT_Slave1.SetRelay(2);
                btn_Slave1Relay2.BackColor = System.Drawing.Color.Green;
                CT_client1 = 1;
            }
            else if (btn_Slave1Relay2.BackColor == System.Drawing.Color.Green)
            {
                IOT_Slave1.ResetRelay(2);
                btn_Slave1Relay2.BackColor = System.Drawing.Color.Red;
                CT_client1 = 1;
            }
        }

        private void Btn_Slave1Relay3_Click(object sender, EventArgs e)
        {
            if (btn_Slave1Relay3.BackColor == System.Drawing.Color.Red)
            {
                IOT_Slave1.SetRelay(3);
                btn_Slave1Relay3.BackColor = System.Drawing.Color.Green;
                CT_client1 = 1;
            }
            else if (btn_Slave1Relay3.BackColor == System.Drawing.Color.Green)
            {
                IOT_Slave1.ResetRelay(3);
                btn_Slave1Relay3.BackColor = System.Drawing.Color.Red;
                CT_client1 = 1;
            }
        }

        private void Btn_Slave1Relay4_Click(object sender, EventArgs e)
        {
            if (btn_Slave1Relay4.BackColor == System.Drawing.Color.Red)
            {
                IOT_Slave1.SetRelay(4);
                btn_Slave1Relay4.BackColor = System.Drawing.Color.Green;
                CT_client1 = 1;
            }
            else if (btn_Slave1Relay4.BackColor == System.Drawing.Color.Green)
            {
                IOT_Slave1.ResetRelay(4);
                btn_Slave1Relay4.BackColor = System.Drawing.Color.Red;
                CT_client1 = 1;
            }
        }

        private void Btn_Slave1OutPut1_Click(object sender, EventArgs e)
        {
            if (btn_Slave1OutPut1.BackColor == System.Drawing.Color.Red)
            {
                IOT_Slave1.SetOutput(1);
                btn_Slave1OutPut1.BackColor = System.Drawing.Color.Green;
                CT_client1 = 1;
            }
            else if (btn_Slave1OutPut1.BackColor == System.Drawing.Color.Green)
            {
                IOT_Slave1.ResetOutput(1);
                btn_Slave1OutPut1.BackColor = System.Drawing.Color.Red;
                CT_client1 = 1;
            }
        }

        private void Btn_Slave1OutPut2_Click(object sender, EventArgs e)
        {
            if (btn_Slave1OutPut2.BackColor == System.Drawing.Color.Red)
            {
                IOT_Slave1.SetOutput(2);
                btn_Slave1OutPut2.BackColor = System.Drawing.Color.Green;
                CT_client1 = 1;
            }
            else if (btn_Slave1OutPut2.BackColor == System.Drawing.Color.Green)
            {
                IOT_Slave1.ResetOutput(2);
                btn_Slave1OutPut2.BackColor = System.Drawing.Color.Red;
                CT_client1 = 1;
            }
        }

        private void Btn_Slave1OutPut3_Click(object sender, EventArgs e)
        {
            if (btn_Slave1OutPut3.BackColor == System.Drawing.Color.Red)
            {
                IOT_Slave1.SetOutput(3);
                btn_Slave1OutPut3.BackColor = System.Drawing.Color.Green;
                CT_client1 = 1;
            }
            else if (btn_Slave1OutPut3.BackColor == System.Drawing.Color.Green)
            {
                IOT_Slave1.ResetOutput(3);
                btn_Slave1OutPut3.BackColor = System.Drawing.Color.Red;
                CT_client1 = 1;
            }
        }

        private void Btn_Slave1OutPut4_Click(object sender, EventArgs e)
        {
            if (btn_Slave1OutPut4.BackColor == System.Drawing.Color.Red)
            {
                IOT_Slave1.SetOutput(4);
                btn_Slave1OutPut4.BackColor = System.Drawing.Color.Green;
                CT_client1 = 1;
            }
            else if (btn_Slave1OutPut4.BackColor == System.Drawing.Color.Green)
            {
                IOT_Slave1.ResetOutput(4);
                btn_Slave1OutPut4.BackColor = System.Drawing.Color.Red;
                CT_client1 = 1;
            }
        }

        private void Btn_Slave1OutPut5_Click(object sender, EventArgs e)
        {
            if (btn_Slave1OutPut5.BackColor == System.Drawing.Color.Red)
            {
                IOT_Slave1.SetOutput(5);
                btn_Slave1OutPut5.BackColor = System.Drawing.Color.Green;
                CT_client1 = 1;
            }
            else if (btn_Slave1OutPut5.BackColor == System.Drawing.Color.Green)
            {
                IOT_Slave1.ResetOutput(5);
                btn_Slave1OutPut5.BackColor = System.Drawing.Color.Red;
                CT_client1 = 1;
            }
        }

        private void Btn_Slave1OutPut6_Click(object sender, EventArgs e)
        {
            if (btn_Slave1OutPut6.BackColor == System.Drawing.Color.Red)
            {
                IOT_Slave1.SetOutput(6);
                btn_Slave1OutPut6.BackColor = System.Drawing.Color.Green;
                CT_client1 = 1;
            }
            else if (btn_Slave1OutPut6.BackColor == System.Drawing.Color.Green)
            {
                IOT_Slave1.ResetOutput(6);
                btn_Slave1OutPut6.BackColor = System.Drawing.Color.Red;
                CT_client1 = 1;
            }
        }

        private void Btn_Slave1OutPut7_Click(object sender, EventArgs e)
        {
            if (btn_Slave1OutPut7.BackColor == System.Drawing.Color.Red)
            {
                IOT_Slave1.SetOutput(7);
                btn_Slave1OutPut7.BackColor = System.Drawing.Color.Green;
                CT_client1 = 1;
            }
            else if (btn_Slave1OutPut7.BackColor == System.Drawing.Color.Green)
            {
                IOT_Slave1.ResetOutput(7);
                btn_Slave1OutPut7.BackColor = System.Drawing.Color.Red;
                CT_client1 = 1;
            }
        }

        private void Btn_Slave1OutPut8_Click(object sender, EventArgs e)
        {
            if (btn_Slave1OutPut8.BackColor == System.Drawing.Color.Red)
            {
                IOT_Slave1.SetOutput(8);
                btn_Slave1OutPut8.BackColor = System.Drawing.Color.Green;
                CT_client1 = 1;
            }
            else if (btn_Slave1OutPut8.BackColor == System.Drawing.Color.Green)
            {
                IOT_Slave1.ResetOutput(8);
                btn_Slave1OutPut8.BackColor = System.Drawing.Color.Red;
                CT_client1 = 1;
            }
        }

        private void Btn_Slave1SetAnalog_Click(object sender, EventArgs e)
        {
            double value;
            if (txt_Slave1Analog1.Text != "")
            {
                value = Convert.ToDouble(txt_Slave1Analog1.Text);
                if (value > 10) value = 10;
                if (value < 0) value = 0;
                IOT_Slave1.SetAnalog(1, value);
            }

            if (txt_Slave1Analog2.Text != "")
            {
                value = Convert.ToDouble(txt_Slave1Analog2.Text);
                if (value > 10) value = 10;
                if (value < 0) value = 0;
                IOT_Slave1.SetAnalog(2, value);
            }
            if (txt_Slave1Analog1.Text != "" || txt_Slave1Analog2.Text != "")
            {
                CT_client1 = 1;
            }
        }

        private void Btn_Slave1SetTimeUpdate_Click(object sender, EventArgs e)
        {
            if(txt_Slave1TimeUpdate.Text != "")
            {
                double value = Convert.ToDouble(txt_Slave1TimeUpdate.Text);
                IOT_Slave1.SetTimeUpdate(value);
                CT_client1 = 2;
            }
        }

        private void Btn_Slave1SetRealTime_Click(object sender, EventArgs e)
        {
            CT_client1 = 3;
        }

        private void Txt_Slave1Id_TextChanged(object sender, EventArgs e)
        {
            if(txt_Slave1Id.Text != "")
            {
                double value = Convert.ToDouble(txt_Slave1Id.Text);
                IOT_Slave1.SetID(Convert.ToUInt16(value));
            }
        }
        #endregion

        private char ascii_to_hex(UInt16 ascii)
        {
            if (ascii > 0x39) return Convert.ToChar(ascii - 0x37);
            else return Convert.ToChar(ascii - 0x30);
        }

        private void KeyPress_GetDigit(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            listener.Stop();
        }
    }
}
