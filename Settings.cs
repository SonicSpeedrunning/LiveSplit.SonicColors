using System;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.SonicColors
{
    public partial class Settings : UserControl
    {
        public bool StartAny { get; set; }
        public bool StartSonicSimulator { get; set; }
        public bool StartEggShuttle { get; set; }
        public bool ResetAny { get; set; }
        public bool ResetEggShuttle { get; set; }
        public bool TropicalResortAct1 { get; set; }
        public bool TropicalResortAct2 { get; set; }
        public bool TropicalResortAct3 { get; set; }
        public bool TropicalResortAct4 { get; set; }
        public bool TropicalResortAct5 { get; set; }
        public bool TropicalResortAct6 { get; set; }
        public bool TropicalResortBoss { get; set; }
        public bool SweetMountainAct1 { get; set; }
        public bool SweetMountainAct2 { get; set; }
        public bool SweetMountainAct3 { get; set; }
        public bool SweetMountainAct4 { get; set; }
        public bool SweetMountainAct5 { get; set; }
        public bool SweetMountainAct6 { get; set; }
        public bool SweetMountainBoss { get; set; }
        public bool StarlightCarnivalAct1 { get; set; }
        public bool StarlightCarnivalAct2 { get; set; }
        public bool StarlightCarnivalAct3 { get; set; }
        public bool StarlightCarnivalAct4 { get; set; }
        public bool StarlightCarnivalAct5 { get; set; }
        public bool StarlightCarnivalAct6 { get; set; }
        public bool StarlightCarnivalBoss { get; set; }
        public bool PlanetWispAct1 { get; set; }
        public bool PlanetWispAct2 { get; set; }
        public bool PlanetWispAct3 { get; set; }
        public bool PlanetWispAct4 { get; set; }
        public bool PlanetWispAct5 { get; set; }
        public bool PlanetWispAct6 { get; set; }
        public bool PlanetWispBoss { get; set; }
        public bool AquariumParkAct1 { get; set; }
        public bool AquariumParkAct2 { get; set; }
        public bool AquariumParkAct3 { get; set; }
        public bool AquariumParkAct4 { get; set; }
        public bool AquariumParkAct5 { get; set; }
        public bool AquariumParkAct6 { get; set; }
        public bool AquariumParkBoss { get; set; }
        public bool AsteroidCoasterAct1 { get; set; }
        public bool AsteroidCoasterAct2 { get; set; }
        public bool AsteroidCoasterAct3 { get; set; }
        public bool AsteroidCoasterAct4 { get; set; }
        public bool AsteroidCoasterAct5 { get; set; }
        public bool AsteroidCoasterAct6 { get; set; }
        public bool AsteroidCoasterBoss { get; set; }
        public bool TerminalVelocityAct1 { get; set; }
        public bool TerminalVelocityBoss { get; set; }
        public bool TerminalVelocityAct2 { get; set; }
        public bool SonicSim1_1 { get; set; }
        public bool SonicSim1_2 { get; set; }
        public bool SonicSim1_3 { get; set; }
        public bool SonicSim2_1 { get; set; }
        public bool SonicSim2_2 { get; set; }
        public bool SonicSim2_3 { get; set; }
        public bool SonicSim3_1 { get; set; }
        public bool SonicSim3_2 { get; set; }
        public bool SonicSim3_3 { get; set; }
        public bool SonicSim4_1 { get; set; }
        public bool SonicSim4_2 { get; set; }
        public bool SonicSim4_3 { get; set; }
        public bool SonicSim5_1 { get; set; }
        public bool SonicSim5_2 { get; set; }
        public bool SonicSim5_3 { get; set; }
        public bool SonicSim6_1 { get; set; }
        public bool SonicSim6_2 { get; set; }
        public bool SonicSim6_3 { get; set; }
        public bool SonicSim7_1 { get; set; }
        public bool SonicSim7_2 { get; set; }
        public bool SonicSim7_3 { get; set; }

        public Settings()
        {
            InitializeComponent();

            // General settings
            chkStartAny.DataBindings.Add("Checked", this, "StartAny", false, DataSourceUpdateMode.OnPropertyChanged);
            chkStartSonicSimulator.DataBindings.Add("Checked", this, "StartSonicSimulator", false, DataSourceUpdateMode.OnPropertyChanged);
            chkStartEggShuttle.DataBindings.Add("Checked", this, "StartEggShuttle", false, DataSourceUpdateMode.OnPropertyChanged);
            chkResetAny.DataBindings.Add("Checked", this, "ResetAny", false, DataSourceUpdateMode.OnPropertyChanged);
            chkResetEggShuttle.DataBindings.Add("Checked", this, "ResetEggShuttle", false, DataSourceUpdateMode.OnPropertyChanged);

            // Tropical Resort
            chkTR1.DataBindings.Add("Checked", this, "TropicalResortAct1", false, DataSourceUpdateMode.OnPropertyChanged);
            chkTR2.DataBindings.Add("Checked", this, "TropicalResortAct2", false, DataSourceUpdateMode.OnPropertyChanged);
            chkTR3.DataBindings.Add("Checked", this, "TropicalResortAct3", false, DataSourceUpdateMode.OnPropertyChanged);
            chkTR4.DataBindings.Add("Checked", this, "TropicalResortAct4", false, DataSourceUpdateMode.OnPropertyChanged);
            chkTR5.DataBindings.Add("Checked", this, "TropicalResortAct5", false, DataSourceUpdateMode.OnPropertyChanged);
            chkTR6.DataBindings.Add("Checked", this, "TropicalResortAct6", false, DataSourceUpdateMode.OnPropertyChanged);
            chkTRB.DataBindings.Add("Checked", this, "TropicalResortBoss", false, DataSourceUpdateMode.OnPropertyChanged);

            // Sweet Mountain
            chkSM1.DataBindings.Add("Checked", this, "SweetMountainAct1", false, DataSourceUpdateMode.OnPropertyChanged);
            chkSM2.DataBindings.Add("Checked", this, "SweetMountainAct2", false, DataSourceUpdateMode.OnPropertyChanged);
            chkSM3.DataBindings.Add("Checked", this, "SweetMountainAct3", false, DataSourceUpdateMode.OnPropertyChanged);
            chkSM4.DataBindings.Add("Checked", this, "SweetMountainAct4", false, DataSourceUpdateMode.OnPropertyChanged);
            chkSM5.DataBindings.Add("Checked", this, "SweetMountainAct5", false, DataSourceUpdateMode.OnPropertyChanged);
            chkSM6.DataBindings.Add("Checked", this, "SweetMountainAct6", false, DataSourceUpdateMode.OnPropertyChanged);
            chkSMB.DataBindings.Add("Checked", this, "SweetMountainBoss", false, DataSourceUpdateMode.OnPropertyChanged);

            // Starlight Carnival
            chkSC1.DataBindings.Add("Checked", this, "StarlightCarnivalAct1", false, DataSourceUpdateMode.OnPropertyChanged);
            chkSC2.DataBindings.Add("Checked", this, "StarlightCarnivalAct2", false, DataSourceUpdateMode.OnPropertyChanged);
            chkSC3.DataBindings.Add("Checked", this, "StarlightCarnivalAct3", false, DataSourceUpdateMode.OnPropertyChanged);
            chkSC4.DataBindings.Add("Checked", this, "StarlightCarnivalAct4", false, DataSourceUpdateMode.OnPropertyChanged);
            chkSC5.DataBindings.Add("Checked", this, "StarlightCarnivalAct5", false, DataSourceUpdateMode.OnPropertyChanged);
            chkSC6.DataBindings.Add("Checked", this, "StarlightCarnivalAct6", false, DataSourceUpdateMode.OnPropertyChanged);
            chkSCB.DataBindings.Add("Checked", this, "StarlightCarnivalBoss", false, DataSourceUpdateMode.OnPropertyChanged);

            // Planet Wisp
            chkPW1.DataBindings.Add("Checked", this, "PlanetWispAct1", false, DataSourceUpdateMode.OnPropertyChanged);
            chkPW2.DataBindings.Add("Checked", this, "PlanetWispAct2", false, DataSourceUpdateMode.OnPropertyChanged);
            chkPW3.DataBindings.Add("Checked", this, "PlanetWispAct3", false, DataSourceUpdateMode.OnPropertyChanged);
            chkPW4.DataBindings.Add("Checked", this, "PlanetWispAct4", false, DataSourceUpdateMode.OnPropertyChanged);
            chkPW5.DataBindings.Add("Checked", this, "PlanetWispAct5", false, DataSourceUpdateMode.OnPropertyChanged);
            chkPW6.DataBindings.Add("Checked", this, "PlanetWispAct6", false, DataSourceUpdateMode.OnPropertyChanged);
            chkPWB.DataBindings.Add("Checked", this, "PlanetWispBoss", false, DataSourceUpdateMode.OnPropertyChanged);

            // Aquarium Park
            chkAP1.DataBindings.Add("Checked", this, "AquariumParkAct1", false, DataSourceUpdateMode.OnPropertyChanged);
            chkAP2.DataBindings.Add("Checked", this, "AquariumParkAct2", false, DataSourceUpdateMode.OnPropertyChanged);
            chkAP3.DataBindings.Add("Checked", this, "AquariumParkAct3", false, DataSourceUpdateMode.OnPropertyChanged);
            chkAP4.DataBindings.Add("Checked", this, "AquariumParkAct4", false, DataSourceUpdateMode.OnPropertyChanged);
            chkAP5.DataBindings.Add("Checked", this, "AquariumParkAct5", false, DataSourceUpdateMode.OnPropertyChanged);
            chkAP6.DataBindings.Add("Checked", this, "AquariumParkAct6", false, DataSourceUpdateMode.OnPropertyChanged);
            chkAPB.DataBindings.Add("Checked", this, "AquariumParkBoss", false, DataSourceUpdateMode.OnPropertyChanged);

            // Asteroid Coaster
            chkAC1.DataBindings.Add("Checked", this, "AsteroidCoasterAct1", false, DataSourceUpdateMode.OnPropertyChanged);
            chkAC2.DataBindings.Add("Checked", this, "AsteroidCoasterAct2", false, DataSourceUpdateMode.OnPropertyChanged);
            chkAC3.DataBindings.Add("Checked", this, "AsteroidCoasterAct3", false, DataSourceUpdateMode.OnPropertyChanged);
            chkAC4.DataBindings.Add("Checked", this, "AsteroidCoasterAct4", false, DataSourceUpdateMode.OnPropertyChanged);
            chkAC5.DataBindings.Add("Checked", this, "AsteroidCoasterAct5", false, DataSourceUpdateMode.OnPropertyChanged);
            chkAC6.DataBindings.Add("Checked", this, "AsteroidCoasterAct6", false, DataSourceUpdateMode.OnPropertyChanged);
            chkACB.DataBindings.Add("Checked", this, "AsteroidCoasterBoss", false, DataSourceUpdateMode.OnPropertyChanged);

            // Terminal Velocity
            chkTV1.DataBindings.Add("Checked", this, "TerminalVelocityAct1", false, DataSourceUpdateMode.OnPropertyChanged);
            chkTVB.DataBindings.Add("Checked", this, "TerminalVelocityBoss", false, DataSourceUpdateMode.OnPropertyChanged);
            chkTV2.DataBindings.Add("Checked", this, "TerminalVelocityAct2", false, DataSourceUpdateMode.OnPropertyChanged);

            // Sonic Simulator
            chk1_1.DataBindings.Add("Checked", this, "SonicSim1_1", false, DataSourceUpdateMode.OnPropertyChanged);
            chk1_2.DataBindings.Add("Checked", this, "SonicSim1_2", false, DataSourceUpdateMode.OnPropertyChanged);
            chk1_3.DataBindings.Add("Checked", this, "SonicSim1_3", false, DataSourceUpdateMode.OnPropertyChanged);
            chk2_1.DataBindings.Add("Checked", this, "SonicSim2_1", false, DataSourceUpdateMode.OnPropertyChanged);
            chk2_2.DataBindings.Add("Checked", this, "SonicSim2_2", false, DataSourceUpdateMode.OnPropertyChanged);
            chk2_3.DataBindings.Add("Checked", this, "SonicSim2_3", false, DataSourceUpdateMode.OnPropertyChanged);
            chk3_1.DataBindings.Add("Checked", this, "SonicSim3_1", false, DataSourceUpdateMode.OnPropertyChanged);
            chk3_2.DataBindings.Add("Checked", this, "SonicSim3_2", false, DataSourceUpdateMode.OnPropertyChanged);
            chk3_3.DataBindings.Add("Checked", this, "SonicSim3_3", false, DataSourceUpdateMode.OnPropertyChanged);
            chk4_1.DataBindings.Add("Checked", this, "SonicSim4_1", false, DataSourceUpdateMode.OnPropertyChanged);
            chk4_2.DataBindings.Add("Checked", this, "SonicSim4_2", false, DataSourceUpdateMode.OnPropertyChanged);
            chk4_3.DataBindings.Add("Checked", this, "SonicSim4_3", false, DataSourceUpdateMode.OnPropertyChanged);
            chk5_1.DataBindings.Add("Checked", this, "SonicSim5_1", false, DataSourceUpdateMode.OnPropertyChanged);
            chk5_2.DataBindings.Add("Checked", this, "SonicSim5_2", false, DataSourceUpdateMode.OnPropertyChanged);
            chk5_3.DataBindings.Add("Checked", this, "SonicSim5_3", false, DataSourceUpdateMode.OnPropertyChanged);
            chk6_1.DataBindings.Add("Checked", this, "SonicSim6_1", false, DataSourceUpdateMode.OnPropertyChanged);
            chk6_2.DataBindings.Add("Checked", this, "SonicSim6_2", false, DataSourceUpdateMode.OnPropertyChanged);
            chk6_3.DataBindings.Add("Checked", this, "SonicSim6_3", false, DataSourceUpdateMode.OnPropertyChanged);
            chk7_1.DataBindings.Add("Checked", this, "SonicSim7_1", false, DataSourceUpdateMode.OnPropertyChanged);
            chk7_2.DataBindings.Add("Checked", this, "SonicSim7_2", false, DataSourceUpdateMode.OnPropertyChanged);
            chk7_3.DataBindings.Add("Checked", this, "SonicSim7_3", false, DataSourceUpdateMode.OnPropertyChanged);

            //
            // Default Values
            //
            StartAny = true;
            StartEggShuttle = true;
            StartSonicSimulator = true;
            ResetAny = true;
            ResetEggShuttle = true;
            TropicalResortAct1 = true;
            TropicalResortAct2 = true;
            TropicalResortAct3 = true;
            TropicalResortAct4 = true;
            TropicalResortAct5 = true;
            TropicalResortAct6 = true;
            TropicalResortBoss = true;
            SweetMountainAct1 = true;
            SweetMountainAct2 = true;
            SweetMountainAct3 = true;
            SweetMountainAct4 = true;
            SweetMountainAct5 = true;
            SweetMountainAct6 = true;
            SweetMountainBoss = true;
            StarlightCarnivalAct1 = true;
            StarlightCarnivalAct2 = true;
            StarlightCarnivalAct3 = true;
            StarlightCarnivalAct4 = true;
            StarlightCarnivalAct5 = true;
            StarlightCarnivalAct6 = true;
            StarlightCarnivalBoss = true;
            PlanetWispAct1 = true;
            PlanetWispAct2 = true;
            PlanetWispAct3 = true;
            PlanetWispAct4 = true;
            PlanetWispAct5 = true;
            PlanetWispAct6 = true;
            PlanetWispBoss = true;
            AquariumParkAct1 = true;
            AquariumParkAct2 = true;
            AquariumParkAct3 = true;
            AquariumParkAct4 = true;
            AquariumParkAct5 = true;
            AquariumParkAct6 = true;
            AquariumParkBoss = true;
            AsteroidCoasterAct1 = true;
            AsteroidCoasterAct2 = true;
            AsteroidCoasterAct3 = true;
            AsteroidCoasterAct4 = true;
            AsteroidCoasterAct5 = true;
            AsteroidCoasterAct6 = true;
            AsteroidCoasterBoss = true;
            TerminalVelocityAct1 = true;
            TerminalVelocityBoss = true;
            TerminalVelocityAct2 = true;
            SonicSim1_1 = true;
            SonicSim1_2 = true;
            SonicSim1_3 = true;
            SonicSim2_1 = true;
            SonicSim2_2 = true;
            SonicSim2_3 = true;
            SonicSim3_1 = true;
            SonicSim3_2 = true;
            SonicSim3_3 = true;
            SonicSim4_1 = true;
            SonicSim4_2 = true;
            SonicSim4_3 = true;
            SonicSim5_1 = true;
            SonicSim5_2 = true;
            SonicSim5_3 = true;
            SonicSim6_1 = true;
            SonicSim6_2 = true;
            SonicSim6_3 = true;
            SonicSim7_1 = true;
            SonicSim7_2 = true;
            SonicSim7_3 = true;
        }

        public XmlNode GetSettings(XmlDocument doc)
        {
            XmlElement settingsNode = doc.CreateElement("Settings");
            settingsNode.AppendChild(ToElement(doc, "StartAny", StartAny));
            settingsNode.AppendChild(ToElement(doc, "StartEggShuttle", StartEggShuttle));
            settingsNode.AppendChild(ToElement(doc, "StartSonicSimulator", StartSonicSimulator));
            settingsNode.AppendChild(ToElement(doc, "ResetAny", ResetAny));
            settingsNode.AppendChild(ToElement(doc, "ResetEggShuttle", ResetEggShuttle));
            settingsNode.AppendChild(ToElement(doc, "TropicalResortAct1", TropicalResortAct1));
            settingsNode.AppendChild(ToElement(doc, "TropicalResortAct2", TropicalResortAct2));
            settingsNode.AppendChild(ToElement(doc, "TropicalResortAct3", TropicalResortAct3));
            settingsNode.AppendChild(ToElement(doc, "TropicalResortAct4", TropicalResortAct4));
            settingsNode.AppendChild(ToElement(doc, "TropicalResortAct5", TropicalResortAct5));
            settingsNode.AppendChild(ToElement(doc, "TropicalResortAct6", TropicalResortAct6));
            settingsNode.AppendChild(ToElement(doc, "TropicalResortBoss", TropicalResortBoss));
            settingsNode.AppendChild(ToElement(doc, "SweetMountainAct1", SweetMountainAct1));
            settingsNode.AppendChild(ToElement(doc, "SweetMountainAct2", SweetMountainAct2));
            settingsNode.AppendChild(ToElement(doc, "SweetMountainAct3", SweetMountainAct3));
            settingsNode.AppendChild(ToElement(doc, "SweetMountainAct4", SweetMountainAct4));
            settingsNode.AppendChild(ToElement(doc, "SweetMountainAct5", SweetMountainAct5));
            settingsNode.AppendChild(ToElement(doc, "SweetMountainAct6", SweetMountainAct6));
            settingsNode.AppendChild(ToElement(doc, "SweetMountainBoss", SweetMountainBoss));
            settingsNode.AppendChild(ToElement(doc, "StarlightCarnivalAct1", StarlightCarnivalAct1));
            settingsNode.AppendChild(ToElement(doc, "StarlightCarnivalAct2", StarlightCarnivalAct2));
            settingsNode.AppendChild(ToElement(doc, "StarlightCarnivalAct3", StarlightCarnivalAct3));
            settingsNode.AppendChild(ToElement(doc, "StarlightCarnivalAct4", StarlightCarnivalAct4));
            settingsNode.AppendChild(ToElement(doc, "StarlightCarnivalAct5", StarlightCarnivalAct5));
            settingsNode.AppendChild(ToElement(doc, "StarlightCarnivalAct6", StarlightCarnivalAct6));
            settingsNode.AppendChild(ToElement(doc, "StarlightCarnivalBoss", StarlightCarnivalBoss));
            settingsNode.AppendChild(ToElement(doc, "PlanetWispAct1", PlanetWispAct1));
            settingsNode.AppendChild(ToElement(doc, "PlanetWispAct2", PlanetWispAct2));
            settingsNode.AppendChild(ToElement(doc, "PlanetWispAct3", PlanetWispAct3));
            settingsNode.AppendChild(ToElement(doc, "PlanetWispAct4", PlanetWispAct4));
            settingsNode.AppendChild(ToElement(doc, "PlanetWispAct5", PlanetWispAct5));
            settingsNode.AppendChild(ToElement(doc, "PlanetWispAct6", PlanetWispAct6));
            settingsNode.AppendChild(ToElement(doc, "PlanetWispBoss", PlanetWispBoss));
            settingsNode.AppendChild(ToElement(doc, "AquariumParkAct1", AquariumParkAct1));
            settingsNode.AppendChild(ToElement(doc, "AquariumParkAct2", AquariumParkAct2));
            settingsNode.AppendChild(ToElement(doc, "AquariumParkAct3", AquariumParkAct3));
            settingsNode.AppendChild(ToElement(doc, "AquariumParkAct4", AquariumParkAct4));
            settingsNode.AppendChild(ToElement(doc, "AquariumParkAct5", AquariumParkAct5));
            settingsNode.AppendChild(ToElement(doc, "AquariumParkAct6", AquariumParkAct6));
            settingsNode.AppendChild(ToElement(doc, "AquariumParkBoss", AquariumParkBoss));
            settingsNode.AppendChild(ToElement(doc, "AsteroidCoasterAct1", AsteroidCoasterAct1));
            settingsNode.AppendChild(ToElement(doc, "AsteroidCoasterAct2", AsteroidCoasterAct2));
            settingsNode.AppendChild(ToElement(doc, "AsteroidCoasterAct3", AsteroidCoasterAct3));
            settingsNode.AppendChild(ToElement(doc, "AsteroidCoasterAct4", AsteroidCoasterAct4));
            settingsNode.AppendChild(ToElement(doc, "AsteroidCoasterAct5", AsteroidCoasterAct5));
            settingsNode.AppendChild(ToElement(doc, "AsteroidCoasterAct6", AsteroidCoasterAct6));
            settingsNode.AppendChild(ToElement(doc, "AsteroidCoasterBoss", AsteroidCoasterBoss));
            settingsNode.AppendChild(ToElement(doc, "TerminalVelocityAct1", TerminalVelocityAct1));
            settingsNode.AppendChild(ToElement(doc, "TerminalVelocityBoss", TerminalVelocityBoss));
            settingsNode.AppendChild(ToElement(doc, "TerminalVelocityAct2", TerminalVelocityAct2));
            settingsNode.AppendChild(ToElement(doc, "SonicSim1_1", SonicSim1_1));
            settingsNode.AppendChild(ToElement(doc, "SonicSim1_2", SonicSim1_2));
            settingsNode.AppendChild(ToElement(doc, "SonicSim1_3", SonicSim1_3));
            settingsNode.AppendChild(ToElement(doc, "SonicSim2_1", SonicSim2_1));
            settingsNode.AppendChild(ToElement(doc, "SonicSim2_2", SonicSim2_2));
            settingsNode.AppendChild(ToElement(doc, "SonicSim2_3", SonicSim2_3));
            settingsNode.AppendChild(ToElement(doc, "SonicSim3_1", SonicSim3_1));
            settingsNode.AppendChild(ToElement(doc, "SonicSim3_2", SonicSim3_2));
            settingsNode.AppendChild(ToElement(doc, "SonicSim3_3", SonicSim3_3));
            settingsNode.AppendChild(ToElement(doc, "SonicSim4_1", SonicSim4_1));
            settingsNode.AppendChild(ToElement(doc, "SonicSim4_2", SonicSim4_2));
            settingsNode.AppendChild(ToElement(doc, "SonicSim4_3", SonicSim4_3));
            settingsNode.AppendChild(ToElement(doc, "SonicSim5_1", SonicSim5_1));
            settingsNode.AppendChild(ToElement(doc, "SonicSim5_2", SonicSim5_2));
            settingsNode.AppendChild(ToElement(doc, "SonicSim5_3", SonicSim5_3));
            settingsNode.AppendChild(ToElement(doc, "SonicSim6_1", SonicSim6_1));
            settingsNode.AppendChild(ToElement(doc, "SonicSim6_2", SonicSim6_2));
            settingsNode.AppendChild(ToElement(doc, "SonicSim6_3", SonicSim6_3));
            settingsNode.AppendChild(ToElement(doc, "SonicSim7_1", SonicSim7_1));
            settingsNode.AppendChild(ToElement(doc, "SonicSim7_2", SonicSim7_2));
            settingsNode.AppendChild(ToElement(doc, "SonicSim7_3", SonicSim7_3));

            return settingsNode;
        }

        public void SetSettings(XmlNode settings)
        {
            StartAny = ParseBool(settings, "StartAny", true);
            StartSonicSimulator = ParseBool(settings, "StartSonicSimulator", true);
            StartEggShuttle = ParseBool(settings, "StartEggShuttle", true);
            ResetAny = ParseBool(settings, "ResetAny", true);
            ResetEggShuttle = ParseBool(settings, "ResetEggShuttle", true);
            TropicalResortAct1 = ParseBool(settings, "TropicalResortAct1", true);
            TropicalResortAct2 = ParseBool(settings, "TropicalResortAct2", true);
            TropicalResortAct3 = ParseBool(settings, "TropicalResortAct3", true);
            TropicalResortAct4 = ParseBool(settings, "TropicalResortAct4", true);
            TropicalResortAct5 = ParseBool(settings, "TropicalResortAct5", true);
            TropicalResortAct6 = ParseBool(settings, "TropicalResortAct6", true);
            TropicalResortBoss = ParseBool(settings, "TropicalResortBoss", true);
            SweetMountainAct1 = ParseBool(settings, "SweetMountainAct1", true);
            SweetMountainAct2 = ParseBool(settings, "SweetMountainAct2", true);
            SweetMountainAct3 = ParseBool(settings, "SweetMountainAct3", true);
            SweetMountainAct4 = ParseBool(settings, "SweetMountainAct4", true);
            SweetMountainAct5 = ParseBool(settings, "SweetMountainAct5", true);
            SweetMountainAct6 = ParseBool(settings, "SweetMountainAct6", true);
            SweetMountainBoss = ParseBool(settings, "SweetMountainBoss", true);
            StarlightCarnivalAct1 = ParseBool(settings, "StarlightCarnivalAct1", true);
            StarlightCarnivalAct2 = ParseBool(settings, "StarlightCarnivalAct2", true);
            StarlightCarnivalAct3 = ParseBool(settings, "StarlightCarnivalAct3", true);
            StarlightCarnivalAct4 = ParseBool(settings, "StarlightCarnivalAct4", true);
            StarlightCarnivalAct5 = ParseBool(settings, "StarlightCarnivalAct5", true);
            StarlightCarnivalAct6 = ParseBool(settings, "StarlightCarnivalAct6", true);
            StarlightCarnivalBoss = ParseBool(settings, "StarlightCarnivalBoss", true);
            PlanetWispAct1 = ParseBool(settings, "PlanetWispAct1", true);
            PlanetWispAct2 = ParseBool(settings, "PlanetWispAct2", true);
            PlanetWispAct3 = ParseBool(settings, "PlanetWispAct3", true);
            PlanetWispAct4 = ParseBool(settings, "PlanetWispAct4", true);
            PlanetWispAct5 = ParseBool(settings, "PlanetWispAct5", true);
            PlanetWispAct6 = ParseBool(settings, "PlanetWispAct6", true);
            PlanetWispBoss = ParseBool(settings, "PlanetWispBoss", true);
            AquariumParkAct1 = ParseBool(settings, "AquariumParkAct1", true);
            AquariumParkAct2 = ParseBool(settings, "AquariumParkAct2", true);
            AquariumParkAct3 = ParseBool(settings, "AquariumParkAct3", true);
            AquariumParkAct4 = ParseBool(settings, "AquariumParkAct4", true);
            AquariumParkAct5 = ParseBool(settings, "AquariumParkAct5", true);
            AquariumParkAct6 = ParseBool(settings, "AquariumParkAct6", true);
            AquariumParkBoss = ParseBool(settings, "AquariumParkBoss", true);
            AsteroidCoasterAct1 = ParseBool(settings, "AsteroidCoasterAct1", true);
            AsteroidCoasterAct2 = ParseBool(settings, "AsteroidCoasterAct2", true);
            AsteroidCoasterAct3 = ParseBool(settings, "AsteroidCoasterAct3", true);
            AsteroidCoasterAct4 = ParseBool(settings, "AsteroidCoasterAct4", true);
            AsteroidCoasterAct5 = ParseBool(settings, "AsteroidCoasterAct5", true);
            AsteroidCoasterAct6 = ParseBool(settings, "AsteroidCoasterAct6", true);
            AsteroidCoasterBoss = ParseBool(settings, "AsteroidCoasterBoss", true);
            TerminalVelocityAct1 = ParseBool(settings, "TerminalVelocityAct1", true);
            TerminalVelocityBoss = ParseBool(settings, "TerminalVelocityBoss", true);
            TerminalVelocityAct2 = ParseBool(settings, "TerminalVelocityAct2", true);
            SonicSim1_1 = ParseBool(settings, "SonicSim1_1", true);
            SonicSim1_2 = ParseBool(settings, "SonicSim1_2", true);
            SonicSim1_3 = ParseBool(settings, "SonicSim1_3", true);
            SonicSim2_1 = ParseBool(settings, "SonicSim2_1", true);
            SonicSim2_2 = ParseBool(settings, "SonicSim2_2", true);
            SonicSim2_3 = ParseBool(settings, "SonicSim2_3", true);
            SonicSim3_1 = ParseBool(settings, "SonicSim3_1", true);
            SonicSim3_2 = ParseBool(settings, "SonicSim3_2", true);
            SonicSim3_3 = ParseBool(settings, "SonicSim3_3", true);
            SonicSim4_1 = ParseBool(settings, "SonicSim4_1", true);
            SonicSim4_2 = ParseBool(settings, "SonicSim4_2", true);
            SonicSim4_3 = ParseBool(settings, "SonicSim4_3", true);
            SonicSim5_1 = ParseBool(settings, "SonicSim5_1", true);
            SonicSim5_2 = ParseBool(settings, "SonicSim5_2", true);
            SonicSim5_3 = ParseBool(settings, "SonicSim5_3", true);
            SonicSim6_1 = ParseBool(settings, "SonicSim6_1", true);
            SonicSim6_2 = ParseBool(settings, "SonicSim6_2", true);
            SonicSim6_3 = ParseBool(settings, "SonicSim6_3", true);
            SonicSim7_1 = ParseBool(settings, "SonicSim7_1", true);
            SonicSim7_2 = ParseBool(settings, "SonicSim7_2", true);
            SonicSim7_3 = ParseBool(settings, "SonicSim7_3", true);
        }

        static bool ParseBool(XmlNode settings, string setting, bool default_ = false)
        {
            return settings[setting] != null ? (bool.TryParse(settings[setting].InnerText, out bool val) ? val : default_) : default_;
        }

        static XmlElement ToElement<T>(XmlDocument document, string name, T value)
        {
            XmlElement str = document.CreateElement(name);
            str.InnerText = value.ToString();
            return str;
        }
    }
}
