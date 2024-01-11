using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterMind__Forms_
{
    public partial class EndMessage : Form
    {



        public EndMessage()
        {
            InitializeComponent();
            this.FormClosed += EndMessage_EndMessageClosed;
        }
        private void EndMessage_Load(object sender, EventArgs e)
        {

        }

        Menu congratulationsMessage;
        private FormClosedEventHandler EndMessage_EndMessageClosed;

        //Menu mainMenu;

        public EndMessage(Menu congratulations)
        {
           congratulationsMessage = congratulations;
        }
    }
}
