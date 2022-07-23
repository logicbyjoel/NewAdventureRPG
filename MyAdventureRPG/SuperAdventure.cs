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
            // move to home
            MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            _player.Inventory.Add(new InventoryItem(World.ItemByID(World.ITEM_ID_RUSTY_SWORD), 1));

            // output Player stats on labels   
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            lblGold.Text = _player.Gold.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
            lblLevel.Text = _player.Level.ToString();
        } // end SuperAdventure constructor

        // button for an attempt to move north
        private void btnNorth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToNorth);
        }
        // button for an attempt to move soutn
        private void btnSouth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToSouth);
        }
        // button for an attempt to move  east
        private void btnEast_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToEast);
        }
        // button for an attempt to move  west
        private void btnWest_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToWest);
        }

        // shared function to call upon any player movements (NOTE: newLocation == destination)
        private void MoveTo(Location destination)
        {
            // check if this location requires an item to enter
            if (!_player.HasRequiredItemToEnterThisLocation(destination))
            {
                    // show message
                    rtbMessages.Text += "You must have a " + destination.ItemRequiredToEnter.Name + " to enter this loaciton." + Environment.NewLine;
                    // do not allow player to enter (stop processing this move and exit function)
                    return;
            }

            // update the player's current location
            _player.CurrentLocation = destination;

            // show/hide the available movement(buttons)
            btnNorth.Visible = (destination.LocationToNorth != null);
            btnSouth.Visible = (destination.LocationToSouth != null);
            btnEast.Visible = (destination.LocationToEast != null);
            btnWest.Visible = (destination.LocationToWest != null);

            // show location name and description
            rtbLocation.Text = destination.Name + Environment.NewLine;
            rtbLocation.Text = destination.Description +Environment.NewLine;

            // refill player's health (assume player healed/rested during move)
            _player.CurrentHitPoints = _player.MaximumHitPoints;

            // update hit points display in UI
            lblHitPoints.Text = _player.MaximumHitPoints.ToString();

            // chedck if this location have a quest
            if(destination.QuestAvailableHere != null)
            {
                // check if the player have this quest
                // check if quest is completed
                bool playerAlreadyHasQuest = _player.HasThisQuest(destination.QuestAvailableHere);
                bool playerAlreadyCompletedQuest = _player.CompletedThisQuest(destination.QuestAvailableHere);


                // check if the player have this quest
                if (playerAlreadyHasQuest)
                {
                    // check if quest is completed
                    if (!playerAlreadyCompletedQuest)
                    {
                        // (else) check if player havs items to complee this quest
                        bool playerHasAllItemsToCompleteQuest = _player.HasAllQuestCompletionItems(destination.QuestAvailableHere);

                        // player has all items to complete the qest
                        if (playerHasAllItemsToCompleteQuest)
                        {
                            // complete the questl; display message
                            rtbMessages.Text += Environment.NewLine;
                            rtbMessages.Text += "You coimplete the " + destination.QuestAvailableHere.Name + "quest. " + Environment.NewLine;

                            _player.RemoveQuestCompletionItems(destination.QuestAvailableHere);

                            // give quest rewareds
                            rtbMessages.Text += "You receive: " + Environment.NewLine;
                            rtbMessages.Text += destination.QuestAvailableHere.RewarExperiencePoints.ToString() + "experience points "+ Environment.NewLine;
                            rtbMessages.Text += destination.QuestAvailableHere.RewardGold.ToString() + " gold" + Environment.NewLine;
                            rtbMessages.Text += destination.QuestAvailableHere.RewardItem.Name + Environment.NewLine;
                            rtbMessages.Text += Environment.NewLine;

                            _player.ExperiencePoints += destination.QuestAvailableHere.RewarExperiencePoints;
                            _player.Gold += destination.QuestAvailableHere.RewardGold;

                            // add the reward item to the player's inventory
                            _player.AddItemToInventory(destination.QuestAvailableHere.RewardItem);

                            // mark player's quest as completed
                            // find the quest in the play'ers quest list
                            _player.MarkQuestCompleted(destination.QuestAvailableHere);
                        }

                    }

                }
                // (else) if player does not have teh quest , give player the quest
                else
                {
                    // show messages (singular or plural forms of items)
                    rtbMessages.Text += "you receive the " + destination.QuestAvailableHere.Name + " quest." + Environment.NewLine;
                    rtbMessages.Text += destination.QuestAvailableHere.Description + Environment.NewLine;
                    rtbMessages.Text += "To complete it, return with: " + Environment.NewLine;
                    foreach(QuestCompletionItem qci in destination.QuestAvailableHere.QuestCompletionItems)
                    {
                        if(qci.Quantity == 1)
                        {
                            rtbMessages.Text += qci.Quantity.ToString() + " " + qci.Details.Name + Environment.NewLine;
                        }
                        else
                        {
                            rtbMessages.Text += qci.Quantity.ToString() + " " + qci.Details.NamePlural + Environment.NewLine;
                        }
                    }
                    rtbMessages.Text += Environment.NewLine;
                    // add this quest to  player's quest list
                    _player.Quests.Add(new PlayerQuest(destination.QuestAvailableHere));
                }
            }
            // check if there is a boss at this loaciotn
            if(destination.BossLivingHere != null)
            {
                // if true , diplay mesage of what player sees
                rtbMessages.Text += "You see a " + destination.BossLivingHere.Name + Environment.NewLine;

                // spawn new boss, pertaining to current location, to fight using the standard boss in the world
                Boss standardBoss = World.BossByID(destination.BossLivingHere.ID);

                _currentBoss = new Boss(standardBoss.ID, standardBoss.Name, standardBoss.MaximumDamage, standardBoss.RewardExperiencePoints, standardBoss.RewardGold, standardBoss.CurrentHitPoints, standardBoss.MaximumHitPoints);

                // populate boss' loot items
                foreach (LootItem lootItem in standardBoss.LootTable)
                {
                    _currentBoss.LootTable.Add(lootItem);
                }

                // show combat combo boxes and buttons
                cboPotions.Visible = true;
                cboWeapons.Visible = true;
                btnUsePotion.Visible = true;
                btnUseWeapon.Visible = true;
            }
            // else, there is no boss at this destination
            else
            {
                _currentBoss = null;

                // hide combat comboboxes and buttons
                cboWeapons.Visible = false;
                cboPotions.Visible = false;
                btnUsePotion.Visible = false;
                btnUseWeapon.Visible = false;
            }
            // refresh player's inventory list
        UpdateInventoryListInUI();

            // refresh player's quest list
            UpdateQuestListInUI();

            // refresh player's weapons combobox
            UpdateWeaponListInUI();

            // refresh player's potions combobox
            UpdatePotionListInUI();
        }   // end MoveTo()


        // update the player's inventory in UI
        private void UpdateInventoryListInUI()
        {
            // set the table in data grid view
            dgvInventory.RowHeadersVisible = false;
            dgvInventory.ColumnCount = 2;
            dgvInventory.Columns[0].Name = "Name";
            dgvInventory.Columns[0].Width = 197;
            dgvInventory.Columns[1].Name = "Quantity";
            dgvInventory.Rows.Clear();
            foreach (InventoryItem inventoryItem in _player.Inventory)
            {
                //find items with quantity of 1 or more
                if(inventoryItem.Quantity > 0)
                {
                    // add the items as a new array of objects
                    dgvInventory.Rows.Add(new[] {inventoryItem.Details.Name, inventoryItem.Quantity.ToString()});
                }
            }
        }   // end UpdateInventoryInUI()

        private void UpdateQuestListInUI()
        {
            // set the data grid view table
            dgvQuests.RowHeadersVisible = false;
            dgvQuests.ColumnCount = 2;
            dgvQuests.Columns[0].Name = "Name";
            dgvQuests.Columns[0].Width = 197;
            dgvQuests.Columns[1].Name = "Done?";
            dgvQuests.Columns.Clear();
            foreach(PlayerQuest playerQuest in _player.Quests)
            {
                //create the player's quests as a UI elemnent
                dgvQuests.Rows.Add(new[] { playerQuest.Details.Name, playerQuest.IsCompleted.ToString() });
            }
        }   // end UpdateQuestListInUI()

        private void UpdateWeaponListInUI()
        {
            // create new weapon list
            List<Weapon> weapons = new List<Weapon>();
            foreach(InventoryItem inventoryItem in _player.Inventory)
            {
                // NOTE: Details is of the Item class
                if(inventoryItem.Details is Weapon) 
                {
                    if (inventoryItem.Quantity > 0)
                    {
                        weapons.Add((Weapon)inventoryItem.Details);
                    }
                }
            }
            if(weapons.Count == 0)
            {
                // player has no weapons, hide the weapon cmbobox nad "use" button
                cboWeapons.Visible = false;
                btnUseWeapon.Visible = false;
            }
            else
            {
                cboWeapons.DataSource = weapons;
                cboWeapons.DisplayMember = "Name";
                cboWeapons.ValueMember = "ID";
                cboWeapons.SelectedIndex = 0;
            }
        }   // end UpdateWeaponListInUI()

        private void UpdatePotionListInUI()
        {
            List<HealingPotion> potionList = new List<HealingPotion>();
            foreach (InventoryItem inventoryItem in _player.Inventory)
            {
                if(inventoryItem.Details is HealingPotion)
                {
                    if(inventoryItem.Quantity > 0)
                    {
                        // NOTE: Details is of the Item class
                        potionList.Add((HealingPotion)inventoryItem.Details);
                    }
                }
            }
            if(potionList.Count == 0)
            {
                // player has no potions, hide the potion combobox and "use" btn
                cboPotions.Visible = false;
                btnUsePotion.Visible = false;
            }
            else
            {
                // add potions to UI
                cboPotions.DataSource = potionList;
                cboPotions.DisplayMember = "Name";
                cboPotions.ValueMember = "ID";
                cboPotions.SelectedIndex = 0;
            }
        }   // end UpdatePotionsListInUI()

        // button use weapon
        private void btnUseWeapon_Click(object sender, EventArgs e)
        {

        }
        // button use potion
        private void btnUsePotion_Click(object sender, EventArgs e)
        {

        }
    }
}
