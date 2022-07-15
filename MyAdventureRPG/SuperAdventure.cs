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
        // create global player variable (class level) ..keep? 
        private Player _player;        // create global boss variable (class level)
        private Boss _currentBoss;
        public SuperAdventure()
        {
            InitializeComponent();

            // declare Player object, store in class-level variable
            // UPDATE: this instantiation now must meet parameterized construcotr of Player class
            _player = new Player(10, 10, 20, 0, 1); // keep?

            // output Player stats on labels    .. keep?
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            lblGold.Text = _player.Gold.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
            lblLevel.Text = _player.Level.ToString();
        } // end SuperAdventure constructor



        // update the player's current location

        // show location name and description

        // show/hide the available movement(buttons)

        // refill player's health (assume player healed/rested during move)

        // update hit points display in UI

        // chedck if this location have a quest

        // check if the player have this quest

        // check if quest is completed

        // (else) check if player havs items to complee this quest

        // complete the quest

        // show messages

        // remove quest completion items from inventory

        // give quest rewareds
        
        // mark player's quest as completed

        // if player does not have teh quest , give player the quest

        // show message

        // add this quest to  player's quest list

        // check if there is a boss at this loaciotn

        // if true , diplay mesage

        // spawn new boss to fight

        // show combat combo boxes and buttons

        // repopulate comboboxes (in case the inventory has changed)

        // if boss at this location is false, hide combat comboboxes and buttons

        // refresh the player's inventory int he UI (in case it has changed)

        // refresh the player's queest list in the UI (in case it has changeed)

        // refresh cboWeapons combobox in the UI

        // refresh cboPotgions in the UI

        // end?

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void SuperAdventure_Load(object sender, EventArgs e)
        {

        }
        // TODO: add functions to button clicks

        // button north
        private void btnNorth_Click(object sender, EventArgs e)
        {

        }
        // button soutn
        private void btnSouth_Click(object sender, EventArgs e)
        {

        }
        // button east
        private void btnEast_Click(object sender, EventArgs e)
        {

        }
        // button west
        private void btnWest_Click(object sender, EventArgs e)
        {

        }
        // button use weapon
        private void btnUseWeapon_Click(object sender, EventArgs e)
        {

        }
        // button use potion
        private void btnUsePotion_Click(object sender, EventArgs e)
        {

        }

        // shared function to call upon any player movements
        private void MoveTo(Location destination)
        {
            // check if this location requires an item to enter
            if (destination.ItemRequiredToEnter != null)
            {

                // if player does not have required item
                bool playerHasRequiredItem = false;

                foreach (InventoryItem ii in _player.Inventory)
                {
                    if(ii.Details.ID == destination.ItemRequiredToEnter.ID)
                    {
                        //has required item
                        playerHasRequiredItem = true;
                        break;
                    }
                }

                if (!playerHasRequiredItem)
                {
                    // show message
                    rtbMessages.Text += "You must have a " + destination.ItemRequiredToEnter.Name + " to enter this loaciton." + Environment.NewLine;
                    // do not allow player to enter (stop processing this move)
                    return;
                }
            }
        }   // end MoveTo()
    }
}
