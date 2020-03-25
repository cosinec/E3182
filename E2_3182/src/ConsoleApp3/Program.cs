using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nameSpace1
{
    using System;
    using System.IO.Ports;
    using System.Threading;

    public class Program
    {
        static bool continue_;
        static SerialPort Port;

        public static void Main()
        {
            string message;
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            Thread readThread = new Thread(Read);

            // 创建串口对象
            Port = new SerialPort();

            // 设置端口对象的属性
            Port.PortName = PortName(Port.PortName);
            Port.BaudRate = setPBR(Port.BaudRate);
            Port.Parity = portParity(Port.Parity);
            Port.DataBits = portDateBits(Port.DataBits);
            Port.StopBits = stopBits(Port.StopBits);
            Port.Handshake = PortHandshake(Port.Handshake);

            //超时时间
            Port.ReadTimeout = 500;
            Port.WriteTimeout = 500;

            Port.Open();
            continue_ = true;
            readThread.Start();

            Console.WriteLine("退出请输入“quit”");

            while (continue_)
            {
                message = Console.ReadLine();

                if (stringComparer.Equals("quit", message))
                {
                    continue_ = false;
                }
                else
                {
                    System.DateTime currentTime = new System.DateTime();
                    currentTime = System.DateTime.Now;
                    string time_s = currentTime.ToString();
                    message = "[SENT " + time_s + "] " + message;//发送信息
                    Console.WriteLine(message);//在控制台输出发送的信息
                    Port.WriteLine(
                        String.Format("{0}", message));
                }
            }

            readThread.Join();
            Port.Close();
        }

        public static void Read()
        {
            while (continue_)
            {
                try
                {
                    string message = Port.ReadLine();
                    System.DateTime currentTime = new System.DateTime();
                    currentTime = System.DateTime.Now;
                    string strTime = currentTime.ToString();
                    message = "[REVC " + strTime + "] " + message;//接收串口信息
                    Console.WriteLine(message);//在控制台输出接收的信息
                }
                catch (TimeoutException) { }
            }
        }

        //设置端口
        public static string PortName(string defaultPortName)
        {
            string nameOfPort;
            Console.WriteLine("参数设置：");
            Console.Write("可选端口:");
            foreach (string s in SerialPort.GetPortNames())
            {
                Console.Write("   {0}   ", s);
            }
            Console.WriteLine();
            Console.Write("输出 COM 端口值，不区分大小写(默认: {0}): ", defaultPortName);
            nameOfPort = Console.ReadLine();

            if (nameOfPort == "" || !(nameOfPort.ToLower()).StartsWith("com"))
            {
                Console.WriteLine("使用端口默认值");
                nameOfPort = defaultPortName;
            }
            Console.WriteLine();
            return nameOfPort;
        }

        // 设置波特率
        public static int setPBR(int defaultPBR)
        {
            string baudRate;

            Console.Write("输入波特率(默认:{0}): ", defaultPBR);
            baudRate = Console.ReadLine();

            if (baudRate == "")
            {
                Console.WriteLine("使用波特率默认值");
                baudRate = defaultPBR.ToString();
            }
            Console.WriteLine();
            return int.Parse(baudRate);
        }

        // 设置端口奇偶值
        public static Parity portParity(Parity defaultPortParity)
        {
            string parity;

            Console.Write("可用奇偶值:");
            foreach (string s in Enum.GetNames(typeof(Parity)))
            {
                Console.Write("   {0}   ", s);
            }
            Console.WriteLine();
            Console.Write("输入奇偶值参数(默认: {0}):", defaultPortParity.ToString(), true);
            parity = Console.ReadLine();

            if (parity == "")
            {
                Console.WriteLine("使用奇偶值默认值");
                parity = defaultPortParity.ToString();
            }
            Console.WriteLine();
            return (Parity)Enum.Parse(typeof(Parity), parity, true);
        }
        //设置数据位
        public static int portDateBits(int defaultPortDataBits)
        {
            string dataBits;

            Console.Write("设置数据位 (默认: {0}): ", defaultPortDataBits);
            dataBits = Console.ReadLine();

            if (dataBits == "")
            {
                Console.WriteLine("使用数据位默认值");
                dataBits = defaultPortDataBits.ToString();
            }
            Console.WriteLine();
            return int.Parse(dataBits.ToUpperInvariant());
        }

        //设置停止位
        public static StopBits stopBits(StopBits defaultStopBits)
        {
            string stopBits;

            Console.Write("可用停止位:");
            foreach (string s in Enum.GetNames(typeof(StopBits)))
            {
                Console.Write("   {0}  ", s);
            }
            Console.WriteLine();
            Console.Write("输入停止位 (默认: {0}):", defaultStopBits.ToString());
            stopBits = Console.ReadLine();

            if (stopBits == "")
            {
                Console.WriteLine("使用停止位默认值");
                stopBits = defaultStopBits.ToString();
            }
            Console.WriteLine();
            return (StopBits)Enum.Parse(typeof(StopBits), stopBits, true);
        }
        public static Handshake PortHandshake(Handshake defaultPortHandshake)
        {
            string handshake = defaultPortHandshake.ToString();
            return (Handshake)Enum.Parse(typeof(Handshake), handshake, true);
        }
    }
}

