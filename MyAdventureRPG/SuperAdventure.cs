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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void SuperAdventure_Load(object sender, EventArgs e)
        {

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

                        foreach(QuestCompletionItem qci in destination.QuestAvailableHere.QuestCompletionItems)
                        {
                            bool foundItemInPlayersInventory = false;

                            // check all inventory items to see if they have it and enought quantity
                            foreach (InventoryItem ii in _player.Inventory)
                            {
                                // check for item ID in player's inventory
                                if(ii.Details.ID == qci.Details.ID)
                                {
                                    foundItemInPlayersInventory = true;

                                    if(ii.Quantity < qci.Quantity)
                                    {
                                        // player does not have enough of this item to complete the quest
                                        playerHasAllItemsToCompleteQuest = false;

                                        // no need to continue checking for other questitems
                                        break;
                                    }
                                    // item found, do not check the rest of player's inventory
                                    break;
                                }
                            }
                            // if item not found, set our var and stop looking for other items
                            if(!foundItemInPlayersInventory)
                            {
                                // player does not have this item in inventory
                                playerHasAllItemsToCompleteQuest = false;
                                // no need to continue to searhc for other quest completion items
                                break;
                            }
                        }
                        // player has all items to complete the qest
                        if (playerHasAllItemsToCompleteQuest)
                        {
                            // complete the quest
                            // display message
                            rtbMessages.Text += Environment.NewLine;
                            rtbMessages.Text += "You coimplete the " + destination.QuestAvailableHere.Name + "quest. " + Environment.NewLine;

                            // remove quest completion items from inventory
                            foreach(QuestCompletionItem qci in destination.QuestAvailableHere.QuestCompletionItems)
                            {
                                foreach(InventoryItem ii in _player.Inventory)
                                {
                                    if(ii.Details.ID == qci.Details.ID)
                                    {
                                        ii.Quantity -= qci.Quantity;
                                        break;
                                    }
                                }
                            }

                            // give quest rewareds
                            rtbMessages.Text += "You receive: " + Environment.NewLine;
                            rtbMessages.Text += destination.QuestAvailableHere.RewarExperiencePoints.ToString() + "experience points "+ Environment.NewLine;
                            rtbMessages.Text += destination.QuestAvailableHere.RewardGold.ToString() + " gold" + Environment.NewLine;
                            rtbMessages.Text += destination.QuestAvailableHere.RewardItem.Name + Environment.NewLine;
                            rtbMessages.Text += Environment.NewLine;

                            _player.ExperiencePoints += destination.QuestAvailableHere.RewarExperiencePoints;
                            _player.Gold += destination.QuestAvailableHere.RewardGold;

                            // add the reward item to the player's inventory
                            bool addedItemToPlayerInventory = false;

                            foreach (InventoryItem ii in _player.Inventory)
                            {
                                // check for existence of this item in inventory
                                if(ii.Details.ID == destination.QuestAvailableHere.RewardItem.ID)
                                {
                                    // player already has item, increase the quantity
                                    ii.Quantity++;

                                    addedItemToPlayerInventory = true;

                                    break;
                                }
                            }

                            // player does not have the item, add it to their inventory
                            if (!addedItemToPlayerInventory)
                            {
                                _player.Inventory.Add(new InventoryItem(destination.QuestAvailableHere.RewardItem, 1));
                            }

                            // mark player's quest as completed
                            // find the quest in the play'ers quest list
                            foreach (PlayerQuest pq in _player.Quests)
                            {
                                if(pq.Details.ID == destination.QuestAvailableHere.ID)
                                {
                                    pq.IsCompleted = true;

                                    break;
                                }
                            }
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

            // refresh the player's inventory list in the UI (in case it has changed)
            dgvInventory.RowHeadersVisible = false;

            dgvInventory.ColumnCount = 2;
            dgvInventory.Columns[0].Name = "Name";
            dgvInventory.Columns[0].Width = 197;
            dgvInventory.Columns[1].Name = "Quantity";

            dgvInventory.Rows.Clear();

            foreach(InventoryItem inventoryItem in _player.Inventory)
            {
                if(inventoryItem.Quantity > 0)
                {
                    dgvInventory.Rows.Add(new[] { inventoryItem.Details.Name, inventoryItem.Quantity.ToString() });
                }
            }

            // refresh the player's queest list in the UI (in case it has changeed)
            dgvQuests.RowHeadersVisible = false;

            dgvQuests.ColumnCount = 2;
            dgvQuests.Columns[0].Name = "Name";
            dgvQuests.Columns[0].Width = 197;
            dgvQuests.Columns[1].Name = "Done?";

            dgvQuests.Rows.Clear();

            foreach (PlayerQuest playerQuest in _player.Quests)
            {
                dgvQuests.Rows.Add(new[] { playerQuest.Details.Name, playerQuest.IsCompleted.ToString() });
            }

            // refresh cboWeapons combobox in the UI
            List<Weapon> weapons = new List<Weapon>();

            foreach (InventoryItem inventoryItem in _player.Inventory)
            {
                if (inventoryItem.Details is Weapon)
                {
                    if(inventoryItem.Quantity > 0)
                    {
                        weapons.Add((Weapon)inventoryItem.Details);
                    }    
                }
            }

            if(weapons.Count == 0)
            {
                // player has no wepons, hide the weapon combobox and "use" buttons
                cboWeapons.Visible = false;
                btnUseWeapon.Visible = false;
            }
            else
            {
                cboWeapons.DataSource = weapons;
                cboWeapons.DisplayMember = "Name";
                cboWeapons.ValueMember = "ID";

                // start at iundex 0
                cboWeapons.SelectedIndex = 0;
            }

            // repopulate potions comboboxes (in case the inventory has changed)
            List<HealingPotion> healingPotions = new List<HealingPotion>();

            foreach (InventoryItem inventoryItem in _player.Inventory)
            {
                if(inventoryItem.Details is HealingPotion)
                {
                    if(inventoryItem.Quantity > 0)
                    {
                        healingPotions.Add((HealingPotion)inventoryItem.Details);
                    }
                }
            }

            if(healingPotions.Count == 0)
            {
                // player has no more helaing potions, hide teh combobox and "use" button
                cboPotions.Visible = false;
                btnUsePotion.Visible = false;
            }
            else
            {
                cboPotions.DataSource = healingPotions;
                cboPotions.DisplayMember = "Name";
                cboPotions.ValueMember = "ID";

                cboPotions.SelectedIndex = 0;
            }
        }   // end MoveTo()

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
