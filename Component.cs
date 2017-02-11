using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Diagnostics;
using LiveSplit.ComponentUtil;

namespace LiveSplit.SaboteurTracker
{
    public class Component : IComponent
    {
        private Settings settings;

        public string ComponentName => "Saboteur Perk Progression Tracker";

        public float PaddingTop => 0;
        public float PaddingLeft => 0;
        public float PaddingBottom => 0;
        public float PaddingRight => 0;

        public float VerticalHeight => settings.GraphHeight + 4;
        public float MinimumWidth => 180;
        public float HorizontalWidth => settings.GraphWidth + 4;
        public float MinimumHeight => settings.GraphHeight + 4;

        public IDictionary<string, Action> ContextMenuControls => null;
        bool firstLoad = true;

        private int graphHeight;
        private int graphWidth;

        private int v_MayhemTier1;
        private const int v_maxMayhemTier1 = 5;
        private int v_MayhemTier2;
        private const int v_maxMayhemTier2 = 20;
        private int v_MayhemTier3;
        private const int v_maxMayhemTier3 = 20;

        DeepPointer _pMayhemTier1;
        DeepPointer _pMayhemTier2;
        DeepPointer _pMayhemTier3;

        private int numberofDisplayedElements = 0;
        private int field1DisplayMode = 0;
        private int field2DisplayMode = 0;
        private int field3DisplayMode = 0;
        private bool field1Enabled = true;
        private bool field2Enabled = false;
        private bool field3Enabled = false;
        private bool overrideTextColorEnabled = false;

        private Color OverrideTextColor;

        private System.Diagnostics.Process process;

        private Bitmap bmpBuffer;
        private Graphics gBuffer;

        private Brush BGBrush;
        private byte BGBrushOpacity;
        private Brush BackgroundColorCompleted;
        private byte BGCompletedOpacity;

        private bool CompletedColorEnabled = false;
        private Brush ComplitionColorIncomplete;
        private Brush ComplitionColorCompleted;

        private StringFormat valueTextFormat;
        private StringFormat descriptiveTextFormat;

        public Component(LiveSplitState state)
        {

            valueTextFormat = new StringFormat(StringFormatFlags.NoWrap);
            valueTextFormat.LineAlignment = StringAlignment.Center;
            valueTextFormat.Alignment = StringAlignment.Far;

            descriptiveTextFormat = new StringFormat(StringFormatFlags.NoWrap);
            descriptiveTextFormat.LineAlignment = StringAlignment.Center;
            descriptiveTextFormat.Alignment = StringAlignment.Near;

            settings = new Settings();
            settings.HandleDestroyed += SettingsUpdated;
            SettingsUpdated(null, null);

            _pMayhemTier1 = new DeepPointer(0x010968AC, 0x5C, 0x13C);
            _pMayhemTier2 = new DeepPointer(0x010968AC, 0x60, 0x13C);
            _pMayhemTier3 = new DeepPointer(0x010968AC, 0x64, 0x13C);
        }

        private void SettingsUpdated(object sender, EventArgs e)
        {
            CalculateSize();
            field1DisplayMode = settings.field1DisplayMode;
            field2DisplayMode = settings.field2DisplayMode;
            field3DisplayMode = settings.field3DisplayMode;
            field1Enabled = settings.field1Enabled;
            field2Enabled = settings.field2Enabled;
            field3Enabled = settings.field3Enabled;
            overrideTextColorEnabled = settings.fieldOverrideTextColor;
            CompletedColorEnabled = settings.fieldCompletionColorsEnabled;

            BackgroundColorCompleted = new SolidBrush(settings.BackgroundColorCompleted);
            BGCompletedOpacity = settings.BackgroundColorCompleted.A;
            OverrideTextColor = settings.OverrideTextColor;
            ComplitionColorIncomplete = new SolidBrush(settings.ComplitionColorIncomplete);
            ComplitionColorCompleted = new SolidBrush(settings.ComplitionColorCompleted);
            BGBrush = new SolidBrush(settings.BackgroundColor);
            BGBrushOpacity = settings.BackgroundColor.A;



            if (graphHeight != settings.GraphHeight || graphWidth != settings.GraphWidth)
            {
                graphHeight = settings.GraphHeight;
                graphWidth = settings.GraphWidth;

                bmpBuffer = new Bitmap(graphWidth, graphHeight);
                gBuffer = Graphics.FromImage(bmpBuffer);
                gBuffer.Clear(Color.Transparent);
                gBuffer.CompositingMode = CompositingMode.SourceCopy;
            }
        }

        private void CalculateSize()
        {
            numberofDisplayedElements = 0;
            int size = 0;
            if (settings.field1Enabled)
            {
                size += 24;
                numberofDisplayedElements++;
            }

            if (settings.field2Enabled)
            {
                size += 24;
                numberofDisplayedElements++;
            }

            if (settings.field3Enabled)
            {
                size += 24;
                numberofDisplayedElements++;
            }

            if (settings.field4Enabled)
            {
                size += 24;
                numberofDisplayedElements++;
            }

            if (settings.field5Enabled)
            {
                size += 24;
                numberofDisplayedElements++;
            }

            settings.GraphHeight = size;
        }

        private static Color Blend(Color backColor, Color color, double amount)
        {
            byte a = (byte)((color.A * amount) + backColor.A * (1 - amount));
            byte r = (byte)((color.R * amount) + backColor.R * (1 - amount));
            byte g = (byte)((color.G * amount) + backColor.G * (1 - amount));
            byte b = (byte)((color.B * amount) + backColor.B * (1 - amount));
            return Color.FromArgb(a, r, g, b);
        }

        public void DrawGraph(Graphics g, LiveSplitState state, float width, float height)
        {
            if (firstLoad)
            {
                SettingsUpdated(null, null);
                firstLoad = false;
            }
            // figure out where to draw the graph
            RectangleF graphRect = new RectangleF();
            graphRect.Y = (height - graphHeight) / 2;
            graphRect.Width = width;
            graphRect.Height = graphHeight;
            graphRect.X = 0;

            // draw descriptive text
            Font font = state.LayoutSettings.TextFont;
            Brush fontBrush;
            if (!overrideTextColorEnabled)
                fontBrush = new SolidBrush(state.LayoutSettings.TextColor);
            else
                fontBrush = new SolidBrush(OverrideTextColor);
            Brush seperatorBrush = new SolidBrush(state.LayoutSettings.ThinSeparatorsColor);
            RectangleF rect = graphRect;
            if (BGBrushOpacity != 0)
                g.FillRectangle(BGBrush, rect);
            rect.X += 5;
            rect.Width -= 10;
            float x = rect.X;
            float y = 2;
            float yDifference = rect.Height / numberofDisplayedElements;

            if (field1Enabled)
            {
                DrawElementInTracker(field1DisplayMode, g, font, fontBrush, x, y, rect.Width, yDifference);
                y += 24;

            }
            if(field2Enabled)
            {
                g.FillRectangle(seperatorBrush, 0, y, width, 1);
                DrawElementInTracker(field2DisplayMode, g, font, fontBrush, x, y, rect.Width, yDifference);
                y += 24;
            }
            if (field3Enabled)
            {
                g.FillRectangle(seperatorBrush, 0, y, width, 1);
                DrawElementInTracker(field3DisplayMode, g, font, fontBrush, x, y, rect.Width, yDifference);
                y += 24;
            }

            // draw value text
        }

        private void DrawElementInTracker(int DisplayMode, Graphics g, Font font, Brush brush, float x, float y, float width, float height)
        {
            switch(DisplayMode)
            {
                case (int)elementType.Mayhem1:
                    if (BGCompletedOpacity != 0 && v_MayhemTier1 == v_maxMayhemTier1)
                        g.FillRectangle(BackgroundColorCompleted, x, y, width, height);

                    g.DrawString("Nazis thrown to death:", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                    if (CompletedColorEnabled)
                    {
                        if(v_MayhemTier1 != v_maxMayhemTier1)
                            g.DrawString(v_MayhemTier1.ToString() + " / " + v_maxMayhemTier1.ToString(), font, ComplitionColorIncomplete, new RectangleF(x, y, width, height), valueTextFormat);
                        else
                            g.DrawString(v_MayhemTier1.ToString() + " / " + v_maxMayhemTier1.ToString(), font, ComplitionColorCompleted, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    else
                        g.DrawString(v_MayhemTier1.ToString() + " / " + v_maxMayhemTier1.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    break;
                case (int)elementType.Mayhem2:
                    if (BGCompletedOpacity != 0 && v_MayhemTier2 == v_maxMayhemTier2)
                        g.FillRectangle(BackgroundColorCompleted, x, y, width, height);

                    g.DrawString("Nazis driven over:", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                    if (CompletedColorEnabled)
                    {
                        if (v_MayhemTier2 != v_maxMayhemTier2)
                            g.DrawString(v_MayhemTier2.ToString() + " / " + v_maxMayhemTier2.ToString(), font, ComplitionColorIncomplete, new RectangleF(x, y, width, height), valueTextFormat);
                        else
                            g.DrawString(v_MayhemTier2.ToString() + " / " + v_maxMayhemTier2.ToString(), font, ComplitionColorCompleted, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    else
                        g.DrawString(v_MayhemTier2.ToString() + " / " + v_maxMayhemTier2.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    break;
                case (int)elementType.Mayhem3:
                    if (BGCompletedOpacity != 0 && v_MayhemTier3 == v_maxMayhemTier3)
                        g.FillRectangle(BackgroundColorCompleted, x, y, width, height);

                    g.DrawString("Vehicles blown:", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                    if (CompletedColorEnabled)
                    {
                        if (v_MayhemTier3 != v_maxMayhemTier3)
                            g.DrawString(v_MayhemTier3.ToString() + " / " + v_maxMayhemTier3.ToString(), font, ComplitionColorIncomplete, new RectangleF(x, y, width, height), valueTextFormat);
                        else
                            g.DrawString(v_MayhemTier3.ToString() + " / " + v_maxMayhemTier3.ToString(), font, ComplitionColorCompleted, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    else
                        g.DrawString(v_MayhemTier3.ToString() + " / " + v_maxMayhemTier3.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    break;
            }
        }

        private void DrawBackground(Graphics g, LiveSplitState state, float width, float height)
        {
            if (settings.BackgroundColor.A == 0 && settings.BackgroundColorCompleted.A == 0)
            {
                Brush gradientBrush = new SolidBrush(settings.BackgroundColor);
                g.FillRectangle(gradientBrush, 0, 0, width, height);
            }
        }

        public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion)
        {
            DrawBackground(g, state, width, VerticalHeight);
            DrawGraph(g, state, width, VerticalHeight);
        }

        public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion)
        {
            DrawBackground(g, state, HorizontalWidth, height);
            DrawGraph(g, state, HorizontalWidth, height);
        }
        
        public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {

            if (process != null && !process.HasExited && process.ProcessName.ToLower() == "saboteur")
            {
                v_MayhemTier1 = _pMayhemTier1.Deref<int>(process);
                v_MayhemTier2 = _pMayhemTier2.Deref<int>(process);
                v_MayhemTier3 = _pMayhemTier3.Deref<int>(process);

                if (invalidator != null)
                {
                    invalidator.Invalidate(0, 0, width, height);
                }
            }
            else
            {
                process = System.Diagnostics.Process.GetProcessesByName("saboteur").FirstOrDefault();
            }            
        }
        public System.Windows.Forms.Control GetSettingsControl(LayoutMode mode)
        {
            return settings;
        }

        public void SetSettings(System.Xml.XmlNode settings)
        {
            this.settings.SetSettings(settings);
        }

        public System.Xml.XmlNode GetSettings(System.Xml.XmlDocument document)
        {
            return settings.GetSettings(document);
        }

        public int GetSettingsHashCode()
        {
            return settings.GetSettingsHashCode();
        }

        protected virtual void Dispose(bool disposing)
        {
            bmpBuffer.Dispose();
            valueTextFormat.Dispose();
            descriptiveTextFormat.Dispose();
            settings.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
