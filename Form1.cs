using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Driver;


namespace cosmosDatalog
{
    public partial class Form1 : Form
    {
        /*---------------------------------------------------------------------------*/
        /*                                                                           */
        /*---------------------------------------------------------------------------*/
        static class Constants
        {
            public const int TEXT_LENGTH = 2000;
            public const int DIGIT_NUM = 10;			// number of digit for string buffer 
            public const int DEC_POINT = 3;			    // number of decimal point for all data
            public const int DATA_NUM = 10;			    // number of data to read from PLC (DT)
            public const int PROD_NUM = 50;			    // number of product
            public const int LOG_BYTE_NUM = 105;        // number of byte of one row of log data
            public const int LOG_START_BYTE_NUM = 182;  // number of byte of one row of log data 
            public const int READ_DELAY = 800;  // number of byte of one row of log data 
        }

        //SerialPort[] _serialPort = new SerialPort[2];
        string convertDec;
        string[] PortNo = new string[2];
        string[] LogFileName = new string[2];
        char[] send_data = new char[Constants.TEXT_LENGTH];
        string read_data, read_data1, read_data2, bufStore, buf;
        int ValToAsc, DataReady = 0, toggle = 0, LogCnt = 0;
        bool RunFlag = false, bClose = false;
        bool bTimer1 = false;
        string sCmd;

        //const string DIR_LOG1 = @"C:/Machine 23-104 (IBC)/";
        //const string DIR_LOG2 = @"C:/Machine 23-105 (Drum Line 1)/";
        //const string DIR_LOG3 = @"C:/Machine 23-106 (Drum Line 2)/";

        const string INIT =@"c:/Users/cosmosInit.ini";

        //const String MAT_NO = "D0502005025";
        //const String PROD_NAME = "D0504005049";
        //const String PROC_ORDER = "D0506005065";
        //const String BATCH_NO = "D0508005086";
        //const String PROD_WT = "D0512005120";
        //const String BATCH_START = "D0513005132";
        //const String BATCH_END = "D0513505137";
        //const String USER_ID = "D0510005103";
        //const String TOT_FILLED = "D0512505126";
        const String PLC_DATA_RDY = "D2912029120";// PLC Write 1 when Data Ready
        const String PC_ON = "D0030000300"; // PC write 1 every 1 s
        //const String PLC_ON = "D00500405004"; // PC write 1 every 1 s
        //const String BATCH_START_RDY = "D0500105001";
        //const String BATCH_END_RDY = "D0500205002";
       
        const String DATA1 = "D2912129138";
        const String DATA2 = "D2913929150";

        const int DELAY_RD = 800;

        public Form1()
        {
            InitializeComponent();
            GetInit();
         
        }

        //****************************************************************//
        //   
        //
        //****************************************************************//
        void GetInit()
        {

       
            try
            {
                /*

                if (!File.Exists(INIT))
                {
                    File.Create(INIT).Dispose();
                    using (TextWriter tw = new StreamWriter(INIT))
                    {
                        tw.WriteLine("COM1");
                        PortNo[0] = "COM1";
                        tw.WriteLine("COM2");
                        PortNo[1] = "COM2";

                    }
                }
                else if (File.Exists(INIT))
                {
                    using (StreamReader sr = File.OpenText(INIT))
                    {
                        PortNo[0] = sr.ReadLine();
                        PortNo[1] = sr.ReadLine();
                    }
                }
                cbPort.Text = PortNo[0];
                */
                PortNo[0] = "COM2";
                cbPort.Text = "COM2";


            }
            catch (SystemException e)
            {
                PortNo[0] = "COM2";
                MessageBox.Show(e.Message, "ERROR"); }






        }//void GetInit

        void chkDir(string dir)
        {
            // check directory, if not exist; create
            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(dir))
                {
                    //Console.WriteLine("That path exists already.");
                    return;
                }

                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(dir);
                //Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));

            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }

        }

        //****************************************************************//
        //   
        //
        //****************************************************************//
        void Open_Port()
        {

            int i = 0;

            try
            {
                if (_serialPort.IsOpen == false)
                {

                    _serialPort.PortName = PortNo[i];
                    _serialPort.DataBits = 7;
                    _serialPort.Parity = System.IO.Ports.Parity.Even;
                    _serialPort.StopBits = System.IO.Ports.StopBits.Two;
                    _serialPort.Open();
                }// if
            }
            catch (Exception e)
            { MessageBox.Show(e.Message, "ERROR");
                
            }



        }//void Open_Port

        //****************************************************************//
        //
        //****************************************************************//
        void Close_Port()
        {
            for (int i = 0; i < 2; i++)
            {
                if (_serialPort.IsOpen == true)
                {
                    _serialPort.Close();
                }//if
            }//for
            //ovalStart.Enabled = true;
        }

        void Run()
        {
            int iPlcNum = 1;
            timer1.Enabled = true;
            do
            {
                SetPcOn(iPlcNum); 
                ScanPLC(iPlcNum);
             
                if (DataReady == 1)
                {
                    DataReady = 0;
                   
                    //ChkLogFile(iPlcNum);
                    //LogBatchEnd(iPlcNum);
                    HandleData(iPlcNum);
                    RstDataRdy(iPlcNum, 0);
                }//if (DataReady == 1)

                iPlcNum++;
                if (iPlcNum > 3)
                {
                    iPlcNum = 1;
                }               

                if (bClose)
                {
                    Close_Port();
                    RunFlag = false;
                    bClose = false;
                    timer1.Enabled = false;

                }//if (bClose)

                Application.DoEvents();
            } while (RunFlag);

            lbRunStop.Text = "  STOP  ";
            lbRunStop.ForeColor = System.Drawing.Color.Red;
            pBox_Run.BackColor = System.Drawing.Color.Red;
            cbPort.Enabled = true;

        }// void Run()

        /*---------------------------------------------------------------------------*/
        /*                                                                           */
        /*---------------------------------------------------------------------------*/
        void GetLogFile(int numb, string FileName)
        {
            // Get File name for log
        /*
            switch (numb)
            {
                case 1:
                    LogFileName[0] = DIR_LOG1 + FileName  + ".csv";
                    break;
                case 2:
                    LogFileName[1] = DIR_LOG2 + FileName + ".csv";
                    break;
                case 3:
                    LogFileName[2] = DIR_LOG3 + FileName + ".csv";
                    break;  
                default:
                    LogFileName[0] = DIR_LOG1 + FileName + ".csv";
                    break;
            }//swith
        */
        }//void GetLogFile(int numb)

        /*---------------------------------------------------------------------------*/
        /*                                                                           */
        /*---------------------------------------------------------------------------*/
        void ChkLogFile(int numb)
        {
            /*
            bool bFileExist = false;
            string sWrtData, sPlcNumb, sBatch_Start, sBatch_Start_DT,  sProc_Order, sFileName;
            string sMat_No, sProd_Name, sBatch_No,sProd_Wt, sBatch_End="", sUser;
            
            // read batch start date
            sPlcNumb = "0" + numb.ToString();
            sWrtData = ("%" + sPlcNumb + "#RD" + BATCH_START + "**\r");           
            _serialPort.Write(sWrtData);
            Thread.Sleep(DELAY_RD);
            read_data2 = "";
            read_data2 = _serialPort.ReadExisting();
            ConvertPLC_DateTime("d", 6+ (1*4), read_data2);
            sBatch_Start = buf;
            ConvertPLC_DateTime("t", 6+(0 * 4), read_data2);
            sBatch_Start_DT = sBatch_Start + " " + buf;

           
            // read process order 
            sWrtData = ("%" + sPlcNumb + "#RD" + PROC_ORDER + "**\r");
            _serialPort.Write(sWrtData);
            Thread.Sleep(DELAY_RD);
            read_data2 = "";
            read_data2 = _serialPort.ReadExisting();
            ConvertToAlphStr(6, 6 * 4, read_data2); // startpos,length,string
            sProc_Order = buf;

            sFileName = sBatch_Start + "_" + sProc_Order;
            sFileName = sFileName.Trim();
            
            GetLogFile(numb, sFileName);

            
     
            

            try
            {
                // check is file already created
                if (File.Exists(@LogFileName[numb-1])) { bFileExist = true; }
                if (bFileExist) { LogBatchEnd(numb); }
                if (!bFileExist)
                {
                    // Open File for append
                    using (FileStream F = new FileStream(@LogFileName[numb-1], FileMode.Append))
                    {
                        using (StreamWriter writetext = new StreamWriter(F))
                        {                            
                            string sMch_Name;

                            switch (numb)
                            {
                                case 2:
                                    sMch_Name = "Machine 23-106 (Drum Line 1)";
                                    break;
                                case 3:
                                    sMch_Name = "Machine 23-105 (Drum Line 2)";
                                    break;
                                default:
                                    sMch_Name = "MACHINE 23-104 (IBC)"; 
                                    break;
                            }


                            // read Materail number 
                            sWrtData = ("%" + sPlcNumb + "#RD" + MAT_NO + "**\r");
                            _serialPort.Write(sWrtData);
                            Thread.Sleep(DELAY_RD);
                            read_data2 = "";
                            read_data2 = _serialPort.ReadExisting();
                            ConvertToAlphStr(6, 6 * 4, read_data2); // startpos,length,string
                            sMat_No = buf;

                            // read ProdName 
                            sWrtData = ("%" + sPlcNumb + "#RD" + PROD_NAME + "**\r");
                            _serialPort.Write(sWrtData);
                            Thread.Sleep(DELAY_RD);
                            read_data2 = "";
                            read_data2 = _serialPort.ReadExisting();
                            ConvertToAlphStr(6, 10 * 4, read_data2); // startpos,length,string
                            sProd_Name = buf;

                            // read Batch number 
                            sWrtData = ("%" + sPlcNumb + "#RD" + BATCH_NO + "**\r");
                            _serialPort.Write(sWrtData);
                            Thread.Sleep(DELAY_RD);
                            read_data2 = "";
                            read_data2 = _serialPort.ReadExisting();
                            ConvertToAlphStr(6, 7 * 4, read_data2); // startpos,length,string
                            sBatch_No = buf;

                            // read Product Weight 
                            sWrtData = ("%" + sPlcNumb + "#RD" + PROD_WT + "**\r");
                            _serialPort.Write(sWrtData);
                            Thread.Sleep(DELAY_RD);
                            read_data2 = "";
                            read_data2 = _serialPort.ReadExisting();
                            ConvertToDecStr(1, 6, read_data2); // (int decPos, int startPos , string read_data )  	     			
                            sProd_Wt = convertDec;

                            // read User ID 
                            sWrtData = ("%" + sPlcNumb + "#RD" + USER_ID + "**\r");
                            _serialPort.Write(sWrtData);
                            Thread.Sleep(DELAY_RD);
                            read_data2 = "";
                            read_data2 = _serialPort.ReadExisting();
                            ConvertToAlphStr(6, 4 * 4, read_data2); // startpos,length,string
                            sUser = buf;


                            writetext.WriteLine(","+ sMch_Name);
                            writetext.WriteLine("Material No.," + sMat_No);
                            writetext.WriteLine("Process Order," + sProc_Order );
                            writetext.WriteLine("Batch Number," + sBatch_No);
                            writetext.WriteLine("Product Weight (kg)," + sProd_Wt);
                            writetext.WriteLine("Batch Start Time," + sBatch_Start_DT);
                            writetext.WriteLine("Batch End Time," + sBatch_End);
                            writetext.WriteLine("User ID/Name," + sUser);
                            writetext.WriteLine("Total Filled per Batch(kg)," + sTot_Fill);

                            writetext.WriteLine("");
                            writetext.WriteLine("Drum No.,ISO Tank No.,Qr Code Number,Tare Wt.(kg),Gross Wt.(kg),Net Wt.(kg),Filling Tolerance (kg),Start Fill Time,End Fill Time");
                            rtbStatus.Text = rtbStatus.Text + "line " + numb.ToString() + " log file created \r\n";
                         



                        }//using (StreamW
                    }//using (FileS
                }//if (!bFileExist)
            }//try
            catch (SystemException e)
            {
                rtbStatus.Text = rtbStatus.Text + e.Message + "\r\n";
            }

          */
        }// ChkLogFile(

        /*---------------------------------------------------------------------------*/
        /*                                                                           */
        /*---------------------------------------------------------------------------*/
        void LogBatchEnd( int numb)
        {
            /*

            string sWrtData, sBATCH_END, sPlcNumb;


            // DT? == "0100" i.e Batch End
            // set DT? to "0101" i.e logfile create
            sPlcNumb = "0" + numb.ToString();
            sWrtData = ("%" + sPlcNumb + "#RD" + BATCH_END_RDY + "**\r");

            _serialPort.Write(sWrtData);
            Thread.Sleep(Constants.READ_DELAY);

            read_data2 = "";
            read_data2 = _serialPort.ReadExisting();

            try
            {

                string ss = read_data2.Substring(6, 4);
                // batch response already
                if (ss == "0100")
                {
                    // read batch end
                    sWrtData = ("%" + "01" + "#RD" + BATCH_END + "**\r");
                    _serialPort.Write(sWrtData);
                    Thread.Sleep(DELAY_RD);
                    read_data2 = "";
                    read_data2 = _serialPort.ReadExisting();
                    ConvertPLC_DateTime("d", 6 + (1 * 4), read_data2);
                    sBATCH_END = buf;
                    ConvertPLC_DateTime("t", 6 + (0 * 4), read_data2);
                    sBATCH_END = sBATCH_END + " " + buf;

                    try
                    {
                        var lines = File.ReadAllLines(LogFileName[numb-1]);
                        lines[6] = ("Batch End Time:," + sBATCH_END);

                        rtbStatus.Text = rtbStatus.Text + "Batch End : " + sBATCH_END + "\r";
                        File.WriteAllLines(LogFileName[numb-1], lines);


                    }
                    catch (SystemException)
                    {
                        rtbStatus.Text = rtbStatus.Text + "eror in log batch end \r\n";
                    }

                    RstDataRdy(numb, 2);

                }// if (ss == "0100")

            }
            catch { }
            */

        }//void LogBatchEnd(int numb)


        //****************************************************************//
        //   
        //
        //****************************************************************//
        void ScanPLC(int numb)
        {
            string sPlcNumb;
            string sWrtData;
            //int port;

            DataReady = 0;
            toggle ^= 1;
            tbToggle.Text = toggle.ToString();
            
            sPlcNumb = "0" + numb.ToString();

            //sWrtData = ("%" + sPlcNumb + "#RD" + PLC_DATA_RDY + "**\r"); 
            sWrtData = ("@00RD0100000156*\r"); 
            

            if (_serialPort.IsOpen == true)
            {
                _serialPort.Write(sWrtData);
                Thread.Sleep(Constants.READ_DELAY);

                read_data = "";
                read_data = _serialPort.ReadExisting();
                try
                {
                    // error no data receive from PLC
                    if (read_data.Length > 9)
                    {
                        string ss = read_data.Substring(7, 4);
                        if (ss == "0001")
                        {
                            DataReady = 1;

                            // read D211 to D217               
                            sWrtData = ("@00RD0211000753*\r");
                            _serialPort.Write(sWrtData);
                            Thread.Sleep(Constants.READ_DELAY);
                            read_data = "";
                            read_data = _serialPort.ReadExisting();

                            // read remain data1
                            // read D110 to D123
                            // D110 = team a/b
                            // D111 = batch no
                            // D112 = silo no
                            // D113 = Target Weight
                            // D114 - D123 = rcp Name
                            
                            sWrtData = ("@00RD0110001453*\r");
                            _serialPort.Write(sWrtData);
                            Thread.Sleep(Constants.READ_DELAY);
                            read_data1 = "";
                            read_data1 = _serialPort.ReadExisting();                    
                            

                        }

                    }//if (read_data.Length > 5)
                }// try
                catch (SystemException)
                { rtbStatus.Text = rtbStatus.Text + "No Data From PLC \r\n"; }






            }//if (_serialPort[port].IsOpen == true)

        }//scanPLC

        void HandleData(int numb)
        {
            string sTeam, sDt, sRcpName, sMatName;
            string sBatchNo, sSiloNo, sTargWt, sActWt, sDiffWt;

            var dbClient = new MongoClient("mongodb://127.0.0.1:27017");

            IMongoDatabase db = dbClient.GetDatabase("cosmosDB");
          
            bufStore = "";

            // Get material name D211 to D216 (6 )
            ConvertToAlphStr(7, 6 * 4, read_data);      // startpos,length,string
            sMatName = buf;

            // Get Actual weight D217
            ConvertToDecStr(0, 30, read_data);           // dec, start_pos, data 
            sActWt = convertDec;                         // add converted data to buffer                       	     			

            // get date time
            sDt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

            // Get Team A/B
            ConvertToDecStr(0, 6, read_data1);
            sTeam = convertDec;

            // Get Batch No.
            ConvertToDecStr(0, 6 + (1*4) , read_data1);                 // dec, start_pos, data 
            sBatchNo = convertDec;					            // add converted data to buffer  

            // Get silo number)                                                                           	         
            ConvertToDecStr(0, 6 + (2 * 4), read_data1); 		// (int decPos, int startPos , string read_data )  	     			
            sSiloNo = convertDec;                               // add converted data to buffer                       	     			


            // Get Target weight.
            ConvertToDecStr(0, 6 + (3 * 4 ), read_data1);            // dec, start_pos, data 
            sTargWt = convertDec;                                 // add converted data to buffer                       	     			


            // Get recipe name ( 15 words )
            ConvertToAlphStr(7 + (4*4), 10 * 4, read_data1);     // startpos,length,string
            sRcpName = buf;						                // add converted data to buffer   

            


            // Get Diffwt weight.
            //ConvertToDecStr(0, 6 + (11*4), read_data1);          // dec, start_pos, data 
            //sDiffWt = convertDec;                                // add converted data to buffer                       	     			
            int difwt = int.Parse(sActWt) - int.Parse(sTargWt);
            sDiffWt = difwt.ToString();

            try
            {
                var N530logs = db.GetCollection<BsonDocument>("N530");
                var doc = new BsonDocument
                {
                    //{"date", DateTime.UtcNow},
                    {"date", DateTime.UtcNow.ToString("o")},
                    {"team", sTeam},
                    {"rcpName", sRcpName},
                    {"matName", sMatName},
                    {"batchNo", sBatchNo},
                    {"siloNo", sSiloNo},
                    {"targWt", sTargWt},
                    {"actWt", sActWt},
                    {"diffWt", sDiffWt}
                };

                N530logs.InsertOne(doc);
                rtbStatus.Text = rtbStatus.Text + sDt + " -- Silo : " + sSiloNo + "; Weight :" + sActWt + "\r";
            }
            catch (SystemException)
            {
                rtbStatus.Text = rtbStatus.Text + "error in save log \n\r";
            }

        }// HandleData()

        //****************************************************************//
        //   
        //
        //****************************************************************//
        void delay(int ms)
        {

            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = ms;
            aTimer.Enabled = true;
            bTimer1 = false;

            do
            { Application.DoEvents(); } while (!bTimer1);

        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }


        //****************************************************************//
        //   
        //
        //****************************************************************//
        void RstDataRdy(int numb, int type)
        {
            string sPlcNumb;

            sPlcNumb = "0" + numb.ToString();
            // reset data ready
            if (type == 0)
            {
                //sCmd = ("%" + "01" + "#WDD28970289700000**\r");
                sCmd = ("@00WD0100000052*\r");

            }
            // set batch start read done 
            if (type == 1)
            {
                //          sCmd = ("%" + "01" + "#WDD02990029906500**\r");
             //   sCmd = ("%" + sPlcNumb + "#WD" + BATCH_START_RDY + "6500**\r");

            }

            // set batch end read done 
            if (type == 2)
            {
                //        sCmd = ("%" + "01" + "#WDD02991029916500**\r");
         //       sCmd = ("%" + sPlcNumb + "#WD" + BATCH_END_RDY + "6500**\r");

            }



            _serialPort.Write(sCmd);

            Thread.Sleep(Constants.READ_DELAY);
            //delay(5000);    // delay ms                      
            read_data2 = _serialPort.ReadExisting();

        }//void RstDataRdy(int numb)

        //****************************************************************//
        //   
        //
        //****************************************************************//
        void SetPcOn(int numb)
        {
            string sPlcNumb;

            if (_serialPort.IsOpen == true)
            {
                sPlcNumb = "0" + numb.ToString();
                sCmd = ("%" + sPlcNumb + "#WD" + PC_ON + "0100**\r");
                _serialPort.Write(sCmd);
                Thread.Sleep(Constants.READ_DELAY);
                read_data2 = _serialPort.ReadExisting();
            }

        }//void RstDataRdy(int numb)



        /*---------------------------------------------------------------------------*/
        /* convert hex number (str) to integer (str)                                 */
        /*   e.g  data from PLC "D204" means hex"04D2" = Int"1234"					 */
        /*                                                                           */
        /*  ConvertToDecStr (1, 0 , "d204" )                                         */
        /*---------------------------------------------------------------------------*/
        void ConvertToDecStr(int decPos, int startPos, string ReadStr)
        {
            string s1, s2, s3;
            int i, temp = 0;
            //float f ;
            double f;

            try
            {
                /*
                s1 = ReadStr.Substring(startPos, 2);
                s2 = ReadStr.Substring(startPos + 2, 2);
                s3 = s2 + s1;
                */
                s3 = ReadStr.Substring(startPos + 1, 4);

                i = int.Parse(s3, System.Globalization.NumberStyles.HexNumber);
                

                temp = i;
                // if i <0 : negetive value
                if (i > 32767)
                {
                    temp = i ^ 0xffff;
                    temp = temp + 1;
                }

                f = temp;

                if (decPos > 0)
                {
                    for (int k = 0; k < decPos; k++)
                    {
                        f /= 10;
                    }
                }

                f = Math.Round(f, decPos);
                s3 = Convert.ToString(f);

                if (i > 32767)
                {
                    s3 = "-" + s3;
                }
                convertDec = s3;

                /*
                convertDec = s3;
                if (!(decPos == 0))
                {
                    convertDec = s3.Insert((s3.Length - decPos), ".");
                }
                  
                */

            }
            catch (SystemException)
            {
                rtbStatus.Text = rtbStatus.Text + "error  in ConvertToDecStr \r\n";
            }




        }

        //****************************************************************//
        //   
        //
        //****************************************************************//
        void ConvertTo2DecStr(int decPos, int startPos, string ReadStr)
        {
            string s1, s2, s3, s4, s5;
            int i;

            try
            {
                s1 = ReadStr.Substring(startPos, 2);
                s2 = ReadStr.Substring(startPos + 2, 2);
                s3 = ReadStr.Substring(startPos + 4, 2);
                s4 = ReadStr.Substring(startPos + 6, 2);

                s5 = s4 + s3 + s2 + s1;

                i = Int32.Parse(s5, System.Globalization.NumberStyles.HexNumber);
                s5 = i.ToString();
                if (decPos != 0) {
                    convertDec = s5.Insert((s5.Length - decPos), ".");
                } else
                    { convertDec = s5; }
            }
            catch (SystemException)
            {
                rtbStatus.Text = rtbStatus.Text + "error in convert to 3 dec \n\r";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            rtbStatus.Text = "";
        }


        /*---------------------------------------------------------------------------*/
        /*   Get PLC Data and Time Data                                              */
        /*   e.g  data from PLC "~~~ 00 40 11 15 05 14 ~~~"        					 */
        /*	   return 15-05-14 11:40:00         									 */
        /*   e.g   ConvertPLC_DateTime(0,"004011150514")                             */
        /*---------------------------------------------------------------------------*/
        void ConvertPLC_DateTime(string dt, int startPos, string ReadStr)
        {
            string s1, s2, s3;

            /*            
            s4 = read_data.Substring(startPos, 2);
            int1 = AscToInt(s4);
            s4 = read_data.Substring(startPos + 2, 2);
            int2 = AscToInt(s4);
            s1 = int2.ToString() + int1.ToString();

            s4 = read_data.Substring(startPos + 4, 2);
            int1 = AscToInt(s4);
            s4 = read_data.Substring(startPos + 6, 2);
            int2 = AscToInt(s4);
            s2 = int2.ToString() + int1.ToString();

            s4 = read_data.Substring(startPos + 8, 2);
            int1 = AscToInt(s4);
            s4 = read_data.Substring(startPos + 10, 2);
            int2 = AscToInt(s4);
            s3 = int2.ToString() + int1.ToString();
            */

            try
            {
                switch (dt)
                {

                    case "d":
                        s1 = ReadStr.Substring(startPos + 2, 2);
                        s2 = ReadStr.Substring(startPos + 4, 2);
                        s3 = ReadStr.Substring(startPos + 6, 2);
                        buf = s1 + "-" + s2 + "-" + s3;
                        break;

                    case "t":
                        s1 = ReadStr.Substring(startPos + 0, 2);
                        s2 = ReadStr.Substring(startPos + 2, 2);
                        s3 = ReadStr.Substring(startPos + 4, 2);
                        buf = s3 + ":" + s2 + ":" + s1;
                        break;

                    default:
                        break;
                }

            }
            catch (SystemException)
            {
                rtbStatus.Text = rtbStatus.Text + "Data Received Error 5 \r\n";
            }


        }

        /*---------------------------------------------------------------------------*/
        /*   convert rearrange (str) to alphabet (str)                               */
        /*   e.g  data from PLC "1A32M4" means "A1234M"         					 */
        /*	   arg lenght = numbers of words										 */
        /*          ConvertToAlphStr(0, 6, "ABCDEF")                                 */
        /*---------------------------------------------------------------------------*/
        void ConvertToAlphStr(int startPos, int lenght, string ReadStr)
        {
            string s1, s2, s3, s4, s5 = "";
            int i, j = 0, k;
            int int1;

            k = lenght / 4;

            try
            {
                for (i = 0; i < k; i++)
                {
                    s1 = ReadStr.Substring(startPos + j, 1);
                    j++;
                    s2 = ReadStr.Substring(startPos + j, 1);
                    j++;
                    s3 = s1 + s2;

                    s1 = ReadStr.Substring(startPos + j, 1);
                    j++;
                    s2 = ReadStr.Substring(startPos + j, 1);
                    j++;
                    s4 = s1 + s2;

                    int1 = Convert.ToInt32(s3, 16);
                    s5 = s5 + char.ConvertFromUtf32(int1);
                    int1 = Convert.ToInt32(s4, 16);
                    s5 = s5 + char.ConvertFromUtf32(int1);
                }//for (i = 0; i < k; i++)
                buf = s5;
            }//try

            catch (SystemException)
            {
                rtbStatus.Text = rtbStatus.Text + "Error In Receive Data4 \r\n";
            }
        }// void ConvertToAlphStr(int startPos, int lenght, string ReadStr)

        /*---------------------------------------------------------------------------*/
        /* convert ascii to integer (char)   e.g 0x30 -> 0                           */
        /*---------------------------------------------------------------------------*/
        int AscToInt(string s)
        {
            int result;

            switch (s)
            {
                case "30":
                    result = 0;
                    break;
                case "31":
                    result = 1;
                    break;
                case "32":
                    result = 2;
                    break;
                case "33":
                    result = 3;
                    break;
                case "34":
                    result = 4;
                    break;
                case "35":
                    result = 5;
                    break;
                case "36":
                    result = 6;
                    break;
                case "37":
                    result = 7;
                    break;
                case "38":
                    result = 8;
                    break;
                case "39":
                    result = 9;
                    break;
                default:
                    result = 0;
                    break;
            }
            return result;

        }

        /*---------------------------------------------------------------------------*/
        /* convert integer to ascii   e.g 1 -> 0x31                                  */
        /*---------------------------------------------------------------------------*/
        char IntToAsc(int value, int divv)
        {
            char result;

            ValToAsc = value / divv;
            switch (value % divv)
            {
                case 0:
                    result = '0';
                    break;
                case 1:
                    result = '1';
                    break;
                case 2:
                    result = '2';
                    break;
                case 3:
                    result = '3';
                    break;
                case 4:
                    result = '4';
                    break;
                case 5:
                    result = '5';
                    break;
                case 6:
                    result = '6';
                    break;
                case 7:
                    result = '7';
                    break;
                case 8:
                    result = '8';
                    break;
                case 9:
                    result = '9';
                    break;
                default:
                    result = '8';
                    break;
            }
            return result;
        }

        private void cbPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            String[] lines;

            if (File.Exists(INIT))
            {
                lines = File.ReadAllLines(INIT);
                if (lines.Count() > 0)
                {
                    String port = "COM" + (cbPort.SelectedIndex + 1).ToString();
                    PortNo[0] = port;
                    lines[0] = port;
                    _serialPort.PortName = port;
                    //File.Delete(INIT);
                    //File.OpenWrite(specfile);
                    File.WriteAllLines(INIT, lines);
                }
            }// if

        }

        private void bn_Clear_Click(object sender, EventArgs e)
        {
            rtbStatus.Text = "";
        }
        private void bn_Start_Click(object sender, EventArgs e)
        {

            if (!RunFlag)
            {
                Open_Port();
                RunFlag = true;
                lbRunStop.Text = "  RUN  ";
                lbRunStop.ForeColor = System.Drawing.Color.Green;
                pBox_Run.BackColor = System.Drawing.Color.Green;
                cbPort.Enabled = false;

                Run();

            }

        }

        private void bn_Stop_Click(object sender, EventArgs e)
        {
            if (RunFlag)
            {
                //Close_Port();
                bClose = true;
                lbRunStop.Text = "STOPPING";
                lbRunStop.ForeColor = System.Drawing.Color.Black;
                pBox_Run.BackColor = System.Drawing.Color.Red;
            }

        }

        private void bn_Exit_Click(object sender, EventArgs e)
        {
            if (!RunFlag)
            {
                Application.Exit();
            }

        }

    }
}
