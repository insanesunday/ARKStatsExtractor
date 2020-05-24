﻿using ARKBreedingStats.Library;
using ARKBreedingStats.species;
using System;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ARKBreedingStats
{
    static class Utils
    {
        /// <summary>
        /// This method returns a shade from red over yellow to green, corresponding to the value p (0-100). If light is given, the brightness is adjusted (-1 to 1)
        /// </summary>
        /// <param name="percent">percent that determines the shade between red and green (0 to 100)</param>
        /// <param name="light">double from -1 to 1. Values greater zero make the color brighter, lower than zero make the color darker.</param>
        /// <param name="blue">true if color should fade from violett to green</param>
        /// <returns>the calculated color.</returns>
        public static Color GetColorFromPercent(int percent, double light = 0, bool blue = false)
        {
            GetRGBFromPercent(out int r, out int g, out int b, percent, light);
            return blue ? Color.FromArgb(b, g, r) : Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// String with ARKml tags.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="percent"></param>
        /// <param name="light"></param>
        /// <returns></returns>
        public static string GetARKmlFromPercent(string text, int percent, double light = 0)
        {
            GetRGBFromPercent(out int r, out int g, out int b, percent, light);
            return GetARKml(text, r, g, b);
        }

        /// <summary>
        /// Returns a string with ARKml tags. Currently that doesn't seem to be supported anymore by the ARK chat.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string GetARKml(string text, int r, int g, int b)
        {
            return "<RichColor Color=\"" + Math.Round(r / 255d, 2).ToString(CultureInfo.InvariantCulture) + "," + Math.Round(g / 255d, 2).ToString(CultureInfo.InvariantCulture) + "," + Math.Round(b / 255d, 2).ToString(CultureInfo.InvariantCulture) + ",1\">" + text + "</>";
        }

        /// <summary>
        /// RGB values for a given percentage (0-100). 0 is red, 100 is green. Light can be adjusted (1 bright, 0 default, -1 dark).
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="percent"></param>
        /// <param name="light"></param>
        private static void GetRGBFromPercent(out int r, out int g, out int b, int percent, double light = 0)
        {
            if (light > 1) { light = 1; }
            if (light < -1) { light = -1; }
            g = (int)(percent * 5.1); // g = percent * 255 / 50 = percent * 5.1
            r = 511 - g;
            b = 0;
            if (r < 0) { r = 0; }
            if (g < 0) { g = 0; }
            if (r > 255) { r = 255; }
            if (g > 255) { g = 255; }
            if (light > 0)
            {
                r = (int)((255 - r) * light + r);
                g = (int)((255 - g) * light + g);
                b = (int)((255 - b) * light + b);
            }
            else
            {
                light += 1;
                r = (int)(r * light);
                g = (int)(g * light);
                //b = (int)(b * light); // b == 0 always
            }
        }

        /// <summary>
        /// Color that represents a mutation.
        /// </summary>
        public static Color MutationColor => Color.FromArgb(225, 192, 255);
        /// <summary>
        /// Color that represents a mutation number over the limit.
        /// </summary>
        public static Color MutationColorOverLimit => Color.FromArgb(255, 200, 200);

        /// <summary>
        /// String icon that represents a sex.
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public static string SexSymbol(Sex g)
        {
            switch (g)
            {
                case Sex.Male:
                    return "♂";
                case Sex.Female:
                    return "♀";
                default:
                    return "?";
            }
        }

        /// <summary>
        /// Color for a sex.
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public static Color SexColor(Sex g)
        {
            switch (g)
            {
                case Sex.Male:
                    return Color.FromArgb(220, 235, 255);
                case Sex.Female:
                    return Color.FromArgb(255, 230, 255);
                default:
                    return SystemColors.Control;
            }
        }

        /// <summary>
        /// String icon that represents a creature status.
        /// </summary>
        /// <param name="status"></param>
        /// <param name="defaultReturn"></param>
        /// <returns></returns>
        public static string StatusSymbol(CreatureStatus status, string defaultReturn = "✓")
        {
            switch (status)
            {
                case CreatureStatus.Dead: return "†";
                case CreatureStatus.Unavailable: return "✗";
                case CreatureStatus.Obelisk: return "⌂";
                case CreatureStatus.Cryopod: return "Θ";
                default: return defaultReturn;
            }
        }

        /// <summary>
        /// Localized creature status string.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string StatusText(CreatureStatus status)
        {
            switch (status)
            {
                case CreatureStatus.Available: return Loc.s(CreatureStatus.Available.ToString());
                case CreatureStatus.Dead: return Loc.s(CreatureStatus.Dead.ToString());
                case CreatureStatus.Unavailable: return Loc.s(CreatureStatus.Unavailable.ToString());
                case CreatureStatus.Obelisk: return Loc.s(CreatureStatus.Obelisk.ToString());
                case CreatureStatus.Cryopod: return Loc.s(CreatureStatus.Cryopod.ToString());
                default: return "n/a";
            }
        }

        /// <summary>
        /// Returns the next possible sex.
        /// </summary>
        /// <param name="sex"></param>
        /// <returns></returns>
        public static Sex NextSex(Sex sex)
        {
            switch (sex)
            {
                case Sex.Female:
                    return Sex.Male;
                case Sex.Male:
                    return Sex.Unknown;
                default:
                    return Sex.Female;
            }
        }

        /// <summary>
        /// Returns the next possible creature status.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static CreatureStatus NextStatus(CreatureStatus status)
        {
            switch (status)
            {
                case CreatureStatus.Available: return CreatureStatus.Unavailable;
                case CreatureStatus.Unavailable: return CreatureStatus.Dead;
                case CreatureStatus.Dead: return CreatureStatus.Obelisk;
                case CreatureStatus.Obelisk: return CreatureStatus.Cryopod;
                default: return CreatureStatus.Available;
            }
        }

        /// <summary>
        /// Probability of the occurence of a stat level, assuming a normal distribution of 180 levels on 7 stats.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static string LevelPercentile(int level)
        {
            double[] prb = { 100, 100, 100, 100, 100, 100, 100, 100, 100, 99.99, 99.98, 99.95, 99.88, 99.72, 99.40, 98.83, 97.85, 96.28, 93.94, 90.62, 86.20, 80.61, 73.93, 66.33, 58.10, 49.59, 41.19, 33.26, 26.08, 19.85, 14.66, 10.50, 7.30, 4.92, 3.21, 2.04, 1.25, 0.75, 0.43, 0.24, 0.13 };
            if (level < 0) level = 0;
            if (level >= prb.Length) level = prb.Length - 1;
            return string.Format(Loc.s("topPercentileLevel"), prb[level].ToString("N2"));
        }

        private static string[] statNames, statNamesAbb, statNamesAberrant, statNamesAberrantAbb;
        public static void InitializeLocalizations()
        {
            statNames = new[] { Loc.s("Health"), Loc.s("Stamina"), Loc.s("Torpidity"), Loc.s("Oxygen"), Loc.s("Food"), Loc.s("Water"), Loc.s("Temperature"), Loc.s("Weight"), Loc.s("Damage"), Loc.s("Speed"), Loc.s("Fortitude"), Loc.s("CraftingSpeed") };
            statNamesAbb = new[] { Loc.s("Health_Abb"), Loc.s("Stamina_Abb"), Loc.s("Torpidity_Abb"), Loc.s("Oxygen_Abb"), Loc.s("Food_Abb"), Loc.s("Water_Abb"), Loc.s("Temperature_Abb"), Loc.s("Weight_Abb"), Loc.s("Damage_Abb"), Loc.s("Speed_Abb"), Loc.s("Fortitude_Abb"), Loc.s("CraftingSpeed_Abb") };
            statNamesAberrant = new[] { Loc.s("Health"), Loc.s("ChargeCapacity"), Loc.s("Torpidity"), Loc.s("ChargeRegeneration"), Loc.s("Food"), Loc.s("Water"), Loc.s("Temperature"), Loc.s("Weight"), Loc.s("ChargeEmissionRange"), Loc.s("Speed"), Loc.s("Fortitude"), Loc.s("CraftingSpeed") };
            statNamesAberrantAbb = new[] { Loc.s("Health_Abb"), Loc.s("ChargeCapacity_Abb"), Loc.s("Torpidity_Abb"), Loc.s("ChargeRegeneration_Abb"), Loc.s("Food_Abb"), Loc.s("Water_Abb"), Loc.s("Temperature_Abb"), Loc.s("Weight_Abb"), Loc.s("ChargeEmissionRange_Abb"), Loc.s("Speed_Abb"), Loc.s("Fortitude_Abb"), Loc.s("CraftingSpeed_Abb") };
        }

        /// <summary>
        /// String that represents the localized stat name.
        /// </summary>
        /// <param name="statIndex"></param>
        /// <param name="abbreviation"></param>
        /// <param name="glowSpecies"></param>
        /// <returns></returns>
        public static string StatName(int statIndex, bool abbreviation = false, bool glowSpecies = false)
        {
            if (statNames == null || statIndex < 0 || statIndex >= statNames.Length)
                return "";
            if (glowSpecies)
            {
                return abbreviation ? statNamesAberrantAbb[statIndex] : statNamesAberrant[statIndex];
            }
            return abbreviation ? statNamesAbb[statIndex] : statNames[statIndex];
        }

        public static string StatName(species.StatNames sn, bool abbreviation = false, bool glowSpecies = false)
        {
            return StatName((int)sn, abbreviation, glowSpecies);
        }

        /// <summary>
        /// Returns the displayed decimal values of the stat with the given index
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int Precision(int s)
        {
            // damage and speed are percentagevalues, need more precision
            return (s == (int)StatNames.SpeedMultiplier || s == (int)StatNames.MeleeDamageMultiplier || s == (int)StatNames.CraftingSpeedMultiplier) ? 3 : 1;
        }

        /// <summary>
        /// String that represents a duration.
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static string Duration(TimeSpan ts)
        {
            return ts.ToString("dd':'hh':'mm':'ss");
        }

        /// <summary>
        /// String that represents a duration, given in seconds.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static string Duration(int seconds)
        {
            return Duration(new TimeSpan(0, 0, seconds));
        }

        /// <summary>
        /// Returns the formated timespan and also the DateTime when the timespan is over
        /// </summary>
        /// <param name="ts">timespan of countdown</param>
        /// <returns>Returns the timespan and the DateTime when the timespan is over</returns>
        public static string DurationUntil(TimeSpan ts)
        {
            return ts.ToString("d':'hh':'mm':'ss") + " (" + Loc.s("until") + ": " + ShortTimeDate(DateTime.Now.Add(ts)) + ")";
        }

        /// <summary>
        /// Returns a string that represents the given DateTime in a short format.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="omitDateIfToday"></param>
        /// <returns></returns>
        public static string ShortTimeDate(DateTime? dt, bool omitDateIfToday = true)
        {
            if (dt == null) return Loc.s("Unknown");
            return dt.Value.ToShortTimeString() + (omitDateIfToday && DateTime.Today == dt.Value.Date ? string.Empty : " - " + dt.Value.ToShortDateString());
        }

        /// <summary>
        /// String that represents the time left.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string TimeLeft(DateTime dt)
        {
            return dt < DateTime.Now ? "-" : Duration(dt.Subtract(DateTime.Now));
        }

        /// <summary>
        /// Returns the imprinting gain per cuddle, dependent on the maturation time and the cuddle interval multiplier.
        /// </summary>
        /// <param name="maturationTime"></param>
        /// <param name="cuddleIntervalMultiplier"></param>
        /// <returns></returns>
        public static double ImprintingGainPerCuddle(double maturationTime, double cuddleIntervalMultiplier)
        {
            return 1d / Math.Max(1, Math.Floor(maturationTime / (28800 * cuddleIntervalMultiplier)));
        }

        /// <summary>
        /// Returns either black or white, depending on the backcolor, so text can be read well.
        /// </summary>
        /// <param name="backColor"></param>
        /// <returns>ForeColor</returns>
        public static Color ForeColor(Color backColor)
        {
            return backColor.R * .3f + backColor.G * .59f + backColor.B * .11f < 110 ? Color.White : Color.Black;
        }

        public static bool ShowTextInput(string text, out string input, string title = "", string preInput = "")
        {
            Form inputForm = new Form
            {
                Width = 250,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedToolWindow,
                Text = title,
                StartPosition = FormStartPosition.CenterParent,
                ShowInTaskbar = false
            };
            Label textLabel = new Label { Left = 20, Top = 15, Text = text, AutoSize = true };
            TextBox textBox = new TextBox { Left = 20, Top = 40, Width = 200 };
            Button buttonOK = new Button { Text = Loc.s("OK"), Left = 120, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            Button buttonCancel = new Button { Text = Loc.s("Cancel"), Left = 20, Width = 80, Top = 70, DialogResult = DialogResult.Cancel };
            buttonOK.Click += (sender, e) => { inputForm.Close(); };
            buttonCancel.Click += (sender, e) => { inputForm.Close(); };
            inputForm.Controls.Add(textBox);
            inputForm.Controls.Add(buttonOK);
            inputForm.Controls.Add(buttonCancel);
            inputForm.Controls.Add(textLabel);
            inputForm.AcceptButton = buttonOK;
            inputForm.CancelButton = buttonCancel;
            textBox.Text = preInput;
            textBox.SelectAll();

            input = "";
            if (inputForm.ShowDialog() != DialogResult.OK)
                return false;
            input = textBox.Text;
            return true;
        }

        /// <summary>
        /// This function may only be used if the ArkId is unique (when importing files that have ArkId1 and ArkId2)
        /// </summary>
        /// <param name="arkId">ArkId built from ArkId1 and ArkId2, user input from the ingame-representation is not allowed</param>
        /// <returns>Guid built from the ArkId</returns>
        public static Guid ConvertArkIdToGuid(long arkId)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(arkId).CopyTo(bytes, 0);
            return new Guid(bytes);
        }

        public static bool IsArkIdImported(long arkId, Guid guid)
        {
            return arkId != 0
                     && guid == ConvertArkIdToGuid(arkId);
        }

        /// <summary>
        /// Returns the Ark-Id as seen ingame from the unique representation used in ASB
        /// </summary>
        /// <param name="importedArkId"></param>
        /// <returns>Ingame visualisation of the Ark-Id (not unique in rare cases)</returns>
        public static string ConvertImportedArkIdToIngameVisualization(long importedArkId)
        {
            return ((int)(importedArkId >> 32)).ToString() + ((int)importedArkId).ToString();
        }

        /// <summary>
        /// returns a shortened string with an ellipsis in the middle. One third of the beginning is shown and two thirds of then end
        /// </summary>
        /// <param name="longPath">long string</param>
        /// <param name="maxLength">max length of the string</param>
        /// <returns>string with ellipsis in the middle</returns>
        public static string ShortPath(string longPath, int maxLength = 50)
        {
            if (longPath.Length <= maxLength)
                return longPath;
            int begin = maxLength / 3;
            return longPath.Substring(0, begin) + "…" + longPath.Substring(longPath.Length - maxLength + begin + 1);
        }

        /// <summary>
        /// Determines the default import export folder.
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static bool GetFirstImportExportFolder(out string folder)
        {
            folder = "";
            if (Properties.Settings.Default.ExportCreatureFolders != null
                && Properties.Settings.Default.ExportCreatureFolders.Length > 0)
            {
                var loc = settings.ATImportExportedFolderLocation.CreateFromString(Properties.Settings.Default.ExportCreatureFolders[0]);
                if (System.IO.Directory.Exists(loc.FolderPath))
                {
                    folder = loc.FolderPath;
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Changes the color of a control briefly.
        /// </summary>
        /// <param name="c"></param>
        public static async void BlinkAsync(Control ctlr, Color c, int duration = 500, bool foreColor = true)
        {
            if (foreColor)
            {
                Color original = ctlr.ForeColor;
                ctlr.ForeColor = c;
                await Task.Delay(duration);
                ctlr.ForeColor = original;
            }
            else
            {
                Color original = ctlr.BackColor;
                ctlr.BackColor = c;
                await Task.Delay(duration);
                ctlr.BackColor = original;
            }
        }

        /// <summary>
        /// Returns the rectangle of the passed form.
        /// </summary>
        public static Rectangle SaveFormRectangle(Form form) =>
            new Rectangle(form.Location.X, form.Location.Y, form.Width, form.Height);

        /// <summary>
        /// Sets the form rectangle to the passed data.
        /// </summary>
        public static void SetFormRectangle(Form form, Rectangle rect)
        {
            form.Location = new Point(rect.X, rect.Y);
            form.Width = rect.Width;
            form.Height = rect.Height;
        }
    }
}
