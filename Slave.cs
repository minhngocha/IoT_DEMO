using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT_DEMO
{
    public class Slave
    {
        /* parameter */
        private int slaveID;
        public int func_code_handle;
        public int func_code_control;
        /* */
        private int response_flag;
        /* Output Control */
        private UInt16 relay = 0;
        private UInt16 coil = 0;
        private UInt16 analog_out_1 = 0;
        private UInt16 analog_out_2 = 0;
        private UInt16 time_update = 0;
        /* Input state */
        private UInt16 input_220V = 0;
        private UInt16 input_sensor = 0;
        private UInt16[] voltage = new UInt16[4];
        private UInt16[] current = new UInt16[4];
        /* Crc */
        private UInt16 CRC;
        private char hex_to_ascii(UInt16 hex)
        {
            if (hex <= 9) return Convert.ToChar(hex + 0x30);
            else return Convert.ToChar(hex + 0x40 - 0x09);
        }

        private UInt16 calculateCRC(char[] array, int add_start, int size)
        {
            UInt16 temp, temp2, temp1, flag;
            temp = 0xFFFF;

            for (int i = add_start; i < add_start + size; i++)
            {
                temp = Convert.ToUInt16(temp ^ (array[i]));
                for (int j = 1; j <= 8; j++)
                {
                    flag = Convert.ToUInt16(temp & 0x0001);
                    temp >>= 1;
                    if (flag > 0)
                        temp ^= 0xA001;
                }
            }
            temp2 = Convert.ToUInt16(temp / 256);
            temp1 = Convert.ToUInt16(temp % 256);
            temp = Convert.ToUInt16(temp1 * 256 + temp2);
            return temp;
        }

        public void SetID(UInt16 id)
        {
            slaveID = id;
        }

        public char GetIDByte1()
        {
            return hex_to_ascii(Convert.ToChar(slaveID / 16));
        }

        public char GetIDByte2()
        {
            return hex_to_ascii(Convert.ToChar(slaveID % 16));
        }

        public void SetResponseFlag(UInt16 respoinse_code)
        {
            response_flag = respoinse_code;
        }

        public void ResetResponseFlag()
        {
            response_flag = -1;
        }
        public int GetResponseFlag()
        {
            return response_flag;
        }

        public void SetTimeUpdate(double value)
        {
            time_update = Convert.ToUInt16(value);
        }

        public void UpdateRelay(UInt16 value)
        {
            relay = value;
        }

        public void SetRelay(int select)
        {
            switch(select)
            {
                case 1:
                    relay |= 0x01;
                    break;
                case 2:
                    relay |= 0x02;
                    break;
                case 3:
                    relay |= 0x04;
                    break;
                case 4:
                    relay |= 0x08;
                    break;
            }
        }

        public void ResetRelay(int select)
        {
            switch (select)
            {
                case 1:
                    relay &= 0xFE;
                    break;
                case 2:
                    relay &= 0xFD;
                    break;
                case 3:
                    relay &= 0xFB;
                    break;
                case 4:
                    relay &= 0xF7;
                    break;
            }
        }

        public void UpdateOutput(UInt16 value)
        {
            coil = value;
        }

        public void SetOutput(int select)
        {
            switch (select)
            {
                case 1:
                    coil |= 0x01;
                    break;
                case 2:
                    coil |= 0x02;
                    break;
                case 3:
                    coil |= 0x04;
                    break;
                case 4:
                    coil |= 0x08;
                    break;
                case 5:
                    coil |= 0x10;
                    break;
                case 6:
                    coil |= 0x20;
                    break;
                case 7:
                    coil |= 0x40;
                    break;
                case 8:
                    coil |= 0x80;
                    break;
            }
        }

        public void ResetOutput(int select)
        {
            switch (select)
            {
                case 1:
                    coil &= 0xFE;
                    break;
                case 2:
                    coil &= 0xFD;
                    break;
                case 3:
                    coil &= 0xFB;
                    break;
                case 4:
                    coil &= 0xF7;
                    break;
                case 5:
                    coil &= 0xEF;
                    break;
                case 6:
                    coil &= 0xDF;
                    break;
                case 7:
                    coil &= 0xBF;
                    break;
                case 8:
                    coil &= 0x7F;
                    break;
            }
        }

        public void UpdateAnalog(int channel, UInt16 value)
        {
            switch(channel)
            {
                case 1:
                    analog_out_1 = value;
                    break;
                case 2:
                    analog_out_1 = value;
                    break;
            }
        }

        public void SetAnalog(int channel, double value)
        {
            switch(channel)
            {
                case 1:
                    analog_out_1 = Convert.ToUInt16((value / 10) * 0xFFF);
                    break;
                case 2:
                    analog_out_2 = Convert.ToUInt16((value / 10) * 0xFFF);
                    break;
            }
        }

        public void UpdateInputV(UInt16 value)
        {
            input_220V = value;
        }

        public void UpdateInputS(UInt16 value)
        {
            input_sensor = value;
        }

        public void UpdateVoltage(int channel, UInt16 value)
        {
            voltage[channel - 1] = value;
        }

        public void UpdateCurrent(int channel, UInt16 value)
        {
            current[channel - 1] = value;
        }

        public System.Drawing.Color GetRelayState(UInt16 channel)
        {
            if ((relay & Convert.ToUInt16(0x01 << (channel - 1))) == Convert.ToUInt16(0x01 << (channel - 1)))
            {
                return System.Drawing.Color.Green;
            }
            else
            {
                return System.Drawing.Color.Red;
            }
        }

        public System.Drawing.Color GetOutputState(UInt16 channel)
        {
            if ((coil & Convert.ToUInt16(0x01 << (channel - 1))) == Convert.ToUInt16(0x01 << (channel - 1)))
            {
                return System.Drawing.Color.Green;
            }
            else
            {
                return System.Drawing.Color.Red;
            }
        }

        public System.Drawing.Color GetInputV(UInt16 channel)
        {
            if ((input_220V & Convert.ToUInt16(0x01 << (channel - 1))) == Convert.ToUInt16(0x01 << (channel - 1)))
            {
                return System.Drawing.Color.Green;
            }
            else
            {
                return System.Drawing.Color.Red;
            }
        }

        public System.Drawing.Color GetInputS(UInt16 channel)
        {
            if ((input_sensor & Convert.ToUInt16(0x01 << (channel - 1))) == Convert.ToUInt16(0x01 << (channel - 1)))
            {
                return System.Drawing.Color.Green;
            }
            else
            {
                return System.Drawing.Color.Red;
            }
        }

        public UInt16 GetVoltage(UInt16 channel)
        {
            return voltage[channel - 1];
        }

        public UInt16 GetCurrent(UInt16 channel)
        {
            return current[channel - 1];
        }

        public String SetRealTime()
        {
            char[] tx_buf = new char[21];
            DateTime now = DateTime.Now;
            /* Add header byte*/
            tx_buf[0] = '#';
            tx_buf[1] = '#';
            /* Add slave ID*/
            tx_buf[2] = hex_to_ascii(Convert.ToUInt16(slaveID / 16));
            tx_buf[3] = hex_to_ascii(Convert.ToUInt16(slaveID % 16));
            /* Add function code */
            tx_buf[4] = '3';
            /* Add hour */
            tx_buf[5] = hex_to_ascii(Convert.ToChar(now.Hour / 16));
            tx_buf[6] = hex_to_ascii(Convert.ToChar(now.Hour % 16));
            /* *Add minute */
            tx_buf[7] = hex_to_ascii(Convert.ToChar(now.Minute / 16));
            tx_buf[8] = hex_to_ascii(Convert.ToChar(now.Minute % 16));
            /* Add second */
            tx_buf[9] = hex_to_ascii(Convert.ToChar(now.Second / 16));
            tx_buf[10] = hex_to_ascii(Convert.ToChar(now.Second % 16));
            /* *Add date */
            tx_buf[11] = hex_to_ascii(Convert.ToChar(now.Day / 16));
            tx_buf[12] = hex_to_ascii(Convert.ToChar(now.Day % 16));
            /* Add month */
            tx_buf[13] = hex_to_ascii(Convert.ToChar(now.Month / 16));
            tx_buf[14] = hex_to_ascii(Convert.ToChar(now.Month % 16));
            /* *Add year */
            tx_buf[15] = hex_to_ascii(Convert.ToChar((now.Year - 2000) / 16));
            tx_buf[16] = hex_to_ascii(Convert.ToChar((now.Year - 2000) % 16));
            /* calculate CRC */
            CRC = calculateCRC(tx_buf, 2, 15);
            tx_buf[17] = hex_to_ascii(Convert.ToChar((CRC / 256) / 16));
            tx_buf[18] = hex_to_ascii(Convert.ToChar((CRC / 256) % 16));
            tx_buf[19] = hex_to_ascii(Convert.ToChar((CRC % 256) / 16));
            tx_buf[20] = hex_to_ascii(Convert.ToChar((CRC % 256) % 16));
            String str_return = new string(tx_buf);
            return str_return;
        }

        public String SetTimeUpdate()
        {
            char[] tx_buf = new char[13];
            /* Add header byte*/
            tx_buf[0] = '#';
            tx_buf[1] = '#';
            /* Add slave ID*/
            tx_buf[2] = hex_to_ascii(Convert.ToUInt16(slaveID / 16));
            tx_buf[3] = hex_to_ascii(Convert.ToUInt16(slaveID % 16));
            /* Add function code */
            tx_buf[4] = '5';
            /* Add time update */
            tx_buf[5] = hex_to_ascii(Convert.ToChar((time_update / 256) / 16));
            tx_buf[6] = hex_to_ascii(Convert.ToChar((time_update / 256) % 16));
            tx_buf[7] = hex_to_ascii(Convert.ToChar((time_update % 256) / 16));
            tx_buf[8] = hex_to_ascii(Convert.ToChar((time_update % 256) % 16));
            /* calculate CRC */
            CRC = calculateCRC(tx_buf, 2, 7);
            tx_buf[9] = hex_to_ascii(Convert.ToChar((CRC / 256) / 16));
            tx_buf[10] = hex_to_ascii(Convert.ToChar((CRC / 256) % 16));
            tx_buf[11] = hex_to_ascii(Convert.ToChar((CRC % 256) / 16));
            tx_buf[12] = hex_to_ascii(Convert.ToChar((CRC % 256) % 16));
            String str_return = new string(tx_buf);
            return str_return;
        }

        public String ControlOutput()
        {
            char[] tx_buf = new char[20];
            /* Add header byte*/
            tx_buf[0] = '#';
            tx_buf[1] = '#';
            /* Add slave ID*/
            tx_buf[2] = hex_to_ascii(Convert.ToUInt16(slaveID / 16));
            tx_buf[3] = hex_to_ascii(Convert.ToUInt16(slaveID % 16));
            /* Add function code */
            tx_buf[4] = '4';
            /* Add relay state */
            tx_buf[5] = hex_to_ascii(Convert.ToChar(relay));
            /* Add relay coil */
            tx_buf[6] = hex_to_ascii(Convert.ToChar(coil / 16));
            tx_buf[7] = hex_to_ascii(Convert.ToChar(coil % 16));
            /* Add analog 1 */
            tx_buf[8] = hex_to_ascii(Convert.ToChar((analog_out_1 / 256) / 16));
            tx_buf[9] = hex_to_ascii(Convert.ToChar((analog_out_1 / 256) % 16));
            tx_buf[10] = hex_to_ascii(Convert.ToChar((analog_out_1 % 256) / 16));
            tx_buf[11] = hex_to_ascii(Convert.ToChar((analog_out_1 % 256) % 16));
            /* Add analog 2 */
            tx_buf[12] = hex_to_ascii(Convert.ToChar((analog_out_2 / 256) / 16));
            tx_buf[13] = hex_to_ascii(Convert.ToChar((analog_out_2 / 256) % 16));
            tx_buf[14] = hex_to_ascii(Convert.ToChar((analog_out_2 % 256) / 16));
            tx_buf[15] = hex_to_ascii(Convert.ToChar((analog_out_2 % 256) % 16));
            /* calculate CRC */
            CRC = calculateCRC(tx_buf, 2, 14);
            tx_buf[16] = hex_to_ascii(Convert.ToChar((CRC / 256) / 16));
            tx_buf[17] = hex_to_ascii(Convert.ToChar((CRC / 256) % 16));
            tx_buf[18] = hex_to_ascii(Convert.ToChar((CRC % 256) / 16));
            tx_buf[19] = hex_to_ascii(Convert.ToChar((CRC % 256) % 16));
            String str_return = new string(tx_buf);
            return str_return;
        }
    }
}
