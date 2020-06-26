using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AMC2GIFT_GUI
{
    public partial class ControllerGUI : Form
    {
        public ControllerGUI()
        {
            InitializeComponent();
            initConversionTab();
            initAnalyseTab();
            CenterToScreen();
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;

        }
        Boolean enabled1 = false;
        Boolean enabled2 = false;

        OpenFileDialog ofd1 = new OpenFileDialog();
        FolderBrowserDialog ofd2 = new FolderBrowserDialog();
        OpenFileDialog ofd3 = new OpenFileDialog();

        private void initConversionTab()
        {
            //init
            button1.Enabled = false;
            radioButton1.Checked = true;
            radioButton5.Checked = true;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.ScrollBars = ScrollBars.Both;
            textBox3.WordWrap = false;
            label3.Text = "Comment utiliser :"
                + Environment.NewLine + "Selectionner le format du fichier source à droite et son chemin,"
                + Environment.NewLine + "ensuite choisir le format d'exportation et le dossier où sauvegarder le résultat." +
                Environment.NewLine + "Finalement, cliquer sur le bouton central pour démarrer la conversion."
                 +Environment.NewLine + "Le resultat s'entitulera \"resultatConversion\"";
        }
        private void initAnalyseTab()
        {
            //init
            button6.Enabled = false;
            radioButton9.Checked = true;
            textBox6.Enabled = false;
            textBox4.ScrollBars = ScrollBars.Both;
            textBox4.WordWrap = false;
            helpanalyse.Text = "Comment utiliser :"
                + Environment.NewLine + "Selectionner le format du fichier source à droite et son chemin,"
                + Environment.NewLine + "ensuite, cliquer sur le bouton à droite pour démarrer l'analyse de syntaxe.";
        }

        private void enableConversionButton()
        {
            if (enabled1 && enabled2)
                button1.Enabled = true;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (ofd1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = ofd1.FileName;
                enabled1 = true;
                enableConversionButton();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (ofd2.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = ofd2.SelectedPath;
                enabled2 = true;
                enableConversionButton();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (ofd3.ShowDialog() == DialogResult.OK)
            {
                textBox6.Text = ofd3.FileName;
                button6.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String importformat = "AMC";
            String exportformat = "AMC";
            foreach (RadioButton rdo in groupBox1.Controls.OfType<RadioButton>())
            {
                if (rdo.Checked)
                {
                    importformat = rdo.Text;
                    break;
                }
            }

            foreach (RadioButton rdo in groupBox2.Controls.OfType<RadioButton>())
            {
                if (rdo.Checked)
                {
                    exportformat = rdo.Text;
                    break;
                }
            }
            if (Path.GetExtension(textBox1.Text).ToLower().Equals(".txt") || Path.GetExtension(textBox1.Text).ToLower().Equals(".xml"))
            {
                var p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = "AMC2GIFT.exe";
                String extension = ".txt";
                if (exportformat.Equals("XMLMOODLE"))
                    extension = ".xml";
                p.StartInfo.Arguments = "-c " + importformat + " " + textBox1.Text + " " + exportformat + " " + textBox2.Text + "\\resultatConversion" + extension;
                p.Start();
                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                textBox3.Text = output;
                MessageBox.Show("Conversion terminée!");
            }
            else MessageBox.Show("Erreur!\nFormat du fichier en entrée invalide!\nFormats txt et xml supportés uniquement.");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            String analyseformat = "AMC";
            foreach (RadioButton rdo in groupBox3.Controls.OfType<RadioButton>())
            {
                if (rdo.Checked)
                {
                    analyseformat = rdo.Text;
                    break;
                }
            }
            if(Path.GetExtension(textBox6.Text).ToLower().Equals(".txt") || Path.GetExtension(textBox6.Text).ToLower().Equals(".xml"))
            {
                var p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = "AMC2GIFT.exe";
                p.StartInfo.Arguments = "-a " + analyseformat + " " + textBox6.Text;
                p.Start();
                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                textBox4.Text = output;
                MessageBox.Show("Analyse terminée!");
            }
            else MessageBox.Show("Erreur!\nFormat du fichier en entrée invalide!\nFormats txt et xml supportés uniquement.");
        }
    }
}
