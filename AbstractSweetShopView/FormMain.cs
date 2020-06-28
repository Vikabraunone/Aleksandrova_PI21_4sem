﻿using AbstractSweetShopBusinessLogic.BindingModels;
using AbstractSweetShopBusinessLogic.BusinessLogics;
using AbstractSweetShopBusinessLogic.Interfaces;
using System;
using System.Windows.Forms;
using Unity;

namespace AbstractSweetShopView
{
    public partial class FormMain : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly MainLogic logic;

        private readonly IOrderLogic orderLogic;

        private ReportLogic reportLogic;

        private readonly WorkModeling workModeling;

        public FormMain(MainLogic logic, IOrderLogic orderLogic, ReportLogic reportLogic, WorkModeling workModeling)
        {
            InitializeComponent();
            this.logic = logic;
            this.orderLogic = orderLogic;
            this.reportLogic = reportLogic;
            this.workModeling = workModeling;
        }

        private void ингредиентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormIngredients>();
            form.ShowDialog();
        }

        private void кондитерскиеИзделияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormProducts>();
            form.ShowDialog();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var list = orderLogic.Read(null);
                if (list != null)
                {
                    dataGridView.DataSource = list;
                    dataGridView.Columns[0].Visible = false;
                    dataGridView.Columns[1].Visible = false;
                    dataGridView.Columns[2].Width = 200;
                    dataGridView.Columns[3].Visible = false;
                    dataGridView.Columns[4].Width = 150;
                    dataGridView.Columns[5].Visible = false;
                    dataGridView.Columns[6].Width = 200;
                    dataGridView.Columns[7].Width = 100;
                    dataGridView.Columns[8].Width = 100;
                    dataGridView.Columns[9].Width = 100;
                    dataGridView.Columns[10].Width = 100;
                    dataGridView.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCreateOrder_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormCreateOrder>();
            if (form.ShowDialog() == DialogResult.OK)
                LoadData();
        }

        private void buttonTakeOrderInWork_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);
                try
                {
                    logic.TakeOrderInWork(new ChangeStatusBindingModel { OrderId = id });
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonOrderReady_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);
                try
                {
                    logic.FinishOrder(new ChangeStatusBindingModel { OrderId = id });
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonPayOrder_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);
                try
                {
                    logic.PayOrder(new ChangeStatusBindingModel { OrderId = id });
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonOrderRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void productToWordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog { Filter = "docx|*.docx" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    reportLogic.SaveProductsToWord(new ReportBindingModel { FileName = dialog.FileName });
                    MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ordersToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormReportOrders>();
            form.ShowDialog();
        }

        private void productIngredientToPdfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormReportProductIngredient>();
            form.ShowDialog();
        }

        private void клиентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormClients>();
            form.ShowDialog();
            LoadData();
        }

        private void исполнителиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormImplementers>();
            form.ShowDialog();
            LoadData();
        }

        private void запускРаботToolStripMenuItem_Click(object sender, EventArgs e)
        {
            workModeling.DoWork();
        }

        private void письмаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormMessages>();
            form.ShowDialog();
        }
    }
}