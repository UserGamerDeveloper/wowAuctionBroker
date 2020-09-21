using info;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace wowCalc
{
    public partial class Form1 : Form
    {
        static Dictionary<string, Server> serversByName;
        static List<Label> Labels = new List<Label>();
        static Dictionary<int, TextBox> TextBoxByIdItem = new Dictionary<int, TextBox>();
        static List<RealmSummaryData> realmsData = new List<RealmSummaryData>();

        class RealmSummaryData
        {
            public string Name { get; set; }
            public string Money { get; set; }
            public string WaitMoney { get; set; }
            public string LastUpdate { get; set; }
        }

        public Form1()
        {
            InitializeComponent();

            comboBox2.Enabled = false;

            serversByName = Loader.DeserializeServers();

            foreach (var server in serversByName.Values.OrderByDescending(server => server.Money))
            {
                comboBox1.Items.Add(server.name);
                realmsData.Add(new RealmSummaryData()
                {
                    Money = Util.ConvertCopperToGold(server.Money).ToString("N0"),
                    WaitMoney = Util.ConvertCopperToGold(server.moneyMax - server.Money).ToString("N0"),
                    Name = server.name,
                    LastUpdate = string.Format("{0:0.} минут назад", DateTime.Now.Subtract(server.timeUpdate).TotalMinutes)
                });
            }
            comboBox1.SelectedIndexChanged += OnContextMenuChanged;
            textBox1.TextChanged += TextBox1_TextChanged;
            dataGridView1.DataSource = realmsData;
            foreach (var server in serversByName.Values)
            {
                new Thread(new ThreadStart(server.Parse))
                {
                    IsBackground = true
                }
                .Start();
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }

        private void OnContextMenuChanged(object sender, EventArgs e)
        {
            DeleteItems();
            comboBox2.Items.Clear();
            comboBox2.Text = "";
            comboBox2.Enabled = true;

            foreach (var key in serversByName[comboBox1.SelectedItem.ToString()].RecipeDataTrees.Keys)
            {
                comboBox2.Items.Add(key);
            }

            comboBox2.SelectedIndexChanged += OnSelectedIndexChanged;
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            DeleteItems();
            HashSet<ItemData> itemsData = new HashSet<ItemData>();
            foreach (var recipeTree in serversByName[comboBox1.SelectedItem.ToString()].RecipeDataTrees[comboBox2.SelectedItem.ToString()])
            {
                foreach (var id in recipeTree.ItemsData)
                {
                    itemsData.Add(id);
                }
            }
            foreach (var itemData in itemsData)
            {
                int width = 300;
                Point location;
                if (Labels.Count == 0)
                {
                    location = comboBox2.Location;
                    location.Y += comboBox2.Height + 10;
                }
                else
                {
                    location = Labels[Labels.Count - 1].Location;
                    location.Y += Labels[Labels.Count - 1].Height + 10;
                }
                Label label = new Label
                {
                    Size = new System.Drawing.Size(width, 20),
                    Text = itemData.itemName,
                    Location = location
                };
                Labels.Add(label);
                tabControl1.TabPages[1].Controls.Add(label);
                Point point = Labels[Labels.Count - 1].Location;
                point.X += width + 50;
                TextBox textBox = new TextBox
                {
                    Location = point,
                    Name = itemData.id.ToString(),
                    //Text = "0",
                    Size = new System.Drawing.Size(100, 20)
                };
                TextBoxByIdItem.Add(itemData.id, textBox);
                tabControl1.TabPages[1].Controls.Add(textBox);
            }
            if (TextBoxByIdItem.Count == 1)
            {
                Server server = serversByName[comboBox1.SelectedItem.ToString()];
                RecipeData recipeData = server.RecipeDataTrees[comboBox2.SelectedItem.ToString()].First();
                long spending = Convert.ToInt64(recipeData.SPENDING * server.GetSpendingRate());
                TextBoxByIdItem.First().Value.Text = string.Format("{0:# ##}",
                    Math.Floor((recipeData.SellNormalPrice - spending) / recipeData.ID_ITEM_AND_NEED_AMOUNT.Values.First() / 100f));
            }
            else
            {
                foreach (var textBox in TextBoxByIdItem.Values)
                {
                    textBox.KeyUp += new KeyEventHandler(Calculate);
                }
            }
        }

        private void Calculate(object sender, EventArgs e)
        {
            TextBox textBoxSender = sender as TextBox;
            if (textBoxSender.Text != "")
            {
                Server server = serversByName[comboBox1.SelectedItem.ToString()];
                HashSet<RecipeData> recipesDataTree = server.RecipeDataTrees[comboBox2.SelectedItem.ToString()];
                List<RecipeData> recipesData = new List<RecipeData>();
                int senderItemId = Convert.ToInt32(textBoxSender.Name);
                foreach (var recipeData in recipesDataTree)
                {
                    if (recipeData.ID_ITEM_AND_NEED_AMOUNT.Keys.Contains(senderItemId))
                    {
                        recipesData.Add(recipeData);
                    }
                }
                Dictionary<int, long> itemPriceByIdItem = new Dictionary<int, long>();
                List<RecipeData> recipesDataNeedCalculate = new List<RecipeData>();
                foreach (var recipeData in recipesData)
                {
                    List<int> idItemsEmptyPrice = new List<int>();
                    long spending = 0;
                    foreach (var idItem in recipeData.ID_ITEM_AND_NEED_AMOUNT.Keys)
                    {
                        if (TextBoxByIdItem[idItem].Text == "")
                        {
                            idItemsEmptyPrice.Add(idItem);
                        }
                        else
                        {
                            spending += Convert.ToInt64(TextBoxByIdItem[idItem].Text) * 100 * recipeData.ID_ITEM_AND_NEED_AMOUNT[idItem];
                        }
                    }
                    if (idItemsEmptyPrice.Count == 1)
                    {
                        spending += Convert.ToInt64(recipeData.SPENDING * server.GetSpendingRate());

                        long price = Convert.ToInt64(
                            Math.Floor((recipeData.SellNormalPrice - spending) / recipeData.ID_ITEM_AND_NEED_AMOUNT[idItemsEmptyPrice[0]] / 100f));
                        if (itemPriceByIdItem.ContainsKey(idItemsEmptyPrice[0]))
                        {
                            if (itemPriceByIdItem[idItemsEmptyPrice[0]] < price)
                            {
                                itemPriceByIdItem[idItemsEmptyPrice[0]] = price;
                            }
                        }
                        else
                        {
                            itemPriceByIdItem.Add(idItemsEmptyPrice[0], price);
                        }
                    }
                    if (idItemsEmptyPrice.Count == 0)
                    {
                        recipesDataNeedCalculate.Add(recipeData);
                    }
                }
                //foreach (var recipeData in recipesDataNeedCalculate)
                //{
                //    List<int> itemsId = new List<int>(recipeData.ID_ITEM_AND_NEED_AMOUNT.Keys);
                //    itemsId.Remove(senderItemId);
                //    foreach (var itemId in itemsId)
                //    {
                //        List<int> itemsIdConst = new List<int>(itemsId);
                //        int targetItemId = itemId;
                //        itemsIdConst.Remove(targetItemId);
                //        long spending = Convert.ToInt64(recipeData.SPENDING * server.GetSpendingRate());
                //        foreach (var itemIdConst in itemsIdConst)
                //        {
                //            spending += Convert.ToInt64(TextBoxByIdItem[itemIdConst].Text) * 100 * recipeData.ID_ITEM_AND_NEED_AMOUNT[itemIdConst];
                //        }
                //        spending += Convert.ToInt64(recipeData.SPENDING * server.GetSpendingRate());
                //        long price = Convert.ToInt64(
                //            Math.Floor((recipeData.SELL_PRICE - spending) / recipeData.ID_ITEM_AND_NEED_AMOUNT[targetItemId] / 100f));
                //        if (itemPriceByIdItem.ContainsKey(targetItemId))
                //        {
                //            if (itemPriceByIdItem[targetItemId] < price)
                //            {
                //                itemPriceByIdItem.Remove(targetItemId);
                //                itemPriceByIdItem.Add(targetItemId, price);
                //            }
                //        }
                //        else
                //        {
                //            itemPriceByIdItem.Add(targetItemId, price);
                //        }
                //    }
                //}
                foreach (var idItem in itemPriceByIdItem.Keys)
                {
                    TextBoxByIdItem[idItem].Text = itemPriceByIdItem[idItem].ToString();
                }
            }
        }

        private void DeleteItems()
        {
            foreach (var label in Labels)
            {
                tabControl1.TabPages[1].Controls.Remove(label);
            }
            Labels.Clear();
            foreach (var textBox in TextBoxByIdItem.Values)
            {
                tabControl1.TabPages[1].Controls.Remove(textBox);
            }
            TextBoxByIdItem.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var textBox in TextBoxByIdItem.Values)
            {
                textBox.Text = "";
            }
        }
    }
}