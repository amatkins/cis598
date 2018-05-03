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
        private Timer _repeat;

        public AppWindow()
        {
            InitializeComponent();

            _model = new PolyEModel(100, 8, 2, 60);
            branchesText.Text = _model.BranchesString();
            leavesText.Text = _model.LeavesString();
            agentsText.Text = _model.AgentsString();

            _repeat = new Timer();
            _repeat.Interval = 100;
            _repeat.Tick += _repeat_Tick;

            SaveModel(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/model.xml");
        }

        private void _repeat_Tick(object sender, EventArgs e)
        {
            _model.Tick();
            branchesText.Text = _model.BranchesString();
            leavesText.Text = _model.LeavesString();
            agentsText.Text = _model.AgentsString();
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
            if (_repeat.Enabled)
                _repeat.Stop();
            else
                _repeat.Start();
        }
    }
}
