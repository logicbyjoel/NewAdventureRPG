/**************************************    SuperAdventure.cs    *****************************************
 * 
 * Programmer: Joel Godinez
 * 
 * Date: 20 July 2022
 * 
 * Environment: WinForms in Windows 10
 * 
 * Files Included: Program.cs; Engine includes: Boss.cs, HealingPotion.cs, InventoryItem.cs, Item.cs, LIvingCreature.cs, Location.cs, LootItem.cs, Player.cs, PlayerQuest.cs, Quest.cs, QuestCompletionItem.cs, RandomNumberGenerator.cs, Weapon.cs, World.cs
 * 
 * Purpose: drivet the main game logic
 * 
 * Input: called by Program.cs and SuperAdventure.Designer.cs files
 * 
 * Preconditions/Assumptions: class will be called by Program.cs, 
 * 
 * 
 * Output: inform user/player throughout gameplay via the rich text box, send destination object to MoveTo function for player navigation  
 * 
 * Postconditions: destination object sent must be of Location class 
 * 
 * Algorithm:
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * Estimates:        Actuals:
 * Design: 2 hours
 * Implementation: 4hours
 * Testing: 2 hours
 * 
 */

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
        private Player _player;
        // create global boss variable (class level)
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
            InventoryItem lastItem = (InventoryItem)_player.Inventory.LastOrDefault();
            rtbMessages.Text += "You have been equipped with a " + lastItem.Details.Name;
            ScrollToBottomOfMessages();

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
                rtbMessages.Text += "You must have a " + destination.ItemRequiredToEnter.Name + " to enter this location." + Environment.NewLine;
                ScrollToBottomOfMessages();
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
            rtbLocation.Text = destination.Description + Environment.NewLine;

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
                            rtbMessages.Text += "You completed the " + destination.QuestAvailableHere.Name + "quest. " + Environment.NewLine;
                            ScrollToBottomOfMessages();

                            _player.RemoveQuestCompletionItems(destination.QuestAvailableHere);

                            // give quest rewareds
                            rtbMessages.Text += "You receive: " + Environment.NewLine;
                            rtbMessages.Text += destination.QuestAvailableHere.RewarExperiencePoints.ToString() + " experience points." + Environment.NewLine;
                            rtbMessages.Text += destination.QuestAvailableHere.RewardGold.ToString() + " gold" + Environment.NewLine;
                            rtbMessages.Text += destination.QuestAvailableHere.RewardItem.Name + Environment.NewLine;
                            rtbMessages.Text += Environment.NewLine;
                            ScrollToBottomOfMessages();

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
                    rtbMessages.Text += "You receive the " + destination.QuestAvailableHere.Name + " quest." + Environment.NewLine;
                    rtbMessages.Text += destination.QuestAvailableHere.Description + Environment.NewLine;
                    rtbMessages.Text += "To complete it, return with: " + Environment.NewLine;
                    ScrollToBottomOfMessages();
                    foreach (QuestCompletionItem qci in destination.QuestAvailableHere.QuestCompletionItems)
                    {
                        if(qci.Quantity == 1)
                        {
                            rtbMessages.Text += qci.Quantity.ToString() + " " + qci.Details.Name + Environment.NewLine;
                            ScrollToBottomOfMessages();
                        }
                        else
                        {
                            rtbMessages.Text += qci.Quantity.ToString() + " " + qci.Details.NamePlural + Environment.NewLine;
                            ScrollToBottomOfMessages();
                        }
                    }
                    rtbMessages.Text += Environment.NewLine;
                    ScrollToBottomOfMessages();
                    // add this quest to  player's quest list
                    _player.Quests.Add(new PlayerQuest(destination.QuestAvailableHere));
                }
            }
            // check if there is a boss at this loaciotn
            if(destination.BossLivingHere != null)
            {
                // if true , diplay mesage of what player sees
                rtbMessages.Text += "You see a " + destination.BossLivingHere.Name + Environment.NewLine;
                ScrollToBottomOfMessages();

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

            dgvQuests.Rows.Clear();

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
            // Get the currently selected weapon from the cboweapons comboBox
            /*            int selectedIndex = cboWeapons.SelectedIndex;
                        Object selectedItem = cboWeapons.SelectedItem;*/
            Weapon currentWeapon = (Weapon)cboWeapons.SelectedItem;

            // Dtermine amount of damage the player does to teh boss
            int damageToBoss = RandomNumberGenerator.NumberBetween(currentWeapon.MinimumDamage, currentWeapon.MaximumDamage);
            // aply the damage to the boss' CurrentHitPoints
            _currentBoss.CurrentHitPoints -= damageToBoss;
            // display message
            rtbMessages.Text += "You hit the " + _currentBoss.Name + " for " + damageToBoss.ToString() + " points . " + Environment.NewLine;
            ScrollToBottomOfMessages();
            // check if the boss is dead (0 points remaining)
            if (_currentBoss.CurrentHitPoints < 1)
            {
                // display a victory message
                rtbMessages.Text += "The " + _currentBoss.Name + " is down! Well done!" + Environment.NewLine;
                // give player xp points for killing the boss
                _player.ExperiencePoints += _currentBoss.RewardExperiencePoints;
                ScrollToBottomOfMessages();
                // display message
                rtbMessages.Text += "You gained " + _currentBoss.RewardExperiencePoints.ToString() + " XP points!" + Environment.NewLine;
                ScrollToBottomOfMessages();
                // give teh player gold for kiling the boss
                _player.Gold += _currentBoss.RewardGold;
                // display message
                rtbMessages.Text += "And you earned " + _currentBoss.RewardGold.ToString() + " pieces of gold!" + Environment.NewLine + Environment.NewLine;
                ScrollToBottomOfMessages();
                // get random loot items from the boss
                List<InventoryItem> lootedItems = new List<InventoryItem>();

                // display message for each loot item as it is added to the looted items list
                // NOTE: need to compare a random number to the drop percentage 
                foreach (LootItem lootItem in _currentBoss.LootTable)
                {
                    if(RandomNumberGenerator.NumberBetween(1, 100) <= lootItem.DropPercentage)
                    {
                        // add item to lootedItems list (as a InventoryItem) which will be then added to player's inventory
                        lootedItems.Add(new InventoryItem(lootItem.Details, 1));
                        rtbMessages.Text += lootItem.Details.Name + " has been looted!" + Environment.NewLine;
                        ScrollToBottomOfMessages();
                    }
                }
                // if no items were randomly selected, then add the default loot item(s)
                if(lootedItems.Count == 0)
                {
                    foreach (LootItem lootItem in _currentBoss.LootTable)
                    {
                        if (lootItem.IsDefaultItem)
                        {
                            lootedItems.Add(new InventoryItem(lootItem.Details, 1));
                            rtbMessages.Text += lootItem.ToString() + " as DEFAULT " + Environment.NewLine;
                            ScrollToBottomOfMessages();
                        }
                    }
                }
                // add the looted items to the player's inventory
                foreach (InventoryItem inventoryItem in lootedItems)
                {
                    _player.AddItemToInventory(inventoryItem.Details);
                    if(inventoryItem.Quantity == 1)
                    {
                        rtbMessages.Text += "You loot " + inventoryItem.Quantity.ToString() + " " + inventoryItem.Details.Name + Environment.NewLine;
                        ScrollToBottomOfMessages();
                    }
                    else
                    {
                        rtbMessages.Text += "You looted a total of" + inventoryItem.Quantity.ToString() + " " + inventoryItem.Details.NamePlural + Environment.NewLine;
                        ScrollToBottomOfMessages();
                    }
                }
                // refresh player data on UI
                // gold and experience opints
                /*            lblExperience.Refresh();
                                lblGold.Refresh();
                                lblHitPoints.Refresh();*/
                lblHitPoints.Text = _player.CurrentHitPoints.ToString();
                lblGold.Text = _player.Gold.ToString();
                lblExperience.Text = _player.ExperiencePoints.ToString();
                lblLevel.Text = _player.Level.ToString();
                UpdateInventoryListInUI();
                UpdateWeaponListInUI();
                UpdatePotionListInUI(); 
            
            // move playre to current location
            // this will heal the player and create a new boss
            MoveTo(_player.CurrentLocation);
            }

            // if the boss is still alive
            else
            {
                // determine the amount of damage the boss does to the player
                int damageToPlayer = RandomNumberGenerator.NumberBetween(currentWeapon.MinimumDamage, currentWeapon.MaximumDamage);
                // display message
                rtbMessages.Text += "OH " + _currentBoss.Name + " has hit you for " + damageToPlayer.ToString() + " points!" + Environment.NewLine;
                ScrollToBottomOfMessages();
                // subtract the damage from player's CurrentHitPoints
                _player.CurrentHitPoints -= damageToPlayer;
                // refresh player data in UI
                lblHitPoints.Text = _player.CurrentHitPoints.ToString();
                // if player is dead (0 hit points remaining)
                if(_player.CurrentHitPoints <= 0)
                {
                    // display message
                    rtbMessages.Text += "OH NO! You are done for! Better luck next time..." + Environment.NewLine + Environment.NewLine;
                    ScrollToBottomOfMessages();
                    // move player to "home" location
                    MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
                }
            }
        }

        // button use potion
        private void btnUsePotion_Click(object sender, EventArgs e)
        {
            //Get currently selected potion from cboPotions ComboBox
            HealingPotion currentPotion = (HealingPotion)cboPotions.SelectedItem;
            // Add healing amount to player's CurrentHitPoint
            _player.CurrentHitPoints += currentPotion.AmountToHeal;
           // _player.CurrentHitPoints += currentPotion.AmountToHeal;
            // CurrentHitPoints cannot exceed player's MaximumHitPoints
            if(_player.CurrentHitPoints > _player.MaximumHitPoints)
            {
                _player.CurrentHitPoints = _player.MaximumHitPoints;
            }
            // remove the potion from the player's inventory
            foreach (InventoryItem ii in _player.Inventory)
            {
                if(ii.Details.ID == currentPotion.ID)
                {
                    ii.Quantity--;
                    break;
                }
            }
            // display message 
            rtbMessages.Text += "You used the " + currentPotion.Name + " potion. " + Environment.NewLine;
            // boss gets their turn to attack
            // determine the amount of damage the boss does to the player
            int damageToPlayer = RandomNumberGenerator.NumberBetween(0, _currentBoss.MaximumDamage);
            // display message 
            rtbMessages.Text += "The " + _currentBoss.Name + " did " + damageToPlayer.ToString() + " points of damage. " + Environment.NewLine;
            ScrollToBottomOfMessages();
            // subtract damage from the player's CurrentHitPoints
            _player.CurrentHitPoints -= damageToPlayer;
            // if player is dead (0 opints)
            if(_player.CurrentHitPoints < 1)
            {
                // display message
                rtbMessages.Text += "The " + _currentBoss + " killed you." + Environment.NewLine;
                ScrollToBottomOfMessages();
                // move player to Home location
                MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            }

            // refresh player data in UI
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            //lblGold.Text = _player.Gold.ToString();
            //lblExperience.Text = _player.ExperiencePoints.ToString();
            //lblLevel.Text = _player.Level.ToString();
            UpdateInventoryListInUI();
            UpdatePotionListInUI();
        }   // end btnUsePotion_Click()

        // avoid manually scrolling of text box
        private void ScrollToBottomOfMessages()
        {
            rtbMessages.SelectionStart = rtbMessages.Text.Length;
            rtbMessages.ScrollToCaret();
        }   // end ScrollToBottomOfMessages()
    }
}
