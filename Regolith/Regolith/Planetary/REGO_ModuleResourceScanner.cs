﻿using Regolith.Common;

namespace Regolith.Planetary
{
    public class REGO_ModuleResourceScanner : PartModule, IAnimatedModule
    {
        [KSPField(isPersistant = false, guiActive = true, guiName = "Abundance")]
        public string abundanceDisplay;

        protected double abundanceValue = 0;

        [KSPField(isPersistant = false)]
        public string ResourceName = "";

        [KSPField]
        public bool isActive = false;

        [KSPField] 
        public int ScannerType = 0;

        [KSPField]
        public float MaxAbundanceAltitude = 500000000f;

        public override void OnStart(StartState state)
        {
            if (state == StartState.Editor)
            {
                return;
            }
            Setup();
        }

        private void Setup()
        {
            var suffix = "";
            switch (ScannerType)
            {
                case 0:
                    suffix = "Crust";
                    break;
                case 1:
                    suffix = "Ocean";
                    break;
                case 2:
                    suffix = "Atmo";
                    break;
                case 3:
                    suffix = "IntPl";
                    break;
                   
            }
            Fields["abundanceDisplay"].guiName = ResourceName + "[" + suffix + "]";
            part.force_activate();
        }

        public override void OnUpdate()
        {
            CheckAbundanceDisplay();
        }

        private void CheckAbundanceDisplay()
        {
            if (Utilities.GetAltitude(vessel) > MaxAbundanceAltitude && !vessel.Landed && ScannerType == 0)
            {
                abundanceDisplay = "Too high";
            }
            else if (!vessel.Splashed && ScannerType == 1)
            {
                abundanceDisplay = "Unavailable";
            }
            else
            {
                DisplayAbundance();
            }
        }

        private void DisplayAbundance()
        {
            if (abundanceValue > 0.001)
            {
                abundanceDisplay = (abundanceValue * 100.0).ToString("0.00") + "%";
            }
            else
            {
                abundanceDisplay = (abundanceValue * 100.0).ToString("0.0000") + "%";
            }
        }

        private void ToggleEvent(string name, bool status)
        {
            Events[name].active = status;
            Events[name].guiActive = status;
        }


        public override void OnFixedUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                abundanceValue = RegolithResourceMap.GetAbundance(vessel.latitude, vessel.longitude, ResourceName,
                    vessel.mainBody.flightGlobalsIndex, ScannerType, vessel.altitude);
            }
        }

        public void EnableModule()
        {
            isEnabled = true;
        }

        public void DisableModule()
        {
            isEnabled = false;
        }

        public bool ModuleIsActive()
        {
            return isActive;
        }
    }
}