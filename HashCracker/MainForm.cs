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
        private UInt64 Progress,Last;
        private int UpdateCounter;

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
                       
            LockObject = new object();

            FoundPassword = null;
            Progress = Last = 0;
            UpdateCounter = 10;

            comboAlgo.SelectedIndex = 0;
        }

        /// <summary>
        /// Převede zadané číslo do číselné soustavy o daném základu
        /// </summary>
        /// <param name="Number">Číslo</param>
        /// <param name="Base">Základ nové číselné soustavy</param>
        /// <returns>Cifry čísla v dané číselné soustavě</returns>
        private int[] ConvertToBase(UInt64 Number, int Base)
        {
            if (Base <= 1)
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
            
            // Data z předané struktury
            short CharsetLength = (short)Info.Charset.Length;
            int MaximumLength = Info.MaxLength;
            UInt64 Combinations = (UInt64)Math.Pow((double)CharsetLength, (double)MaximumLength);

            string Test = String.Empty;
            for (Progress = 0; Progress < Combinations; Progress++)
            {
                // Pokračuj jen v případě, že se podaří získat vlastictví uzamykacího objektu, jinak na něj čekej.
                lock (LockObject)
                {
                    Test = String.Empty;
                    int[] Digits = ConvertToBase(Progress, CharsetLength); // Převeď číslo do daného číselného základu

                    // Vytvoř z čísla v daném základu text
                    for (int j = 0; j < Digits.Length; j++)
                       Test += Info.Charset[Digits[j]];
                    
                    // Porovnej zahashovaný text se vstupem
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
            foreach (byte b in Data) // Pokud je byte menší než 16, musíme před něj přidat nulu (15 je F, takže musí být 0F)
                HexString += b < 16 ? "0" + Convert.ToString(b, 16) : Convert.ToString(b, 16);
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

        /// <summary>
        /// Spočítá MD160 hashi ze vstupního řetězce
        /// </summary>
        /// <param name="In">Vstupní řetězec</param>
        /// <returns>MD160 hashe v hexadecimální notaci</returns>
        private string RIPEMD160Hash(string In)
        {
            using (RIPEMD160 Hash = RIPEMD160.Create())
            {
                Hash.Initialize();
                byte[] OutHash = Hash.ComputeHash(Encoding.ASCII.GetBytes(In));

                return ConvertHexToString(OutHash);
            }
        }

        /// <summary>
        /// Spočítá SHA1 hashi ze vstupního řetězce
        /// </summary>
        /// <param name="In">Vstupní řetězec</param>
        /// <returns>SHA1 hashe v hexadecimální notaci</returns>
        private string SHA1Hash(string In)
        {
            using (SHA1 Hash = SHA1.Create())
            {
                Hash.Initialize();
                byte[] OutHash = Hash.ComputeHash(Encoding.ASCII.GetBytes(In));

                return ConvertHexToString(OutHash);
            }
        }

        /// <summary>
        /// Zruší vlákno a připraví program na další operaci
        /// </summary>
        private void JobDone()
        {
            CrackingThread = null;
            Progress = 0;

            labelResult.Visible = true;
            groupInput.Enabled = true;
            buttonStartStop.Text = "Spustit";
        }

        #region Události

        private void buttonStartStop_Click(object sender, EventArgs e)
        {
            if (CrackingThread == null)
            {
                // Vytvoř a naplň strukturu CrackerInfo pro vlákno které bude crackovat
                CrackerInfo Info = new CrackerInfo();
                Info.Hash = textHash.Text;
                Info.Charset = textCharset.Text;
                Info.MaxLength = (int)numericLength.Value;

                // Vyber delegát daného hashovacího algoritmu podle toho co uživatel vybral v políčku Algoritmus
                switch (comboAlgo.SelectedIndex)
                {
                    case 0: Info.F = new HashFunction(MD5Hash); break;
                    case 1: Info.F = new HashFunction(SHA1Hash); break;
                    case 2: Info.F = new HashFunction(SHA256Hash); break;
                    case 3: Info.F = new HashFunction(RIPEMD160Hash); break;

                    default: Info.F = new HashFunction(MD5Hash); break;
                }

                labelResult.Visible = false;
                FoundPassword = null;

                // Vytvoř a spusť vlákno
                CrackingThread = new Thread(new ParameterizedThreadStart(Cracker));
                CrackingThread.Start(Info);

                groupInput.Enabled = false;
                buttonStartStop.Text = "Pozastavit";
            }
            else if (CrackingThread.ThreadState == ThreadState.Running)
            {
                // Získej vlastnictví uzamykacího objektu a pozastav tak vlákno
                buttonStartStop.Text = "Obnovit";
                Monitor.Enter(LockObject);
            }
            else if (CrackingThread.ThreadState == ThreadState.WaitSleepJoin)
            {
                // Uvolni vlastnictví uzakmykacího objektu a umožni vláknu pokračovat
                Monitor.Exit(LockObject);
                buttonStartStop.Text = "Pozastavit";
            }
        }

        private void textCharset_Validating(object sender, CancelEventArgs e)
        {
            // Zkontroluj, jestli se v poli charset neopakuje některý znak
            char[] chars = textCharset.Text.ToCharArray();

            int count = chars.Distinct().Count();
            if (count != chars.Length)
            {
                e.Cancel = true;
                MessageBox.Show("Byla zadána neplatná znaková sada!","Chyba",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Zavíráme hlavní okno...
            
            if (CrackingThread == null) return;
            if (CrackingThread.ThreadState == ThreadState.Running)
            {
                // Vlákno běží ukonči jej
                CrackingThread.Abort();
            }
            else if (CrackingThread.ThreadState == ThreadState.WaitSleepJoin)
            {
                // Vlákno stojí...
                try
                {
                    // Jestli stojí, kvůli tomu že uživatel kliknul na Pauzu, zkus jej uvolnit a pak ukončit
                    Monitor.Exit(LockObject); // Uvolni, pokud vlastníme aby nenastal deadlock
                    Thread.Sleep(10);
                    CrackingThread.Abort();
                }
                catch (SynchronizationLockException)
                {
                    // Tady nic moc nenaděláme, vlákno stojí z neznámého důvodu.
                    CrackingThread.Abort();
                }
            }
        }

                
        private void timerUpdater_Tick(object sender, EventArgs e)
        {
            // Spočti celkový počet kombinací a zbývající počet kombinací k vyzkoušení
            labelTotalCombinations.Text = String.Format("Celkový počet kombinací: {0}", Combinations);
            labelRemainingCombinations.Text = String.Format("Zbývá vyzkoušet: {0}", Combinations - Progress);

            if (CrackingThread == null) return;

            // Spočti rychlost
            double Speed = (double)(Progress - Last) / ((double)timerUpdater.Interval / 1000.0);
            double RemainingTime = Speed != 0 ? (Combinations-Progress)/Speed : 0;
            Last = Progress;

            // Obnovuj čítač rychlosti a času po vteřině a půl
            if (UpdateCounter > 15) 
            {
                labelSpeed.Text = String.Format("Rychlost: {0} kombinací/s", Math.Round(Speed, 2));
                labelTimeRemaining.Text = String.Format("Zbývá ({0}): {1}", RemainingTime >= 3600 ? "hodin" : RemainingTime >= 120 ? "minut" : "sekund",
                                                                            Math.Ceiling(RemainingTime >= 3600 ? RemainingTime / 3600 : RemainingTime >= 120 ? RemainingTime / 60 : RemainingTime));
                UpdateCounter = 0; 
            }            
            UpdateCounter++;

            // Obnovuj progress bar
            progressTotal.Value = (int)(Progress * 100 / Combinations);

            if (FoundPassword != null)
            { 
                // Jestli bylo nalezeno heslo, zobraz jej a jsme hotovi
                labelResult.Text = String.Format("Výsledek: {0}", FoundPassword);
                JobDone();
            }
            if (FoundPassword == null && CrackingThread.ThreadState == ThreadState.Stopped)
            {
                // Heslo nebylo nalezeno, jsme hotovi
                labelResult.Text = String.Format("Nic nenalezeno");
                JobDone();
            }
        }

        private void textHash_Validating(object sender, CancelEventArgs e)
        {
            // Ověř, zda byl zadán platný hexadecimální hash
            char[] allowedChars = {'0','1','2','3','4','5','6','7','8','9','0','a','b','c','d','e','f'};

            for (int i = 0; i < textHash.Text.Length; i++)
            {
                if (!allowedChars.Contains(textHash.Text[i]))
                {
                    e.Cancel = true;
                    MessageBox.Show("Byl zadán neplatný hash!","Chyba",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }

            }

        }

        #endregion
    }
}
