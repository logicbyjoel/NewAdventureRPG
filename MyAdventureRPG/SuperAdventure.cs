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
            // initialize values for Player properties
            _player.CurrentHitPoints = 10;
            _player.MaximumHitPoints = 10;
            _player.Gold = 20;
            _player.ExperiencePoints = 0;
            _player.Level = 1;

            // output Player stats on labels
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            lblGold.Text = _player.Gold.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
            lblLevel.Text = _player.Level.ToString();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void SuperAdventure_Load(object sender, EventArgs e)
        {

        }
    }
}
