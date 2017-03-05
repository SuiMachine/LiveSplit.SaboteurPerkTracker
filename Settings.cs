using LiveSplit.ComponentUtil;
using LiveSplit.UI;
using System;
using System.IO;
using System.Xml;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Collections.Generic;

namespace LiveSplit.SaboteurTracker
{
    enum elementType
    {
        [Description("Perks - Brawling")]
        PerksBrawling,
        [Description("Perks - Hardware")]
        PerksHardware,
        [Description("Perks - Sniping")]
        PerksSniping,
        [Description("Perks - Explosives")]
        PerksExplosives,
        [Description("Perks - Demolitions")]
        PerksDemolitions,
        [Description("Perks - Sabotage")]
        PerksSabotage,
        [Description("Perks - Mayhem")]
        PerksMayhem,
        [Description("Perks - Racing")]
        PerksRacing,
        [Description("Perks - Mechanics")]
        PerksMechanics,
        [Description("Perks - Evasion")]
        PerksEvasion,
        [Description("Mayhem Tier 1")]
        Mayhem1,
        [Description("Mayhem Tier 2")]
        Mayhem2,
        [Description("Mayhem Tier 3")]
        Mayhem3
    }

    partial class Settings : UserControl
    {
        #region Properties
        public Color BackgroundColor { get; set; }
        public Color BackgroundColorCompleted { get; set; }

        public Color OverrideTextColor { get; set; }
        public Color ComplitionColorIncomplete { get; set; }
        public Color ComplitionColorCompleted { get; set; }

        public int GraphWidth { get; set; }
        public int GraphHeight { get; set; }

        public bool field1Enabled { get; set; }
        public bool field2Enabled { get; set; }
        public bool field3Enabled { get; set; }
        public bool field4Enabled { get; set; }
        public bool field5Enabled { get; set; }

        public bool field6Enabled { get; set; }
        public bool field7Enabled { get; set; }
        public bool field8Enabled { get; set; }
        public bool field9Enabled { get; set; }
        public bool field10Enabled { get; set; }
        public bool field11Enabled { get; set; }

        public int field1DisplayMode { get; set; }
        public int field2DisplayMode { get; set; }
        public int field3DisplayMode { get; set; }
        public int field4DisplayMode { get; set; }
        public int field5DisplayMode { get; set; }
        public int field6DisplayMode { get; set; }
        public int field7DisplayMode { get; set; }
        public int field8DisplayMode { get; set; }
        public int field9DisplayMode { get; set; }
        public int field10DisplayMode { get; set; }
        public int field11DisplayMode { get; set; }


        public bool fieldCompletionColorsEnabled { get; set; }
        public bool fieldOverrideTextColor { get; set; }
        #endregion


        public Settings()
        {
            InitializeComponent();
            setStartValues();

            GraphWidth = 200;
            GraphHeight = 30;

            //Color Buttons
            btnBackgroundColor1.DataBindings.Add("BackColor", this, "BackgroundColor", false, DataSourceUpdateMode.OnPropertyChanged);
            btnBackgroundColorCompleted.DataBindings.Add("BackColor", this, "BackgroundColorCompleted", false, DataSourceUpdateMode.OnPropertyChanged);
            btnOverrideTextColor.DataBindings.Add("BackColor", this, "OverrideTextColor", false, DataSourceUpdateMode.OnPropertyChanged);
            btnColorIncompleted.DataBindings.Add("BackColor", this, "ComplitionColorIncomplete", false, DataSourceUpdateMode.OnPropertyChanged);
            btnColorCompleted.DataBindings.Add("BackColor", this, "ComplitionColorCompleted", false, DataSourceUpdateMode.OnPropertyChanged);

            //Checkboxes
            CB_EnableCompletedColor.DataBindings.Add("Checked", this, "fieldCompletionColorsEnabled", false, DataSourceUpdateMode.OnPropertyChanged);
            CB_OverrideTextColorEnabled.DataBindings.Add("Checked", this, "fieldOverrideTextColor", false, DataSourceUpdateMode.OnPropertyChanged);

            C_EnableField2.DataBindings.Add("Checked", this, "field2Enabled", false, DataSourceUpdateMode.OnPropertyChanged);
            C_EnableField3.DataBindings.Add("Checked", this, "field3Enabled", false, DataSourceUpdateMode.OnPropertyChanged);
            C_EnableField4.DataBindings.Add("Checked", this, "field4Enabled", false, DataSourceUpdateMode.OnPropertyChanged);
            C_EnableField5.DataBindings.Add("Checked", this, "field5Enabled", false, DataSourceUpdateMode.OnPropertyChanged);
            C_EnableField6.DataBindings.Add("Checked", this, "field6Enabled", false, DataSourceUpdateMode.OnPropertyChanged);
            C_EnableField7.DataBindings.Add("Checked", this, "field7Enabled", false, DataSourceUpdateMode.OnPropertyChanged);
            C_EnableField8.DataBindings.Add("Checked", this, "field8Enabled", false, DataSourceUpdateMode.OnPropertyChanged);
            C_EnableField9.DataBindings.Add("Checked", this, "field9Enabled", false, DataSourceUpdateMode.OnPropertyChanged);
            C_EnableField10.DataBindings.Add("Checked", this, "field10Enabled", false, DataSourceUpdateMode.OnPropertyChanged);
            C_EnableField11.DataBindings.Add("Checked", this, "field11Enabled", false, DataSourceUpdateMode.OnPropertyChanged);

            CB_Field1.DataBindings.Add("SelectedIndex", this, "field1DisplayMode", false, DataSourceUpdateMode.OnPropertyChanged);
            CB_Field2.DataBindings.Add("SelectedIndex", this, "field2DisplayMode", false, DataSourceUpdateMode.OnPropertyChanged);
            CB_Field3.DataBindings.Add("SelectedIndex", this, "field3DisplayMode", false, DataSourceUpdateMode.OnPropertyChanged);
            CB_Field4.DataBindings.Add("SelectedIndex", this, "field4DisplayMode", false, DataSourceUpdateMode.OnPropertyChanged);
            CB_Field5.DataBindings.Add("SelectedIndex", this, "field5DisplayMode", false, DataSourceUpdateMode.OnPropertyChanged);
            CB_Field6.DataBindings.Add("SelectedIndex", this, "field6DisplayMode", false, DataSourceUpdateMode.OnPropertyChanged);
            CB_Field7.DataBindings.Add("SelectedIndex", this, "field7DisplayMode", false, DataSourceUpdateMode.OnPropertyChanged);
            CB_Field8.DataBindings.Add("SelectedIndex", this, "field8DisplayMode", false, DataSourceUpdateMode.OnPropertyChanged);
            CB_Field9.DataBindings.Add("SelectedIndex", this, "field9DisplayMode", false, DataSourceUpdateMode.OnPropertyChanged);
            CB_Field10.DataBindings.Add("SelectedIndex", this, "field10DisplayMode", false, DataSourceUpdateMode.OnPropertyChanged);
            CB_Field11.DataBindings.Add("SelectedIndex", this, "field11DisplayMode", false, DataSourceUpdateMode.OnPropertyChanged);

            CB_Field2.DataBindings.Add("Enabled", this, "field2Enabled", false, DataSourceUpdateMode.OnPropertyChanged);
            CB_Field3.DataBindings.Add("Enabled", this, "field3Enabled", false, DataSourceUpdateMode.OnPropertyChanged);
            CB_Field4.DataBindings.Add("Enabled", this, "field4Enabled", false, DataSourceUpdateMode.OnPropertyChanged);
            CB_Field5.DataBindings.Add("Enabled", this, "field5Enabled", false, DataSourceUpdateMode.OnPropertyChanged);
            CB_Field6.DataBindings.Add("Enabled", this, "field6Enabled", false, DataSourceUpdateMode.OnPropertyChanged);
            CB_Field7.DataBindings.Add("Enabled", this, "field7Enabled", false, DataSourceUpdateMode.OnPropertyChanged);
            CB_Field8.DataBindings.Add("Enabled", this, "field8Enabled", false, DataSourceUpdateMode.OnPropertyChanged);
            CB_Field9.DataBindings.Add("Enabled", this, "field9Enabled", false, DataSourceUpdateMode.OnPropertyChanged);
            CB_Field10.DataBindings.Add("Enabled", this, "field10Enabled", false, DataSourceUpdateMode.OnPropertyChanged);
            CB_Field11.DataBindings.Add("Enabled", this, "field11Enabled", false, DataSourceUpdateMode.OnPropertyChanged);


            AddComboboxDataSources();

        }

        private void setStartValues()
        {
            GraphHeight = 0;
            field1Enabled = true;
            field2Enabled = false;
            field3Enabled = false;
            field4Enabled = false;
            field5Enabled = false;
            field6Enabled = false;
            field7Enabled = false;
            field8Enabled = false;
            field9Enabled = false;
            field10Enabled = false;
            field11Enabled = false;

            field1DisplayMode = (int)elementType.Mayhem1;
            field2DisplayMode = (int)elementType.Mayhem1;
            field3DisplayMode = (int)elementType.Mayhem1;
            field4DisplayMode = (int)elementType.Mayhem1;
            field5DisplayMode = (int)elementType.Mayhem1;
            field6DisplayMode = (int)elementType.Mayhem1;
            field7DisplayMode = (int)elementType.Mayhem1;
            field8DisplayMode = (int)elementType.Mayhem1;
            field9DisplayMode = (int)elementType.Mayhem1;
            field10DisplayMode = (int)elementType.Mayhem1;
            field11DisplayMode = (int)elementType.Mayhem1;

            fieldCompletionColorsEnabled = false;
            fieldOverrideTextColor = false;

            BackgroundColor = Color.Transparent;
            BackgroundColorCompleted = Color.Transparent;
            OverrideTextColor = Color.White;
            ComplitionColorIncomplete = Color.White;
            ComplitionColorCompleted = Color.White;
        }

        private void ColorButtonClick(object sender, EventArgs e)
        {
            SettingsHelper.ColorButtonClick((Button)sender, this);
        }        

        public void SetSettings(System.Xml.XmlNode node)
        {
            System.Xml.XmlElement element = (System.Xml.XmlElement) node;

            GraphWidth = SettingsHelper.ParseInt(element["GraphWidth"]);
            GraphHeight = SettingsHelper.ParseInt(element["GraphHeight"]);
            field1DisplayMode = SettingsHelper.ParseInt(element["F1DisplayMode"]);
            field2Enabled = SettingsHelper.ParseBool(element["F2Enabled"]);
            field2DisplayMode = SettingsHelper.ParseInt(element["F2DisplayMode"]);
            field3Enabled = SettingsHelper.ParseBool(element["F3Enabled"]);
            field3DisplayMode = SettingsHelper.ParseInt(element["F3DisplayMode"]);
            field4Enabled = SettingsHelper.ParseBool(element["F4Enabled"]);
            field4DisplayMode = SettingsHelper.ParseInt(element["F4DisplayMode"]);
            field5Enabled = SettingsHelper.ParseBool(element["F5Enabled"]);
            field5DisplayMode = SettingsHelper.ParseInt(element["F5DisplayMode"]);
            field6Enabled = SettingsHelper.ParseBool(element["F6Enabled"]);
            field6DisplayMode = SettingsHelper.ParseInt(element["F6DisplayMode"]);
            field7Enabled = SettingsHelper.ParseBool(element["F7Enabled"]);
            field7DisplayMode = SettingsHelper.ParseInt(element["F7DisplayMode"]);
            field8Enabled = SettingsHelper.ParseBool(element["F8Enabled"]);
            field8DisplayMode = SettingsHelper.ParseInt(element["F8DisplayMode"]);
            field9Enabled = SettingsHelper.ParseBool(element["F9Enabled"]);
            field9DisplayMode = SettingsHelper.ParseInt(element["F9DisplayMode"]);
            field10Enabled = SettingsHelper.ParseBool(element["F10Enabled"]);
            field10DisplayMode = SettingsHelper.ParseInt(element["F10DisplayMode"]);
            field11Enabled = SettingsHelper.ParseBool(element["F11Enabled"]);
            field11DisplayMode = SettingsHelper.ParseInt(element["F11DisplayMode"]);

            fieldOverrideTextColor = SettingsHelper.ParseBool(element["fieldOverrideTextColor"]);
            fieldCompletionColorsEnabled = SettingsHelper.ParseBool(element["fieldCompletionColorsEnabled"]);

            BackgroundColor = SettingsHelper.ParseColor(element["BackgroundColor"]);
            BackgroundColorCompleted = SettingsHelper.ParseColor(element["BackgroundColorCompleted"]);
            OverrideTextColor = SettingsHelper.ParseColor(element["OverrideTextColor"]);
            ComplitionColorIncomplete = SettingsHelper.ParseColor(element["ComplitionColorIncomplete"]);
            ComplitionColorCompleted = SettingsHelper.ParseColor(element["ComplitionColorCompleted"]);
        }

        public System.Xml.XmlNode GetSettings(System.Xml.XmlDocument document)
        {
            System.Xml.XmlElement parent = document.CreateElement("Settings");
            CreateSettingsNode(document, parent);
            return parent;
        }

        public int GetSettingsHashCode()
        {
            return CreateSettingsNode(null, null);
        }

        private int CreateSettingsNode(System.Xml.XmlDocument document, System.Xml.XmlElement parent)
        {
            return SettingsHelper.CreateSetting(document, parent, "Version", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version) ^
            SettingsHelper.CreateSetting(document, parent, "GraphWidth", GraphWidth) ^
            SettingsHelper.CreateSetting(document, parent, "GraphHeight", GraphHeight) ^
            SettingsHelper.CreateSetting(document, parent, "F1DisplayMode", field1DisplayMode) ^
            SettingsHelper.CreateSetting(document, parent, "F2Enabled", field2Enabled) ^
            SettingsHelper.CreateSetting(document, parent, "F2DisplayMode", field2DisplayMode) ^
            SettingsHelper.CreateSetting(document, parent, "F3Enabled", field3Enabled) ^
            SettingsHelper.CreateSetting(document, parent, "F3DisplayMode", field3DisplayMode) ^
            SettingsHelper.CreateSetting(document, parent, "F4Enabled", field4Enabled) ^
            SettingsHelper.CreateSetting(document, parent, "F4DisplayMode", field4DisplayMode) ^
            SettingsHelper.CreateSetting(document, parent, "F5Enabled", field5Enabled) ^
            SettingsHelper.CreateSetting(document, parent, "F5DisplayMode", field5DisplayMode) ^
            SettingsHelper.CreateSetting(document, parent, "F6Enabled", field6Enabled) ^
            SettingsHelper.CreateSetting(document, parent, "F6DisplayMode", field6DisplayMode) ^
            SettingsHelper.CreateSetting(document, parent, "F7Enabled", field7Enabled) ^
            SettingsHelper.CreateSetting(document, parent, "F7DisplayMode", field7DisplayMode) ^
            SettingsHelper.CreateSetting(document, parent, "F8Enabled", field8Enabled) ^
            SettingsHelper.CreateSetting(document, parent, "F8DisplayMode", field8DisplayMode) ^
            SettingsHelper.CreateSetting(document, parent, "F9Enabled", field9Enabled) ^
            SettingsHelper.CreateSetting(document, parent, "F9DisplayMode", field9DisplayMode) ^
            SettingsHelper.CreateSetting(document, parent, "F10Enabled", field10Enabled) ^
            SettingsHelper.CreateSetting(document, parent, "F10DisplayMode", field10DisplayMode) ^
            SettingsHelper.CreateSetting(document, parent, "F11Enabled", field11Enabled) ^
            SettingsHelper.CreateSetting(document, parent, "F11DisplayMode", field11DisplayMode) ^

            SettingsHelper.CreateSetting(document, parent, "fieldOverrideTextColor", fieldOverrideTextColor) ^
            SettingsHelper.CreateSetting(document, parent, "fieldCompletionColorsEnabled", fieldCompletionColorsEnabled) ^
            SettingsHelper.CreateSetting(document, parent, "BackgroundColor", BackgroundColor) ^
            SettingsHelper.CreateSetting(document, parent, "BackgroundColorCompleted", BackgroundColorCompleted) ^
            SettingsHelper.CreateSetting(document, parent, "OverrideTextColor", OverrideTextColor) ^
            SettingsHelper.CreateSetting(document, parent, "ComplitionColorIncomplete", ComplitionColorIncomplete) ^
            SettingsHelper.CreateSetting(document, parent, "ComplitionColorCompleted", ComplitionColorCompleted);
        }
    }
}
