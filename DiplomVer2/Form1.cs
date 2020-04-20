using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiplomVer2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormAddProject(listBox1);
            form.Show();
            DataBaseWorker.ListBoxRefresh(listBox1);
        }

        private void buttonChooseDB_Click(object sender, EventArgs e)
        {
            var open = new OpenFileDialog();
            open.Filter = "База данных (*.mdf)|*.mdf" +
                "" +
                "|Все файлы (*.*)|*.*";
            if (open.ShowDialog() == DialogResult.OK)
            {
                labelDBName.Text = open.SafeFileName; //имя.расширение
                labelDBName.ForeColor = Color.White;
                labelDBName.Image = null;
                //labelDBName.Image = Image.FromFile(@"C:\Users\Dan\Desktop\icons\icons8-database-view-32.png");

                DataBaseWorker.ConnectionDataBase(open.FileName);
                DataBaseWorker.ListBoxItemsAdd(listBox1);
                DataBaseWorker.ListBoxRefresh(listBox1);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DataBaseWorker.ButtonDelete(listBox1.SelectedItem);

            DataBaseWorker.ListBoxRefresh(listBox1);
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {  
            try
            {
                ProjectName.projectName = listBox1.SelectedItem.ToString();

                ucResult1.label1.Text = ProjectName.projectName;

                DataBaseWorker.ConstructorFillFromDB("ValueEMS", ProjectName.projectName);

                DataBaseWorker.CreateTableResult(DataBaseWorker.calculate.ProjectName, DataBaseWorker.calculate.TimeValueElecticShortVoltage(10000, 500));

                DataBaseWorker.ChartsLoad(ucResult1.cartesianChart1, DataBaseWorker.calculate.ProjectName, "Напряженность электрического поля в ближней зоне излучения");

                DataBaseWorker.DeleteTableRusult(DataBaseWorker.calculate.ProjectName);

                DataBaseWorker.CreateTableResult(DataBaseWorker.calculate.ProjectName, DataBaseWorker.calculate.TimeValueElecticLongVoltage(100, 5));

                DataBaseWorker.ChartsLoad(ucResult1.cartesianChart2, DataBaseWorker.calculate.ProjectName, "Напряженность электрического поля в дальней зоне излучения");

                DataBaseWorker.DeleteTableRusult(DataBaseWorker.calculate.ProjectName);

                DataBaseWorker.CreateTableResult(DataBaseWorker.calculate.ProjectName, DataBaseWorker.calculate.TimeValueMagneticShortVoltage(100, 5));

                DataBaseWorker.ChartsLoad(ucResult1.cartesianChart3, DataBaseWorker.calculate.ProjectName, "Напряженность магнитного поля в ближней зоне излучения");

                DataBaseWorker.DeleteTableRusult(DataBaseWorker.calculate.ProjectName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ucResult1.Visible = true;
            ucChange1.Visible = false;
            ucHome1.Visible = false;
        }

        private void buttonChange_Click(object sender, EventArgs e)
        {
            try
            {
                string text = listBox1.SelectedItem.ToString();
                ProjectName.projectName = text;
                ucChange1.label1.Text = text;

                ucChange1.textBoxWireLength.Text = DataBaseWorker.GetValueFromDB("wireLength ", text);
                ucChange1.textBoxResZ1.Text = DataBaseWorker.GetValueFromDB("resistanceZ1", text);
                ucChange1.textBoxRezZ2.Text = DataBaseWorker.GetValueFromDB("resistanceZ2", text);
                ucChange1.textBoxDistanceBeetweenWire.Text = DataBaseWorker.GetValueFromDB("distanceBetweenWire", text);
                ucChange1.textBoxRadiusWire.Text = DataBaseWorker.GetValueFromDB("radiusWire", text);
                ucChange1.textBoxElectricAntennaWite.Text = DataBaseWorker.GetValueFromDB("electricAntennaWire", text);
                ucChange1.textBoxLengthAntennaWire.Text = DataBaseWorker.GetValueFromDB("lengthAntennfWire", text);
                ucChange1.textBoxDielectricConstant.Text = DataBaseWorker.GetValueFromDB("dielectricConstant", text);
                ucChange1.textBoxCirculatFrequency.Text = DataBaseWorker.GetValueFromDB("circularFrequency", text);
                ucChange1.textBoxWaveLengthOfField.Text = DataBaseWorker.GetValueFromDB("wavelengthOfField", text);
                ucChange1.textBoxMediumPower.Text = DataBaseWorker.GetValueFromDB("mediumPower", text);
                ucChange1.textBoxCoeffAntenn.Text = DataBaseWorker.GetValueFromDB("сoeffAntenn", text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ucResult1.Visible = false;
            ucChange1.Visible = true;
            ucHome1.Visible = false;
           


        }

        private void Button1_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ucResult1.Visible = false;
            ucHome1.Visible = true;
            ucChange1.Visible = false;
        }

        private void ucChange2_Load(object sender, EventArgs e)
        {
            
        }
    }

    static class ProjectName
    {
        public static string projectName { get; set; }
    }
}
