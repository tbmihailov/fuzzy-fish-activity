using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FuzzyFramework.Dimensions;
using FuzzyFramework.Sets;
using FuzzyFramework.Graphics;
using FuzzyFramework;
using FuzzyFramework.Defuzzification;
using FuzzyFramework.Members;
using System.Globalization;
using Common.Helpers;

namespace FishingAdvisor
{
    public partial class FishingActivity : Form
    {

        #region Definition of dimensions
        protected static IContinuousDimension temperature;
        protected static IContinuousDimension deltaTemperature;

        //protected static IContinuousDimension windSpeed;
        //protected static IContinuousDimension deltaWindSpeed;

        protected static IContinuousDimension deltaPressure;

        protected static IContinuousDimension dayTime;

        //protected static IContinuousDimension moonPhase;

        protected static IContinuousDimension fishActivity;
        #endregion

        #region Basic single-dimensional fuzzy sets

        //WEATHER

        //for temp:
        public static ContinuousSet lowTemperature;
        public static ContinuousSet highTemperature;
        public static ContinuousSet goodTemperature;

        //for delta temp:
        public static ContinuousSet risingTemperature;
        public static ContinuousSet fallingTemperature;
        public static ContinuousSet constantTemperature;

        ////for wind speed:
        //public static ContinuousSet slowWind;
        //public static ContinuousSet fastFind;
        //public static ContinuousSet goodWind;

        ////for delta wind speed:
        //public static ContinuousSet risingWindSpeed;
        //public static ContinuousSet fallingWindSpeed;
        //public static ContinuousSet constantWindSpeed;

        ////for pressure:
        //public static ContinuousSet highPressure;
        //public static ContinuousSet normalPressure;

        //for delta pressure:
        public static ContinuousSet weatherGettingBetter;
        public static ContinuousSet weatherNoChange;
        public static ContinuousSet weatherGettingWorse;

        //ASTRONOMY

        //moon & sun peak times:
        public static ContinuousSet moonRise;
        public static ContinuousSet moonSet;
        public static ContinuousSet sunRise;
        public static ContinuousSet sunSet;

        //moon:
        //public static DiscreteSet moonPhases;

        //for action
        public static ContinuousSet lowActivity;
        public static ContinuousSet mediumActivity;
        public static ContinuousSet highActivity;

        #endregion

        #region Internal properties
        protected string _expression = null;
        protected FuzzyRelation _relation;
        protected DefuzzificationFactory.DefuzzificationMethod _defuzzMethod = DefuzzificationFactory.DefuzzificationMethod.CenterOfGravity;
        protected Defuzzification _defuzzification;
        protected bool _ready = false;
        protected bool _waitingForBuild = false;
        protected bool _building = false;
        Form _parentForm;
        #endregion

        #region Constructor
        public FishingActivity(Form parentForm)
        {
            _parentForm = parentForm;

            InitializeComponent();

            buildSets();
            _ready = true;
            buildRelationNow(true);
        }

        private void buildSets()
        {
            //WEATHER
            //for temp:
            int minTemp = -30;
            int avgTemp = 3;
            int maxTemp = +45;
            BuildTemperatureSets(minTemp, avgTemp, maxTemp);

            //for delta temp:
            BuildDeltaTemperatureSets();

            //for delta pressure:
            BuildDeltaPressureSets();

            //ASTRONOMY
            //moon & sun peak times:
            BuildMoonSunPeakSets(txtSunrise.Text, txtSunset.Text, txtMoonrise.Text, txtMoonset.Text);

            //moon:
            //public static DiscreteSet moonPhases;

            //for action
            buildActivitySets();
        }

        protected void buildActivitySets()
        {
            //0-10
            //fishActivity = new ContinuousDimension("FishActivity", "Low to High", "", 0, 10);

            //lowActivity = new RightQuadraticSet(fishActivity, "Low activity", 0, 3, 8);
            //highActivity = new LeftQuadraticSet(fishActivity, "High activity", 0, 6, 10);

            fishActivity = new ContinuousDimension("FishActivity", "Low to High", "", -10, 10);

            lowActivity = new RightQuadraticSet(fishActivity, "Low activity", -10, -5, 5);
            highActivity = new LeftQuadraticSet(fishActivity, "High activity", -5, 5, 10);

            mediumActivity = new BellSet(fishActivity, "Medium activity", 0, 3, 5);

            DrawFuzzySetToPictureBox(lowActivity, pictureBoxLowActivity);
            DrawFuzzySetToPictureBox(highActivity, pictureBoxHighActivity);
            DrawFuzzySetToPictureBox(mediumActivity, pictureBoxMediumActivity);
        }


        private void BuildMoonSunPeakSets(string sunrise, string sunset, string moonrise, string moonset)
        {
            var sunriseTime = DateHelper.ParseDate(sunrise).TimeOfDay;
            var sunsetTime = DateHelper.ParseDate(sunset).TimeOfDay;
            var moonriseTime = DateHelper.ParseDate(moonrise).TimeOfDay;
            var moonsetTime = DateHelper.ParseDate(moonset).TimeOfDay;

            BuildMoonSunPeakSets(sunriseTime, sunsetTime, moonriseTime, moonsetTime);
        }

        private void BuildMoonSunPeakSets(TimeSpan sunrise, TimeSpan sunset, TimeSpan moonrise, TimeSpan moonset)
        {

            dayTime = new ContinuousDimension("time", "Time of the day", "t,m", -120, 24 * 60 - 1);

            int sunriseTime = (int)sunrise.TotalMinutes;
            sunRise = new BellSet(dayTime, "Sunrise", sunriseTime, 45, 120);

            int sunsetTime = (int)sunset.TotalMinutes;
            sunSet = new BellSet(dayTime, "Sunset", sunsetTime, 45, 120);

            int moonriseTime = (int)moonrise.TotalMinutes;
            moonRise = new BellSet(dayTime, "Moonrise", moonriseTime, 45, 120);

            int moonsetTime = (int)moonset.TotalMinutes;
            moonSet = new BellSet(dayTime, "Moonset", moonsetTime, 45, 120);

            #region Draw graphics
            DrawFuzzySetToPictureBox(sunRise, pictureBoxSunrise);
            DrawFuzzySetToPictureBox(sunSet, pictureBoxSunset);
            DrawFuzzySetToPictureBox(moonRise, pictureBoxMoonrise);
            DrawFuzzySetToPictureBox(moonSet, pictureBoxMoonset);
            #endregion
        }

        private void BuildTemperatureSets(int minTemp, int avgTemp, int maxTemp)
        {
            temperature = new ContinuousDimension("t", "Temperature - weather", "°C", minTemp, maxTemp);

            lowTemperature = new RightQuadraticSet(temperature, "Low temperature", avgTemp - 10, avgTemp - 5, avgTemp);
            highTemperature = new LeftQuadraticSet(temperature, "High temperature", avgTemp, avgTemp + 5, avgTemp + 10);
            goodTemperature = new BellSet(temperature, "Good temperature", avgTemp, 5, 10);

            #region Draw graphics
            DrawFuzzySetToPictureBox(lowTemperature, pictureBoxLowTemp);
            DrawFuzzySetToPictureBox(highTemperature, pictureBoxHighTemp);
            DrawFuzzySetToPictureBox(goodTemperature, pictureBoxCorrectTemp);
            #endregion
        }

        private void BuildDeltaTemperatureSets()
        {
            deltaTemperature = new ContinuousDimension("∆t", "Change of temperature for past minutes", "°C/t", -10, +10);

            fallingTemperature = new RightQuadraticSet(deltaTemperature, "Falling temperature", -6, -2, 0);
            risingTemperature = new LeftQuadraticSet(deltaTemperature, "Rising temperature", 0, 2, 6);
            constantTemperature = new BellSet(deltaTemperature, "Constant temperature", 0, 2, 5);

            #region Draw graphics
            DrawFuzzySetToPictureBox(fallingTemperature, pictureBoxFallingTemp);
            DrawFuzzySetToPictureBox(risingTemperature, pictureBoxRisingTemp);
            DrawFuzzySetToPictureBox(constantTemperature, pictureBoxConstantTemp);
            #endregion
        }

        private void BuildDeltaPressureSets()
        {
            int normalPressure = 0;//normal
            int changeStep = 100;

            deltaPressure = new ContinuousDimension("∆t", "Change of temperature for past minutes", "°C/t", normalPressure - changeStep * 2, normalPressure + changeStep * 2);

            weatherGettingBetter = new RightQuadraticSet(deltaPressure, "Getting better (Falling pressure)", normalPressure - changeStep, normalPressure - changeStep / 2, normalPressure);
            weatherGettingWorse = new LeftQuadraticSet(deltaPressure, "Getting worse (Rising pressure)", normalPressure, normalPressure + changeStep / 2, normalPressure + changeStep);
            weatherNoChange = new BellSet(deltaPressure, "Constant temperature", normalPressure, changeStep / 2, changeStep);

            #region Draw graphics
            DrawFuzzySetToPictureBox(weatherGettingBetter, pictureBoxWeatherGettingBetter);
            DrawFuzzySetToPictureBox(weatherGettingWorse, pictureBoxWeatherGettingWorse);
            DrawFuzzySetToPictureBox(weatherNoChange, pictureBoxWeatherNoChange);
            #endregion
        }


        private static void DrawFuzzySetToPictureBox(FuzzySet fuzzySet, PictureBox picBox)
        {
            RelationImage imgGettingBetter = new RelationImage(fuzzySet);
            Bitmap bmpGettingBetter = new Bitmap(picBox.Width, picBox.Height);
            imgGettingBetter.DrawImage(Graphics.FromImage(bmpGettingBetter));
            picBox.Image = bmpGettingBetter;
        }
        #endregion

        #region Event handlers

        private void inputControls_ValueChanged(object sender, EventArgs e)
        {
            buildRelation();
        }

        private void timerBuildRelation_Tick(object sender, EventArgs e)
        {
            if (_waitingForBuild && !_building)
                buildRelationNow(false);
        }
        #endregion

        #region Builiding sets

        protected void buildRelation()
        {
            //time consuming
            _waitingForBuild = true;
        }


        protected void buildRelationNow(bool initial)
        {
            if (!_ready)
                return;

            _waitingForBuild = false;
            _building = true;

            bool _expressionChanged = false;

            //вземаме входящите данни
            decimal inputTemperature = txtTemp.Value;
            decimal inputDeltaTemperature = txtDeltaTemp.Value;
            int inputDayTime = (int)dayTimePicker.Value.TimeOfDay.TotalMinutes;
            decimal inputDeltaPressurre = txtDeltaPressure.Value;

            //Оценяваме релацията от потребителския интерфейс, като го компилираме до C# код
            #region Expression evaluation with C#
            string strExpression = txtExpression.Text;
            PrependFullName(ref strExpression, "lowTemperature");
            PrependFullName(ref strExpression, "highTemperature");
            PrependFullName(ref strExpression, "goodTemperature");
            PrependFullName(ref strExpression, "risingTemperature");
            PrependFullName(ref strExpression, "fallingTemperature");
            PrependFullName(ref strExpression, "constantTemperature");
            PrependFullName(ref strExpression, "weatherGettingBetter");
            PrependFullName(ref strExpression, "weatherNoChange");
            PrependFullName(ref strExpression, "weatherGettingWorse");
            PrependFullName(ref strExpression, "moonRise");
            PrependFullName(ref strExpression, "moonSet");
            PrependFullName(ref strExpression, "sunRise");
            PrependFullName(ref strExpression, "sunSet");
            PrependFullName(ref strExpression, "lowActivity");
            PrependFullName(ref strExpression, "highActivity");
            PrependFullName(ref strExpression, "mediumActivity");

            string sampleEval = @"((lowTemperature&risingTemperature)&highActivity)%
((weatherGettingWorse)&highActivity)
%((weatherGettingBetter)&highActivity)%
((moonRise|moonSet|sunRise|sunSet)&highActivity)%

((goodTemperature&risingTemperature)& lowActivity)%
((goodTemperature&fallingTemperature)&lowActivity)%

((goodTemperature&constantTemperature)&mediumActivity)%
((!moonRise&!moonSet&!sunRise&!sunSet)&mediumActivity)
";

            object obj = Evaluator.Eval(strExpression);

            if (obj != null)
            {
                if (!(obj is FuzzyRelation))
                {
                    MessageBox.Show(String.Format("ERROR: Object of type FuzzyRelation expected as the result of the expression.\r\nThis object is type {0}.", obj.GetType().FullName),
                        "Error evaluating expression", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else
                {
                    //Вземаме резултата от оценяването
                    _relation = (FuzzyRelation)obj;
                    if (_expression != txtExpression.Text)
                        _expressionChanged = true;
                    _expression = txtExpression.Text;

                }
            }
            #endregion

            //Дефъзификация, чрез използване на Center of Gravity
            #region Defuzzification
            DefuzzificationFactory.DefuzzificationMethod method = DefuzzificationFactory.DefuzzificationMethod.CenterOfGravity;
            _defuzzification = DefuzzificationFactory.GetDefuzzification(
                _relation,
                new Dictionary<IDimension, decimal> {
                        { temperature, inputTemperature },
                        { deltaTemperature, inputDeltaTemperature },
                        { dayTime, inputDayTime },
                        { deltaPressure, inputDeltaPressurre }
                },
                method
                );

            _defuzzMethod = method;
            #endregion

            #region Output value
            string unit = ((IContinuousDimension)_defuzzification.OutputDimension).Unit;
            lblOutput.Text = _defuzzification.CrispValue.ToString("F5") + (string.IsNullOrEmpty(unit) ? "" : " " + unit);
            #endregion


            Cursor.Current = Cursors.WaitCursor;

            #region storing TreeView selection
            //Store information about currenlty selected node. It will become handy
            //when selecting the same node after the refresh (if applicable)
            List<int> selectedNodePath = new List<int>();

            if (treeViewRelation.SelectedNode != null)
            {
                TreeNode pointer = treeViewRelation.SelectedNode;
                while (pointer != null)
                {
                    selectedNodePath.Add(pointer.Index);
                    pointer = pointer.Parent;
                }
            }
            else if (initial)
            {
                selectedNodePath.Add(0);
            }
            #endregion

            TreeSource ts = new TreeSource(_defuzzification);
            ts.DrawImageOnNodeSelect = false;
            ts.BuildTree(treeViewRelation, pictureBoxGraph, lblGraphCaption);

            #region restoring TreeView selection
            if ((!_expressionChanged || initial) && selectedNodePath.Count() > 0 && selectedNodePath[selectedNodePath.Count() - 1] < treeViewRelation.Nodes.Count)
            {
                //We will now try to restore the selection
                TreeNode pointer = treeViewRelation.Nodes[selectedNodePath[selectedNodePath.Count() - 1]];

                for (int i = selectedNodePath.Count() - 2; i >= 0; i--)
                {
                    if (selectedNodePath[i] >= pointer.Nodes.Count)
                    {
                        pointer = null;
                        break;
                    }
                    pointer = pointer.Nodes[selectedNodePath[i]];
                }

                if (pointer != null)
                {
                    treeViewRelation.SelectedNode = pointer;
                    ts.DrawDetailImage(pointer);
                }
            }

            Cursor.Current = Cursors.Default;
            ts.DrawImageOnNodeSelect = true;
            #endregion

            _building = false;
        }

        #endregion

        /// <summary>
        /// variables to class current properties
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="variable"></param>
        protected void PrependFullName(ref string expression, string variable)
        {
            string classFullName = this.GetType().FullName;
            expression = expression.Replace(variable, classFullName + "." + variable);
        }



        private void FishingActivity_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_parentForm != null)
            {
                if (!(!this.Visible && _parentForm.Visible))
                {
                    //no => Close the whole application, including the parent form
                    _parentForm.Close();
                }
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            int minutes = trackBarTime.Value;
            dayTimePicker.Value = DateTime.Now.Date.AddMinutes(minutes);
            buildRelation();
        }

        private void dayTimePicker_ValueChanged(object sender, EventArgs e)
        {
            int timePickerMinutes = (int)dayTimePicker.Value.TimeOfDay.TotalMinutes;
            if (trackBarTime.Value != timePickerMinutes)
            {
                trackBarTime.Value = timePickerMinutes;
            }

            buildRelation();
        }

    }
}
