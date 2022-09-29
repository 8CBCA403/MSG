using splatoon3Tester.core;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace splatoon3Tester
{
    public partial class Form1 : Form
    {
        Debugger debugger;
        public Form1()
        {
            InitializeComponent();
        }
        private static uint money1 = 0x1092E4;
        private static uint weaponStamp1 = 0x109304;
        private static uint money2 = 0x20D3C4;
        private static uint weaponStamp2 = 0x20D3E4;
        private static uint weaponPiece1 = 0x1cc754;
        private long baseHeap1Addr = -1;
        private static readonly long TID = 0x0100C2500FC20000L;
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                debugger = new Debugger(new SocketConnection(ipTextBox.Text, 7331));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"connect error {ex.Message}");
                return;
            }

            long pid = debugger.getPids().Reverse().Take(5).Where(pid => TID == debugger.getTitleId(pid)).FirstOrDefault(-1);
            if (pid == -1)
            {
                debugger.close();
                MessageBox.Show($"not splatoon3");
                return;
            }
            debugger.attach(pid);
            debugger.resume();
            MemoryInfo? mi = debugger.query(0, 5000).Where(memoryInfo => memoryInfo.getType() == MemoryType.HEAP && memoryInfo.getSize() == 0x14B6000).LastOrDefault();
            debugger.resume();
            if (mi == null)
            {
                debugger.close();
                MessageBox.Show($"can not find base address");
                return;
            }
            baseHeap1Addr = mi.getAddress();
            int m1 = debugger.peek32(baseHeap1Addr + money1);
            int m2 = debugger.peek32(baseHeap1Addr + money2);
            int weaponStampNum = debugger.peek32(baseHeap1Addr + weaponStamp1);
            int weaponStampNumCheck = debugger.peek32(baseHeap1Addr + weaponStamp2);
            debugger.resume();
            if (m1 != m2)
            {
                MessageBox.Show($"money data error");
                baseHeap1Addr = -1;
                return;
            } else if (weaponStampNum != weaponStampNumCheck)
            {
                MessageBox.Show($"weaponStampNum data error");
                baseHeap1Addr = -1;
                return;
            }

            int superSeaSnailsNum = debugger.peek32(baseHeap1Addr + money1 + 4);
            
            int weaponPieceNum1 = debugger.peek32(baseHeap1Addr + weaponPiece1);
            debugger.resume();
            textBoxMoney.Text = Convert.ToString((uint)m1);
            textBoxSuperSeaSnails.Text = Convert.ToString(superSeaSnailsNum);
            textBoxWeaponStamp.Text = Convert.ToString(weaponStampNum);
            textBoxWeaponPiece1.Text = Convert.ToString(weaponPieceNum1);
        }

        
        private void button3_Click(object sender, EventArgs e)
        {
            if (baseHeap1Addr == -1)
            {
                MessageBox.Show($"Please connect");
                return;
            }
            try {
                uint money = uint.Parse(textBoxMoney.Text);
                if (money > 9999999) money = 9999999;
                debugger.poke32(baseHeap1Addr + money1, money);
                debugger.poke32(baseHeap1Addr + money2, money);
                uint weaponStamp = uint.Parse(textBoxWeaponStamp.Text);
                if (weaponStamp > 999) weaponStamp = 999;
                debugger.poke32(baseHeap1Addr + weaponStamp1, weaponStamp);
                debugger.poke32(baseHeap1Addr + weaponStamp2, weaponStamp);
                uint weaponPieceNum = uint.Parse(textBoxWeaponPiece1.Text);
                if (weaponPieceNum > 999) weaponPieceNum = 999;
                debugger.poke32(baseHeap1Addr + weaponPiece1, weaponPieceNum);
                debugger.close();
                MessageBox.Show($"Success");
            }
            catch(Exception)
            {
                MessageBox.Show($"Set value failed");
            }
        }
    }
}