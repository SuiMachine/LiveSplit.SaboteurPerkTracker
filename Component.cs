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
        enum GameVersions
        {
            Origin = 31395840,
            GOG = 19122752
        }

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

        //Brawling
        private int v_BrawlingTier1;
        private const int v_maxBrawlingTier1 = 2;
        private int v_BrawlingTier2;
        private const int v_maxBrawlingTier2 = 10;
        private int v_BrawlingTier3;
        private const int v_maxBrawlingTier3 = 5;
        private DeepPointer _pBrawlingTier1 = null;
        private DeepPointer _pBrawlingTier2 = null;
        private DeepPointer _pBrawlingTier3 = null;

        //Hardware
        private int v_HardwareTier1;
        private const int v_maxHardwareTier1 = 5;
        private int v_HardwareTier2;
        private const int v_maxHardwareTier2 = 5;
        private int v_HardwareTier3;
        private const int v_maxHardwareTier3 = 2;
        private DeepPointer _pHardwareTier1 = null;
        private DeepPointer _pHardwareTier2 = null;
        private DeepPointer _pHardwareTier3 = null;


        //Sniping
        private int v_SnipingTier1;
        private const int v_maxSnipingTier1 = 5;
        private int v_SnipingTier2;
        private const int v_maxSnipingTier2 = 15;
        private int v_SnipingTier3;
        private const int v_maxSnipingTier3 = 10;
        private DeepPointer _pSnipingTier1 = null;
        private DeepPointer _pSnipingTier2 = null;
        private DeepPointer _pSnipingTier3 = null;

        //Explosives
        private int v_ExplosivesTier1;
        private const int v_maxExplosivesTier1 = 10;
        private int v_ExplosivesTier2;
        private const int v_maxExplosivesTier2 = 5;
        private int v_ExplosivesTier3;
        private const int v_maxExplosivesTier3 = 10;
        private DeepPointer _pExplosivesTier1 = null;
        private DeepPointer _pExplosivesTier2 = null;
        private DeepPointer _pExplosivesTier3 = null;

        //Demolitions
        private int v_DemolitionsTier1;
        private const int v_maxDemolitionsTier1 = 3;
        private int v_DemolitionsTier2;
        private const int v_maxDemolitionsTier2 = 5;
        private int v_DemolitionsTier3;
        private const int v_maxDemolitionsTier3 = 5;
        private DeepPointer _pDemolitionsTier1 = null;
        private DeepPointer _pDemolitionsTier2 = null;
        private DeepPointer _pDemolitionsTier3 = null;

        //Sabotage
        private int v_SabotageTier1;
        private const int v_maxSabotageTier1 = 5;
        private int v_SabotageTier2;
        private const int v_maxSabotageTier2 = 10;
        private int v_SabotageTier3;
        private const int v_maxSabotageTier3 = 4;
        private DeepPointer _pSabotageTier1 = null;
        private DeepPointer _pSabotageTier2 = null;
        private DeepPointer _pSabotageTier3 = null;

        //Mayhem
        private int v_MayhemTier1;
        private const int v_maxMayhemTier1 = 5;
        private int v_MayhemTier2;
        private const int v_maxMayhemTier2 = 20;
        private int v_MayhemTier3;
        private const int v_maxMayhemTier3 = 20;
        private DeepPointer _pMayhemTier1 = null;
        private DeepPointer _pMayhemTier2 = null;
        private DeepPointer _pMayhemTier3 = null;

        //Racing
        private int v_RacingTier1;
        private const int v_maxRacingTier1 = 1;
        private int v_RacingTier2;
        private const int v_maxRacingTier2 = 1;
        private int v_RacingTier3;
        private const int v_maxRacingTier3 = 1;
        private DeepPointer _pRacingTier1 = null;
        private DeepPointer _pRacingTier2 = null;
        private DeepPointer _pRacingTier3 = null;

        //Mechanics
        private int v_MechanicsTier1;
        private const int v_maxMechanicsTier1 = 5;
        private int v_MechanicsTier2;
        private const int v_maxMechanicsTier2 = 12;
        private int v_MechanicsTier3;
        private const int v_maxMechanicsTier3 = 38;
        private DeepPointer _pMachanicsTier1 = null;
        private DeepPointer _pMachanicsTier2 = null;
        private DeepPointer _pMachanicsTier3 = null;

        //Evasion
        private int v_EvasionTier1;
        private const int v_maxEvasionTier1 = 5;
        private int v_EvasionTier2;
        private const int v_maxEvasionTier2 = 1;
        private int v_EvasionTier3;
        private const int v_maxEvasionTier3 = 1;
        private DeepPointer _pEvasionTier1 = null;
        private DeepPointer _pEvasionTier2 = null;
        private DeepPointer _pEvasionTier3 = null;

        private bool foundProcess = false;


        private int numberofDisplayedElements = 0;
        private int field1DisplayMode = 0;
        private int field2DisplayMode = 0;
        private int field3DisplayMode = 0;
        private int field4DisplayMode = 0;
        private int field5DisplayMode = 0;
        private int field6DisplayMode = 0;
        private int field7DisplayMode = 0;
        private int field8DisplayMode = 0;
        private int field9DisplayMode = 0;
        private int field10DisplayMode = 0;
        private int field11DisplayMode = 0;

        private bool field1Enabled = true;
        private bool field2Enabled = false;
        private bool field3Enabled = false;
        private bool field4Enabled = false;
        private bool field5Enabled = false;
        private bool field6Enabled = false;
        private bool field7Enabled = false;
        private bool field8Enabled = false;
        private bool field9Enabled = false;
        private bool field10Enabled = false;
        private bool field11Enabled = false;

        private bool overrideTextColorEnabled = false;

        private Color OverrideTextColor;

        private Process gameProcess;

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
        }

        private void SettingsUpdated(object sender, EventArgs e)
        {
            CalculateSize();
            field1DisplayMode = settings.field1DisplayMode;
            field2DisplayMode = settings.field2DisplayMode;
            field3DisplayMode = settings.field3DisplayMode;
            field4DisplayMode = settings.field4DisplayMode;
            field5DisplayMode = settings.field5DisplayMode;
            field6DisplayMode = settings.field6DisplayMode;
            field7DisplayMode = settings.field7DisplayMode;
            field8DisplayMode = settings.field8DisplayMode;
            field9DisplayMode = settings.field9DisplayMode;
            field10DisplayMode = settings.field10DisplayMode;
            field11DisplayMode = settings.field11DisplayMode;

            field1Enabled = settings.field1Enabled;
            field2Enabled = settings.field2Enabled;
            field3Enabled = settings.field3Enabled;
            field4Enabled = settings.field4Enabled;
            field5Enabled = settings.field5Enabled;
            field6Enabled = settings.field6Enabled;
            field7Enabled = settings.field7Enabled;
            field8Enabled = settings.field8Enabled;
            field9Enabled = settings.field9Enabled;
            field10Enabled = settings.field10Enabled;
            field11Enabled = settings.field11Enabled;

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

            if (settings.field6Enabled)
            {
                size += 24;
                numberofDisplayedElements++;
            }

            if (settings.field7Enabled)
            {
                size += 24;
                numberofDisplayedElements++;
            }

            if (settings.field8Enabled)
            {
                size += 24;
                numberofDisplayedElements++;
            }

            if (settings.field9Enabled)
            {
                size += 24;
                numberofDisplayedElements++;
            }

            if (settings.field10Enabled)
            {
                size += 24;
                numberofDisplayedElements++;
            }

            if (settings.field11Enabled)
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
            if (field4Enabled)
            {
                g.FillRectangle(seperatorBrush, 0, y, width, 1);
                DrawElementInTracker(field4DisplayMode, g, font, fontBrush, x, y, rect.Width, yDifference);
                y += 24;
            }
            if (field5Enabled)
            {
                g.FillRectangle(seperatorBrush, 0, y, width, 1);
                DrawElementInTracker(field5DisplayMode, g, font, fontBrush, x, y, rect.Width, yDifference);
                y += 24;
            }
            if (field6Enabled)
            {
                g.FillRectangle(seperatorBrush, 0, y, width, 1);
                DrawElementInTracker(field6DisplayMode, g, font, fontBrush, x, y, rect.Width, yDifference);
                y += 24;
            }
            if (field7Enabled)
            {
                g.FillRectangle(seperatorBrush, 0, y, width, 1);
                DrawElementInTracker(field7DisplayMode, g, font, fontBrush, x, y, rect.Width, yDifference);
                y += 24;
            }
            if (field8Enabled)
            {
                g.FillRectangle(seperatorBrush, 0, y, width, 1);
                DrawElementInTracker(field8DisplayMode, g, font, fontBrush, x, y, rect.Width, yDifference);
                y += 24;
            }
            if (field9Enabled)
            {
                g.FillRectangle(seperatorBrush, 0, y, width, 1);
                DrawElementInTracker(field9DisplayMode, g, font, fontBrush, x, y, rect.Width, yDifference);
                y += 24;
            }
            if (field10Enabled)
            {
                g.FillRectangle(seperatorBrush, 0, y, width, 1);
                DrawElementInTracker(field10DisplayMode, g, font, fontBrush, x, y, rect.Width, yDifference);
                y += 24;
            }
            if (field11Enabled)
            {
                g.FillRectangle(seperatorBrush, 0, y, width, 1);
                DrawElementInTracker(field11DisplayMode, g, font, fontBrush, x, y, rect.Width, yDifference);
                y += 24;
            }

            // draw value text
        }

        private void DrawElementInTracker(int DisplayMode, Graphics g, Font font, Brush brush, float x, float y, float width, float height)
        {
            switch(DisplayMode)
            {
                #region Brawling
                case (int)elementType.PerksBrawling:
                    if (BGCompletedOpacity != 0 && v_BrawlingTier3 == v_maxBrawlingTier3)
                        g.FillRectangle(BackgroundColorCompleted, x, y, width, height);

                    if (v_BrawlingTier1 != v_maxBrawlingTier1)
                    {
                        g.DrawString("Nazis punched to death:", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                        g.DrawString(v_BrawlingTier1.ToString() + " / " + v_maxBrawlingTier1.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    else if (v_BrawlingTier2 != v_maxBrawlingTier2)
                    {
                        g.DrawString("Nazis stealth killed:", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                        g.DrawString(v_BrawlingTier2.ToString() + " / " + v_maxBrawlingTier2.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    else
                    {
                        g.DrawString("Nazi generals stealth killed:", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                        if (CompletedColorEnabled)
                        {
                            if (v_BrawlingTier3 != v_maxBrawlingTier3)
                                g.DrawString(v_BrawlingTier3.ToString() + " / " + v_maxBrawlingTier3.ToString(), font, ComplitionColorIncomplete, new RectangleF(x, y, width, height), valueTextFormat);
                            else
                                g.DrawString(v_BrawlingTier3.ToString() + " / " + v_maxBrawlingTier3.ToString(), font, ComplitionColorCompleted, new RectangleF(x, y, width, height), valueTextFormat);
                        }
                        else
                            g.DrawString(v_BrawlingTier3.ToString() + " / " + v_maxBrawlingTier3.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    break;
                #endregion
                #region Hardware
                case (int)elementType.PerksHardware:
                    if (BGCompletedOpacity != 0 && v_HardwareTier3 == v_maxHardwareTier3)
                        g.FillRectangle(BackgroundColorCompleted, x, y, width, height);

                    if (v_HardwareTier1 != v_maxHardwareTier1)
                    {
                        g.DrawString("Nazis killed with bullets:", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                        g.DrawString(v_HardwareTier1.ToString() + " / " + v_maxHardwareTier1.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    else if (v_HardwareTier2 != v_maxHardwareTier2)
                    {
                        g.DrawString("Terror Squad Nazis Killed:", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                        g.DrawString(v_HardwareTier2.ToString() + " / " + v_maxHardwareTier2.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    else
                    {
                        g.DrawString("Destroy a Zeppelin and Wulf tank:", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                        if (CompletedColorEnabled)
                        {
                            if (v_HardwareTier3 != v_maxHardwareTier3)
                                g.DrawString(v_HardwareTier3.ToString() + " / " + v_maxHardwareTier3.ToString(), font, ComplitionColorIncomplete, new RectangleF(x, y, width, height), valueTextFormat);
                            else
                                g.DrawString(v_HardwareTier3.ToString() + " / " + v_maxHardwareTier3.ToString(), font, ComplitionColorCompleted, new RectangleF(x, y, width, height), valueTextFormat);
                        }
                        else
                            g.DrawString(v_HardwareTier3.ToString() + " / " + v_maxHardwareTier3.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    break;
                #endregion
                #region Sniping
                case (int)elementType.PerksSniping:
                    //Todo
                    if (BGCompletedOpacity != 0 && v_SnipingTier3 == v_maxSnipingTier3)
                        g.FillRectangle(BackgroundColorCompleted, x, y, width, height);

                    if (v_SnipingTier1 != v_maxSnipingTier1)
                    {
                        g.DrawString("Nazis killed with sniper rilfe (scoped):", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                        g.DrawString(v_SnipingTier1.ToString() + " / " + v_maxSnipingTier1.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    else if (v_SnipingTier2 != v_maxSnipingTier2)
                    {
                        g.DrawString("Headshots with sniper rifle (scoped):", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                        g.DrawString(v_SnipingTier2.ToString() + " / " + v_maxSnipingTier2.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    else
                    {
                        g.DrawString("Double kills with sniper rifle (scoped):", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                        if (CompletedColorEnabled)
                        {
                            if (v_SnipingTier3 != v_maxBrawlingTier3)
                                g.DrawString(v_SnipingTier3.ToString() + " / " + v_maxSnipingTier3.ToString(), font, ComplitionColorIncomplete, new RectangleF(x, y, width, height), valueTextFormat);
                            else
                                g.DrawString(v_SnipingTier3.ToString() + " / " + v_maxSnipingTier3.ToString(), font, ComplitionColorCompleted, new RectangleF(x, y, width, height), valueTextFormat);
                        }
                        else
                            g.DrawString(v_SnipingTier3.ToString() + " / " + v_maxSnipingTier3.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    break;
                #endregion
                #region Explosives
                case (int)elementType.PerksExplosives:
                    //Todo
                    if (BGCompletedOpacity != 0 && v_ExplosivesTier1 == v_maxExplosivesTier3)
                        g.FillRectangle(BackgroundColorCompleted, x, y, width, height);

                    if (v_ExplosivesTier1 != v_maxExplosivesTier1)
                    {
                        g.DrawString("Nazis killed with grenades:", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                        g.DrawString(v_ExplosivesTier1.ToString() + " / " + v_maxExplosivesTier1.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    else if (v_ExplosivesTier2 != v_maxExplosivesTier2)
                    {
                        g.DrawString("Nazis killed with single explosion:", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                        g.DrawString(v_ExplosivesTier2.ToString() + " / " + v_maxExplosivesTier2.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    else
                    {
                        g.DrawString("Nazi killed in 10 seconds:", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                        if (CompletedColorEnabled)
                        {
                            if (v_ExplosivesTier3 != v_maxExplosivesTier3)
                                g.DrawString(v_ExplosivesTier3.ToString() + " / " + v_maxExplosivesTier3.ToString(), font, ComplitionColorIncomplete, new RectangleF(x, y, width, height), valueTextFormat);
                            else
                                g.DrawString(v_ExplosivesTier3.ToString() + " / " + v_maxExplosivesTier3.ToString(), font, ComplitionColorCompleted, new RectangleF(x, y, width, height), valueTextFormat);
                        }
                        else
                            g.DrawString(v_ExplosivesTier3.ToString() + " / " + v_maxExplosivesTier3.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    break;
                #endregion
                #region Demolitions
                case (int)elementType.PerksDemolitions:
                    if (BGCompletedOpacity != 0 && v_DemolitionsTier3 == v_maxDemolitionsTier3)
                        g.FillRectangle(BackgroundColorCompleted, x, y, width, height);

                    if (v_DemolitionsTier1 != v_maxDemolitionsTier1)
                    {
                        g.DrawString("Nazis vehicles blown:", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                        g.DrawString(v_DemolitionsTier1.ToString() + " / " + v_maxDemolitionsTier1.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    else if (v_DemolitionsTier2 != v_maxBrawlingTier2)
                    {
                        g.DrawString("Nazis vehicles blown in 300 seconds:", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                        g.DrawString(v_DemolitionsTier2.ToString() + " / " + v_maxDemolitionsTier2.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    else
                    {
                        g.DrawString("Wulf Tanks destroyed:", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                        if (CompletedColorEnabled)
                        {
                            if (v_DemolitionsTier3 != v_maxDemolitionsTier3)
                                g.DrawString(v_DemolitionsTier3.ToString() + " / " + v_maxDemolitionsTier3.ToString(), font, ComplitionColorIncomplete, new RectangleF(x, y, width, height), valueTextFormat);
                            else
                                g.DrawString(v_DemolitionsTier3.ToString() + " / " + v_maxDemolitionsTier3.ToString(), font, ComplitionColorCompleted, new RectangleF(x, y, width, height), valueTextFormat);
                        }
                        else
                            g.DrawString(v_DemolitionsTier3.ToString() + " / " + v_maxDemolitionsTier3.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    break;
                #endregion
                #region Sabotage
                case (int)elementType.PerksSabotage:
                    //Todo
                    if (BGCompletedOpacity != 0 && v_SabotageTier3 == v_maxSabotageTier3)
                        g.FillRectangle(BackgroundColorCompleted, x, y, width, height);

                    if (v_SabotageTier1 != v_maxSabotageTier1)
                    {
                        g.DrawString("Nazi installations destroyed:", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                        g.DrawString(v_SabotageTier1.ToString() + " / " + v_maxSabotageTier1.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    else if (v_SabotageTier2 != v_maxSabotageTier2)
                    {
                        g.DrawString("Nazis towers destroyed:", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                        g.DrawString(v_SabotageTier2.ToString() + " / " + v_maxSabotageTier2.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    else
                    {
                        g.DrawString("Train bridges destroyed:", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                        if (CompletedColorEnabled)
                        {
                            if (v_SabotageTier3 != v_maxSabotageTier3)
                                g.DrawString(v_SabotageTier3.ToString() + " / " + v_maxSabotageTier3.ToString(), font, ComplitionColorIncomplete, new RectangleF(x, y, width, height), valueTextFormat);
                            else
                                g.DrawString(v_SabotageTier3.ToString() + " / " + v_maxSabotageTier3.ToString(), font, ComplitionColorCompleted, new RectangleF(x, y, width, height), valueTextFormat);
                        }
                        else
                            g.DrawString(v_SabotageTier3.ToString() + " / " + v_maxSabotageTier3.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    break;
                #endregion
                #region PerksMayhem
                case (int)elementType.PerksMayhem:
                    if (BGCompletedOpacity != 0 && v_MayhemTier3 == v_maxMayhemTier3)
                        g.FillRectangle(BackgroundColorCompleted, x, y, width, height);

                    if(v_MayhemTier1 != v_maxMayhemTier1)
                    {
                        g.DrawString("Nazis thrown to death:", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                        g.DrawString(v_MayhemTier1.ToString() + " / " + v_maxMayhemTier1.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    else if(v_MayhemTier2 != v_maxMayhemTier2)
                    {
                        g.DrawString("Nazis driven over:", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                        g.DrawString(v_MayhemTier2.ToString() + " / " + v_maxMayhemTier2.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    else
                    {
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
                    }
                    break;
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
                #endregion
                #region Racing
                case (int)elementType.PerksRacing:
                    if (BGCompletedOpacity != 0 && v_RacingTier3 == v_maxRacingTier3)
                        g.FillRectangle(BackgroundColorCompleted, x, y, width, height);

                    if (v_RacingTier1 != v_maxRacingTier1)
                    {
                        g.DrawString("Time trial won:", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                        g.DrawString(v_RacingTier1.ToString() + " / " + v_maxRacingTier1.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    else if (v_RacingTier2 != v_maxRacingTier2)
                    {
                        g.DrawString("Country Race 1 won::", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                        g.DrawString(v_RacingTier2.ToString() + " / " + v_maxRacingTier2.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    else
                    {
                        g.DrawString("Country Race 2 won:", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                        if (CompletedColorEnabled)
                        {
                            if (v_RacingTier3 != v_maxRacingTier3)
                                g.DrawString(v_RacingTier3.ToString() + " / " + v_maxRacingTier3.ToString(), font, ComplitionColorIncomplete, new RectangleF(x, y, width, height), valueTextFormat);
                            else
                                g.DrawString(v_RacingTier3.ToString() + " / " + v_maxRacingTier3.ToString(), font, ComplitionColorCompleted, new RectangleF(x, y, width, height), valueTextFormat);
                        }
                        else
                            g.DrawString(v_RacingTier3.ToString() + " / " + v_maxRacingTier3.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    break;
                #endregion
                #region Mechanics
                case (int)elementType.PerksMechanics:
                    if (BGCompletedOpacity != 0 && v_MechanicsTier3 == v_maxMechanicsTier3)
                        g.FillRectangle(BackgroundColorCompleted, x, y, width, height);

                    if (v_MechanicsTier1 != v_maxMechanicsTier1)
                    {
                        g.DrawString("Civilian vehicles collected:", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                        g.DrawString(v_MechanicsTier1.ToString() + " / " + v_maxMechanicsTier1.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    else if (v_MechanicsTier2 != v_maxMechanicsTier2)
                    {
                        g.DrawString("Nazi vehicles collected:", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                        g.DrawString(v_MechanicsTier2.ToString() + " / " + v_maxMechanicsTier2.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    else
                    {
                        g.DrawString("Vehicles collected:", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                        if (CompletedColorEnabled)
                        {
                            if (v_MechanicsTier3 != v_maxMechanicsTier3)
                                g.DrawString(v_MechanicsTier3.ToString() + " / " + v_maxMechanicsTier3.ToString(), font, ComplitionColorIncomplete, new RectangleF(x, y, width, height), valueTextFormat);
                            else
                                g.DrawString(v_MechanicsTier3.ToString() + " / " + v_maxMechanicsTier3.ToString(), font, ComplitionColorCompleted, new RectangleF(x, y, width, height), valueTextFormat);
                        }
                        else
                            g.DrawString(v_MechanicsTier3.ToString() + " / " + v_maxMechanicsTier3.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    break;
                #endregion
                #region Evasion
                case (int)elementType.PerksEvasion:
                    //Todo
                    if (BGCompletedOpacity != 0 && v_EvasionTier3 == v_maxEvasionTier3)
                        g.FillRectangle(BackgroundColorCompleted, x, y, width, height);

                    if (v_EvasionTier1 != v_maxBrawlingTier1)
                    {
                        g.DrawString("Times escaped from Level 2 alarm:", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                        g.DrawString(v_EvasionTier1.ToString() + " / " + v_maxEvasionTier1.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    else if (v_EvasionTier2 != v_maxEvasionTier2)
                    {
                        g.DrawString("Escaped from Level 3 alarm:", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                        g.DrawString(v_EvasionTier2.ToString() + " / " + v_maxEvasionTier2.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    else
                    {
                        g.DrawString("Escaped from Level 5 alarm:", font, brush, new RectangleF(x, y, width, height), descriptiveTextFormat);
                        if (CompletedColorEnabled)
                        {
                            if (v_EvasionTier3 != v_maxEvasionTier3)
                                g.DrawString(v_EvasionTier3.ToString() + " / " + v_maxEvasionTier3.ToString(), font, ComplitionColorIncomplete, new RectangleF(x, y, width, height), valueTextFormat);
                            else
                                g.DrawString(v_EvasionTier3.ToString() + " / " + v_maxEvasionTier3.ToString(), font, ComplitionColorCompleted, new RectangleF(x, y, width, height), valueTextFormat);
                        }
                        else
                            g.DrawString(v_EvasionTier3.ToString() + " / " + v_maxEvasionTier3.ToString(), font, brush, new RectangleF(x, y, width, height), valueTextFormat);
                    }
                    break;
                    #endregion
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
            gameProcess = getGameProcess();
            if (gameProcess != null && !gameProcess.HasExited )
            {
                //Brawling
                v_BrawlingTier1 = _pBrawlingTier1.Deref<int>(gameProcess);
                v_BrawlingTier2 = _pBrawlingTier2.Deref<int>(gameProcess);
                v_BrawlingTier3 = _pBrawlingTier3.Deref<int>(gameProcess);

                //Hardware
                v_HardwareTier1 = _pHardwareTier1.Deref<int>(gameProcess);
                v_HardwareTier2 = _pHardwareTier2.Deref<int>(gameProcess);
                v_HardwareTier3 = _pHardwareTier3.Deref<int>(gameProcess);

                //Sniping
                v_SnipingTier1 = _pSnipingTier1.Deref<int>(gameProcess);
                v_SnipingTier2 = _pSnipingTier2.Deref<int>(gameProcess);
                v_SnipingTier3 = _pSnipingTier3.Deref<int>(gameProcess);

                //Explosives
                v_ExplosivesTier1 = _pExplosivesTier1.Deref<int>(gameProcess);
                v_ExplosivesTier2 = _pExplosivesTier2.Deref<int>(gameProcess);
                v_ExplosivesTier3 = _pExplosivesTier3.Deref<int>(gameProcess);

                //Demolitions
                v_DemolitionsTier1 = _pDemolitionsTier1.Deref<int>(gameProcess);
                v_DemolitionsTier2 = _pDemolitionsTier2.Deref<int>(gameProcess);
                v_DemolitionsTier3 = _pDemolitionsTier3.Deref<int>(gameProcess);

                //Sabotage
                v_SabotageTier1 = _pDemolitionsTier1.Deref<int>(gameProcess);
                v_SabotageTier2 = _pDemolitionsTier2.Deref<int>(gameProcess);
                v_SabotageTier3 = _pDemolitionsTier3.Deref<int>(gameProcess);

                //Mayhem
                v_MayhemTier1 = _pMayhemTier1.Deref<int>(gameProcess);
                v_MayhemTier2 = _pMayhemTier2.Deref<int>(gameProcess);
                v_MayhemTier3 = _pMayhemTier3.Deref<int>(gameProcess);

                //Racing
                v_RacingTier1 = _pRacingTier1.Deref<int>(gameProcess);
                v_RacingTier2 = _pRacingTier2.Deref<int>(gameProcess);
                v_RacingTier3 = _pRacingTier3.Deref<int>(gameProcess);

                //Mechanics
                v_MechanicsTier1 = _pMachanicsTier1.Deref<int>(gameProcess);
                v_MechanicsTier2 = _pMachanicsTier2.Deref<int>(gameProcess);
                v_MechanicsTier3 = _pMachanicsTier3.Deref<int>(gameProcess);

                //Evasion
                v_EvasionTier1 = _pEvasionTier1.Deref<int>(gameProcess);
                v_EvasionTier2 = _pEvasionTier2.Deref<int>(gameProcess);
                v_EvasionTier3 = _pEvasionTier3.Deref<int>(gameProcess);


                if (invalidator != null)
                {
                    invalidator.Invalidate(0, 0, width, height);
                }
            }       
        }

        private Process getGameProcess()
        {
            Process process = Process.GetProcessesByName("saboteur").FirstOrDefault();
            if(process != null)
            {
                if(!foundProcess)
                {
                    if (process.MainModule.ModuleMemorySize == (int)GameVersions.GOG)
                    {
                        //Brawling
                        _pBrawlingTier1 = new DeepPointer(0x010968AC, 0x14, 0x13C);
                        _pBrawlingTier2 = new DeepPointer(0x010968AC, 0x18, 0x13C);
                        _pBrawlingTier3 = new DeepPointer(0x010968AC, 0x1C, 0x13C);

                        //Hardware
                        _pHardwareTier1 = new DeepPointer(0x010968AC, 0x20, 0x13C);
                        _pHardwareTier2 = new DeepPointer(0x010968AC, 0x24, 0x13C);
                        _pHardwareTier3 = new DeepPointer(0x010968AC, 0x28, 0x13C);

                        //Sniping
                        _pSnipingTier1 = new DeepPointer(0x010968AC, 0x2C, 0x13C);
                        _pSnipingTier2 = new DeepPointer(0x010968AC, 0x30, 0x13C);
                        _pSnipingTier3 = new DeepPointer(0x010968AC, 0x34, 0x13C);

                        //Explosives
                        _pExplosivesTier1 = new DeepPointer(0x010968AC, 0x38, 0x13C);
                        _pExplosivesTier2 = new DeepPointer(0x010968AC, 0x3C, 0x13C);
                        _pExplosivesTier3 = new DeepPointer(0x010968AC, 0x40, 0x13C);

                        //Demolitions
                        _pDemolitionsTier1 = new DeepPointer(0x010968AC, 0x44, 0x13C);
                        _pDemolitionsTier2 = new DeepPointer(0x010968AC, 0x48, 0x13C);
                        _pDemolitionsTier3 = new DeepPointer(0x010968AC, 0x4C, 0x13C);

                        //Sabotage
                        _pSabotageTier1 = new DeepPointer(0x010968AC, 0x50, 0x13C);
                        _pSabotageTier2 = new DeepPointer(0x010968AC, 0x54, 0x13C);
                        _pSabotageTier3 = new DeepPointer(0x010968AC, 0x58, 0x13C);

                        //Mayhem
                        _pMayhemTier1 = new DeepPointer(0x010968AC, 0x5C, 0x13C);
                        _pMayhemTier2 = new DeepPointer(0x010968AC, 0x60, 0x13C);
                        _pMayhemTier3 = new DeepPointer(0x010968AC, 0x64, 0x13C);

                        //Racing
                        _pRacingTier1 = new DeepPointer(0x010968AC, 0x68, 0x13C);
                        _pRacingTier2 = new DeepPointer(0x010968AC, 0x6C, 0x13C);
                        _pRacingTier3 = new DeepPointer(0x010968AC, 0x70, 0x13C);

                        //Mechanics
                        _pMayhemTier1 = new DeepPointer(0x010968AC, 0x74, 0x13C);
                        _pMayhemTier2 = new DeepPointer(0x010968AC, 0x78, 0x13C);
                        _pMayhemTier3 = new DeepPointer(0x010968AC, 0x7C, 0x13C);

                        //Evasion
                        _pEvasionTier1 = new DeepPointer(0x010968AC, 0x80, 0x13C);
                        _pEvasionTier2 = new DeepPointer(0x010968AC, 0x84, 0x13C);
                        _pEvasionTier3 = new DeepPointer(0x010968AC, 0x88, 0x13C);
                    }
                    else
                    {
                        //Brawling
                        _pBrawlingTier1 = new DeepPointer(0x010968AC, 0x14, 0x13C);
                        _pBrawlingTier2 = new DeepPointer(0x010968AC, 0x18, 0x13C);
                        _pBrawlingTier3 = new DeepPointer(0x010968AC, 0x1C, 0x13C);

                        //Hardware
                        _pHardwareTier1 = new DeepPointer(0x010968AC, 0x20, 0x13C);
                        _pHardwareTier2 = new DeepPointer(0x010968AC, 0x24, 0x13C);
                        _pHardwareTier3 = new DeepPointer(0x010968AC, 0x28, 0x13C);

                        //Sniping
                        _pSnipingTier1 = new DeepPointer(0x010968AC, 0x2C, 0x13C);
                        _pSnipingTier2 = new DeepPointer(0x010968AC, 0x30, 0x13C);
                        _pSnipingTier3 = new DeepPointer(0x010968AC, 0x34, 0x13C);

                        //Explosives
                        _pExplosivesTier1 = new DeepPointer(0x010968AC, 0x38, 0x13C);
                        _pExplosivesTier2 = new DeepPointer(0x010968AC, 0x3C, 0x13C);
                        _pExplosivesTier3 = new DeepPointer(0x010968AC, 0x40, 0x13C);

                        //Demolitions
                        _pDemolitionsTier1 = new DeepPointer(0x010968AC, 0x44, 0x13C);
                        _pDemolitionsTier2 = new DeepPointer(0x010968AC, 0x48, 0x13C);
                        _pDemolitionsTier3 = new DeepPointer(0x010968AC, 0x4C, 0x13C);

                        //Sabotage
                        _pSabotageTier1 = new DeepPointer(0x010968AC, 0x50, 0x13C);
                        _pSabotageTier2 = new DeepPointer(0x010968AC, 0x54, 0x13C);
                        _pSabotageTier3 = new DeepPointer(0x010968AC, 0x58, 0x13C);

                        //Mayhem
                        _pMayhemTier1 = new DeepPointer(0x010968AC, 0x5C, 0x13C);
                        _pMayhemTier2 = new DeepPointer(0x010968AC, 0x60, 0x13C);
                        _pMayhemTier3 = new DeepPointer(0x010968AC, 0x64, 0x13C);

                        //Racing
                        _pRacingTier1 = new DeepPointer(0x010968AC, 0x68, 0x13C);
                        _pRacingTier2 = new DeepPointer(0x010968AC, 0x6C, 0x13C);
                        _pRacingTier3 = new DeepPointer(0x010968AC, 0x70, 0x13C);

                        //Mechanics
                        _pMachanicsTier1 = new DeepPointer(0x010968AC, 0x74, 0x13C);
                        _pMachanicsTier2 = new DeepPointer(0x010968AC, 0x78, 0x13C);
                        _pMachanicsTier3 = new DeepPointer(0x010968AC, 0x7C, 0x13C);

                        //Evasion
                        _pEvasionTier1 = new DeepPointer(0x010968AC, 0x80, 0x13C);
                        _pEvasionTier2 = new DeepPointer(0x010968AC, 0x84, 0x13C);
                        _pEvasionTier3 = new DeepPointer(0x010968AC, 0x88, 0x13C);
                    }
                }
                foundProcess = true;
                return process;
            }
            foundProcess = false;

            return null;
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
