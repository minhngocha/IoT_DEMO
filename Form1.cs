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
                        IOT_Slave1.slaveID = 0x01;
                        sw.Write(IOT_Slave1.ControlOutput());
                    }
                    else if (CT_client1 == 2)
                    {
                        CT_client1 = -1;
                        IOT_Slave1.slaveID = 0x01;
                        sw.Write(IOT_Slave1.SetTimeUpdate());
                    }
                    else if (CT_client1 == 3)
                    {
                        CT_client1 = -1;
                        IOT_Slave1.slaveID = 0x01;
                        sw.Write(IOT_Slave1.SetRealTime());
                    }
                }
                else
                {
                    client.Close();
                }
            }
        }











        /* Handle data client 1 */
        private void handle_data_client1(char[] array, int length)
        {
            int data_handle = length;
            while (data_handle > 0)
            {
                data_handle--;
                
                /* */
                if (array[0] == '#')
                {
                    if (array[1] == '#')
                    {
                        if (array[2] == IOT_Slave1.GetIDByte1())
                        {
                            if (array[3] == IOT_Slave1.GetIDByte2())
                            {
                                if (array[4] == '1')
                                {
                                    pic_Slave1Connect.BackColor = System.Drawing.Color.Green;
                                }
                            }
                        }
                    }
                }
            }
        }

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

        private void Txt_Slave1Analog1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if(!char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void Txt_Slave1Analog2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void Btn_Slave1SetAnalog_Click(object sender, EventArgs e)
        {
            double value = Convert.ToDouble(txt_Slave1Analog1.Text);
            if (value > 10) value = 10;
            if (value < 0) value = 0;
            IOT_Slave1.SetAnalog(1, value);

            value = Convert.ToDouble(txt_Slave1Analog2.Text);
            if (value > 10) value = 10;
            if (value < 0) value = 0;
            IOT_Slave1.SetAnalog(2, value);
            CT_client1 = 1;
        }

        private void Txt_Slave1TimeUpdate_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void Btn_Slave1SetTimeUpdate_Click(object sender, EventArgs e)
        {
            double value = Convert.ToDouble(txt_Slave1TimeUpdate.Text);
            IOT_Slave1.SetTimeUpdate(value);
            CT_client1 = 2;
        }

        private void Btn_Slave1SetRealTime_Click(object sender, EventArgs e)
        {
            CT_client1 = 3;
        }

        private void Txt_Slave1Id_TextChanged(object sender, EventArgs e)
        {
            double value = Convert.ToDouble(txt_Slave1Id.Text);
            IOT_Slave1.SetID(Convert.ToUInt16(value));
        }

        private void Txt_Slave1Id_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }
    }
}
