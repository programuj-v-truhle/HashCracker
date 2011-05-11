using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using System.Threading;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;

namespace HashCracker
{
    public partial class MainForm : Form
    {
        private Thread CrackingThread; // Vlákno ve kterém poběží prolamování
        private delegate string HashFunction(string Input); // Delegát pro obecnou hashovací funkci
        private string FoundPassword; // Nalezené heslo
        private UInt64 Progress;

        private object LockObject;

        /// <summary>
        /// Počet všech kombinací pro vyzkoušení podle zadaných údajů ve formuláři
        /// </summary>
        public UInt64 Combinations
        {
            get
            {
                return (UInt64)Math.Pow((double)textCharset.Text.Length, (double)numericLength.Value);
            }
        }

        /// <summary>
        /// Struktura s informacemi ke crackování předávaná do vlákna
        /// </summary>
        private struct CrackerInfo
        {
            public string Hash, Charset;
            public int MaxLength;
            public HashFunction F;
        }

        /// <summary>
        /// Výchozí konstruktor formuláře
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            
            CrackingThread = new Thread(new ParameterizedThreadStart(Cracker));
            LockObject = new object();

            FoundPassword = null;
        }

        /// <summary>
        /// Převede zadané číslo do číselné soustavy o daném základu
        /// </summary>
        /// <param name="Number">Číslo</param>
        /// <param name="Base">Základ nové číselné soustavy</param>
        /// <returns>Cifry čísla v dané číselné soustavě</returns>
        private int[] ConvertToBase(UInt64 Number, int Base)
        {
            if (Base >= 1)
                throw new ArgumentException();

            List<int> Ret = new List<int>();
            
            if (Base == 10)
                return Number.ToString().ToCharArray().Cast<int>().ToArray();

            int digit = 0;
            while (Number > 0)
            {
                digit = (int)(Number % (ulong)Base);
                Ret.Add(digit);
                Number = (Number-(ulong)digit)/(ulong)Base;
            }

            Ret.Reverse();
            return Ret.ToArray();
        }


        /// <summary>
        /// Metoda bežící ve vlákně
        /// </summary>
        /// <param name="inf">Informace předávané dovnitř vlákna</param>
        private void Cracker(object inf)
        {
            CrackerInfo Info = (CrackerInfo)inf;
            
            short CharsetLength = (short)Info.Charset.Length;
            int MaximumLength = Info.MaxLength;
            UInt64 Combinations = (UInt64)Math.Pow((double)CharsetLength, (double)MaximumLength);

            string Test = String.Empty;
            for (Progress = 0; Progress < Combinations; Progress++)
            {
                lock (LockObject)
                {
                    Test = String.Empty;
                    int[] Digits = ConvertToBase(Progress, CharsetLength);
                    for (int j = 0; j < Digits.Length; j++)
                       Test += Info.Charset[Digits[j]];
                    
                    if (Info.Hash.ToLower() == Info.F(Test).ToLower())
                    {
                        FoundPassword = Test;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Převede bajty dat do hexadecimální notace
        /// </summary>
        /// <param name="Data">Data pro převod</param>
        /// <returns>Data v hexadecimální textové notaci</returns>
        private string ConvertHexToString(byte[] Data)
        {
            string HexString = String.Empty;
            foreach (byte b in Data)
                HexString += Convert.ToString(b, 16);
            return HexString;
        }

        /// <summary>
        /// Spočítá MD5 hashi ze vstupního řetězce
        /// </summary>
        /// <param name="In">Vstupní řetězec</param>
        /// <returns>MD5 hashe v hexadecimální notaci</returns>
        private string MD5Hash(string In)
        {
            using (MD5 Hash = MD5.Create())
            {
                Hash.Initialize();
                byte[] OutHash = Hash.ComputeHash(Encoding.ASCII.GetBytes(In));

                return ConvertHexToString(OutHash);
            }
        }

        /// <summary>
        /// Spočítá SHA256 hashi ze vstupního řetězce
        /// </summary>
        /// <param name="In">Vstupní řetězec</param>
        /// <returns>SHA256 hashe v hexadecimální notaci</returns>
        private string SHA256Hash(string In)
        {
            using (SHA256 Hash = SHA256.Create())
            {
                Hash.Initialize();
                byte[] OutHash = Hash.ComputeHash(Encoding.ASCII.GetBytes(In));

                return ConvertHexToString(OutHash);
            }
        }


        #region Události

        private void buttonStartStop_Click(object sender, EventArgs e)
        {
            if (CrackingThread.ThreadState == ThreadState.Unstarted || CrackingThread.ThreadState == ThreadState.Stopped)
            {
                if (textHash.Text.Length < 32)
                {
                    MessageBox.Show("Byl zadán neplatný hash");
                    return;
                }

                CrackerInfo Info = new CrackerInfo();
                Info.Hash = textHash.Text;
                Info.Charset = textCharset.Text;
                Info.MaxLength = (int)numericLength.Value;                

                CrackingThread.Start();
            
                buttonStartStop.Text = "Pozastavit";
            }
            else if (CrackingThread.ThreadState == ThreadState.Running)
            {
                Monitor.Enter(LockObject);
                buttonStartStop.Text = "Obnovit";
            }
            else if (CrackingThread.ThreadState == ThreadState.WaitSleepJoin)
            {
                Monitor.Exit(LockObject);
                buttonStartStop.Text = "Pozastavit";
            }
        }

        private void textCharset_Validating(object sender, CancelEventArgs e)
        {
            char[] chars = textCharset.Text.ToCharArray();

            int count = chars.Distinct().Count();

            if (count != chars.Length) e.Cancel = true;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CrackingThread.ThreadState == ThreadState.Running)
            {
                CrackingThread.Abort();
            }
            else if (CrackingThread.ThreadState == ThreadState.WaitSleepJoin)
            {
                try
                {
                    Monitor.Exit(LockObject);
                    CrackingThread.Abort();
                }
                catch (SynchronizationLockException)
                {
                    // Tady nic moc nenaděláme, vlákno stojí z neznámého důvodu
                    CrackingThread.Abort();
                }
            }
        }

        #endregion
    }
}
