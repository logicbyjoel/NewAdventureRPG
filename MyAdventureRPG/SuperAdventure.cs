using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;   // link local Engine Project

namespace MyAdventureRPG
{
    public partial class SuperAdventure : Form
    {
        // create global player variable (class level)
        private Player _player;
        public SuperAdventure()
        {
            InitializeComponent();
            // declare Player object, store in class-level variable
            _player = new Player();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void SuperAdventure_Load(object sender, EventArgs e)
        {

        }
    }
}
