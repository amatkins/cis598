using System;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace PolyE
{
    public partial class AppWindow : Form
    {
        private PolyEModel _model;

        public AppWindow()
        {
            InitializeComponent();

            _model = new PolyEModel(20, 5, 1, 40);
            branchesText.Text = _model.BranchesString();
            leavesText.Text = _model.LeavesString();
            agentsText.Text = _model.AgentsString();

            SaveModel(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/model.xml");
        }

        private void SaveModel(string path)
        {
            DataContractSerializerSettings dcSettings = new DataContractSerializerSettings();
            dcSettings.KnownTypes = new Type[] { typeof(Expression), typeof(ExprGenerator), typeof(Conditional), typeof(Modification) };

            DataContractSerializer ser = new DataContractSerializer(typeof(PolyEModel), dcSettings);

            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.CloseOutput = true;
            xmlSettings.Indent = true;
            xmlSettings.IndentChars = "  ";
            xmlSettings.NewLineChars = Environment.NewLine;
            //xmlSettings.NewLineHandling = NewLineHandling.Entitize;
            xmlSettings.NewLineOnAttributes = false;
            xmlSettings.WriteEndDocumentOnClose = false;

            using (XmlWriter stream = XmlWriter.Create(path, xmlSettings))
            {
                ser.WriteObject(stream, _model);
                stream.Close();
            }
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            _model.Tick();
            branchesText.Text = _model.BranchesString();
            leavesText.Text = _model.LeavesString();
            agentsText.Text = _model.AgentsString();
        }
    }
}
